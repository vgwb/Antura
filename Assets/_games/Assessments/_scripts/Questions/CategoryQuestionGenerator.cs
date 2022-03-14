using Antura.Extensions;
using Antura.LivingLetters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Assessment
{
    /// <summary>
    /// Question Generator for assessments that requires to categorize something
    /// </summary>
    public class CategoryQuestionGenerator : IQuestionGenerator
    {
        private QuestionGeneratorState state;
        private int numberOfMaxAnswers;
        private int numberOfRounds;
        private List<ILivingLetterData>[] answersBuckets;
        private ArabicCategoryProvider categoryProvider;

        public CategoryQuestionGenerator(IQuestionProvider questionProvider,
                                            ArabicCategoryProvider categoryProvider,
                                            AssessmentAudioManager dialogues,
                                            int maxAnsw, int rounds)
        {
            state = QuestionGeneratorState.Uninitialized;
            numberOfMaxAnswers = maxAnsw;
            numberOfRounds = rounds;
            answersBuckets = new List<ILivingLetterData>[3];
            this.categoryProvider = categoryProvider;
            this.dialogues = dialogues;

            for (int i = 0; i < 3; i++)
            {
                answersBuckets[i] = new List<ILivingLetterData>();
            }
            ClearCache();
            FillBuckets(questionProvider);
        }

        /// <summary>
        /// IQuestionProvider returns answers sorted by category.( S,S,M,M,M,M)
        /// After we have divided answers into category buckets, we use this function
        /// to select desired number of elements from random buckets. Called each Round.
        /// </summary>
        private void NumberOfAnswersFromEachBucket()
        {
            for (int i = 0, count = roundElementsForCategory.Length; i < count; ++i)
            {
                roundElementsForCategory[i] = 0;
            }

            int picksThisRound = numberOfMaxAnswers;
            int totalAnswers = answersBuckets[0].Count + answersBuckets[1].Count + answersBuckets[2].Count;

            while (picksThisRound > 0 && totalAnswers > 0)
            {
                int pickFromBucketN = -1;

                //ok as long as we have 10 or less buckets
                // try to be fair (but never use infinite loop.)
                for (int i = 0; i < 1000000 && pickFromBucketN == -1; i++)
                {
                    int temp = UnityEngine.Random.Range(0, 3);

                    if (answersBuckets[temp].Count > roundElementsForCategory[temp])
                    {
                        pickFromBucketN = temp;
                    }
                }

                if (pickFromBucketN == -1)
                {
                    //and use a little bias if computation took to long.
                    for (int i = 0; i < 3; i++)
                    {
                        if (answersBuckets[i].Count > roundElementsForCategory[i])
                        {
                            pickFromBucketN = i;
                            break;
                        }
                    }
                }

                if (pickFromBucketN == -1)
                {
                    throw new InvalidOperationException("buckets empty");
                }
                picksThisRound--;
                totalAnswers--;

                roundElementsForCategory[pickFromBucketN]++;
            }

            if (picksThisRound == numberOfMaxAnswers)
            {
                throw new InvalidOperationException("buckets empty");
            }
        }

        /// <summary>
        /// Draw the Questions/Answers for the whole game session (6 answers)
        /// It is called just once before the 3 rounds. Answers are removed
        /// from Buckets when GetNextQuestion is called.
        /// </summary>
        private void FillBuckets(IQuestionProvider questionProvider)
        {
            int max = numberOfRounds * numberOfMaxAnswers;

            for (int i = 0; i < max; i++)
            {
                var pack = questionProvider.GetNextQuestion();

                foreach (var answ in pack.GetCorrectAnswers())
                {
                    //Arabic has different order!
                    for (int j = 0; j < categoryProvider.GetCategories(); j++)
                    {
                        if (categoryProvider.Compare(j, answ))
                        {
                            answersBuckets[j].Add(pack.GetQuestion());
                        }
                    }
                }
            }
        }

        private Answer GenerateCorrectAnswer(ILivingLetterData correctAnswer)
        {
            return
            ItemFactory.Instance.SpawnAnswer(correctAnswer, true, dialogues);
        }

        public void InitRound()
        {
            if (state != QuestionGeneratorState.Uninitialized && state != QuestionGeneratorState.Completed)
            {
                throw new InvalidOperationException("Cannot initialized");
            }

            state = QuestionGeneratorState.Initialized;
            ClearCache();
            NumberOfAnswersFromEachBucket();
        }

        private void ClearCache()
        {
            totalAnswers = new List<Answer>();
            totalQuestions = new List<IQuestion>();
            partialAnswers = null;
            currentCategory = 0;
        }

        public void CompleteRound()
        {
            if (state != QuestionGeneratorState.Initialized)
                throw new InvalidOperationException("Not Initialized");

            state = QuestionGeneratorState.Completed;
            currentCategory = 0;
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

        private int currentCategory;

        // Categories
        private int[] roundElementsForCategory = new int[3];
        private AssessmentAudioManager dialogues;

        public IQuestion GetNextQuestion()
        {
            if (state != QuestionGeneratorState.Initialized)
            {
                throw new InvalidOperationException("Not Initialized");
            }
            state = QuestionGeneratorState.QuestionFeeded;

            //____________________________________
            //Prepare answers for next method call
            //____________________________________

            // Assumption: Here each category have enough elements
            int amount = roundElementsForCategory[currentCategory];
            var answers = new List<Answer>();

            int correctCount = 0;
            for (int i = 0; i < amount; i++)
            {
                var answer = answersBuckets[currentCategory].Pull();
                var correctAnsw = GenerateCorrectAnswer(answer);

                correctCount++;
                answers.Add(correctAnsw);
                totalAnswers.Add(correctAnsw);
            }

            partialAnswers = answers.ToArray();

            // Generate the question
            var question = GenerateQuestion(correctCount);
            totalQuestions.Add(question);

            // Generate placeholders
            for (int i = 0; i < numberOfMaxAnswers; i++)
            {
                GeneratePlaceHolder(question, AssessmentOptions.Instance.AnswerType);
            }
            currentCategory++;
            return question;
        }

        private IQuestion GenerateQuestion(int correctCount)
        {
            var q = categoryProvider.SpawnCustomObject(currentCategory);
            return new CategoryQuestion(q, correctCount, dialogues);
        }

        private void GeneratePlaceHolder(IQuestion question, LivingLetterDataType dataType)
        {
            var placeholder = ItemFactory.Instance.SpawnPlaceholder(dataType);
            placeholder.transform.localPosition = new Vector3(0, 5, 0);
            question.TrackPlaceholder(placeholder.gameObject);
        }
    }
}
