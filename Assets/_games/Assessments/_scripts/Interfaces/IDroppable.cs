using System;
using UnityEngine;

namespace Antura.Assessment
{
    public interface IDroppable
    {
        /// <summary>
        /// Enable input on this GO
        /// </summary>
        void Enable();

        /// <summary>
        /// Disable input on this GO
        /// </summary>
        void Disable();

        /// <summary>
        /// Link to dragManager
        /// </summary>
        void SetDragManager(IDragManager dragManager);

        void StartDrag(Action<IDroppable> onDestroyed);

        void StopDrag();

        void LinkToPlaceholder(PlaceholderBehaviour behaviour);

        PlaceholderBehaviour GetLinkedPlaceholder();

        void Detach(bool jumpBack);

        Transform GetTransform();

        Answer GetAnswer();
    }
}
