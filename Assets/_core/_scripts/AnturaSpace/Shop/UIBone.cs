using Antura.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class UIBone : MonoBehaviour
    {
        public bool empty;
        public bool noBorder;

        public void OnEnable()
        {
            Switch();
        }

        public void Switch()
        {
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"{AppManager.I.Player.PetData.SelectedPet}/UI/bone{(empty ? "_empty" : "")}{(noBorder ? "_noBorder" : "")}");
        }
    }
}
