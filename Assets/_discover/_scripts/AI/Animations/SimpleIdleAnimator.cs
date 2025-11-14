using DG.Tweening;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Discover
{

    public class SimpleIdleAnimator : MonoBehaviour
    {
        [Header("Idle Animation Settings")]
        [SerializeField] private GameObject targetObject;
        [SerializeField] private Vector3 rotationAmplitude = new Vector3(2f, 0f, 2f); // Degrees: subtle sway (X/Z)
        [SerializeField] private Vector3 positionAmplitude = new Vector3(0.02f, 0f, 0.02f); // Units: gentle X/Z drift
        [SerializeField] private float rotationDuration = 2f; // Rotation cycle length (s)
        [SerializeField] private float positionDuration = 3f; // Position cycle length (s)
        [SerializeField] private float scaleAmount = 0.02f; // Relative scale wiggle
        [SerializeField] private float scaleDuration = 2.5f; // Scale cycle length (s)
        [SerializeField] private float minPauseDuration = 0.5f; // Min random pause (s)
        [SerializeField] private float maxPauseDuration = 2f; // Max random pause (s)

        private Vector3 originalRotation;
        private Vector3 originalPosition;
        private Sequence idleSequence;
        private Transform TargetTransform => targetObject != null ? targetObject.transform : transform;

        private void Start()
        {
            originalRotation = TargetTransform.localEulerAngles;
            originalPosition = TargetTransform.localPosition;

            // Build the looping sequence with random pause
            BuildIdleSequence();
            idleSequence.Restart(); // Kick off immediately
        }

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        public void RebuildIdleSequence()
        {
            TargetTransform.localEulerAngles = originalRotation;
            TargetTransform.localPosition = originalPosition;
            BuildIdleSequence();
            idleSequence.Restart();
        }

        private void BuildIdleSequence()
        {
            idleSequence?.Kill(); // Clean up if rebuilding

            originalRotation = TargetTransform.localEulerAngles;
            originalPosition = TargetTransform.localPosition;
            Vector3 baseScale = TargetTransform.localScale;
            Vector3 rotTarget = originalRotation + rotationAmplitude;
            Vector3 posTarget = originalPosition + positionAmplitude;
            Vector3 scaleTarget = baseScale * (1f + scaleAmount);

            Sequence rotationSequence = DOTween.Sequence()
                .Append(TargetTransform.DOLocalRotate(rotTarget, rotationDuration * 0.5f).SetEase(Ease.InOutSine))
                .Append(TargetTransform.DOLocalRotate(originalRotation, rotationDuration * 0.5f).SetEase(Ease.InOutSine));

            Sequence moveSequence = DOTween.Sequence()
                .Append(TargetTransform.DOLocalMove(posTarget, positionDuration * 0.5f).SetEase(Ease.InOutSine))
                .Append(TargetTransform.DOLocalMove(originalPosition, positionDuration * 0.5f).SetEase(Ease.InOutSine));

            Sequence scaleSequence = DOTween.Sequence()
                .Append(TargetTransform.DOScale(scaleTarget, scaleDuration * 0.5f).SetEase(Ease.InOutSine))
                .Append(TargetTransform.DOScale(baseScale, scaleDuration * 0.5f).SetEase(Ease.InOutSine));

            idleSequence = DOTween.Sequence()
                .SetLoops(-1)
                .SetEase(Ease.Linear);

            idleSequence.Insert(0f, rotationSequence);
            idleSequence.Insert(0f, moveSequence);
            idleSequence.Insert(0f, scaleSequence);

            float randomPause = Random.Range(minPauseDuration, maxPauseDuration);
            idleSequence.AppendInterval(randomPause);
        }

        private void OnValidate()
        {
            // Rebuild sequence on Inspector changes for live preview
            if (Application.isPlaying && idleSequence != null)
            {
                BuildIdleSequence();
                idleSequence.Restart();
            }
        }

        private void OnDestroy()
        {
            idleSequence?.Kill();
        }
    }
}
