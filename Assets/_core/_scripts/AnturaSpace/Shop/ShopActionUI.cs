using Antura.AnturaSpace.UI;
using Antura.Audio;
using Antura.Database;
using Antura.Helpers;
using Antura.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Antura.AnturaSpace
{
    public class ShopActionUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public Image iconUI;
        public TextMeshProUGUI amountUI;
        public UIButton buttonUI;
        public RenderedMeshUI renderedMeshUI;

        private ShopAction shopAction;

        public ShopAction ShopAction { get { return shopAction; } }

        public void SetAction(ShopAction shopAction)
        {
            this.shopAction = shopAction;

            if (shopAction.ObjectToRender != null)
            {
                renderedMeshUI.scaleMultiplier = shopAction.scaleMultiplier;
                renderedMeshUI.eulOffset = shopAction.eulOffset;
                renderedMeshUI.AssignObjectToRender(shopAction.ObjectToRender);
                iconUI.enabled = false;
            }
            else
            {
                iconUI.sprite = shopAction.iconSprite;
                iconUI.enabled = true;
            }

            amountUI.text = shopAction.bonesCost.ToString();
            UpdateAction();
        }

        public void UpdateAction()
        {
            if (shopAction != null)
            {
                bool isLocked = shopAction.IsLocked;
                buttonUI.Lock(isLocked);
            }
        }

        public void OnClick()
        {
            if (AnturaSpaceScene.I.TutorialMode
                && AnturaSpaceScene.I.tutorialManager.CurrentTutorialFocus != this)
                return;

            if ((ShopDecorationsManager.I.ShopContext == ShopContext.Purchase || shopAction.CanPurchaseAnywhere)
                && shopAction.IsClickButton)
            {
                if (!shopAction.IsLocked)
                {
                    shopAction.PerformAction();
                }
                else
                {
                    ErrorFeedback();
                }
            }
        }

        private int minHeightForDragAction = 80;
        public ScrollRect scrollRect;

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Push the drag action to the scroll rect too
            if (scrollRect != null)
            {
                scrollRect.OnBeginDrag(eventData);
                scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            errorAlreadyPlayed = false;

            if (scrollRect != null)
            {
                // Push the drag action to the scroll rect too
                scrollRect.OnEndDrag(eventData);
                scrollRect.movementType = ScrollRect.MovementType.Elastic;
            }
        }

        private bool errorAlreadyPlayed = false;
        public void OnDrag(PointerEventData eventData)
        {
            // Push the drag action to the scroll rect too
            if (scrollRect != null)
                scrollRect.OnDrag(eventData);

            if (AnturaSpaceScene.I.TutorialMode
                && AnturaSpaceScene.I.tutorialManager.CurrentTutorialFocus != this)
                return;

            if (ShopDecorationsManager.I.ShopContext == ShopContext.Purchase
                && !shopAction.IsClickButton)
            {
                var mousePos = AnturaSpaceUI.I.ScreenToUIPoint(Input.mousePosition);
                var buttonPos = AnturaSpaceUI.I.WorldToUIPoint(transform.position);
                if (mousePos.y - buttonPos.y > minHeightForDragAction)
                {
                    if (!shopAction.IsLocked)
                    {
                        shopAction.PerformDrag();
                        AudioManager.I.PlaySound(Sfx.OK);
                    }
                    else
                    {
                        ErrorFeedback();
                    }

                }
            }
        }

        void ErrorFeedback()
        {
            if (errorAlreadyPlayed)
                return;
            errorAlreadyPlayed = true;

            AudioManager.I.PlaySound(Sfx.KO);

            // Optionally, play an audio that tells why we cannot buy it anymore
            /*
            if (shopAction.NotEnoughBones)
            {
                AudioManager.I.PlayDialogue(LocalizationDataId.ReservedArea_SectionDescription_Error);
            }
            else
            {
                AudioManager.I.PlayDialogue(shopAction.errorLocalizationID);
            }*/
        }

    }
}
