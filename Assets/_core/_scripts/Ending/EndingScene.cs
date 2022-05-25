using Antura.Animation;
using Antura.Core;
using Antura.Dog;
using Antura.CameraEffects;
using Antura.Keeper;
using Antura.Helpers;
using Antura.LivingLetters;
using Antura.UI;
using System.Collections;
using UnityEngine;

namespace Antura.Scenes
{
    /// <summary>
    /// Manages the Ending scene, which shows a non-interactive introduction to the game.
    /// </summary>
    public class EndingScene : SceneBase
    {
        [Header("References")]
        public LivingLetterController[] Letters;

        public AnturaAnimationController Antura;

        public float m_StateDelay = 1.0f;
        public float m_EndDelay = 2.0f;
        const float FadeTime = 5;

        bool m_Start = true;
        bool m_End = false;

        Vector3 m_CameraStartPosition;
        Vector3 m_CameraEndPosition;
        public Vector3 cameraOffset = new Vector3(0, 5.0f, -10.0f);
        public float m_CameraVelocity = 0.1f;

        public AnimationCurve cameraAnimationCurve;
        public AnimationCurve fadeOutCurve;
        public VignettingSimple vignetting;

        public GameObject environment;
        AutoMove[] autoMoveObjects;

        float time;
        float fadeOutTime;

        bool fadeIn = true;
        private bool useText => AppManager.I.ContentEdition.LearnMethod.ShowEndSceneBigText;
        float lastAlpha = 0;
        private bool textFadeIn = false;

        public TextRender text;
        public CanvasRenderer panel;

        protected override void Start()
        {
            base.Start();
            GlobalUI.ShowPauseMenu(false);

            // We force subtitles on when we enter this scene to show the player that they can read
            if (!AppManager.I.AppSettings.KeeperSubtitlesEnabled)
            {
                AppManager.I.AppSettingsManager.ToggleKeeperSubtitles();
            }

            m_CameraEndPosition = Camera.main.transform.position;
            m_CameraStartPosition = m_CameraEndPosition + cameraOffset;
            autoMoveObjects = environment.GetComponentsInChildren<AutoMove>();

            var lettersData = AppManager.I.Teacher.GetAllTestLetterDataLL();
            foreach (var l in Letters)
            {
                l.Init(lettersData.GetRandom());
                l.State = LLAnimationStates.LL_dancing;
            }

            Antura.State = AnturaAnimationStates.dancing;

            text.SetSentence(Database.LocalizationDataId.End_Scene_2);
            text.Alpha = 0;
            panel.SetAlpha(0);
        }

        void OnEnable()
        {
            Debugging.DebugManager.OnSkipCurrentScene += SkipScene;
        }

        void OnDisable()
        {
            Debugging.DebugManager.OnSkipCurrentScene -= SkipScene;
        }

        void SkipScene()
        {
            StopCoroutine(DoEnding());
            KeeperManager.I.StopSpeaking();
            AppManager.I.NavigationManager.GoToNextScene();
        }

        void Update()
        {
            time += Time.deltaTime * m_CameraVelocity;
            float t = cameraAnimationCurve.Evaluate(time);

            var newAlpha = Mathf.Lerp(lastAlpha, textFadeIn ? 1 : 0, Time.deltaTime);
            if (lastAlpha != newAlpha) {
                text.Alpha = newAlpha;
                panel.SetAlpha(newAlpha);
                lastAlpha = newAlpha;
            }

            if (fadeIn)
            {
                vignetting.fadeOut = Mathf.Pow((1 - t), 2);
            }
            else
            {
                fadeOutTime += Time.deltaTime;
                vignetting.fadeOut = Mathf.Lerp(0, 1, fadeOutTime / FadeTime);
            }

            for (int i = 0; i < autoMoveObjects.Length; ++i)
                autoMoveObjects[i].SetTime(t);

            if (m_Start)
            {
                m_Start = false;
                Debug.Log("Start Ending");

                StartCoroutine(DoEnding());
            }
            else
            {
                if (m_End)
                {
                    AppManager.I.NavigationManager.GoToNextScene();
                    m_End = false;
                    return;
                }
            }

            Camera.main.transform.position = Vector3.Lerp(m_CameraStartPosition, m_CameraEndPosition, t);

        }


        IEnumerator DoEnding()
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

            yield return new WaitForSeconds(3);
            yield return new WaitForSeconds(m_StateDelay);

            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.End_Scene_1_1, false, true, OnCompleted);
            yield return new WaitUntil(CheckIfCompleted);
            KeeperManager.I.PlayDialogue(AppManager.I.ContentEdition.LearnMethod.EndSceneLocID, false, true, OnCompleted);

            yield return new WaitForSeconds(m_StateDelay);
            yield return new WaitUntil(CheckIfCompleted);
            if (useText)
            {
                textFadeIn = true;
                yield return new WaitForSeconds(5f);
                textFadeIn = false;
            }

            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.End_Scene_3_1, false, true, OnCompleted);
            yield return new WaitUntil(CheckIfCompleted);
            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.End_Scene_3_2, false, true, OnCompleted);
            yield return new WaitUntil(CheckIfCompleted);
            KeeperManager.I.PlayDialogue(Database.LocalizationDataId.End_Scene_3_3, false, true, OnCompleted);

            yield return new WaitUntil(CheckIfCompleted);
            yield return new WaitForSeconds(1);

            fadeIn = false;
            yield return new WaitForSeconds(FadeTime);
            m_End = true;

            AppManager.I.Player.SetFinalShown();
        }
    }
}
