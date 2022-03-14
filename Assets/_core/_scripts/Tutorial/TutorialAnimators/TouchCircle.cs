using DG.Tweening;
using UnityEngine;

namespace Antura.TutorialAnimators
{
    public class TouchCircle : MonoBehaviour
    {
        [Header("Options")]
        public float ScaleMultiplier = 1.2f;

        public float PulseDuration = 0.5f;

        Tween pulseTween;

        void Start()
        {
            pulseTween = this.transform.DOScale(this.transform.localScale * ScaleMultiplier, PulseDuration).SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        void OnDestroy()
        {
            pulseTween.Kill();
        }
    }
}
