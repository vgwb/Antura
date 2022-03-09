namespace Antura.Assessment
{
    public interface IPlaceholder
    {
        void SetQuestion(IQuestion question);

        IQuestion GetQuestion();
    }
}
