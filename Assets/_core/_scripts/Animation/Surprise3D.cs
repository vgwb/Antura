using UnityEngine;
using DG.Tweening;

namespace Antura.Animation
{
    public class Surprise3D : MonoBehaviour
    {
        private Tweener rotateTweener;
        private Tweener scaleTweener;

        private bool _pulsing;
        public void SetPulsing(bool choice)
        {
            _pulsing = choice;

            transform.localScale = Vector3.one * 0.2f;

            if (rotateTweener != null)
                rotateTweener.Kill();
            if (scaleTweener != null)
                scaleTweener.Kill();

            if (_pulsing)
            {
                rotateTweener = transform.DORotate(Vector3.up * 360, 1f, RotateMode.WorldAxisAdd).SetLoops(-1);
                scaleTweener = transform.DOScale(Vector3.one * 0.3f, 0.5f).SetEase(Ease.OutBounce).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                rotateTweener = transform.DORotate(Vector3.up * 360, 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
            }
        }
    }
}
