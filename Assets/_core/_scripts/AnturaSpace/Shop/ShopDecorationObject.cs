using Antura.AnturaSpace.UI;
using DG.DeExtensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.AnturaSpace
{
    public class ShopDecorationObject : MonoBehaviour
    {
        public ShopDecorationSlotType slotType;
        public string id;

        private ShopActionUI shopActionUI;
        public RawImage RawImage
        {
            get
            {
                if (shopActionUI == null)
                    shopActionUI = AnturaSpaceUI.I.ShopPanelUI.GetActionUIByName("ShopAction_Decoration_" + id);
                return shopActionUI.renderedMeshUI.GetRawImage();
            }
        }

        public void OnMouseDown()
        {
            //Debug.Log("SHOP CONTEXT: " + ShopDecorationsManager.I.ShopContext);
            if (ShopDecorationsManager.I.ShopContext == ShopContext.Purchase)
            {
                ShopDecorationsManager.I.StartDragPlacement(this, false);
            }
        }

        #region Feedback

        private ShopSlotFeedback feedback;

        public void Initialise(GameObject slotFeedbackPrefabGo)
        {
            var feedbackGo = Instantiate(slotFeedbackPrefabGo);
            feedbackGo.transform.SetParent(transform);
            feedbackGo.transform.SetLocalScale(1);
            feedbackGo.transform.localPosition = Vector3.zero;
            feedbackGo.transform.localEulerAngles = Vector3.zero;
            feedback = feedbackGo.GetComponent<ShopSlotFeedback>();
        }

        private Tween pulseTween;

        private void SetAsPreview()
        {
            pulseTween = transform.DOScale(Vector3.one * 1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
        }

        private void SetAsReal()
        {
            pulseTween.Kill(true);
            transform.SetLocalScale(1);
        }

        public void FocusHighlight(bool choice)
        {
            if (choice)
                SetAsPreview();
            else
                SetAsReal();

            feedback.FocusHighlight(choice);
        }

        public void Spawn()
        {
            feedback.Spawn();
        }

        public void Despawn()
        {
            feedback.Despawn();
        }

        #endregion

    }
}
