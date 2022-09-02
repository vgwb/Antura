using Antura.Helpers;
using Antura.LivingLetters;
using Antura.Core;
using System.Collections.Generic;
using Antura.Language;

namespace Antura.Minigames.MakeFriends
{
    /// <summary>
    /// This sample class generates 10 quizzes of type "I give you 2 words, you find the common letter(s)"
    /// </summary>
    public class MakeFriendsQuestionProvider : IQuestionProvider
    {
        private List<MakeFriendsQuestionPack> questions = new List<MakeFriendsQuestionPack>();
        private int currentQuestion;
        private readonly int quizzesCount = 10;

        public MakeFriendsQuestionProvider()
        {
            currentQuestion = -1;

            List<ILivingLetterData> correctAnswers;
            List<ILivingLetterData> wrongAnswers;
            LL_WordData newWordData1;
            LL_WordData newWordData2;
            var wordLetters1 = new List<ILivingLetterData>();
            var wordLetters2 = new List<ILivingLetterData>();
            var commonLetters = new List<ILivingLetterData>();
            var uncommonLetters = new List<ILivingLetterData>();

            for (int iteration = 0; iteration < quizzesCount; iteration++)
            {
                // Get 2 words with at least 1 common letter
                int outerLoopAttempts = 50;
                do
                {
                    newWordData1 = null;
                    newWordData2 = null;
                    wordLetters1.Clear();
                    wordLetters2.Clear();
                    commonLetters.Clear();
                    uncommonLetters.Clear();

                    newWordData1 = AppManager.I.Teacher.GetRandomTestWordDataLL();
                    foreach (var letterData in LanguageSwitcher.I.GetHelper(LanguageUse.Learning).SplitWord(AppManager.I.DB, newWordData1.Data))
                    {
                        wordLetters1.Add(new LL_LetterData(letterData.letter));
                    }

                    int innerLoopAttempts = 50;
                    do
                    {
                        newWordData2 = AppManager.I.Teacher.GetRandomTestWordDataLL();
                        innerLoopAttempts--;
                    } while (newWordData2.Id == newWordData1.Id && innerLoopAttempts > 0);

                    if (innerLoopAttempts <= 0)
                    {
                        UnityEngine.Debug.LogError("MakeFriends QuestionProvider Could not find 2 different words!");
                    }

                    foreach (var letterData in LanguageSwitcher.I.GetHelper(LanguageUse.Learning).SplitWord(AppManager.I.DB, newWordData2.Data))
                    {
                        wordLetters2.Add(new LL_LetterData(letterData.letter));
                    }

                    // Find common letter(s) (without repetition)
                    for (int i = 0; i < wordLetters1.Count; i++)
                    {
                        var letter = wordLetters1[i];

                        //if (wordLetters2.Contains(letter))
                        if (wordLetters2.Exists(x => x.Id == letter.Id))
                        {
                            //if (!commonLetters.Contains(letter))
                            if (!commonLetters.Exists(x => x.Id == letter.Id))
                            {
                                commonLetters.Add(letter);
                            }
                        }
                    }

                    // Find uncommon letters (without repetition)
                    for (int i = 0; i < wordLetters1.Count; i++)
                    {
                        var letter = wordLetters1[i];

                        //if (!wordLetters2.Contains(letter))
                        if (!wordLetters2.Exists(x => x.Id == letter.Id))
                        {
                            //if (!uncommonLetters.Contains(letter))
                            if (!uncommonLetters.Exists(x => x.Id == letter.Id))
                            {
                                uncommonLetters.Add(letter);
                            }
                        }
                    }

                    for (int i = 0; i < wordLetters2.Count; i++)
                    {
                        var letter = wordLetters2[i];

                        if (!wordLetters1.Contains(letter))
                        {
                            if (!uncommonLetters.Contains(letter))
                            {
                                uncommonLetters.Add(letter);
                            }
                        }
                    }
                    outerLoopAttempts--;
                } while (commonLetters.Count == 0 && outerLoopAttempts > 0);

                if (outerLoopAttempts <= 0)
                {
                    UnityEngine.Debug.LogError("MakeFriends QuestionProvider Could not find enough data for QuestionPack #" + iteration
                        + "\nInfo: Word1: " + ArabicFixer.Fix(newWordData1.TextForLivingLetter) + " Word2: " + ArabicFixer.Fix(newWordData2.TextForLivingLetter)
                    + "\nWordLetters1: " + wordLetters1.Count + " WordLetters2: " + wordLetters2.Count
                    + "\nCommonLetters: " + commonLetters.Count + " UncommonLetters: " + uncommonLetters.Count);
                }

                commonLetters.Shuffle();
                uncommonLetters.Shuffle();

                correctAnswers = new List<ILivingLetterData>(commonLetters);
                wrongAnswers = new List<ILivingLetterData>(uncommonLetters);

                var currentPack = new MakeFriendsQuestionPack(newWordData1, newWordData2, wrongAnswers, correctAnswers);
                questions.Add(currentPack);
            }
        }

        IQuestionPack IQuestionProvider.GetNextQuestion()
        {
            currentQuestion++;

            if (currentQuestion >= questions.Count)
            {
                currentQuestion = 0;
            }

            return questions[currentQuestion];
        }
    }
}

