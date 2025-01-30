using System;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialogueSignal : MonoBehaviour
    {
        #region Serialized

        [SerializeField] bool animateIco;
        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] GameObject balloon_talk, balloon_info_action;
        [DeEmptyAlert]
        [SerializeField] Transform iconContainer;
        [DeEmptyAlert]
        [SerializeField] GameObject ico_talk, ico_info, ico_action;

        #endregion

        Transform trans;
        Transform targetTrans;
        Tween showTween, loopTween;

        #region Unity

        void Awake()
        {
            trans = this.transform;

            showTween = this.transform.DOScale(0, 0.35f).From().SetAutoKill(false).Pause()
                .SetEase(Ease.OutCubic)
                .OnRewind(() =>
                {
                    loopTween.Rewind();
                    this.gameObject.SetActive(false);
                    targetTrans = null;
                });
            loopTween = iconContainer.DOScale(1.1f, 0.45f).From(Vector3.one * 0.9f).SetAutoKill(false).Pause()
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
            trans.position = targetTrans.position;
        }

        #endregion

        #region Public Methods

        public void ShowFor(Interactable interactable)
        {
            SetAppearance(interactable);
            Show(interactable.IconTransform);

        }

        public void Hide()
        {
            showTween.PlayBackwards();
        }

        #endregion

        #region Methods

        void Show(Transform target)
        {
            showTween.Restart();
            if (animateIco) loopTween.Restart();
            this.gameObject.SetActive(true);
            targetTrans = target;
        }

        void SetAppearance(Interactable interactable)
        {
            balloon_talk.gameObject.SetActive(false);
            balloon_info_action.gameObject.SetActive(false);
            ico_talk.gameObject.SetActive(false);
            ico_info.gameObject.SetActive(false);
            ico_action.gameObject.SetActive(false);
            
            switch (interactable.InteractionType)
            {
                case InteractionType.Talk:
                    balloon_talk.gameObject.SetActive(true);
                    ico_talk.gameObject.SetActive(true);
                    break;
                case InteractionType.Look:
                    balloon_info_action.gameObject.SetActive(true);
                    ico_info.gameObject.SetActive(true);
                    break;
                case InteractionType.Use:
                    balloon_info_action.gameObject.SetActive(true);
                    ico_action.gameObject.SetActive(true);
                    break;
            }
        }
        
        #endregion
    }
}
