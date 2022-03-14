using Antura.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.UI
{
    /// <summary>
    /// Manages the activation/deactivation of UIButtons on certain conditions
    /// </summary>
    public static class UIDirector
    {
        private static bool initialized;
        private static readonly List<UIButton> allActiveUIButtons = new List<UIButton>();

        public static void Init()
        {
            if (initialized)
            { return; }

            initialized = true;
            AppManager.I.NavigationManager.OnSceneStartTransition += OnSceneStartTransition;
        }

        #region Public Methods

        public static void Add(UIButton button)
        {
            if (!allActiveUIButtons.Contains(button))
            { allActiveUIButtons.Add(button); }
        }

        public static void Remove(UIButton button)
        {
            allActiveUIButtons.Remove(button);
        }

        #endregion

        #region Methods

        public static void DeactivateAllUI()
        {
            foreach (UIButton bt in allActiveUIButtons)
            {
                bt.Bt.interactable = false;
            }
            // NOTE: would be nicer and faster to just set EventSystem.current.enabled to FALSE, but there's non-UI elements that rely on it apparently,
            // and will throw a NullReferenceException if it's disabled (for example the MAP)
            //            EventSystem.current.enabled = false;
        }

        #endregion

        #region Callbacks

        static void OnSceneStartTransition()
        {
            DeactivateAllUI();
        }

        #endregion
    }
}
