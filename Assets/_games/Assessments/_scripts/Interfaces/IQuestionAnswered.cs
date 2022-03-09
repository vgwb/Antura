namespace Antura.Assessment
{
    public interface IQuestionDecoration
    {
        void TriggerOnAnswered();

        float TimeToWait();

        void TriggerOnSpawned();
    }
}
