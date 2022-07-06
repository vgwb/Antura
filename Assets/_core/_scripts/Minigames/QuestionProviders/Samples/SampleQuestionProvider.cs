using Antura.Core;
using Antura.Helpers;
using System.Collections.Generic;
using System.Linq;
using Antura.Language;


namespace Antura.LivingLetters.Sample
{
    /// <summary>
    /// Example implementation of IQuestionProvider.
    /// Not to be used in actual production code.
    /// This sample class generates 32 quizzes of type "I give you a word, you say what letters it contains"
    /// </summary>
    public class SampleQuestionProvider : IQuestionProvider
    {
        private List<SampleQuestionPack> questions = new List<SampleQuestionPack>();

        private int currentQuestion;

        public SampleQuestionProvider()
        {
            currentQuestion = -1;

            for (int i = 0; i < 1; i++) {
                var correctAnswers = new List<ILivingLetterData>();
                var wrongAnswers = new List<ILivingLetterData>();

                var newWordData = AppManager.I.Teacher.GetRandomTestWordDataLL();
                //LL_WordData newWordData = AppManager.I.Teacher.GetRandomTestWordDataLL(new WordFilters(requireDiacritics: true));
                //LL_WordData newWordData = new LL_WordData(AppManager.I.DB.GetWordDataById("wolf"));

                if (newWordData == null) {
                    return;
                }

                foreach (var letterData in LanguageSwitcher.I.GetHelper(LanguageUse.Learning).SplitWord(AppManager.I.DB, newWordData.Data)) {
                    correctAnswers.Add(new LL_LetterData(letterData.letter));
                }

                correctAnswers = correctAnswers.Distinct().ToList();

                // At least 4 wrong letters
                while (wrongAnswers.Count < 4) {
                    var letter = AppManager.I.Teacher.GetRandomTestLetterLL();

                    if (!CheckIfContains(correctAnswers, letter) && !CheckIfContains(wrongAnswers, letter)) {
                        wrongAnswers.Add(letter);
                    }
                }

                var currentPack = new SampleQuestionPack(newWordData, wrongAnswers, correctAnswers);
                questions.Add(currentPack);
            }
        }

        static bool CheckIfContains(List<ILivingLetterData> list, ILivingLetterData letter)
        {
            for (int i = 0, count = list.Count; i < count; ++i) {
                if (list[i].Id == letter.Id) {
                    return true;
                }
            }
            return false;
        }

        IQuestionPack IQuestionProvider.GetNextQuestion()
        {
            currentQuestion++;

            if (currentQuestion >= questions.Count) {
                currentQuestion = 0;
            }

            return questions[currentQuestion];
        }
    }
}
