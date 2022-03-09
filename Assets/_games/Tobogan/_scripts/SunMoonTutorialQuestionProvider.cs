using System.Linq;
using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Core;

namespace Antura.Minigames.Tobogan
{
    public class SunMoonTutorialQuestionProvider : IQuestionProvider
    {
        IQuestionProvider provider;

        IQuestionPack sunQuestion;
        IQuestionPack moonQuestion;
        int questionsDone = 0;

        public SunMoonTutorialQuestionProvider(IQuestionProvider provider)
        {
            this.provider = provider;

            var db = AppManager.I.DB;
            var sunWord = db.GetWordDataById("the_sun");
            var sunData = new LL_ImageData(sunWord.Id, sunWord);
            var moonWord = db.GetWordDataById("the_moon");
            var moonData = new LL_ImageData(moonWord.Id, moonWord);

            sunQuestion = new SampleQuestionPack(new LL_WordData(sunWord.Id, sunWord), new ILivingLetterData[] { moonData }, new ILivingLetterData[] { sunData });
            moonQuestion = new SampleQuestionPack(new LL_WordData(moonWord.Id, moonWord), new ILivingLetterData[] { sunData }, new ILivingLetterData[] { moonData });
        }

        IQuestionPack IQuestionProvider.GetNextQuestion()
        {
            questionsDone++;

            if (questionsDone == 1)
                return sunQuestion;
            else if (questionsDone == 2)
                return moonQuestion;
            else
            {
                var data = provider.GetNextQuestion();

                var correct = data.GetCorrectAnswers();
                var wrong = data.GetWrongAnswers();

                var correctImages = correct.ToArray();
                var wrongImages = wrong.ToArray();

                for (int i = 0; i < correctImages.Length; ++i)
                    correctImages[i] = new LL_ImageData(correctImages[i].Id);

                for (int i = 0; i < wrongImages.Length; ++i)
                    wrongImages[i] = new LL_ImageData(wrongImages[i].Id);

                return new SampleQuestionPack(data.GetQuestion(), wrongImages, correctImages);
            }
        }
    }
}
