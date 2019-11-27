using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.UI;
using Antura.Helpers;
using System.Collections.Generic;
using Antura.Keeper;
using UnityEngine;

namespace Antura.ReservedArea
{
    /// <summary>
    /// Pop-up that allows access to the reserved area with a parental lock.
    /// the current unlock sequence is clicking 5 green + 1 red
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

        private bool UseForcedSequence;
        public int ForceSequenceFirstColorIndex = 0;
        public int ForceSequenceSecondColorIndex = 0;
        public int ForceSequenceFirstColorPresses = 1;

        void OnEnable()
        {
            KeeperManager.I.PlayDialogue("Parental_Gate", keeperMode: KeeperMode.NativeNoSubtitles);
            firstButtonClickCounter = 0;

            UseForcedSequence = EditionConfig.I.ReservedAreaForcedSeq;

            // Selecting two buttons at random
            var availableIndices = new List<int>();
            for (var i = 0; i < nButtons; i++) {
                availableIndices.Add(i);
            }
            var selectedIndices = availableIndices.RandomSelect(2);
            firstButtonIndex = selectedIndices[0];
            secondButtonIndex = selectedIndices[1];

            if (UseForcedSequence) firstButtonIndex = ForceSequenceFirstColorIndex;
            if (UseForcedSequence) secondButtonIndex = ForceSequenceSecondColorIndex;

            // Number of clicks at random
            const int min_number = 4;
            const int max_number = 7;
            firstButtonClicksTarget = Random.Range(min_number, max_number);

            if (UseForcedSequence) firstButtonClicksTarget = ForceSequenceFirstColorPresses;

            // Native
            string[] numbersWordsNative = { "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
            string[] colorsWordsNative = { "verde", "rojo", "azul", "amarillo" };

            // Learning
            string[] numbersWordsLearning = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] colorsWordsLearning = { "green", "red", "blue", "yellow" };

            //// arabic
            //// TODO: refactor this and get them from the localization data instead
            //string[] colorsWordsLearning = { "الأخضر", "الأحمر", "الأزرق", "الأصفر" };
            //string[] numbersWordsLearning =
            //{
            //    "مرة واحدة",
            //    "مرتين",
            //    "ثلاث مرات",
            //    "أربع مرات",
            //    "خمس مرات",
            //    "ست مرات",
            //    "سبع مرات",
            //    "ثماني مرات",
            //    "تسع مرات"
            //};

            string numberWordNative = numbersWordsNative[firstButtonClicksTarget - 1];
            string firstColorWordNative = colorsWordsNative[firstButtonIndex];
            string secondColorWordNative = colorsWordsNative[secondButtonIndex];

            var titleLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_Title);
            var sectionIntroLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_SectionDescription_Intro);
            var sectionErrorLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_SectionDescription_Error);
            var sectionGateCodeLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.Parental_Gate_Code);

            string nativeIntroduction = "";
            nativeIntroduction += "<b>" + titleLoc.NativeText + "</b> \n";
            nativeIntroduction += sectionIntroLoc.NativeText + "\n\n";
            nativeIntroduction += sectionGateCodeLoc.NativeText;
            //nativeIntroduction += string.Format("Pulsa <b>{0}</b> veces el botón <b>{1}</b>, luego pulsa <b>{2}</b> una vez", numberWordNative, firstColorWordNative, secondColorWordNative);
            nativeIntroduction += "\n\n" + sectionErrorLoc.NativeText;
            nativeTextUI.SetText(nativeIntroduction, Language.LanguageUse.Learning);

            string numberWordLearning = numbersWordsLearning[firstButtonClicksTarget - 1];
            string firstColorWordLearning = colorsWordsLearning[firstButtonIndex];
            string secondColorWordLearning = colorsWordsLearning[secondButtonIndex];

            string learningIntroduction = "";
            learningIntroduction += "<b>" + titleLoc.LearningText + "</b> \n";
            learningIntroduction += sectionIntroLoc.LearningText + "\n\n";

            if (UseForcedSequence)
                learningIntroduction += sectionGateCodeLoc.LearningText;
            else
                learningIntroduction += string.Format("Press <b>{0}</b> times the <b>{1}</b> button, then press the <b>{2}</b> one once.", numberWordLearning, firstColorWordLearning, secondColorWordLearning);
            // arabic
            //learningIntroduction += string.Format("لفتح القفل، اضغط الزر {0} {2} ، ثم الزر {1} مرة واحدة", firstColorWordLearning,
            //secondColorWordLearning, numberWordLearning);

            learningIntroduction += "\n\n" + sectionErrorLoc.LearningText;

            //Debug.Log(arabicIntroduction);
            learningTextUI.SetText(learningIntroduction, Language.LanguageUse.Learning);
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