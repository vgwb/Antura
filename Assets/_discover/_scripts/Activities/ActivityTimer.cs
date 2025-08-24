using System;
using System.Collections;
using System.Diagnostics;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Antura.Discover.Activities
{
    public class ActivityTimer : MonoBehaviour
    {
        #region EVENTS

        public ActionEvent OnTimerElapsed = new("ActivityTimer.OnTimerElapsed");

        #endregion

        #region Serialized

        [DeEmptyAlert]
        [SerializeField] Image bar;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfTime;
        [SerializeField] Gradient timerGradient;

        #endregion

        readonly Stopwatch watch = new Stopwatch();
        Coroutine coTimer;

        #region Unity

        void OnDestroy()
        {
            this.StopAllCoroutines();
        }

        #endregion

        #region Public Methods

        public void RestartTimer(int fromSeconds)
        {
            bar.fillAmount = 0;
            watch.Restart();
            this.RestartCoroutine(ref coTimer, CO_Timer(fromSeconds));
        }

        public void PauseTimer()
        {
            watch.Stop();
        }

        public void ResumeTimer()
        {
            watch.Start();
        }

        public void CancelTimer()
        {
            watch.Stop();
            this.CancelCoroutine(ref coTimer);
        }

        #endregion

        #region Methods

        IEnumerator CO_Timer(int seconds)
        {
            int ms = seconds * 1000;
            while (watch.ElapsedMilliseconds < ms)
            {
                bar.fillAmount = watch.ElapsedMilliseconds / (float)ms;
                bar.color = timerGradient.Evaluate(bar.fillAmount);
                UpdateText(Mathf.CeilToInt((ms - watch.ElapsedMilliseconds) * 0.001f));
                yield return null;
            }

            watch.Stop();
            bar.fillAmount = 1;
            bar.color = timerGradient.Evaluate(1);
            UpdateText(0);
            OnTimerElapsed.Dispatch();
        }

        void UpdateText(int currSeconds)
        {
            tfTime.text = TimeSpan.FromSeconds(currSeconds).ToString(@"mm\:ss");
        }

        #endregion
    }
}
