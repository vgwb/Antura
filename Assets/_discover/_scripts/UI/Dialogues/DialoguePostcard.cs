﻿using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialoguePostcard : MonoBehaviour
    {
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] Image img;

        #endregion
        
        public bool active { get; private set; }
        
        Tween showTween, hideTween;
        
        #region Unity

        void Start()
        {
            RectTransform rt = this.GetComponent<RectTransform>();
            Vector3 defScale = this.transform.localScale;
            Vector3 defRot = this.transform.localEulerAngles;
            Vector2 defAnchoredP = rt.anchoredPosition;
            showTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .AppendInterval(0.2f)
                .Append(this.transform.DOScale(defScale, 0.5f).From(0).SetEase(Ease.OutBack))
                .Join(rt.DOAnchorPos(defAnchoredP, 0.3f).From(defAnchoredP + new Vector2(-180, -660)).SetEase(Ease.OutCubic))
                .Join(this.transform.DOLocalRotate(defRot, 0.5f, RotateMode.FastBeyond360).From(new Vector3(0, 0, 360)).SetEase(Ease.OutBack));
            hideTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Join(this.transform.DOScale(0, 0.5f).From(defScale).SetEase(Ease.InQuad))
                .Join(rt.DOAnchorPos(defAnchoredP + new Vector2(290, 340), 0.5f).From(defAnchoredP).SetEase(Ease.InQuad))
                .Join(this.transform.DOLocalRotate(new Vector3(0, 0, 60), 0.5f).From(defRot).SetEase(Ease.InSine))
                .OnComplete(() => this.gameObject.SetActive(false));
            
            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
            hideTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Show()
        {
            // TODO > Pass image as parameter
            active = true;
            hideTween.Complete();
            showTween.Restart();
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            active = false;
            showTween.Complete();
            hideTween.Restart();
        }

        #endregion
    }
}