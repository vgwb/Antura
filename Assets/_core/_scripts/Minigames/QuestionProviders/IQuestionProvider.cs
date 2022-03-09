namespace Antura.LivingLetters
{
    /// <summary>
    /// The question provider takes the responsibility to provide a set of questions to its user;
    /// And a description that could describe the questions set (e.g. the string ID for "Find the correct letter used in the word" in arabic).
    /// </summary>
    public interface IQuestionProvider
    {
        /// <summary>
        /// Provide me another question
        /// </summary>
        IQuestionPack GetNextQuestion();
    }
}
