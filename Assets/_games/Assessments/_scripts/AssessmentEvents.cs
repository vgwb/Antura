using System.Collections;

namespace Antura.Assessment
{
    public class AssessmentEvents
    {
        public delegate IEnumerator CoroutineEvent();

        public CoroutineEvent OnPreQuestionsAnswered = null;

        public CoroutineEvent OnAllQuestionsAnswered = null;

        public CoroutineEvent OnAllQuestionsAnsweredPlacer = null;

        public IEnumerator NoEvent()
        {
            yield return null;
        }
    }
}
