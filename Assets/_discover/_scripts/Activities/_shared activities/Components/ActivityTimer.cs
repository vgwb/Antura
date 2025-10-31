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
        [SerializeField] TMP_Text timeTextField;
        [SerializeField] Gradient timerGradient;

        #endregion

        readonly Stopwatch watch = new Stopwatch();
        Coroutine timerCoroutine;

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
            this.RestartCoroutine(ref timerCoroutine, CO_Timer(fromSeconds));
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
            this.CancelCoroutine(ref timerCoroutine);
        }

        #endregion

        #region Methods

        IEnumerator CO_Timer(int seconds)
        {
            int milliseconds = seconds * 1000;
            while (watch.ElapsedMilliseconds < milliseconds)
            {
                bar.fillAmount = watch.ElapsedMilliseconds / (float)milliseconds;
                bar.color = timerGradient.Evaluate(bar.fillAmount);
                UpdateText(Mathf.CeilToInt((milliseconds - watch.ElapsedMilliseconds) * 0.001f));
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
            timeTextField.text = TimeSpan.FromSeconds(currSeconds).ToString(@"mm\:ss");
        }

        #endregion
    }
}
