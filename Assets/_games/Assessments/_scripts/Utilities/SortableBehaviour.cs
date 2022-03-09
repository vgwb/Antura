using DG.Tweening;
using System;
using Antura.Core;
using Antura.UI;
using UnityEngine;

namespace Antura.Assessment
{
    internal class SortableBehaviour : MonoBehaviour, IDroppable
    {
        IDragManager dragManager = null;
        Tween tween = null;

        public void SetDragManager(IDragManager dragManager)
        {
            this.dragManager = dragManager;
        }

        public Answer GetAnswer()
        {
            return GetComponent<Answer>();
        }

        void OnMouseDown()
        {
            if (GlobalUI.PauseMenu.IsMenuOpen)
                return;
            if (!dragEnabled)
            {
                return;
            }

            dragManager.StartDragging(this);
            SetScale(1.3f);
        }

        void SetScale(float scale)
        {
            if (tween != null)
            {
                tween.Kill(false);
            }
            tween = transform.DOScale(scale, 0.4f).OnComplete(() => tween = null);
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

        Action<IDroppable> OnGoDestroyed = null;

        public void StartDrag(Action<IDroppable> onDestroyed)
        {
            OnGoDestroyed = onDestroyed;
            SetScale(1.3f);
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
            SetScale(1f);
            var v = transform.localPosition;
            transform.localPosition = new Vector3(v.x, v.y, 5);
        }

        private int index = -1;
        public bool SetSortIndex(int a)
        {
            if (index != a)
            {
                index = a;
                return true;
            }
            return false;
        }

        public void LinkToPlaceholder(PlaceholderBehaviour behaviour)
        {

        }

        public void Detach(bool jumpBack = true)
        {

        }

        public Transform GetTransform()
        {
            return transform;
        }

        public PlaceholderBehaviour GetLinkedPlaceholder()
        {
            return null;
        }

        private Tween tweenMove = null;
        internal void Move(Vector3 position, float v)
        {
            if (tweenMove != null && tweenMove.IsComplete() == false)
            {
                tweenMove.Kill(false);
            }

            tweenMove = transform.DOLocalMove(position, v);
        }
    }
}
