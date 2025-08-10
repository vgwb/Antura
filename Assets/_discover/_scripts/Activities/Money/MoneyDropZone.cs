using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Discover.Activities
{
    /// <summary>
    /// Base class: a UI area that accepts money tokens.
    /// </summary>
    public abstract class MoneyDropZone : MonoBehaviour, IDropHandler
    {
        public abstract void OnDrop(PointerEventData eventData);

        protected DraggableMoney GetDraggable(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return null;
            return eventData.pointerDrag.GetComponent<DraggableMoney>();
        }
    }
}
