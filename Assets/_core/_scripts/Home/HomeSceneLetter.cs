using System;
using System.Linq;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Scenes;
using Antura.Teacher;
using Antura.UI;
using UnityEngine;

namespace Antura.LivingLetters
{
    public class HomeSceneLetter : MonoBehaviour
    {
        private LivingLetterController LivingLetter;
        private EditionSelectionManager EditionSelectionManager;

        void Awake()
        {
            EditionSelectionManager = FindObjectOfType<EditionSelectionManager>();
        }

        public void OnMouseDown()
        {
            if (GlobalUI.PauseMenu.IsMenuOpen) return;
            if (EditionSelectionManager.selectNativeLanguagePanel.isActiveAndEnabled) return;
            if (EditionSelectionManager.selectLearningContentPanel.isActiveAndEnabled) return;

            LivingLetter = GetComponent<LivingLetterController>();
            LivingLetter.ToggleDance();
            ChangeLetter(true);
        }

        public void ChangeLetter(bool playSound = false)
        {
            LivingLetter = GetComponent<LivingLetterController>();
            var letterFilters = new LetterFilters
            {
                excludeDiphthongs = true,
                excludeDiacritics = AppManager.I.ContentEdition.PlayNameSoundWithForms ? LetterFilters.ExcludeDiacritics.All : LetterFilters.ExcludeDiacritics.None
            };
            LL_LetterData letter;
            try
            {
                letter = AppManager.I.Teacher.GetRandomTestLetterLL(letterFilters, useMaxJourneyData: true);
            }
            catch (Exception)
            {
                Debug.LogError("Exception while trying to fetch a letter from the DB. Reverting to the first letter");
                letter = AppManager.I.DB.GetAllLetterData().FirstOrDefault().ConvertToLivingLetterData() as LL_LetterData;
            }
            LivingLetter.Init(letter);

            if (playSound)
            {
                var soundType = LetterDataSoundType.Phoneme;
                if (AppManager.I.ContentEdition.PlayNameSoundWithForms)
                    soundType = LetterDataSoundType.Name;
                AudioManager.I.PlayLetter(letter.Data, true, soundType);
            }
        }
    }
}
