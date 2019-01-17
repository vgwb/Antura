using System;
using System.Collections.Generic;

namespace Antura.LivingLetters.Sample
{
    /// <summary>
    /// Example implementation of IQuestionPack.
    /// Not to be used in actual production code.
    /// </summary>
    class SampleQuestionPack : IQuestionPack
    {
        ILivingLetterData questionSentence;
        IEnumerable<ILivingLetterData> wrongAnswersSentence;
        IEnumerable<ILivingLetterData> correctAnswersSentence;

        public SampleQuestionPack(ILivingLetterData questionSentence, IEnumerable<ILivingLetterData> wrongAnswersSentence,
            IEnumerable<ILivingLetterData> correctAnswersSentence)
        {
            this.questionSentence = questionSentence;
            this.wrongAnswersSentence = wrongAnswersSentence;
            this.correctAnswersSentence = correctAnswersSentence;
        }

        ILivingLetterData IQuestionPack.GetQuestion()
        {
            return questionSentence;
        }

        IEnumerable<ILivingLetterData> IQuestionPack.GetWrongAnswers()
        {
            return wrongAnswersSentence;
        }

        public IEnumerable<ILivingLetterData> GetCorrectAnswers()
        {
            return correctAnswersSentence;
        }

        public IEnumerable<ILivingLetterData> GetQuestions()
        {
            throw new Exception("This provider can not use this method");
        }
    }
}