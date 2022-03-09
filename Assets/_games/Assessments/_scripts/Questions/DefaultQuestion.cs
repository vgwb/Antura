using Antura.LivingLetters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Assessment
{
    public class DefaultQuestion : IQuestion
    {
        private StillLetterBox view;
        private int placeholdersCount;

        public DefaultQuestion(StillLetterBox letter, int placeholders, AssessmentAudioManager dialogues)
        {
            view = letter;
            placeholdersCount = placeholders;
            placeholdersSet = new List<GameObject>();
            var question = letter.gameObject.AddComponent<QuestionBehaviour>();
            question.SetQuestion(this, dialogues);
        }

        public GameObject gameObject
        {
            get
            {
                return view.gameObject;
            }
        }

        public QuestionBehaviour QuestionBehaviour
        {
            get
            {
                return view.GetComponent<QuestionBehaviour>();
            }
        }

        public ILivingLetterData Image()
        {
            throw new NotImplementedException("Not implemented (on purpose)");
        }

        public ILivingLetterData LetterData()
        {
            return view.Data;
        }

        public int PlaceholdersCount()
        {
            return placeholdersCount;
        }

        private List<GameObject> placeholdersSet;

        public void TrackPlaceholder(GameObject gameObject)
        {
            placeholdersSet.Add(gameObject);
        }

        public IEnumerable<GameObject> GetPlaceholders()
        {
            if (placeholdersSet.Count != placeholdersCount)
            {
                throw new InvalidOperationException("Something wrong. Check Question placer");
            }
            return placeholdersSet;
        }

        private AnswerSet answerSet;

        public void SetAnswerSet(AnswerSet answerSet)
        {
            this.answerSet = answerSet;
        }

        public AnswerSet GetAnswerSet()
        {
            return answerSet;
        }
    }
}
