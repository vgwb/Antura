using Antura.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Antura.Core
{
    /// <summary>
    /// Manager that handles scene load and transitions between scenes.
    /// </summary>
    public class SceneTransitionManager
    {
        // Parameters
        private float transitionCloseTime = 0.15f;

        public Action OnSceneStartTransition;
        public Action OnSceneEndTransition;

        #region Loading State

        private bool _isTransitioning = false;

        public bool IsTransitioning
        {
            get { return _isTransitioning; }
        }

        public int NumberOfLoadedScenes = 0;

        #endregion

        #region Enabling / Disabling

        public void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        #endregion

        #region Settings

        /// <summary>
        /// Actual transition settings.
        /// </summary>
        public SceneTransitionSettings TransitionSettings = new SceneTransitionSettings();

        /// <summary>
        /// Set scene transition settings.
        /// </summary>
        /// <param name="_transitionSettings"></param>
        public void SetSettings(SceneTransitionSettings _transitionSettings)
        {
            TransitionSettings = _transitionSettings;
        }

        #endregion

        #region Load Scene

        /// <summary>
        /// Load scene. Call just once, otherwise transition will be reset and not triggered.
        /// </summary>
        /// <param name="_sceneToLoad">The name of the scene to load</param>
        public void LoadSceneWithTransition(string _sceneToLoad)
        {
            LoadSceneWithTransition(_sceneToLoad, TransitionSettings);
        }

        /// <summary>
        /// Load scene. Call just once, otherwise transition will be reset and not triggered.
        /// </summary>
        /// <param name="_sceneToLoad">The name of the scene to load</param>
        /// <param name="_transitionSettings">Custom transition settings</param>
        public void LoadSceneWithTransition(string _sceneToLoad, SceneTransitionSettings _transitionSettings)
        {
            _isTransitioning = true;
            if (OnSceneStartTransition != null)
            {
                OnSceneStartTransition();
            }

            SceneTransitioner.Show(true, delegate
            { OnSceneTransitionComplete(_sceneToLoad); });
        }

        void OnSceneTransitionComplete(string _sceneToLoad)
        {
            SceneManager.LoadScene(_sceneToLoad);
            AppManager.I.OnSceneChanged();
        }

        #endregion

        #region On Scene Loaded

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (SceneTransitioner.IsShown)
            {
                AppManager.I.StartCoroutine(CloseSceneTransitionerCO(transitionCloseTime));
            }

            NumberOfLoadedScenes++;
        }

        IEnumerator CloseSceneTransitionerCO(float _waitTime)
        {
            yield return new WaitForSeconds(_waitTime);
            SceneTransitioner.Show(false);

            _isTransitioning = false;
            if (OnSceneEndTransition != null)
            {
                OnSceneEndTransition();
            }
        }

        #endregion
    }
}
