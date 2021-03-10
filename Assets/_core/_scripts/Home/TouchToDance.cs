using System.Collections;
using System.Collections.Generic;
using Antura.Audio;
using Antura.Core;
using Antura.Teacher;
using UnityEngine;

namespace Antura.LivingLetters
{
    public class TouchToDance : MonoBehaviour
    {
        public void OnMouseDown()
        {
            var view = GetComponent<LivingLetterController>();
            view.ToggleDance();

            var letterFilters = new LetterFilters();
            letterFilters.excludeDiphthongs = true;
            var letter = AppManager.I.Teacher.GetRandomTestLetterLL(letterFilters, useMaxJourneyData: true);
            view.Init(letter);

            AudioManager.I.PlayLetter(letter.Data, true, callback: () => Debug.LogError("CALLBACK " + letter.Data.Id));
        }
    }
}