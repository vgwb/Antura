using Antura.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// Button for selecting an avatar in the Profile Selector.
    /// </summary>
    public class ProfileSelectorAvatarButton : UIButton
    {
        public Image AvatarImg;

        private CanvasGroup cGroup;

        #region Unity

        protected override void Awake()
        {
            base.Awake();

            if (cGroup == null)
            {
                cGroup = this.gameObject.AddComponent<CanvasGroup>();
            }
        }

        #endregion

        #region Public Methods

        public void SetAvatar(int _avatarIndex)
        {
            AvatarImg.sprite = Resources.Load<Sprite>(AppConfig.RESOURCES_DIR_AVATARS + _avatarIndex);
        }

        public void SetInteractivity(bool _interactive)
        {
            if (cGroup == null)
            {
                cGroup = this.gameObject.AddComponent<CanvasGroup>();
            }
            cGroup.alpha = _interactive ? 1 : 0.3f;
            Bt.interactable = _interactive;
        }

        #endregion
    }
}
