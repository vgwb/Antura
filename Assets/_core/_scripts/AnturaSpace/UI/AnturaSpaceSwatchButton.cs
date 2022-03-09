using Antura.Rewards;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.AnturaSpace.UI
{
    /// <summary>
    /// Button used to select a color for an item in the Antura Space scene.
    /// </summary>
    public class AnturaSpaceSwatchButton : UIButton
    {
        public GameObject IcoLock;
        public GameObject IcoNew;
        public Image[] ColorImgs;

        [System.NonSerialized]
        public RewardColorItem Data;

        public override void Lock(bool _doLock)
        {
            base.Lock(_doLock);

            IcoLock.SetActive(_doLock);
            if (_doLock)
                IcoNew.SetActive(false);
            if (AnturaSpaceUI.I.HideLockedSwatchesColors)
            {
                ColorImgs[0].gameObject.SetActive(!_doLock);
                ColorImgs[1].gameObject.SetActive(!_doLock);
            }
        }

        public void SetAsNew(bool _isNew)
        {
            IcoNew.SetActive(_isNew);
        }

        public void SetColors(Color _color0, Color _color1)
        {
            _color0.a = 1;
            _color1.a = 1;
            ColorImgs[0].color = _color0;
            ColorImgs[1].color = _color1;
        }
    }
}
