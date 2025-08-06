using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using Antura.Audio;
using Antura.Core;

namespace Antura.Discover
{
    public class ProgressDisplay : MonoBehaviour
    {
        [Header("References")]
        public TextMeshProUGUI TfCount;
        public RectTransform BoneImg;

        [SerializeField, ReadOnly] int total_progress;
        [SerializeField, ReadOnly] int currentProgress;

        int progressValue
        {
            get { return currentProgress; }
            set
            {
                currentProgress = value;
                TfCount.text = value + "/" + total_progress;
            }
        }

        bool setupDone;
        Tween showTween, increaseTween;

        #region Unity

        void Start()
        {
            Setup();

        }

        void Setup()
        {
            if (setupDone)
            { return; }

            setupDone = true;

            SetValue(0);
            showTween = this.transform.DOScale(0.001f, 0.35f).From().SetEase(Ease.OutBack).SetAutoKill(false).Pause()
                .OnRewind(() => this.gameObject.SetActive(false));
            showTween.Complete();
            increaseTween = BoneImg.transform.DOPunchScale(Vector3.one * 0.15f, 0.35f).SetAutoKill(false).Pause();
        }

        void OnDestroy()
        {
            showTween.Kill();
            increaseTween.Kill();
        }

        #endregion

        #region Public Methods

        public void UpdateProgress(int _counter, int _maxSteps)
        {
            total_progress = _maxSteps;
            progressValue = _counter;
            //           Debug.Log("UpdateProgress " + progressValue + " " + maxSteps);
            //           TfCount.text = CalculateCompletenessPercentage(progressValue, maxSteps) + "c";
        }

        public void Show(bool _setValueAuto = true)
        {
            Setup();
            this.gameObject.SetActive(true);
            showTween.PlayForward();
        }

        public void Hide()
        {
            Setup();
            if (increaseTween != null)
            { increaseTween.Complete(); }
            showTween.Rewind();
        }

        public void SetMax(int _bones)
        {
            total_progress = _bones;
        }
        public void SetValue(int _bones)
        {
            progressValue = _bones;
        }

        public void DecreaseBy(int _by)
        {
            progressValue -= _by;
        }

        public void IncreaseByOne(bool _animate = true)
        {
            increaseTween.Restart();
            AudioManager.I.PlaySound(Sfx.Blip);
            progressValue++;
        }

        public int CalculateCompletenessPercentage(int completedSteps, int totalSteps)
        {
            //Debug.Log("CalculateCompletenessPercentage " + completedSteps + " " + totalSteps + " : " + (int)((float)completedSteps / totalSteps * 100));
            if (totalSteps == 0)
            {
                return 0;
            }
            return (int)((float)completedSteps / totalSteps * 100);
        }
        #endregion
    }
}
