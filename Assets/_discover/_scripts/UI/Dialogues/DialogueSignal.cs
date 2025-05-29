using System;
using Antura.Minigames.DiscoverCountry.Interaction;
using DG.De2D;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialogueSignal : MonoBehaviour
    {
        #region Serialized

        [SerializeField] bool animateIco;
        [SerializeField] Color previewColor = Color.white;
        [DeHeader("References")]
        [DeEmptyAlert]
        [SerializeField] GameObject balloon_talk, balloon_info_action;
        [DeEmptyAlert]
        [SerializeField] Transform iconContainer;
        [DeEmptyAlert]
        [SerializeField] SpriteRenderer ico_talk, ico_info, ico_action;
        [DeEmptyAlert]
        [SerializeField] Sprite ico_talk_alt, ico_info_alt, ico_action_alt;
        [DeEmptyAlert]
        [SerializeField] new Collider2D collider;

        #endregion

        bool wasSetup;
        bool isPreviewSignal;
        Interactable targetInteractable;
        Transform trans;
        Transform targetTrans;
        Tween showTween, loopTween;

        #region Unity

        void Awake()
        {
            trans = this.transform;
            
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

        void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject() || targetInteractable == null) return;
            
            InteractionManager.I.ForceNearbyInteractableTo(targetInteractable);
            DiscoverNotifier.Game.OnActClicked.Dispatch();
        }

        #endregion

        #region Public Methods

        public void Setup(bool asPreviewSignal)
        {
            if (wasSetup)
            {
                Debug.LogError($"DialogueSignal \"{this.name}\" has already been setup", this);
                return;
            }

            isPreviewSignal = asPreviewSignal;

            showTween = this.transform.DOScale(0, 0.35f).From().SetAutoKill(false).Pause()
                .SetEase(isPreviewSignal ? Ease.OutCubic : Ease.OutBounce)
                .OnRewind(() => {
                    if (loopTween != null) loopTween.Rewind();
                    this.gameObject.SetActive(false);
                    targetTrans = null;
                });
            
            if (!isPreviewSignal)
            {
                loopTween = iconContainer.DOScale(1.1f, 0.45f).From(Vector3.one * 0.9f).SetAutoKill(false).Pause()
                    .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
        }
        
        public void ShowFor(Interactable interactable, bool immediate = false)
        {
            targetInteractable = interactable;
            collider.enabled = !isPreviewSignal;
            SetAppearance(interactable);
            Show(interactable.IconTransform, immediate);
        }

        public void Hide(bool immediate = false)
        {
            collider.enabled = false;
            if (immediate) showTween.Rewind();
            else showTween.PlayBackwards();
        }

        #endregion

        #region Methods

        void Show(Transform target, bool immediate = false)
        {
            if (immediate) showTween.Complete();
            else showTween.Restart();
            if (!isPreviewSignal && animateIco) loopTween.Restart();
            this.gameObject.SetActive(true);
            targetTrans = target;
        }

        void SetAppearance(Interactable interactable)
        {
            GameObject balloon = null;
            SpriteRenderer ico = null;
            Sprite altSprite = null;
            balloon_talk.gameObject.SetActive(false);
            balloon_info_action.gameObject.SetActive(false);
            ico_talk.gameObject.SetActive(false);
            ico_info.gameObject.SetActive(false);
            ico_action.gameObject.SetActive(false);
            
            switch (interactable.InteractionType)
            {
                case InteractionType.Talk:
                    balloon = balloon_talk;
                    ico = ico_talk;
                    altSprite = ico_talk_alt;
                    break;
                case InteractionType.Look:
                    balloon = balloon_info_action;
                    ico = ico_info;
                    altSprite = ico_info_alt;
                    break;
                case InteractionType.Use:
                    balloon = balloon_info_action;
                    ico = ico_action;
                    altSprite = ico_action_alt;
                    break;
            }
            
            if (!isPreviewSignal && balloon != null) balloon.SetActive(true);
            if (ico != null)
            {
                if (isPreviewSignal)
                {
                    ico.color = previewColor;
                    if (altSprite != null) ico.sprite = altSprite;
                }
                ico.gameObject.SetActive(true);
            }
        }
        
        #endregion
    }
}
