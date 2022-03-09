using System.Collections;
using System.Collections.Generic;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Teacher;
using UnityEngine;

namespace Antura.LivingLetters
{
    public class HomeSceneLetter : MonoBehaviour
    {
        public void OnMouseDown()
        {
            var view = GetComponent<LivingLetterController>();
            view.ToggleDance();

            ChangeLetter(true);
        }

        public void ChangeLetter(bool playSound = false)
        {
            var view = GetComponent<LivingLetterController>();
            var letterFilters = new LetterFilters
            {
                excludeDiphthongs = true,
                excludeDiacritics = AppManager.I.ContentEdition.PlayNameSoundWithForms ? LetterFilters.ExcludeDiacritics.All : LetterFilters.ExcludeDiacritics.None
            };
            var letter = AppManager.I.Teacher.GetRandomTestLetterLL(letterFilters, useMaxJourneyData: true);
            view.Init(letter);

            if (playSound)
            {
                var soundType = LetterDataSoundType.Phoneme;
                if (AppManager.I.ContentEdition.PlayNameSoundWithForms) soundType = LetterDataSoundType.Name;
                AudioManager.I.PlayLetter(letter.Data, true, soundType);
            }
        }
    }
}