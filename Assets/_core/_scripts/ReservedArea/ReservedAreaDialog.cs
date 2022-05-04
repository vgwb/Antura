using System;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.UI;
using Antura.Helpers;
using System.Collections.Generic;
using Antura.Keeper;
using Antura.Language;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Antura.ReservedArea
{
    /// <summary>
    /// Pop-up that allows access to the reserved area with a parental lock.
    /// the current unlock sequence is clicking 5 green + 1 red
    /// </summary>
    public class ReservedAreaDialog : MonoBehaviour
    {
        public GameObject layoutHelpAndLearning;
        public GameObject layoutLearningOnly;

        public TextRender helpTextUI;
        public TextRender learningTextUI;
        public TextRender learningOnlyTextUI;

        public TextRender helpTitleUI;
        public TextRender learningTitleUI;
        public TextRender learningOnlyTitleUI;


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
            if (DebugConfig.I.BypassDialogs)
            {
                UnlockReservedArea();
            }

            KeeperManager.I.PlayDialogue("Parental_Gate", keeperMode: KeeperMode.LearningThenNativeNoSubtitles);
            firstButtonClickCounter = 0;

            UseForcedSequence = AppManager.I.AppEdition.ReservedAreaForcedSeq;

            bool showHelpText = AppManager.I.AppEdition.HelpTextInReservedArea && AppManager.I.ContentEdition.HelpLanguage != AppManager.I.ContentEdition.LearningLanguage;
            layoutHelpAndLearning.SetActive(showHelpText);
            layoutLearningOnly.SetActive(!showHelpText);

            // Selecting two buttons at random
            var availableIndices = new List<int>();
            for (var i = 0; i < nButtons; i++)
            {
                availableIndices.Add(i);
            }
            var selectedIndices = availableIndices.RandomSelect(2);
            firstButtonIndex = selectedIndices[0];
            secondButtonIndex = selectedIndices[1];

            if (UseForcedSequence)
                firstButtonIndex = ForceSequenceFirstColorIndex;
            if (UseForcedSequence)
                secondButtonIndex = ForceSequenceSecondColorIndex;

            // Number of clicks at random
            const int min_number = 2;
            const int max_number = 5;
            firstButtonClicksTarget = Random.Range(min_number, max_number + 1);

            if (UseForcedSequence)
                firstButtonClicksTarget = ForceSequenceFirstColorPresses;

            var numberWord = LocalizationManager.GetLocalizationData((LocalizationDataId)Enum.Parse(typeof(LocalizationDataId), $"UI_Number_{firstButtonClicksTarget}"));
            var colorWords = new LocalizationData[4];
            colorWords[0] = LocalizationManager.GetLocalizationData(LocalizationDataId.UI_Antura_Color_Green);
            colorWords[1] = LocalizationManager.GetLocalizationData(LocalizationDataId.UI_Antura_Color_Red);
            colorWords[2] = LocalizationManager.GetLocalizationData(LocalizationDataId.UI_Antura_Color_Blue);
            colorWords[3] = LocalizationManager.GetLocalizationData(LocalizationDataId.UI_Antura_Color_Yellow);

            var firstColorLocData = colorWords[firstButtonIndex];
            var secondColorLocData = colorWords[secondButtonIndex];

            var titleLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_Title);
            var sectionIntroLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_SectionDescription_Intro);
            var sectionErrorLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_SectionDescription_Error);
            var sectionGateCodeLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.Parental_Gate_Code);
            var sectionGateCodeForcedLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.Parental_Gate_Code_Forced);

            // HELP INTRODUCTION
            var helpTitle = $"{titleLoc.HelpText}";
            helpTitleUI.SetText(helpTitle, LanguageUse.Help);
            string helpIntroduction = "";
            helpIntroduction += $"{sectionIntroLoc.HelpText}\n\n";
            if (UseForcedSequence)
                helpIntroduction += sectionGateCodeForcedLoc.HelpText;
            else
                helpIntroduction += string.Format(sectionGateCodeLoc.HelpText, numberWord.HelpText.ToLower(), firstColorLocData.HelpText.ToLower(), secondColorLocData.HelpText.ToLower());
            helpIntroduction += $"\n\n{sectionErrorLoc.HelpText}";
            helpTextUI.SetText(helpIntroduction, LanguageUse.Help);

            // LEARNING INTRODUCTION
            var learningTitle = $"{titleLoc.LearningText}";
            learningTitleUI.SetText(learningTitle, LanguageUse.Learning);
            learningOnlyTitleUI.SetText(learningTitle, LanguageUse.Learning);
            string learningIntroduction = "";
            learningIntroduction += $"{sectionIntroLoc.LearningText}\n\n";
            if (UseForcedSequence)
                learningIntroduction += sectionGateCodeForcedLoc.LearningText;
            else
                learningIntroduction += string.Format(sectionGateCodeLoc.LearningText, numberWord.LearningText.ToLower(), firstColorLocData.LearningText.ToLower(), secondColorLocData.LearningText.ToLower());
            learningIntroduction += $"\n\n{sectionErrorLoc.LearningText}";

            learningTextUI.SetText(learningIntroduction, LanguageUse.Learning);
            learningOnlyTextUI.SetText(learningIntroduction, LanguageUse.Learning);
        }

        public void OnButtonClick(int buttonIndex)
        {
            AudioManager.I.PlaySound(Sfx.Blip);
            if (buttonIndex == firstButtonIndex)
            {
                firstButtonClickCounter++;
            }
            else if (buttonIndex == secondButtonIndex)
            {
                if (firstButtonClickCounter == firstButtonClicksTarget)
                {
                    UnlockReservedArea();
                }
                else
                {
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
