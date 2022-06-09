using Antura.Core;
using Antura.Keeper;
using Antura.Language;
using DG.DemiLib.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.UI
{
    /// <summary>
    /// Global UI created dynamically at runtime,
    /// contains all global UI elements > Pause, SceneTransitioner, ContinueScreen, PopupScreen
    /// </summary>
    [DeScriptExecutionOrder(-100)]
    public class GlobalUI : MonoBehaviour
    {
        public static GlobalUI I { get; private set; }
        public static SceneTransitioner SceneTransitioner { get; private set; }
        public static ContinueScreen ContinueScreen { get; private set; }
        public static WidgetPopupWindow WidgetPopupWindow { get; private set; }
        public static WidgetSubtitles WidgetSubtitles { get; private set; }
        public static PauseMenu PauseMenu { get; private set; }
        public static PromptPanel Prompt { get; private set; }

        public UIButton BackButton;
        public ActionFeedbackComponent ActionFeedback { get; private set; }

        const string ResourceId = "Prefabs/UI/GlobalUI";
        const string SceneTransitionerResourceId = "Prefabs/UI/SceneTransitionerUI";
        private Action onGoBack;
        private bool disableBackButtonOnClick;

        public static void Init()
        {
            if (I != null)
            { return; }

            I = Instantiate(Resources.Load<GlobalUI>(ResourceId));
            I.gameObject.name = "[GlobalUI]";
        }

        void Awake()
        {
            I = this;

            // Awake or instantiate all global UI elements
            if (SceneTransitioner == null)
            {
                SceneTransitioner = Instantiate(Resources.Load<SceneTransitioner>(SceneTransitionerResourceId));
                SceneTransitioner.name = "[SceneTransitionerUI]";
                DontDestroyOnLoad(SceneTransitioner.gameObject);
            }
            ContinueScreen = StoreAndAwake<ContinueScreen>();
            WidgetPopupWindow = StoreAndAwake<WidgetPopupWindow>();
            WidgetSubtitles = StoreAndAwake<WidgetSubtitles>();
            PauseMenu = StoreAndAwake<PauseMenu>();
            ActionFeedback = StoreAndAwake<ActionFeedbackComponent>();
            Prompt = StoreAndAwake<PromptPanel>();

            if (onGoBack == null)
            {
                BackButton.gameObject.SetActive(false);
            }

            // Listeners
            BackButton.Bt.onClick.AddListener(OnBack);
        }

        void OnDestroy()
        {
            if (I == this)
            { I = null; }
            BackButton.Bt.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// Immediately clears the GlobalUI elements
        /// </summary>
        /// <param name="includeSceneTransitioner">If TRUE (default) also clears the sceneTransitioner, otherwise not</param>
        public static void Clear(bool includeSceneTransitioner = true)
        {
            if (includeSceneTransitioner && SceneTransitioner != null)
            {
                SceneTransitioner.CloseImmediate();
            }
            if (ContinueScreen != null)
            {
                ContinueScreen.Close(true);
            }
            if (WidgetPopupWindow != null)
            {
                WidgetPopupWindow.Close(true);
            }
            if (WidgetSubtitles != null)
            {
                WidgetSubtitles.Close(true);
            }
        }

        /// <summary>
        /// Shows/hides the pause menu
        /// </summary>
        public static void ShowPauseMenu(bool _visible, PauseMenuType _type = PauseMenuType.GameScreen)
        {
            PauseMenu.gameObject.SetActive(_visible);
            PauseMenu.SetType(_type);
        }

        /// <summary>
        /// Shows the BACK button with eventual callback
        /// </summary>
        /// <param name="_doShow">If TRUE shows it, otherwise hides it</param>
        /// <param name="_callback">Callback to fire when button is pressed. If left empty, calls <see cref="NavigationManager.GoBack"/></param>
        public static void ShowBackButton(bool _doShow, Action _callback = null, bool _disableOnClick = true)
        {
            I.BackButton.gameObject.SetActive(_doShow);
            if (_doShow)
            {
                I.disableBackButtonOnClick = _disableOnClick;
                I.BackButton.Bt.interactable = true;
                I.onGoBack = _callback == null ? AppManager.I.NavigationManager.GoBack : _callback;
            }
        }

        /// <summary>
        /// Shows a popup with a YES/NO button and relative callbacks
        /// </summary>
        public static void ShowPrompt(Database.LocalizationDataId id, Action _onYesCallback = null, Action _onNoCallback = null, LanguageUse _languageUse = LanguageUse.Native)
        {
            Prompt.Show(id, _languageUse, _onYesCallback, _onNoCallback);
        }

        public static void ShowPrompt(string _message, Action _onYesCallback = null, Action _onNoCallback = null, LanguageUse _languageUse = LanguageUse.Native)
        {
            Prompt.Show(_message, _languageUse, _onYesCallback, _onNoCallback);
        }

        public static void ClosePrompt()
        {
            Prompt.Close();
        }

        T StoreAndAwake<T>() where T : Component
        {
            T obj = this.GetComponentInChildren<T>(true);
            obj.gameObject.SetActive(true);
            return obj;
        }

        void OnBack()
        {
            if (disableBackButtonOnClick)
            {
                I.BackButton.Bt.interactable = false;
            }
            if (onGoBack != null)
            {
                onGoBack();
            }
        }

        public bool IsFingerOverUI()
        {
            // Mouse is -1, the rest are fingers
            for (int touchIndex = -1; touchIndex < Input.touchCount; touchIndex++)
            {
                int fingerIndex = touchIndex;
                if (touchIndex >= 0)
                {
                    var touch = Input.GetTouch(touchIndex);
                    if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                    {
                        // Skip this touch
                        continue;
                    }
                    fingerIndex = Input.GetTouch(touchIndex).fingerId;
                }

                Vector2 pos;
                if (fingerIndex == -1)
                    pos = Input.mousePosition;
                else
                    pos = Input.GetTouch(touchIndex).position;

                bool isTouching = IsPointerOverUIObject(pos);
                if (isTouching)
                {
                    //Debug.Log("FINGER " + touchIndex + " IS TOUCHING");
                    return true;
                }
            }
            return false;
        }

        private bool IsPointerOverUIObject(Vector2 pos)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = pos;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
