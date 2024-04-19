using DG.Tweening;
using Homer;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class SpeechBalloon : AbstractDialogueBalloon
    {
        #region Methods

        protected override void CreateShowTween()
        {
            showTween = this.transform.DOScale(0, 0.5f).From().SetAutoKill(false).Pause()
                .SetEase(Ease.OutBack)
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