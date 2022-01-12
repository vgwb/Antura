using System.Collections;
using System.Collections.Generic;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
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
            letterFilters.excludeDiacritics = AppManager.I.ContentEdition.PlayNameSoundWithForms ? LetterFilters.ExcludeDiacritics.All : LetterFilters.ExcludeDiacritics.None;
            var letter = AppManager.I.Teacher.GetRandomTestLetterLL(letterFilters, useMaxJourneyData: true);
            view.Init(letter);

            var soundType = LetterDataSoundType.Phoneme;
            if (AppManager.I.ContentEdition.PlayNameSoundWithForms) soundType = LetterDataSoundType.Name;
            AudioManager.I.PlayLetter(letter.Data, true, soundType);
        }
    }
}