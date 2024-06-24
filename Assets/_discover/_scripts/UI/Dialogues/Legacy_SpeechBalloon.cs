using DG.Tweening;
using Homer;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class Legacy_SpeechBalloon : AbstractDialogueBalloon
    {
        #region Methods

        protected override void CreateShowTween()
        {
            showTween = this.transform.DOScale(0, 0.5f).From().SetAutoKill(false).Pause()
                .SetEase(Ease.OutBack)
                .OnComplete(() => {
                    bt.interactable = true;
                })
                .OnRewind(() => {
                    this.gameObject.SetActive(false);
                });
        }

        #endregion
    }
}