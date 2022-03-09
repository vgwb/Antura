#if FALSE
using Antura.Core;
using Antura.UI;
using DG.Tweening;
using System;
using UnityEngine;

namespace Antura.Profile
{
    /// <summary>
    /// Player creation category
    /// </summary>
    public class PlayerCreationUICategory : MonoBehaviour
    {
#region Events

        public event Action<PlayerCreationUICategory, UIButton> OnSelect;
        public event Action<PlayerCreationUICategory> OnDeselectAll;

        void DispatchOnSelect(PlayerCreationUICategory category, UIButton uiButton)
        {
            if (OnSelect != null) { OnSelect(category, uiButton); }
        }

        void DispatchOnDeselectAll(PlayerCreationUICategory category)
        {
            if (OnDeselectAll != null) { OnDeselectAll(category); }
        }

#endregion

#region Serialized

        public PlayerCreationUI.CategoryType CategoryType;

#endregion

        /// <summary>If nothing is selected, returns -1</summary>
        public int SelectedIndex { get; private set; }

        [NonSerialized]
        public UIButton[] UIButtons;

        Sequence _ageAppearanceTween;

#region Unity

        void Awake()
        {
            SelectedIndex = -1;
            UIButtons = GetComponentsInChildren<UIButton>();
            foreach (UIButton uiButton in UIButtons)
            {
                UIButton bt = uiButton;
                bt.Bt.onClick.AddListener(() => OnClick(bt));
            }

            switch (CategoryType)
            {
                case PlayerCreationUI.CategoryType.Color:
                    // Set colors
                    for (int i = 0; i < UIButtons.Length; ++i)
                    {
                        UIButton bt = UIButtons[i];
                        Color color = PlayerTintConverter.ToColor((PlayerTint)(i + 1));
                        bt.ChangeDefaultColors(color, color);
                    }
                    break;
                case PlayerCreationUI.CategoryType.Age:
                    _ageAppearanceTween = DOTween.Sequence().SetAutoKill(false).Pause();
                    for (int i = 0; i < UIButtons.Length; ++i)
                    {
                        _ageAppearanceTween.Insert(i * 0.1f, UIButtons[i].CGroup.DOFade(0, 0.4f).From());
                    }
                    break;
            }
        }

        void OnEnable()
        {
            if (CategoryType == PlayerCreationUI.CategoryType.Age)
            {
                _ageAppearanceTween.Restart();
            }
        }

        void OnDisable()
        {
            if (CategoryType == PlayerCreationUI.CategoryType.Age)
            {
                _ageAppearanceTween.Rewind();
            }
        }

        void OnDestroy()
        {
            _ageAppearanceTween.Kill();
            foreach (UIButton uiButton in UIButtons)
            {
                uiButton.Bt.onClick.RemoveAllListeners();
            }
        }

#endregion

#region Public Methods

        // If index is less than 0 toggles all
        public void Select(int index)
        {
            if (index > UIButtons.Length - 1)
            {
                Debug.LogWarning("PlayerCreationUICategory.Select > Index out of range (captured)");
                return;
            }

            if (index < 0)
            {
                // Deselect all
                if (SelectedIndex >= 0)
                {
                    SelectedIndex = -1;
                    foreach (var uiButton in UIButtons)
                    {
                        uiButton.Toggle(true);
                        if (CategoryType == PlayerCreationUI.CategoryType.Color)
                        {
                            uiButton.transform.localScale = Vector3.one;
                        }
                    }
                    DispatchOnDeselectAll(this);
                }
            }
            else
            {
                // Select index
                SelectedIndex = index;
                for (var i = 0; i < UIButtons.Length; ++i)
                {
                    var bt = UIButtons[i];
                    bt.Toggle(i == index);
                    if (CategoryType == PlayerCreationUI.CategoryType.Color)
                    {
                        bt.transform.localScale = Vector3.one * (i == index ? 1 : 0.75f);
                    }
                }
            }
        }

        public void SetColor(Color color)
        {
            foreach (var uiButton in UIButtons)
            {
                uiButton.ChangeDefaultColors(color);
            }
        }

        public void ResetColor()
        {
            foreach (UIButton uiButton in UIButtons)
            {
                uiButton.ChangeDefaultColors(Color.white);
            }
        }

        // Only used by avatars category
        public void AvatarSetIcon(bool isFemale)
        {
            // TODO use different avatars
            for (var i = 0; i < UIButtons.Length; ++i)
            {
                var sprite = Resources.Load<Sprite>(AppConfig.RESOURCES_DIR_AVATARS + (isFemale ? "F" : "M") + (i + 1));
                UIButtons[i].Ico.sprite = sprite;
            }
        }

#endregion

#region Callbacks

        void OnClick(UIButton bt)
        {
            if (_ageAppearanceTween != null && _ageAppearanceTween.IsPlaying())
            {
                return;
            }

            var deselect = bt.IsToggled && SelectedIndex >= 0
                           && (CategoryType != PlayerCreationUI.CategoryType.Avatar ||
                               PlayerCreationUI.State == PlayerCreationUI.UIState.AvatarCreation);
            if (deselect)
            {
                Select(-1);
            }
            else
            {
                var index = Array.IndexOf(UIButtons, bt);
                Select(index);
                DispatchOnSelect(this, UIButtons[index]);
            }
        }

#endregion
    }
}
#endif
