﻿using System;
using DG.DeExtensions;
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
        [SerializeField] Ease transparencyCurve = Ease.InSine;
        [Range(0, 1f)]
        [SerializeField] float fullTransparencyMinPerc = 0.1f;
        [DeEmptyAlert]
        [SerializeField] SpriteRenderer bg;
        [DeEmptyAlert]
        [SerializeField] SpriteRenderer ico;

        #endregion
        
        public bool IsShown { get; private set; }
        public OutOfBoundsHor OutHor { get; private set; } 
        public OutOfBoundsVert OutVert { get; private set; } 
        
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
            if (!IsShown) return;
            
            if (target == null)
            {
                Hide();
                return;
            }
            
            trans.position = target.position;
            SetTransparency();
            
            // Check out of bounds
            Vector3 viewportP = CameraManager.I.MainCam.WorldToViewportPoint(trans.position);
            OutHor = viewportP.x < 0 && viewportP.z > 0 || viewportP.x > 1 && viewportP.z < 0
                ? OutOfBoundsHor.Left
                : viewportP.z < 0 || viewportP.x < 0 || viewportP.x > 1
                    ? OutOfBoundsHor.Right
                    : OutOfBoundsHor.None;
            OutVert = viewportP.y > 1
                ? OutOfBoundsVert.Top
                : viewportP.y < 0
                    ? OutOfBoundsVert.Bottom
                    : OutOfBoundsVert.None;
        }

        #endregion
        
        #region Public Methods

        public void Show(Transform newTarget)
        {
            IsShown = true;
            target = newTarget;
            showTween.timeScale = 1f;
            showTween.Restart();
            rotateTween.PlayForward();
            barkTween.PlayForward();
            this.gameObject.SetActive(true);
            SetTransparency();
        }

        public void Hide(bool immediate = false)
        {
            IsShown = false;
            OutHor = OutOfBoundsHor.None;
            OutVert = OutOfBoundsVert.None;
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

        #region Methods

        // Sets transparency based on screen position
        void SetTransparency()
        {
            Vector2 screenP = CameraManager.I.MainCam.WorldToScreenPoint(trans.position);
            Vector2 screenSizeHalf = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            screenP -= screenSizeHalf;
            Vector2 screenPercentOffsetClamped = new(screenP.x / screenSizeHalf.x,  screenP.y / screenSizeHalf.y);
            float dist = screenPercentOffsetClamped.magnitude;
            float alpha = 0;
            if (dist > fullTransparencyMinPerc)
            {
                float val = (Mathf.Clamp01(dist) - fullTransparencyMinPerc) / (1 - fullTransparencyMinPerc);
                alpha = DOVirtual.EasedValue(0, 1, val, transparencyCurve);
            }
            bg.SetAlpha(alpha);
            ico.SetAlpha(alpha);
        }

        #endregion
    }
}