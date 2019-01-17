using System.Collections.Generic;

namespace Antura.Assessment
{
    public interface IDragManager : ITimedUpdate
    {
        void ResetRound();
        bool AllAnswered();
        void Enable();

        void DisableInput();
        void EnableInput();

        void AddElements(List<PlaceholderBehaviour> placeholders,
                            List<Answer> answers,
                            List<IQuestion> questions);

        void StartDragging(IDroppable droppable);
        void StopDragging(IDroppable droppable);
        void EnableDragOnly();
        void RemoveDraggables();
        void OnAnswerAdded();
    }
}
