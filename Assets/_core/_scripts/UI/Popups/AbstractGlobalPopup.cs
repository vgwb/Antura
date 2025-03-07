using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AbstractGlobalPopup : MonoBehaviour
    {
        #region EVENTS

        public ActionEvent OnClosing = new("GlobalPopup.OnClosing");

        #endregion

        #region Serialized

        [DeHeader("Abstract Common")]
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfTitle;
        [SerializeField] protected Button btClose; // Can be null for popups that don't use it

        #endregion
        
        CanvasGroup cg;
        Tween openTween;

        #region Unity

        protected virtual void Awake()
        {
            cg = this.GetComponent<CanvasGroup>();
            cg.interactable = false;
            
            openTween = this.transform.DOScale(0, GlobalPopups.ShowTime).From().SetAutoKill(false).Pause().SetUpdate(true)
                .SetEase(Ease.OutBack)
                .OnComplete(() => {
                    cg.interactable = true;
                    BaseOpened();
                })
                .OnRewind(() => this.gameObject.SetActive(false));
            
            if (btClose != null) btClose.onClick.AddListener(Close);
        }
        
        protected virtual void OnDestroy()
        {
            openTween.Kill();
        }

        #endregion

        #region Methods

        protected void BaseClear()
        {
            openTween.Rewind();
        }

        protected void BaseOpen(string title = null)
        {
            tfTitle.text = title;
            cg.interactable = false;
            openTween.Restart();
            this.gameObject.SetActive(true);
        }

        protected virtual void BaseOpened() {}
        
        protected void Close()
        {
            cg.interactable = false;
            openTween.PlayBackwards();
            OnClosing.Dispatch();
        }

        #endregion
    }
}