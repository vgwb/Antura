using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Keeper;
using Antura.UI;
using UnityEngine;

namespace Antura.Book
{
    public struct GenericCategoryData
    {
        public VocabularyChapter area;
        public string Id;
        public string TitleLearning;
        public string TitleNative;
        public string TitleHelp;
        public int Stage;
        public WordDataCategory wordCategory;
        public PhraseDataCategory phraseCategory;
    }

    public enum VocabularyChapter
    {
        None,
        Letters,
        Words,
        Phrases,
        LearningBlock
    }

    /// <summary>
    /// Displays information on all learning items the player has unlocked.
    /// </summary>
    public class VocabularyPanel : MonoBehaviour
    {
        [Header("References")]
        public GameObject LettersPage;
        public GameObject WordsPage;
        public GameObject PhrasesPage;
        public UIButton BtnLetters;
        public UIButton BtnWords;

        private VocabularyChapter currentChapter = VocabularyChapter.None;

        void Start()
        {
        }

        void OnEnable()
        {
            OpenArea(VocabularyChapter.Letters);
        }

        void OpenArea(VocabularyChapter newArea)
        {
            if (newArea != currentChapter)
            {
                currentChapter = newArea;
                activatePanel(currentChapter, true);
                ResetMenuButtons();
            }
        }

        void activatePanel(VocabularyChapter panel, bool status)
        {
            switch (panel)
            {
                case VocabularyChapter.Letters:
                    KeeperManager.I.PlayDialogue(LocalizationDataId.UI_Letters, KeeperMode.LearningNoSubtitles);
                    LettersPage.SetActive(true);
                    WordsPage.SetActive(false);
                    break;
                case VocabularyChapter.Words:
                    KeeperManager.I.PlayDialogue(LocalizationDataId.UI_Words, KeeperMode.LearningNoSubtitles);
                    LettersPage.SetActive(false);
                    WordsPage.SetActive(true);
                    break;
                case VocabularyChapter.Phrases:
                    KeeperManager.I.PlayDialogue(LocalizationDataId.UI_Phrases, KeeperMode.LearningNoSubtitles);
                    PhrasesPage.SetActive(true);
                    break;
            }
        }

        void ResetMenuButtons()
        {
            BtnLetters.Lock(currentChapter == VocabularyChapter.Letters);
            BtnWords.Lock(currentChapter == VocabularyChapter.Words);
            //BtnPhrases.Lock(currentChapter == VocabularyChapter.Phrases);
        }

        #region buttons
        public void BtnOpenLetters()
        {
            OpenArea(VocabularyChapter.Letters);
        }

        public void BtnOpenWords()
        {
            OpenArea(VocabularyChapter.Words);
        }

        public void BtnOpenPhrases()
        {
            OpenArea(VocabularyChapter.Phrases);
        }
        #endregion

        void ResetLL()
        {
        }
    }
}
