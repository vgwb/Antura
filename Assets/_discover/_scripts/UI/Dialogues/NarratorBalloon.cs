using DG.Tweening;
using Homer;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class NarratorBalloon : AbstractDialogueBalloon
    {
        #region Methods

        protected override void CreateShowTween()
        {
            const float duration = 0.6f;
            showTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Join(this.transform.DOScale(0, duration).From().SetEase(Ease.OutBack))
                .Join(this.GetComponent<RectTransform>().DOAnchorPosY(-350, duration).From(true).SetEase(Ease.OutBack))
                .Join(this.transform.DOPunchRotation(new Vector3(0, 0, 18), duration, 8))
                .OnComplete(() => {
                    bt.interactable = currNode.Type == HomerNode.NodeType.TEXT;
                })
                .OnRewind(() => {
                    icoContinueTween.Rewind();
                    this.gameObject.SetActive(false);
                });
        }

        #endregion
    }
}