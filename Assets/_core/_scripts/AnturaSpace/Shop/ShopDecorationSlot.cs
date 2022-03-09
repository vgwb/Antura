using System;
using DG.DeExtensions;
using DG.Tweening;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class ShopDecorationSlot : MonoBehaviour
    {
        public enum SlotHighlight
        {
            Idle,
            Correct,
            Wrong
        }

        private bool assigned = false;
        private ShopDecorationObject _assignedDecorationObject;

        private bool highlighted = false;
        public GameObject highlightMeshGO;

        public event Action<ShopDecorationSlot> OnSelect;

        // @note: set by the Slot Group
        [Header("Auto-generated")]
        public ShopDecorationSlotType slotType;
        public int slotIndex;


        public bool Assigned
        {
            get { return assigned; }
        }

        public ShopDecorationObject AssignedDecorationObject
        {
            get { return _assignedDecorationObject; }
        }


        #region Game Logic

        void Awake()
        {
            Highlight(false);
        }

        #endregion

        #region Assignment

        public void Free()
        {
            if (!assigned)
                return;
            assigned = false;
            _assignedDecorationObject = null;
        }

        public void Assign(ShopDecorationObject assignedDecorationObject)
        {
            if (assigned)
                return;
            assigned = true;
            _assignedDecorationObject = assignedDecorationObject;
            _assignedDecorationObject.transform.SetParent(transform);
            _assignedDecorationObject.transform.localEulerAngles = Vector3.zero;
            _assignedDecorationObject.transform.localPosition = Vector3.zero;
            _assignedDecorationObject.transform.SetLocalScale(1);
            _assignedDecorationObject.gameObject.SetActive(true);
        }

        public bool IsFreeAndAssignableTo(ShopDecorationObject decorationObject)
        {
            return !assigned && IsAssignableTo(decorationObject);
        }

        public bool IsAssignableTo(ShopDecorationObject decorationObject)
        {
            return slotType == decorationObject.slotType;
        }

        public bool HasCurrentlyAssigned(ShopDecorationObject decorationObject)
        {
            return _assignedDecorationObject == decorationObject;
        }

        #endregion


        #region Highlight

        public Material slotHighlightIdleMat;
        public Material slotHighlightCorrectMat;
        public Material slotHighlightWrongMat;

        public void Highlight(bool choice, SlotHighlight slotHighlight = SlotHighlight.Idle)
        {
            highlighted = choice;
            highlightMeshGO.SetActive(choice);
            var renderer = highlightMeshGO.GetComponent<MeshRenderer>();
            switch (slotHighlight)
            {
                case SlotHighlight.Idle:
                    renderer.material = slotHighlightIdleMat;
                    break;
                case SlotHighlight.Correct:
                    renderer.material = slotHighlightCorrectMat;
                    break;
                case SlotHighlight.Wrong:
                    renderer.material = slotHighlightWrongMat;
                    break;
            }
        }

        #endregion

        public void OnMouseUpAsButton()
        {
            if (!highlighted)
                return;
            if (OnSelect != null)
                OnSelect.Invoke(this);
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

        public void FocusHighlight(bool choice)
        {
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
