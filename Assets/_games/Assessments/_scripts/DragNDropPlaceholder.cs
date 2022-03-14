namespace Antura.Assessment
{
    public class DragNDropPlaceholder : IPlaceholder
    {
        IQuestion question = null;
        public IQuestion GetQuestion()
        {
            return question;
        }

        // Question not setted
        public void SetQuestion(IQuestion question)
        {
            this.question = question;
        }
    }
}
