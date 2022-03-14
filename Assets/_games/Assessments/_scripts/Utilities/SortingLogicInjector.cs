using System;

namespace Antura.Assessment
{
    internal class SortingLogicInjector : DefaultLogicInjector
    {
        public SortingLogicInjector(IDragManager dragManager, AssessmentEvents events) : base(dragManager, events)
        {

        }

        protected override void WireQuestion(IQuestion q, AnswerSet answerSet)
        {

        }

        protected override void WireAnswers(Answer[] answers)
        {
            if (answers == null || answers.Length == 0)
            {
                throw new ArgumentException("What am I supposed to sort without any correct answer?");
            }
            foreach (var a in answers)
            {
                var behaviour = a.gameObject.GetComponent<Answer>();
                answersList.Add(behaviour); // TODO: INVESTIGATE WITHIN DRAG MAANGER
            }
        }

        protected override void WirePlaceHolders(IQuestion question)
        {

        }

        protected override void OnAnswersAdded()
        {
            dragManager.OnAnswerAdded();
        }
    }
}
