using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class StartEndPanel : AbstractDialogueBalloon
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] Image bg;
        [DeEmptyAlert]
        [SerializeField] RectTransform content;
        [DeEmptyAlert]
        [SerializeField] RectTransform mascots;
        [DeEmptyAlert]
        [SerializeField] GameObject startMascot, endMascot;
        [SerializeField] EndStar[] stars;

        #endregion

        #region Public Methods

        public new void Show(QuestNode node, bool UseLearningLanguage, bool isStartPanel = true, int totStars = 0)
        {
            if (isStartPanel)
            {
                startMascot.SetActive(true);
                endMascot.SetActive(false);
                for (int i = 0; i < stars.Length; i++) stars[i].gameObject.SetActive(false);
            }
            else
            {
                startMascot.SetActive(false);
                endMascot.SetActive(true);
                for (int i = 0; i < stars.Length; i++)
                {
                    stars[i].gameObject.SetActive(true);
                    stars[i].TurnOn(i < totStars);
                }
            }
            
            base.Show(node, UseLearningLanguage);
        }

        #endregion
        
        #region Methods

        protected override void CreateShowTween()
        {
            const float duration = 0.6f;
            showTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Join(bg.DOFade(0, duration).From().SetEase(Ease.Linear))
                .Join(content.transform.DOScale(0, duration).From().SetEase(Ease.OutBack))
                .Join(content.DOAnchorPosY(-100, duration).From(true).SetEase(Ease.OutBack))
                .Join(mascots.DOAnchorPosY(-440, duration).From(true).SetEase(Ease.OutBack))
                .OnComplete(() => SetInteractable(true))
                .OnRewind(() => this.gameObject.SetActive(false));
        }

        #endregion
    }
}