using Antura.Audio;
using Antura.Minigames;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// Shows a timer during a minigame.
    /// </summary>
    public class MinigamesUITimer : ABSMinigamesUIComponent
    {
        public Image Radial;
        public TextMeshProUGUI TfTimer;
        public Color EndColor = Color.red;

        public float Duration { get; private set; }
        public float Elapsed { get; private set; }
        Sequence timerTween, shakeTween;
        IAudioSource alarmSfxSource; // Can be shake or alarm sound. NULL when not playing anything
        bool currPauseMenuIsOpenState;
        bool wasPlayingBeforePauseMenuWasOpened;
        Tween endTween;

        #region Unity

        void Awake()
        {
            if (!IsSetup)
            {
                Radial.fillAmount = 0;
                TfTimer.text = "";
            }
        }

        void OnDestroy()
        {
            if (alarmSfxSource != null)
                alarmSfxSource.Stop();
            timerTween.Kill();
            shakeTween.Kill();
            endTween.Kill();
        }

        void Update()
        {
            if (currPauseMenuIsOpenState == PauseMenu.I.IsMenuOpen)
                return;

            currPauseMenuIsOpenState = PauseMenu.I.IsMenuOpen;
            if (currPauseMenuIsOpenState)
            {
                wasPlayingBeforePauseMenuWasOpened = timerTween.IsPlaying();
                Pause();
            }
            else
            {
                if (wasPlayingBeforePauseMenuWasOpened)
                    Play();
                else
                {
                    // Just continue playing the audio
                    if (alarmSfxSource != null)
                        alarmSfxSource.Play();
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the duration of the timer. Call this before calling any other method.
        /// </summary>
        /// <param name="_timerDuration">Timer duration in seconds</param>
        /// <param name="_playImmediately">If TRUE the timer starts immediately, otherwise waits for a <see cref="Play"/> call</param>
        public void Setup(float _timerDuration, bool _playImmediately = false)
        {
            Duration = _timerDuration;
            Elapsed = 0;

            if (IsSetup)
            {
                timerTween.Rewind();
                timerTween.Kill();
                shakeTween.Kill(true);
                endTween.Kill(true);
                if (alarmSfxSource != null)
                {
                    alarmSfxSource.Stop();
                    alarmSfxSource = null;
                }
            }

            TfTimer.text = Duration.ToString();

            // Shake tween
            AudioClip alarmSfx = AudioManager.I.GetSfxAudioClip(Sfx.DangerClockLong);
            float duration = Mathf.Min(Duration, alarmSfx.length);
            shakeTween = DOTween.Sequence().SetAutoKill(false)
                .Append(this.transform.DOShakeRotation(duration, new Vector3(0, 0, 20f), 20))
                .Join(this.transform.DOShakeScale(duration, 0.1f, 20));
            shakeTween.Complete();

            // End tween
            endTween = this.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f).SetAutoKill(false).Pause();

            // Timer tween
            Radial.fillAmount = 0;
            timerTween = DOTween.Sequence().SetAutoKill(false)
                .Append(Radial.DOFillAmount(1, Duration).SetEase(Ease.Linear))
                .Join(Radial.DOColor(EndColor, Duration).SetEase(Ease.Linear))
                .OnUpdate(() =>
                {
                    Elapsed = timerTween.Elapsed();
                    TfTimer.text = Mathf.CeilToInt(Duration - Elapsed).ToString();
                    float elapsed = timerTween.Elapsed();
                    float shakeElapsedTarget = elapsed - (timerTween.Duration() - shakeTween.Duration());
                    if (shakeElapsedTarget > 0)
                    {
                        if (shakeTween.fullPosition <= 0)
                        {
                            // Start alarm sound
                            if (alarmSfxSource != null)
                                alarmSfxSource.Stop();
                            alarmSfxSource = AudioManager.I.PlaySound(alarmSfx);
                            alarmSfxSource.Loop = true;
                        }
                        shakeTween.Goto(shakeTween.Duration() - shakeElapsedTarget);
                    }
                    else if (shakeTween.fullPosition > 0)
                    {
                        shakeTween.Rewind();
                        if (alarmSfxSource != null)
                            alarmSfxSource.Stop();
                    }
                })
                .OnComplete(() =>
                {
                    shakeTween.Rewind();
                    endTween.Restart();
                    if (alarmSfxSource != null)
                    {
                        alarmSfxSource.Stop();
                        alarmSfxSource = null;
                    }
                    AudioManager.I.PlaySound(Sfx.AlarmClock);
                });
            if (!_playImmediately)
                timerTween.Pause();

            timerTween.ForceInit();
            shakeTween.Rewind();
            IsSetup = true;
        }

        /// <summary>Plays the timer</summary>
        public void Play()
        {
            if (!Validate("MinigamesUITimer"))
                return;

            if (alarmSfxSource != null)
                alarmSfxSource.Play();
            timerTween.Play();
        }

        /// <summary>Pauses the timer</summary>
        public void Pause()
        {
            if (!Validate("MinigamesUITimer"))
                return;

            if (alarmSfxSource != null)
                alarmSfxSource.Pause();
            timerTween.Pause();
        }

        /// <summary>Rewinds then restarts the timer</summary>
        public void Restart()
        {
            if (!Validate("MinigamesUITimer"))
                return;

            if (alarmSfxSource != null)
                alarmSfxSource.Stop();
            endTween.Rewind();
            timerTween.Restart();
        }

        /// <summary>Rewinds the timer and pauses it</summary>
        public void Rewind()
        {
            if (!Validate("MinigamesUITimer"))
                return;

            if (alarmSfxSource != null)
            {
                alarmSfxSource.Stop();
                alarmSfxSource = null;
            }
            shakeTween.Rewind();
            endTween.Rewind();
            timerTween.Rewind();
        }

        /// <summary>Completes the timer</summary>
        public void Complete()
        {
            if (!Validate("MinigamesUITimer"))
                return;

            if (!shakeTween.IsComplete() && alarmSfxSource != null)
                alarmSfxSource.Stop();
            if (!timerTween.IsComplete())
                alarmSfxSource = AudioManager.I.PlaySound(Sfx.AlarmClock);
            timerTween.Complete();
        }

        /// <summary>Sends the timer to the given time (in seconds)</summary>
        /// <param name="_time">Time (in seconds) to go to</param>
        /// <param name="_andPlay">If TRUE also plays the timer after going to the given position, otherwise pauses it</param>
        public void Goto(float _time, bool _andPlay = false)
        {
            if (!Validate("MinigamesUITimer"))
                return;
            if (_time > timerTween.Duration())
                _time = timerTween.Duration();
            if (Mathf.Approximately(_time, timerTween.Elapsed()))
                return;

            endTween.Rewind();
            timerTween.Goto(_time, _andPlay);
        }

        /// <summary>Sends the timer to the given time percentage (<code>0 to 1</code>)</summary>
        /// <param name="_percentage">Time percentage (<code>0 to 1</code>) to go to</param>
        /// <param name="_andPlay">If TRUE also plays the timer after going to the given position, otherwise pauses it</param>
        public void GotoPercentage(float _percentage, bool _andPlay = false)
        {
            if (!Validate("MinigamesUITimer"))
                return;
            if (_percentage > 1)
                _percentage = 1;
            if (Mathf.Approximately(_percentage, timerTween.ElapsedPercentage()))
                return;

            endTween.Rewind();
            timerTween.Goto(timerTween.Duration() * _percentage, _andPlay);
        }

        #endregion
    }
}
