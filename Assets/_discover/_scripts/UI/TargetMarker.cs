using System;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class TargetMarker : MonoBehaviour
    {
        bool showing;
        Transform trans;
        Transform target;
        Tween showTween;

        #region Unity

        void Start()
        {
            trans = this.transform;
            
            showTween = this.GetComponentInChildren<SpriteRenderer>().DOFade(1, 0.45f).From(0).SetAutoKill(false).Pause()
                .SetDelay(0.3f)
                .SetEase(Ease.Linear)
                .OnRewind(() => this.gameObject.SetActive(false));

            Hide(true);
        }
        
        void OnDestroy()
        {
            showTween.Kill();
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