using Antura.UI;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialogueBalloon : MonoBehaviour
    {
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] TextRender textRender;

        #endregion
        
        Tween showTween;
        
        #region Unity

        void Start()
        {
            showTween = this.transform.DOScale(0, 0.5f).From().SetAutoKill(false).Pause()
                .SetEase(Ease.OutBack)
                .OnRewind(() => this.gameObject.SetActive(false));
            
            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Show(string text)
        {
            textRender.SetText(text);
            showTween.timeScale = 1;
            showTween.Restart();
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            showTween.timeScale = 2;
            showTween.PlayBackwards();
        }

        #endregion
    }
}