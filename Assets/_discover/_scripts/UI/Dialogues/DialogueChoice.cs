using Antura.UI;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    [RequireComponent(typeof(Button), typeof(CanvasGroup))]
    public class DialogueChoice : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] TextRender textRender;

        #endregion
        
        public RectTransform RectT { get; private set; }
        public CanvasGroup CanvasGroup { get; private set; }
        public Button Button { get; private set; }
        public Vector2 DefAnchoredP { get; private set; }
        

        #region Unity

        void Awake()
        {
            RectT = this.GetComponent<RectTransform>();
            CanvasGroup = this.GetComponent<CanvasGroup>();
            DefAnchoredP = RectT.anchoredPosition;
            Button = this.GetComponentInChildren<Button>();
        }

        #endregion
        
        #region Public Methods

        public void SetInteractable(bool interactable)
        {
            Button.interactable = interactable;
        }
        
        public void SetText(string text)
        {
            textRender.SetText(text);
        }

        #endregion
    }
}