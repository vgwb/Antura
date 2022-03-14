using DG.DeExtensions;
using DG.Tweening;
using UnityEngine;

namespace Antura.Tutorial
{
    public class TutorialUIProp : MonoBehaviour
    {
        public bool IsPooled;

        [System.NonSerialized]
        public SpriteRenderer Img;

        protected Transform DefParent;
        int defSortingOrder;
        protected Tween ShowTween;

        #region Unity

        protected virtual void Awake()
        {
            DefParent = this.transform.parent;
            Img = this.GetComponentInChildren<SpriteRenderer>(true);
            defSortingOrder = Img.sortingOrder;

            Img.SetAlpha(0);
            ShowTween = Img.DOFade(1, 0.2f).SetAutoKill(false).Pause()
                .SetEase(Ease.Linear)
                .OnRewind(() =>
                {
                    this.gameObject.SetActive(false);
                    this.transform.parent = DefParent;
                });

            if (!IsPooled)
                this.gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            this.StopAllCoroutines();
            ShowTween.Kill();
        }

        #endregion

        #region Public Methods

        public virtual void Reset()
        {
            this.StopAllCoroutines();
            ShowTween.Rewind();
        }

        public void Show(Transform _parent, Vector3 _position, bool _overlayed = true)
        {
            Reset();
            this.transform.parent = _parent;
            this.transform.LookAt(transform.position + TutorialUI.I.CamT.rotation * Vector3.forward, TutorialUI.I.CamT.up);
            this.transform.position = _position;
            this.transform.localScale = Vector3.one * TutorialUI.GetCameraBasedScaleMultiplier(_position);
            this.gameObject.SetActive(true);
            Img.sortingOrder = _overlayed ? defSortingOrder : 0;
            ShowTween.PlayForward();
        }

        public void Hide(bool _immediate = false)
        {
            if (_immediate)
            {
                ShowTween.Rewind();
            }
            else
            {
                ShowTween.PlayBackwards();
            }
        }

        #endregion
    }
}
