using DG.Tweening;
using System;
using Antura.Book;
using Antura.Core;
using Antura.UI;
using UnityEngine;

namespace Antura.Assessment
{
    public class DroppableBehaviour : MonoBehaviour, IDroppable
    {
        IDragManager dragManager = null;

        public void SetDragManager(IDragManager dragManager)
        {
            this.dragManager = dragManager;
        }

        public Answer GetAnswer()
        {
            return GetComponent<Answer>();
        }

        Vector3 origin; // Memorize starting position for going back
        void OnMouseDown()
        {
            if (GlobalUI.PauseMenu.IsMenuOpen)
                return;
            if (!dragEnabled)
            {
                return;
            }

            // If I place an LL above another one, then the other one should fall down
            // So when I click a LL that is linked I keep its original position
            if (GetLinkedPlaceholder() == null)
            {
                origin = transform.localPosition;
            }

            dragManager.StartDragging(this);
            GetComponent<StillLetterBox>().Grabbed();
        }

        void OnMouseUp()
        {
            if (!dragEnabled)
            {
                return;
            }

            dragManager.StopDragging(this);
        }

        bool dragEnabled = false;
        public void Disable()
        {
            dragEnabled = false;
        }

        public void Enable()
        {
            dragEnabled = true;
        }

        bool canUpdate = false;
        private void Update()
        {
            if (canUpdate)
            {
                var pos = transform.localPosition;
                pos.z = Z;
                transform.localPosition = pos;
            }
        }

        Action<IDroppable> OnGoDestroyed = null;
        public void StartDrag(Action<IDroppable> onDestroyed)
        {
            canUpdate = true;
            OnGoDestroyed = onDestroyed;
            GetComponent<StillLetterBox>().Grabbed();
        }

        void OnDestroy()
        {
            dragEnabled = false;
            if (OnGoDestroyed != null)
            {
                OnGoDestroyed(this);
            }
        }

        public void StopDrag()
        {
            OnGoDestroyed = null;
            GetComponent<StillLetterBox>().TweenScale(1);
            var v = transform.localPosition;
            transform.localPosition = new Vector3(v.x, v.y, Z);
        }

        private float Z = 5f;
        public void SetZ(float Z)
        {
            this.Z = Z;
        }

        public float GetZ()
        {
            return Z;
        }

        PlaceholderBehaviour linkedBehaviour = null;
        public void LinkToPlaceholder(PlaceholderBehaviour behaviour)
        {
            linkedBehaviour = behaviour;
            if (behaviour.LinkedDroppable != null)
            {
                behaviour.LinkedDroppable.Detach();
            }

            // Link this answer to placeholder
            transform.localPosition = behaviour.transform.localPosition;
            linkedBehaviour = behaviour;
            behaviour.LinkedDroppable = this;
        }

        public void Detach(bool jumpBack = true)
        {
            if (jumpBack)
            {
                transform.DOLocalMove(origin, 0.5f).SetEase(Ease.OutBounce);
            }

            if (linkedBehaviour != null)
            {
                var quest = linkedBehaviour.Placeholder.GetQuestion();
                quest.GetAnswerSet().OnRemovedAnswer(GetAnswer());
                linkedBehaviour.LinkedDroppable = null;
            }

            linkedBehaviour = null;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public PlaceholderBehaviour GetLinkedPlaceholder()
        {
            return linkedBehaviour;
        }
    }
}
