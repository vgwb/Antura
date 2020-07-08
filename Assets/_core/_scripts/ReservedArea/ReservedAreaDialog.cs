using System;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.UI;
using Antura.Helpers;
using System.Collections.Generic;
using Antura.Keeper;
using JetBrains.Annotations;
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
        [UsedImplicitly] public TextRender nativeTextUI;
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
            KeeperManager.I.PlayDialogue("Parental_Gate", keeperMode: KeeperMode.LearningThenNativeNoSubtitles);
            firstButtonClickCounter = 0;

            UseForcedSequence = AppManager.I.ParentEdition.ReservedAreaForcedSeq;

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

            var numberWord = AppManager.I.DB.GetWordDataById($"number_0{firstButtonClicksTarget}");

            WordData[] colorWords = new WordData[4];
            colorWords[0] = AppManager.I.DB.GetWordDataById("color_green");;
            colorWords[1] = AppManager.I.DB.GetWordDataById("color_red");
            colorWords[2] = AppManager.I.DB.GetWordDataById("color_blue");
            colorWords[3] = AppManager.I.DB.GetWordDataById("color_yellow");


            var firstColorLocData = colorWords[firstButtonIndex];
            var secondColorLocData = colorWords[secondButtonIndex];

            var titleLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_Title);
            var sectionIntroLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_SectionDescription_Intro);
            var sectionErrorLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.ReservedArea_SectionDescription_Error);
            var sectionGateCodeLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.Parental_Gate_Code);
            var sectionGateCodeForcedLoc = LocalizationManager.GetLocalizationData(LocalizationDataId.Parental_Gate_Code_Forced);

            /* NATIVE INTRODUCTION
            // this is deprecated now. If we want this, we need to extract color and number words for the Native language too.
            //
            string nativeIntroduction = "";
            nativeIntroduction += "<b>" + titleLoc.NativeText + "</b> \n";
            nativeIntroduction += sectionIntroLoc.NativeText + "\n\n";

            if (UseForcedSequence)
                nativeIntroduction += sectionGateForcedCodeLoc.NativeText;
            else
                nativeIntroduction += string.Format(sectionGateCodeLoc.NativeText, numberWord.get(), firstColorLocData.GetNativeText(), secondColorLocData.GetNativeText());
            //nativeIntroduction += string.Format("Pulsa <b>{0}</b> veces el botón <b>{1}</b>, luego pulsa <b>{2}</b> una vez", numberWordNative, firstColorWordNative, secondColorWordNative);
            nativeIntroduction += "\n\n" + sectionErrorLoc.NativeText;
            nativeTextUI.SetText(nativeIntroduction, Language.LanguageUse.Learning);
            */

            string learningIntroduction = "";
            learningIntroduction += $"<b>{titleLoc.LearningText}</b> \n";
            learningIntroduction += $"{sectionIntroLoc.LearningText}\n\n";
            if (UseForcedSequence)
                learningIntroduction += sectionGateCodeForcedLoc.LearningText;
            else
                learningIntroduction += string.Format(sectionGateCodeLoc.LearningText, numberWord.Text.ToLower(), firstColorLocData.Text.ToLower(), secondColorLocData.Text.ToLower());
            learningIntroduction += $"\n\n{sectionErrorLoc.LearningText}";

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