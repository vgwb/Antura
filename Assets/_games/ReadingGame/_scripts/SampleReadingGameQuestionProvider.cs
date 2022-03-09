using System.Collections.Generic;
using System.Linq;
using Antura.Helpers;
using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Core;
using Antura.Language;

namespace Antura.Minigames.ReadingGame
{
    public class SampleReadingGameQuestionProvider : IQuestionProvider
    {
        public SampleReadingGameQuestionProvider()
        {

        }

        IQuestionPack IQuestionProvider.GetNextQuestion()
        {
            var answerData = AppManager.I.DB.GetAllWordData().RandomSelectOne();
            LL_WordData randomWord = new LL_WordData(answerData.Id, answerData);

            StringTestData fakeData = new StringTestData(
                 LanguageSwitcher.I.GetHelper(LanguageUse.Learning).ProcessString(
                     "منذ لم نرك منذ مدة " + randomWord.Data.Text + " منذ مدة" +
                      "منذ لم نرك منذ مدة " +
                        "منذ لم نرك منذ مدة "));

            List<ILivingLetterData> wrongAnswers = new List<ILivingLetterData>();
            while (wrongAnswers.Count < 6)
            {
                var randomData = AppManager.I.DB.GetAllWordData().RandomSelectOne();

                if (randomData.Id != answerData.Id && !wrongAnswers.Any((a) => { return a.Id == randomData.Id; }))
                {
                    wrongAnswers.Add(randomData.ConvertToLivingLetterData());
                }
            }

            return new SampleQuestionPack(fakeData, wrongAnswers, new ILivingLetterData[] { randomWord });
        }
    }
}
