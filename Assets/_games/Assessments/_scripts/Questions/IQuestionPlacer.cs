using Kore.Coroutines;

namespace Antura.Assessment
{
    public interface IQuestionPlacer
    {
        void Place(IQuestion[] question, bool playQuestionSound);
        bool IsAnimating();
        void RemoveQuestions();
        IYieldable PlayQuestionSound();
    }
}
