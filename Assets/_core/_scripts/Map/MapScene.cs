using System.Collections;
using Antura.Core;
using Antura.Debugging;
using Antura.Tutorial;
using UnityEngine;

namespace Antura.Map
{
    /// <summary>
    /// Manages the Map scene, from which the next Play Session can be started.
    /// </summary>
    public class MapScene : SceneBase
    {
        protected override void Start()
        {
            base.Start();
            //KeeperManager.I.PlayDialog(Db.LocalizationDataId.Map_Intro);

            DebugManager.OnSkipCurrentScene += HandleSkipScene;
        }

        void OnDestroy()
        {
            DebugManager.OnSkipCurrentScene -= HandleSkipScene;
        }

        private void HandleSkipScene()
        {
            Play();
        }

        public void GoToAnturaSpace()
        {
            AppManager.I.NavigationManager.GoToAnturaSpace();
        }

        private bool alreadyPressedPlay = false;
        public void Play()
        {
            if (!alreadyPressedPlay)
            {
                alreadyPressedPlay = true;
                AppManager.I.NavigationManager.GoToNextScene();
            }
        }

        #region Tutorial Helper

        private Coroutine tutorialCo;
        public void StartTutorialClickOn(Transform targetTr)
        {
            tutorialCo = StartCoroutine(TutorialHintClickCO(targetTr));
        }

        public void StopTutorialClick()
        {
            if (tutorialCo != null)
            {
                StopCoroutine(tutorialCo);
                tutorialCo = null;
            }
        }

        private IEnumerator TutorialHintClickCO(Transform targetTr)
        {
            yield return new WaitForSeconds(3f);

            TutorialUI.SetCamera(Camera.main);
            while (true)
            {
                TutorialUI.Click(targetTr.position);
                yield return new WaitForSeconds(1.5f);
            }
        }
        #endregion
    }
}