using Antura.Core;
using Antura.Helpers;
using Antura.LivingLetters;
using DG.Tweening;
using Kore.Coroutines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Database;
using UnityEngine;
using Antura.Language;

namespace Antura.Assessment
{
    /// <summary>
    /// Question Generator for asessments that show Image
    /// </summary>
    public class ImageQuestionGenerator : IQuestionGenerator
    {

        private IQuestionProvider provider;
        private QuestionGeneratorState state;
        private IQuestionPack currentPack;

        private bool missingLetter;

        public ImageQuestionGenerator(IQuestionProvider provider, bool missingLetter,
                                        AssessmentAudioManager audioManager,
                                        AssessmentEvents events)
        {
            this.provider = provider;
            this.missingLetter = missingLetter;
            this.audioManager = audioManager;

            if (AssessmentOptions.Instance.CompleteWordOnAnswered)
            {
                events.OnAllQuestionsAnswered = CompleteWordCoroutine;
            }

            if (AssessmentOptions.Instance.ShowFullWordOnAnswered)
            {
                events.OnAllQuestionsAnswered = ShowFullWordCoroutine;
            }

            state = QuestionGeneratorState.Uninitialized;
            ClearCache();
        }

        private IEnumerator ShowFullWordCoroutine()
        {
            var position = new Vector3(0, 1.5f, 5f);
            var LL = ItemFactory.Instance.SpawnQuestion(cacheFullWordData);
            var box = ItemFactory.Instance.SpawnQuestionBox(new StillLetterBox[] { LL });
            box.Show();
            LL.transform.localPosition = position;
            position.z += 1;
            box.transform.localPosition = position;
            LL.InstaShrink();
            LL.Poof();
            audioManager.PlayPoofSound();
            LL.Magnify();
            LL.SetQuestionGreen();

            // HACK: make the image green too
            var questions = GameObject.FindObjectsOfType<QuestionBehaviour>();
            foreach (var questionBehaviour in questions)
            {
                questionBehaviour.GreenyTintQuestion();
            }

            yield return Wait.For(AssessmentOptions.Instance.TimeToShowCompleteWord + 0.5f);
            LL.gameObject.GetComponent<StillLetterBox>().Poof();
            box.gameObject.transform.DOScale(0, 0.4f).OnComplete(() => GameObject.Destroy(box.gameObject));
            LL.gameObject.transform.DOScale(0, 0.4f).OnComplete(() => GameObject.Destroy(LL.gameObject));
            yield return Wait.For(0.41f);
        }

        string cacheCompleteWord = null;
        StillLetterBox cacheCompleteWordLL = null;

        private IEnumerator CompleteWordCoroutine()
        {
            audioManager.PlayPoofSound();
            cacheCompleteWordLL.Poof();
            cacheCompleteWordLL.Label.text = cacheCompleteWord;
            yield return Wait.For(AssessmentOptions.Instance.TimeToShowCompleteWord);
        }

        public void InitRound()
        {
            if (state != QuestionGeneratorState.Uninitialized && state != QuestionGeneratorState.Completed)
            {
                throw new InvalidOperationException("Cannot initialized");
            }

            state = QuestionGeneratorState.Initialized;
            ClearCache();
        }

        private void ClearCache()
        {
            totalAnswers = new List<Answer>();
            totalQuestions = new List<IQuestion>();
            partialAnswers = null;
        }

        public void CompleteRound()
        {
            if (state != QuestionGeneratorState.Initialized)
                throw new InvalidOperationException("Not Initialized");

            state = QuestionGeneratorState.Completed;
        }

        public Answer[] GetAllAnswers()
        {
            if (state != QuestionGeneratorState.Completed)
                throw new InvalidOperationException("Not Completed");

            return totalAnswers.ToArray();
        }

        public IQuestion[] GetAllQuestions()
        {
            if (state != QuestionGeneratorState.Completed)
                throw new InvalidOperationException("Not Completed");

            return totalQuestions.ToArray();
        }

        public Answer[] GetNextAnswers()
        {
            if (state != QuestionGeneratorState.QuestionFeeded)
                throw new InvalidOperationException("Not Initialized");

            state = QuestionGeneratorState.Initialized;
            return partialAnswers;
        }

        List<Answer> totalAnswers;
        List<IQuestion> totalQuestions;
        Answer[] partialAnswers;

