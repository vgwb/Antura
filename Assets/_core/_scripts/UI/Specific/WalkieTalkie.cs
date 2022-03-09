using Antura.Audio;
using DG.Tweening;
using UnityEngine;

namespace Antura.UI
{
    /// <summary>
    /// Shows the WalkieTalkie that the Keeper uses to communicate with the player.
    /// </summary>
    public class WalkieTalkie : MonoBehaviour
    {
        public bool IsShown { get; private set; }
        private bool shouldPulse;
        private Tween showTween, pulseTween;

        public void Setup()
        {
            const float pulseShakeDuration = 0.5f;
            pulseTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(this.transform.DOPunchRotation(new Vector3(0, 0, 20), pulseShakeDuration))
                .Append(this.transform.DOScale(this.transform.localScale * 1.1f, 0.3f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo)
                    .SetAutoKill(false).Pause()
                    .OnComplete(() =>
                    {
                        if (shouldPulse)
                            pulseTween.Goto(pulseShakeDuration, true);
                    })
                );
            pulseTween.ForceInit();

            showTween = this.transform.DOScale(0.0001f, 0.45f).From().SetEase(Ease.OutBack).SetAutoKill(false).Pause()
                .OnRewind(() => this.gameObject.SetActive(false))
                .OnComplete(() => pulseTween.Goto(pulseShakeDuration, true));

            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
            pulseTween.Kill();
        }

        public void Show(bool _doShow, bool _immediate = false)
        {
            IsShown = _doShow;
            if (_doShow)
            {
                this.gameObject.SetActive(true);
                AudioManager.I.PlaySound(Sfx.WalkieTalkie);
                StopPulse(true);
                if (_immediate)
                {
                    showTween.Complete();
                }
                else
                {
                    showTween.PlayForward();
                }
            }
            else
            {
                StopPulse(true);
                if (_immediate)
                {
                    showTween.Rewind();
                }
                else
                {
                    showTween.PlayBackwards();
                }
            }
        }

        public void Pulse()
        {
            shouldPulse = true;
            pulseTween.PlayForward();
        }

        public void StopPulse(bool _immediate = false)
        {
            shouldPulse = false;
            if (_immediate)
            { pulseTween.Rewind(); }
        }
    }
}
