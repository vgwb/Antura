using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    [Serializable]
    public class QuizEntry
    {
        public Antura.Discover.CardData Item;
        public bool IsCorrect;
    }

    [CreateAssetMenu(fileName = "QuizSettingsData", menuName = "Antura/Activity/Quiz Settings")]
    public class QuizSettingsData : ActivitySettingsAbstract
    {
        [Header("Quiz Settings")]
        public string Question;

        [Tooltip("Between 2 and 8 items")]
        public List<QuizEntry> Answers;

        public bool Shuffle;

    }
}
