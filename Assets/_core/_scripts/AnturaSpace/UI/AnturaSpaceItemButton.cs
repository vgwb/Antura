using Antura.Rewards;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.AnturaSpace.UI
{
    /// <summary>
    /// Button for an item in a category in the Antura Space scene.
    /// </summary>
    public class AnturaSpaceItemButton : UIButton
    {
        public RawImage RewardImage;
        public GameObject IcoLock;
        public GameObject IcoNew;
        public Camera RewardCamera;
        public Transform RewardContainer;

        [System.NonSerialized]
        public RewardBaseItem Data;

        public bool IsNew
        {
            get { return isNew && !isNewForceHidden; }
        }

        public bool IsItemLocked
        {
            get { return _isItemLocked; }
        }

        private RenderTexture renderTexture;
        private bool isNew, isNewForceHidden, _isItemLocked;

        #region Unity

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (renderTexture != null)
            {
                renderTexture.Release();
                renderTexture.DiscardContents();
            }
        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            // Create and assing new RenderTexture
            renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            renderTexture.Create();
            GetComponentInChildren<Camera>(true).targetTexture = renderTexture;
            GetComponentInChildren<RawImage>(true).texture = renderTexture;
        }

        public override void Lock(bool _doLock)
        {
            base.Lock(_doLock);

            _isItemLocked = _doLock;

            IcoLock.SetActive(_doLock);
            RewardImage.gameObject.SetActive(!_doLock);
            if (_doLock)
            {
                IcoNew.SetActive(false);
            }
        }

        public void SetAsNew(bool _isNew)
        {
            isNew = _isNew;
            if (!isNewForceHidden)
            {
                IcoNew.SetActive(_isNew);
            }
        }

        public void SetImage(bool _isRenderTexture)
        {
            RewardImage.texture = _isRenderTexture ? renderTexture : null;
        }

        public override void Toggle(bool _activate, bool _animateClick = false)
        {
            base.Toggle(_activate, _animateClick);
            ForceHideNewIcon(_activate);
        }

        void ForceHideNewIcon(bool _forceHide)
        {
            isNewForceHidden = _forceHide;
            IcoNew.SetActive(!_forceHide && isNew);
        }

        #endregion
    }
}
