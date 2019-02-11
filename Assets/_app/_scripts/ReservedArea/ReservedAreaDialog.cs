using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.UI;
using Antura.Helpers;
using System.Collections.Generic;
using UnityEngine;


namespace Antura.ReservedArea
{
    /// <summary>
    /// Pop-up that allows access to the reserved area with a parental lock.
    /// </summary>
    public class ReservedAreaDialog : MonoBehaviour
    {
        public TextRender nativeTextUI;
        public TextRender learningTextUI;

        private int firstButtonClickCounter;
        private const int nButtons = 4;
        private int firstButtonIndex;
        private int secondButtonIndex;
        private int firstButtonClicksTarget;

        void OnEnable()
        {
            AudioManager.I.PlayDialogue("Parental_Gate");
            firstButtonClickCounter = 0;

            // Selecting two buttons at random
            var availableIndices = new List<int>();
            for (var i = 0; i < nButtons; i++) {
                availableIndices.Add(i);
            }
            var selectedIndices = availableIndices.RandomSelect(2);
            firstButtonIndex = selectedIndices[0];
            secondButtonIndex = selectedIndices[1];

            // Number of clicks at random
            const int min_number = 4;
            const int max_number = 7;
            firstButtonClicksTarget = Random.Range(min_number, max_number);

            // Update text
            string[] numberWordsNative = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] colorsWordsNative = { "green", "red", "blue", "yellow" };

            // TODO: refactor this and get them from the localization data instead
            string[] colorsWordsLearning = { "الأخضر", "الأحمر", "الأزرق", "الأصفر" };
            string[] timesWordsLearning =
            {
                "مرة واحدة",
                "مرتين",
                "ثلاث مرات",
                "أربع مرات",
                "خمس مرات",
                "ست مرات",
                "سبع مرات",
                "ثماني مرات",
                "تسع مرات"
            };

            var forcedLanguageUse = Language.LanguageUse.Learning;

            string numberWordNative = numberWordsNative[firstButtonClicksTarget - 1];
            string firstColorWordNative = colorsWordsNative[firstButtonIndex];
            string secondColorWordNative = colorsWordsNative[secondButtonIndex];

            var titleLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_Title);
            var sectionIntroLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_SectionDescription_Intro);
            var sectionErrorLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_SectionDescription_Error);

            nativeTextUI.text =
                "<b>" + titleLoc.LearningText + "</b>" +
                "\n" + sectionIntroLoc.LearningText + //"This section is reserved for parents and guardians." +
                "\n\nPress <b>" + numberWordNative + "</b> times the <b>" + firstColorWordNative + "</b> button, then press the <b>" + secondColorWordNative +
                "</b> one once." +
                "\n\n" + sectionErrorLoc.LearningText; //"If you make an error, retry by re - accessing this panel");

            string numberWordArabic = timesWordsLearning[firstButtonClicksTarget - 1];
            string firstColorWordArabic = colorsWordsLearning[firstButtonIndex];
            string secondColorWordArabic = colorsWordsLearning[secondButtonIndex];

            string learningIntroduction = "";
            learningIntroduction += "<b>" + LocalizationManager.GetTranslation(titleLoc.Id) + "<b/> \n";
            learningIntroduction += LocalizationManager.GetTranslation(sectionIntroLoc.Id) + "\n\n";
            learningIntroduction += string.Format("لفتح القفل، اضغط الزر {0} {2} ، ثم الزر {1} مرة واحدة", firstColorWordArabic,
                secondColorWordArabic, numberWordArabic);
            learningIntroduction += "\n\n" + LocalizationManager.GetTranslation(sectionErrorLoc.Id);

            //Debug.Log(arabicIntroduction);
            learningTextUI.text = learningIntroduction;
        }

        public void OnButtonClick(int buttonIndex)
        {
            AudioManager.I.PlaySound(Sfx.Blip);
            if (buttonIndex == firstButtonIndex) {
                firstButtonClickCounter++;
            } else if (buttonIndex == secondButtonIndex) {
                if (firstButtonClickCounter == firstButtonClicksTarget) {
                    UnlockReservedArea();
                } else {
                    firstButtonClickCounter = firstButtonClicksTarget + 1; // disabling
                }
            }
        }

        void UnlockReservedArea()
        {
            AppManager.I.NavigationManager.GoToReservedArea();
        }
    }
}