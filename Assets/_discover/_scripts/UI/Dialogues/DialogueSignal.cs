using System;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialogueSignal : MonoBehaviour
    {
        #region Serialized

        [SerializeField] Vector3 offsetFromLLBase = new Vector3(0, 1, 0);
        [SerializeField] bool animateIco;
        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] Transform icon;

        #endregion

        Transform trans;
        Transform llTrans;
        Tween showTween, loopTween;

        #region Unity

        void Awake()
        {
            trans = this.transform;
            
            showTween = this.transform.DOScale(0, 0.35f).From().SetAutoKill(false).Pause()
                .SetEase(Ease.OutCubic)
                .OnRewind(() => {
                    loopTween.Rewind();
                    this.gameObject.SetActive(false);
                    llTrans = null;
                });
            // loopTween = icon.DOLocalRotate(new Vector3(0, 0, 9), 0.75f).From(new Vector3(0, 0, -9)).SetAutoKill(false).Pause()
            loopTween = icon.DOScale(1f, 0.75f).From(Vector3.one * 0.9f).SetAutoKill(false).Pause()
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
         
            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
            loopTween.Kill();
        }

        void Update()
        {
            trans.position = llTrans.position + offsetFromLLBase;
        }

        #endregion

        #region Public Methods

        public void ShowFor(EdLivingLetter ll)
        {
            showTween.Restart();
            if (animateIco) loopTween.Restart();
            this.gameObject.SetActive(true);
            llTrans = ll.transform;
        }

        public void Hide()
        {
            showTween.PlayBackwards();
        }

        #endregion
    }
}