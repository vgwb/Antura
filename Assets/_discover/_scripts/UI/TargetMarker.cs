using System;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class TargetMarker : MonoBehaviour
    {
        #region Serialized

        [Range(1, 10)]
        [SerializeField] float bgRotationDuration = 1;
        [Range(1, 30)]
        [SerializeField] int intervalBetweenBarks = 6;
        [DeEmptyAlert]
        [SerializeField] SpriteRenderer bg;
        [DeEmptyAlert]
        [SerializeField] SpriteRenderer ico;

        #endregion
        
        bool showing;
        Transform trans;
        Transform target;
        Tween showTween, rotateTween, barkTween;

        #region Unity

        void Start()
        {
            trans = this.transform;
            
            showTween = trans.DOScale(1, 0.45f).From(0).SetAutoKill(false).Pause()
                .SetDelay(0.3f)
                .SetEase(Ease.Linear)
                .OnRewind(() => {
                    this.gameObject.SetActive(false);
                    rotateTween.Rewind();
                    barkTween.Rewind();
                });

            rotateTween = bg.transform.DOLocalRotate(new Vector3(0, 0, 360), bgRotationDuration, RotateMode.FastBeyond360).SetAutoKill(false).Pause()
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);

            float barkDuration = 0.5f;
            barkTween = DOTween.Sequence().SetAutoKill(false).Pause().SetLoops(-1, LoopType.Restart)
                .Join(ico.transform.DOPunchScale(Vector3.one * 0.1f, barkDuration))
                .Join(ico.transform.DOPunchRotation(new Vector3(0, 0, -16), barkDuration, 8))
                .Join(ico.transform.DOLocalMove(new Vector3(-0.05f, 0.05f, 0), barkDuration * 0.5f).SetRelative().SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine))
                .AppendInterval(intervalBetweenBarks);

            Hide(true);
        }
        
        void OnDestroy()
        {
            showTween.Kill();
            rotateTween.Kill();
            barkTween.Kill();
        }

        void Update()
        {
            if (!showing) return;
            if (target == null)
            {
                Hide();
                return;
            }
            trans.position = target.position;
        }

        #endregion
        
        #region Public Methods

        public void Show(Transform newTarget)
        {
            showing = true;
            target = newTarget;
            showTween.timeScale = 1f;
            showTween.Restart();
            rotateTween.PlayForward();
            barkTween.PlayForward();
            this.gameObject.SetActive(true);
        }

        public void Hide(bool immediate = false)
        {
            showing = false;
            if (immediate)
            {
                showTween.Rewind();
                this.gameObject.SetActive(false);
            }
            else
            {
                showTween.timeScale = 2.5f;
                showTween.PlayBackwards();
            }
        }

        #endregion
    }
}