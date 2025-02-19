using Antura.Minigames.DiscoverCountry.Interaction;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public abstract class AbstractMapIcon : MonoBehaviour
    {
        public abstract bool IsEnabled { get; }

        protected Transform followTarget;
        float defY;
        Tween showTween;
        
        #region Unity

        void Awake()
        {
            defY = this.transform.position.y;
            
            showTween = this.GetComponentInChildren<SpriteRenderer>().DOFade(1, 0.45f).From(0).SetAutoKill(false).Pause()
                .SetDelay(0.3f)
                .SetEase(Ease.Linear);
        }

        void Start()
        {
            Hide(true);
        }

        void OnDestroy()
        {
            showTween.Kill();
        }
        
        public void UpdatePosition()
        {
            Vector3 position = followTarget != null ? followTarget.position : GetPosition();
            position.y = defY;
            this.transform.position = position;
        }

        #endregion
        
        #region Public Methods

        public void Show()
        {
            if (IsEnabled)
            {
                showTween.timeScale = 1f;
                showTween.Restart();
                this.gameObject.SetActive(true);
            }
            else
            {
                Hide(true);
            }
        }

        public void Hide(bool immediate = false)
        {
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

        public void AssignFollowTarget(Transform target)
        {
            followTarget = target;
        }
        
        #endregion

        #region Methods

        // Only used if followTarget is NULL
        protected abstract Vector3 GetPosition();

        #endregion
    }
}