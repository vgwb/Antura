using System;
using DG.DeExtensions;
using DG.Tweening;
using UnityEngine;

namespace Antura.UI
{
    [Flags]
    public enum MinigamesUIElement
    {
        Unset = 1,
        Starbar = 2,
        Timer = 4,
        Lives = 8,
        TimerAndLives = 16
    }

    /// <summary>
    /// General controller of common UI for all minigames.
    /// </summary>
    public class MinigamesUI : MonoBehaviour
    {
        public static MinigamesUIStarbar Starbar;
        public static MinigamesUITimer Timer;
        public static MinigamesUILives Lives;

        static MinigamesUI I;
        const string ResourcePath = "Prefabs/UI/MinigamesUI";
        static MinigamesUIElement elements;
        Sequence showTween;

        #region Unity

        void Awake()
        {
            I = this;

            if (HasElement(MinigamesUIElement.Unset))
            {
                Debug.LogWarning("MinigamesUI ► Elements are Unset");
                return;
            }

            Starbar = this.GetComponentInChildren<MinigamesUIStarbar>(true);
            Timer = this.GetComponentInChildren<MinigamesUITimer>(true);
            Lives = this.GetComponentInChildren<MinigamesUILives>(true);
            Starbar.gameObject.SetActive(HasElement(MinigamesUIElement.Starbar));
            Timer.gameObject.SetActive(HasElement(MinigamesUIElement.Timer));
            Lives.gameObject.SetActive(HasElement(MinigamesUIElement.Lives));
            if (HasElement(MinigamesUIElement.Timer) && HasElement(MinigamesUIElement.Lives))
            {
                // Shift lives under timer
                Lives.RectTransform.SetAnchoredPosY(Timer.RectTransform.anchoredPosition.y - Timer.RectTransform.sizeDelta.y * 0.5f);
            }

            // Activate and animate elements entrance
            const float duration = 0.5f;
            showTween = DOTween.Sequence();
            TweenParams tp = TweenParams.Params.SetEase(Ease.OutBack);
            if (HasElement(MinigamesUIElement.Starbar))
            {
                showTween.Insert(0, Starbar.RectTransform.DOAnchorPosX(0, duration).From().SetAs(tp));
            }
            if (HasElement(MinigamesUIElement.Timer))
            {
                showTween.Insert(0, Timer.RectTransform.DOAnchorPosX(Timer.RectTransform.sizeDelta.x * 0.5f, duration).From().SetAs(tp));
            }
            if (HasElement(MinigamesUIElement.Lives))
            {
                showTween.Insert(0, Lives.RectTransform.DOAnchorPosX(0, duration).From().SetAs(tp));
            }
        }

        void OnDestroy()
        {
            if (I == this)
            {
                I = null;
                elements = MinigamesUIElement.Unset;
            }
            showTween.Kill();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Instantiates the Minigames UI in a scene, with the given parameters.
        /// The minigames UI is destroyed when a new scene is loaded.
        /// <para>
        /// NOTE: calling this method when a MinigamesUI is already in the scene will destroy the current one and create a new one.
        /// </para>
        /// NOTE: after calling this method, you will have to call the <code>Setup</code> method on each separate element
        /// (except for Starbar that doesn't need it).
        /// </summary>
        /// <param name="_elements">UI elements, uses Flag enum. For example, to activate both Starbar and Timer, use:
        /// <para><code>Init(MinigamesUIElement.Starbar | MinigamesUIElement.Lives);</code></para>
        /// while to activate only the Timer, call:
        /// <para><code>Init(MinigamesUIElement.Timer);</code></para>
        /// </param>
        public static void Init(MinigamesUIElement _elements)
        {
            if (I != null)
                Destroy(I.gameObject);

            elements = _elements;
            GameObject go = Instantiate(Resources.Load<GameObject>(ResourcePath));
            go.name = "[MinigamesUI]";
            I = go.GetComponent<MinigamesUI>();
        }

        #endregion

        #region Helpers

        bool HasElement(MinigamesUIElement _element)
        {
            return (elements & _element) == _element;
        }

        #endregion
    }
}
