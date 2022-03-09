using DG.Tweening;
using UnityEngine;

namespace Antura.Tutorial
{
    public class TutorialUIClicker : TutorialUIProp
    {
        public SpriteRenderer Radius0, Radius1;

        #region Unity

        protected override void Awake()
        {
            base.Awake();

            ShowTween.Kill();
            ShowTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(Radius0.transform.DOScale(Radius0.transform.localScale * 1.6f, 0.5f).SetEase(Ease.Linear))
                .Join(Radius1.transform.DOScale(Radius0.transform.localScale * 1.6f, 0.5f).SetEase(Ease.Linear))
                .Join(Radius0.DOFade(1, 0.1f))
                .Join(Radius1.DOFade(1, 0.1f))
                .Insert(0.2f, Radius0.DOFade(0, 0.3f))
                .Join(Radius1.DOFade(0, 0.3f))
                .OnRewind(() => { this.gameObject.SetActive(false); })
                .OnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                    this.transform.parent = DefParent;
                });
        }

        #endregion
    }
}
