using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialoguePostcardFocusView : MonoBehaviour
    {
        #region Events

        public readonly ActionEvent OnClicked = new("DialoguePostcardFocusView.OnClicked");

        #endregion

        #region Serialized

        [DeEmptyAlert]
        [SerializeField] Image img;

        #endregion

        Button bt;

        #region Unity

        void Awake()
        {
            bt = this.GetComponent<Button>();
            
            bt.onClick.AddListener(OnClicked.Dispatch);
        }

        #endregion

        #region Public Methods

        public void Show(Sprite sprite)
        {
            img.sprite = sprite;
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        #endregion
    }
}