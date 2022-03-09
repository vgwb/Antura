using DG.DeExtensions;
using DG.Tweening;
using UnityEngine;

namespace Antura.Tutorial
{
    public class TutorialUIMark : TutorialUIProp
    {
        bool awakeDone;
        Vector3 setImgSize = Vector3.one;

        #region Unity

        protected override void Awake()
        {
            base.Awake();

            awakeDone = true;

            ShowTween.Kill();
            Img.transform.localScale = setImgSize;
            Img.SetAlpha(0);
            ShowTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(Img.DOFade(1, 0.2f))
                .Join(this.transform.DOPunchScale(Vector3.one * 0.4f * TutorialUI.GetCameraBasedScaleMultiplier(this.transform.position),
                    0.4f, 10, 0))
                .Join(this.transform.DOPunchRotation(new Vector3(0, 0, 30), 0.4f))
                .Append(Img.transform.DOLocalRotate(new Vector3(0, 0, -10), 0.7f).SetEase(Ease.Linear))
                .Insert(0.8f, this.transform.DOScale(0.0001f, 0.3f).SetEase(Ease.InBack))
                .OnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                    this.transform.parent = DefParent;
                });
        }

        #endregion

        #region Public Methods

        internal void SetSize(TutorialUI.MarkSize _size)
        {
            switch (_size)
            {
                case TutorialUI.MarkSize.Normal:
                    setImgSize = Vector3.one;
                    if (awakeDone)
                    { Img.transform.localScale = setImgSize; }
                    break;
                case TutorialUI.MarkSize.Big:
                    setImgSize = Vector3.one * 1.5f;
                    if (awakeDone)
                    { Img.transform.localScale = setImgSize; }
                    break;
                case TutorialUI.MarkSize.Huge:
                    setImgSize = Vector3.one * 2;
                    if (awakeDone)
                    { Img.transform.localScale = setImgSize; }
                    break;
            }
        }

        #endregion
    }
}
