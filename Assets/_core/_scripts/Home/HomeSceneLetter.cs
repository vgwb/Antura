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
    public enum HomeLLType
    {
        Letter = 0,
        Word = 1,
        Drawing = 2,
    }
    public class HomeSceneLetter : MonoBehaviour
    {
        public HomeLLType LLType;

        public bool PlaySound = false;

        private LivingLetterController LivingLetter;
        private EditionSelectionManager EditionSelectionManager;

        void Awake()
        {
            EditionSelectionManager = FindFirstObjectByType<EditionSelectionManager>();
        }

        public void OnMouseDown()
        {
            if (GlobalUI.PauseMenu.IsMenuOpen)
                return;
            if (EditionSelectionManager.selectNativeLanguagePanel.isActiveAndEnabled)
                return;
            if (EditionSelectionManager.selectLearningContentPanel.isActiveAndEnabled)
                return;

            LivingLetter = GetComponent<LivingLetterController>();
            LivingLetter.ToggleDance();
            if (LLType == HomeLLType.Letter)
                ChangeLetter();
            else
                ChangeWord();
        }

        public void ChangeLetter()
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

            if (PlaySound)
            {
                var soundType = LetterDataSoundType.Phoneme;
                if (AppManager.I.ContentEdition.PlayNameSoundWithForms)
                    soundType = LetterDataSoundType.Name;
                AudioManager.I.PlayLetter(letter.Data, true, soundType);
            }
        }

        public void ChangeWord()
        {
            LivingLetter = GetComponent<LivingLetterController>();
            var wordFilters = new WordFilters
            {
                excludeDiacritics = true,
                excludeLetterVariations = true,
                requireDiacritics = false,
                excludeArticles = true,
                excludePluralDual = true,
                requireDrawings = LLType == HomeLLType.Drawing ? true : false,
            };

            LL_WordData word = AppManager.I.Teacher.GetRandomTestWordDataLL();
            if (LLType == HomeLLType.Drawing)
            {
                LL_ImageData image = new LL_ImageData(word.Data.GetId(), word.Data);
                LivingLetter.Init(image);
                if (PlaySound)
                    AudioManager.I.PlayWord(image.Data, true);
            }
            else
            {
                LivingLetter.Init(word);
                if (PlaySound)
                    AudioManager.I.PlayWord(word.Data, true);
            }
        }
    }
}
