using TMPro;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class ShopConfirmationPanelUI : MonoBehaviour
    {
        public GameObject bonesCostGo;
        public TextMeshProUGUI bonesCostTextUI;

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
            bonesCostTextUI.text = ShopPhotoManager.I.CurrentPhotoCost.ToString();
            bonesCostGo.SetActive(true);
        }
    }
}