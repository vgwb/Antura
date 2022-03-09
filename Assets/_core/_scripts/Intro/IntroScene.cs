using Antura.Animation;
using Antura.Core;
using Antura.CameraEffects;
using Antura.Keeper;
using Antura.Minigames;
using Antura.UI;
using System.Collections;
using UnityEngine;

namespace Antura.Intro
{
    /// <summary>
    /// Manages the Intro scene, which shows a non-interactive introduction to the game.
    /// </summary>
    public class IntroScene : SceneBase
    {
        [Header("References")]
        public IntroFactory factory;

        CountdownTimer countDown;

        public float m_StateDelay = 1.0f;
        public float m_EndDelay = 2.0f;

        bool m_Start = true;
        bool m_End = false;

        Vector3 m_CameraStartPosition;
        Vector3 m_CameraEndPosition;
        public Vector3 cameraOffset = new Vector3(0, 5.0f, -10.0f);
        public float m_CameraVelocity = 0.1f;

        public IntroRocketCharacter[] m_MazeCharacters;
        public float m_MazeCharactesVelocity = 0.1f;

        public AnimationCurve cameraAnimationCurve;

        //public UnityStandardAssets.ImageEffects.ForegroundCameraEffect foregroundEffect;
        public VignettingSimple vignetting;

        public GameObject environment;
        AutoMove[] autoMoveObjects;

        float time;

        protected override void Start()
        {
            base.Start();
            GlobalUI.ShowPauseMenu(false);

            countDown = new CountdownTimer(m_EndDelay);
            m_CameraEndPosition = Camera.main.transform.position;
            m_CameraStartPosition = m_CameraEndPosition + cameraOffset;
            autoMoveObjects = environment.GetComponentsInChildren<AutoMove>();

            foreach (var mazeCharacter in m_MazeCharacters)
            {
                mazeCharacter.transform.position += new Vector3(0, 10f, 0);
                mazeCharacter.m_Velocity = m_MazeCharactesVelocity;
            }
        }

        void OnEnable()
        {
            Debugging.DebugManager.OnSkipCurrentScene += SkipScene;
        }

        private void CountDown_onTimesUp()
        {
            AppManager.I.NavigationManager.GoToNextScene();
        }

        void OnDisable()
        {
            Debugging.DebugManager.OnSkipCurrentScene -= SkipScene;

            if (countDown != null)
            {
                countDown.onTimesUp -= CountDown_onTimesUp;
            }
            //Debug.Log("OnDisable() Intro scene");
        }

        void SkipScene()
        {
            StopCoroutine(DoIntroduction());
            KeeperManager.I.StopSpeaking();
            AppManager.I.NavigationManager.GoToNextScene();
        }

        void Update()
        {
            time += Time.deltaTime * m_CameraVelocity;
            float t = cameraAnimationCurve.Evaluate(time);

            vignetting.fadeOut = Mathf.Pow((1 - t), 2);

            for (int i = 0; i < autoMoveObjects.Length; ++i)
                autoMoveObjects[i].SetTime(t);

            if (m_Start)
            {
                m_Start = false;
                Debug.Log("Start Introduction");
                foreach (var mazeCharacter in m_MazeCharacters)
                {
                    mazeCharacter.SetDestination();
                }
                StartCoroutine(DoIntroduction());
            }
            else
            {
                if (m_End)
                {
                    countDown.Update(Time.deltaTime);
                }
            }

            Camera.main.transform.position = Vector3.Lerp(m_CameraStartPosition, m_CameraEndPosition, t);
        }

        public void DisableAntura()
        {
            factory.antura.SetAnturaTime(false);
            countDown.Start();
            countDown.onTimesUp += CountDown_onTimesUp;
        }


        IEnumerator DoIntroduction()
        {
            bool completed = false;
            System.Func<bool> CheckIfCompleted = () =>
            {
                if (completed)
                {
                    // Reset it
                    completed = false;
                    return true;
                }
                return false;
            };

            System.Action OnCompleted = () => { completed = true; };

            yield return new WaitForSeconds(m_StateDelay);

            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Intro_Welcome, true, true, OnCompleted);

            yield return new WaitUntil(CheckIfCompleted);
            yield return new WaitForSeconds(m_StateDelay);

            Debug.Log("Start Spawning");
            factory.StartSpawning = true;

            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Intro_Letters_1, true, true, OnCompleted);

            yield return new WaitUntil(CheckIfCompleted);
            yield return new WaitForSeconds(m_StateDelay);

            Debug.Log("Second Intro Letter");
            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Intro_Letters_2, true, true, OnCompleted);

            yield return new WaitUntil(CheckIfCompleted);
            yield return new WaitForSeconds(m_StateDelay);

            factory.antura.SetAnturaTime(true);
            Debug.Log("Antura is enable");
            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Intro_Dog, true, true, OnCompleted);

            yield return new WaitUntil(CheckIfCompleted);
            yield return new WaitForSeconds(m_StateDelay);

            DisableAntura();

            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Intro_Dog_Chase, true, true, OnCompleted);

            yield return new WaitUntil(CheckIfCompleted);

            m_End = true;
        }
    }
}