        public IQuestion GetNextQuestion()
        {
            if (state != QuestionGeneratorState.Initialized)
                throw new InvalidOperationException("Not Initialized");

            state = QuestionGeneratorState.QuestionFeeded;

            currentPack = provider.GetNextQuestion();

            List<Answer> answers = new List<Answer>();
            ILivingLetterData questionData = currentPack.GetQuestion();
            LivingLetterDataType cacheLivingLetterType = LivingLetterDataType.Letter;

            //____________________________________
            //Prepare answers for next method call
            //____________________________________

            if (missingLetter)
            {
                // ### MISSING LETTER ###
                foreach (var wrong in currentPack.GetWrongAnswers())
                {
                    cacheLivingLetterType = wrong.DataType;
                    var wrongAnsw = GenerateWrongAnswer(wrong);

                    answers.Add(wrongAnsw);
                    totalAnswers.Add(wrongAnsw);
                }

                var correct = currentPack.GetCorrectAnswers().ToList()[0];
                cacheLivingLetterType = correct.DataType;

                var correctAnsw = GenerateCorrectAnswer(correct);

                answers.Add(correctAnsw);
                totalAnswers.Add(correctAnsw);

                partialAnswers = answers.ToArray();

                // Generate the question
                var question = GenerateMissingLetterQuestion(questionData, correct);
                totalQuestions.Add(question);
                GeneratePlaceHolder(question, cacheLivingLetterType);
                return question;
            }
            else
            {
                // ### ORDER LETTERS ###
                foreach (var correct in currentPack.GetCorrectAnswers())
                {
                    var correctAnsw = GenerateCorrectAnswer(correct);
                    answers.Add(correctAnsw);
                    totalAnswers.Add(correctAnsw);
                }

                partialAnswers = answers.ToArray();

                // Generate the question
                var question = GenerateQuestion(questionData);
                totalQuestions.Add(question);

                return question;
            }
        }

        private LL_WordData cacheFullWordData;
        private StillLetterBox cacheFullWordDataLL;

        private IQuestion GenerateQuestion(ILivingLetterData data)
        {
            cacheFullWordData = new LL_WordData(data.Id);

            if (AssessmentOptions.Instance.ShowQuestionAsImage)
            {
                data = new LL_ImageData(data.Id);
            }

            cacheFullWordDataLL = ItemFactory.Instance.SpawnQuestion(data);
            return new DefaultQuestion(cacheFullWordDataLL, 0, audioManager);
        }

        private AssessmentAudioManager audioManager;

        private IQuestion GenerateMissingLetterQuestion(ILivingLetterData data, ILivingLetterData letterToRemove)
        {
            var imageData = new LL_ImageData(data.Id);
            LL_WordData word = (LL_WordData)data;
            LL_LetterData letter = (LL_LetterData)letterToRemove;

            cacheCompleteWord = word.TextForLivingLetter;

            var partsToRemove = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).FindLetter(AppManager.I.DB, word.Data, letter.Data, LetterEqualityStrictness.Letter);
            partsToRemove.Shuffle(); //pick a random letter

            string text = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetWordWithMissingLetterText(
                word.Data, partsToRemove[0], removedLetterColor: "#000000");

            //Spawn word, then replace text with text with missing letter
            var wordGO = ItemFactory.Instance.SpawnQuestion(word);
            wordGO.InstaShrink();

            wordGO.Label.text = text;
            cacheCompleteWordLL = wordGO;

            wordGO.SetExtendedBoxCollider();

            return new ImageQuestion(wordGO, imageData, audioManager);
        }

        private Answer GenerateWrongAnswer(ILivingLetterData wrongAnswer)
        {
            return ItemFactory.Instance.SpawnAnswer(wrongAnswer, false, audioManager);
        }

        private void GeneratePlaceHolder(IQuestion question, LivingLetterDataType dataType)
        {
            var placeholder = ItemFactory.Instance.SpawnPlaceholder(dataType);
            placeholder.InstaShrink();
            question.TrackPlaceholder(placeholder.gameObject);
        }

        private Answer GenerateCorrectAnswer(ILivingLetterData correctAnswer)
        {
            return ItemFactory.Instance.SpawnAnswer(correctAnswer, true, audioManager);
        }
    }
}
