using Antura.Core;
using Antura.UI;
using TMPro;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class ShopConfirmationPanelUI : MonoBehaviour
    {
        public GameObject bonesCostGo;
        public TextMeshProUGUI bonesCostTextUI;
        public UIButton buttonCancel;

        public void SetupForPurchase()
        {
            bonesCostTextUI.text = ShopDecorationsManager.I.CurrentDecorationCost.ToString();
            bonesCostGo.SetActive(true);
        }

        public void SetupForDeletion()
        {
            bonesCostGo.SetActive(false);
        }

        public void SetupForPhoto()
        {
            if (AppManager.I.Services.Gallery.HasWriteAccess)
            {
                bonesCostTextUI.text = ShopPhotoManager.I.CurrentPhotoCost.ToString();
                bonesCostGo.SetActive(true);
                buttonCancel.gameObject.SetActive(true);
            }
            else
            {
                bonesCostGo.SetActive(false);
                buttonCancel.gameObject.SetActive(false);
            }

        }
    }
}
