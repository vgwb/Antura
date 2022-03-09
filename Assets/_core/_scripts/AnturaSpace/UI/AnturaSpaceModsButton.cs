using Antura.UI;
using UnityEngine;

namespace Antura.AnturaSpace.UI
{
    public class AnturaSpaceModsButton : UIButton
    {
        private GameObject icoNew;

        public void SetAsNew(bool _isNew)
        {
            if (icoNew == null)
            {
                icoNew = GetComponentInChildren<AnturaSpaceNewIcon>().gameObject;
            }
            icoNew.SetActive(_isNew);
        }
    }
}
