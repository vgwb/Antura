using Antura.Audio;
using Antura.Tutorial;
using Antura.Utilities;
using UnityEngine;

namespace Antura.Core
{
    public class SceneBase : SingletonMonoBehaviour<SceneBase>
    {
        [Header("Base Scene Setup")]
        public Music SceneMusic;

        protected virtual void Start()
        {
            if (AppManager.I.AppSettings.ClassRoomMode > 0)
                return;
            if (SceneMusic != Music.DontChange)
            {
                AudioManager.I.PlayMusic(SceneMusic);
            }
        }

        #region Tutorial Mode

        public bool TutorialMode
        {
            get { return tutorialManager.IsRunning; }
        }

        public TutorialManager tutorialManager;

        #endregion
    }
}
