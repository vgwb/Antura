using Antura.Core;
using Antura.Rewards;
using Antura.Tutorial;
using Antura.UI;
using UnityEngine;

namespace Antura.AnturaSpace.UI
{
    /// <summary>
    /// Button for a category in the Antura Space scene.
    /// </summary>
    public class AnturaSpaceCategoryButton : UIButton
    {
        public enum AnturaSpaceCategory
        {
            Unset,
            HEAD,
            Ears, // Output as EAR_R and EAR_L
            NOSE,
            JAW,
            NECK,
            BACK,
            TAIL,
            Texture,
            Decal
        }

        public AnturaSpaceCategory Category;

        public bool IsNew
        {
            get { return isNew && !isNewForceHidden; }
        }

        public bool Unlocked { get; set; }

        private GameObject icoNew;
        private bool isNew, isNewForceHidden;

        public void SetAsNew(bool _isNew)
        {
            isNew = _isNew;
            if (icoNew == null)
            {
                icoNew = GetComponentInChildren<AnturaSpaceNewIcon>().gameObject;
            }
            if (!isNewForceHidden)
            {
                icoNew.SetActive(_isNew);
            }
        }

        public override void Toggle(bool _activate, bool _animateClick = false)
        {
            base.Toggle(_activate, _animateClick);
            ForceHideNewIcon(_activate);
        }

        void ForceHideNewIcon(bool _forceHide)
        {
            isNewForceHidden = _forceHide;
            if (icoNew == null)
            {
                icoNew = GetComponentInChildren<AnturaSpaceNewIcon>().gameObject;
            }
            icoNew.SetActive(!_forceHide && isNew);
        }
    }
}
