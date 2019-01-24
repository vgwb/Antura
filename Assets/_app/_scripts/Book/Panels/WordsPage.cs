using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.Teacher;
using Antura.UI;
using System.Collections;
using System.Collections.Generic;
using Antura.Language;
using UnityEngine;

namespace Antura.Book
{
    public class WordsPage : MonoBehaviour, IBookPanel
    {
        [Header("Prefabs")]
        public GameObject WordItemPrefab;
        public GameObject SpacerItemPrefab;
        public GameObject CategoryItemPrefab;
        public GameObject SpellingLetterItemPrefab;

        [Header("References")]
        public GameObject DetailPanel;
        public GameObject ListPanel;
        public GameObject ListContainer;
        public GameObject SpellingContainer;
        public GameObject Submenu;
        public GameObject SubmenuContainer;

        public TextRender WordArabicText;
        public TextRender WordDrawingText;

        private WordInfo currentWordInfo;
        private GenericCategoryData currentCategory;
        private LocalizationData CategoryData;
        private GameObject btnGO;

        private string debugSingleWord;

        private void OnEnable()
        {
            debugSingleWord = "";

            var cat = new GenericCategoryData
            {
                area = VocabularyChapter.Words,
                Id = "1",
                wordCategory = WordDataCategory.None,
                Stage = 1
            };
            WordsPanel(cat);
        }

        void WordsPanel(GenericCategoryData _category)
        {
            ListPanel.SetActive(true);
            DetailPanel.SetActive(false);
            currentCategory = _category;

            //Debug.Log("current word cat: " + _category);

            List<WordData> wordsList;
            if (currentCategory.Stage > 0) {
                var contents = TeacherAI.I.VocabularyAi.GetContentsOfStage(currentCategory.Stage);
                var hashList = contents.GetHashSet<WordData>();
                wordsList = new List<WordData>();

                if (debugSingleWord != "") {
                    var customWord = AppManager.I.DB.GetWordDataById(debugSingleWord);
                    wordsList.Add(customWord);
                } else {
                    wordsList.AddRange(hashList);
                }
            } else {
                wordsList = AppManager.I.DB.FindWordData((x) => (x.Category == currentCategory.wordCategory));
            }

            emptyListContainers();

            List<WordInfo> info_list = AppManager.I.ScoreHelper.GetAllWordInfo();
            foreach (var info_item in info_list) {
                if (wordsList.Contains(info_item.data)) {
                    btnGO = Instantiate(WordItemPrefab);
                    btnGO.transform.SetParent(ListContainer.transform, false);
                    btnGO.GetComponent<ItemWord>().Init(this, info_item, false);
                }
            }

            var listStages = AppManager.I.DB.GetAllStageData();
            foreach (var stage in listStages) {
                btnGO = Instantiate(CategoryItemPrefab);
                btnGO.transform.SetParent(SubmenuContainer.transform, false);
                btnGO.GetComponent<MenuItemCategory>().Init(
                    this,
                    new GenericCategoryData
                    {
                        area = VocabularyChapter.Words,
                        Id = stage.Id,
                        Title = LocalizationManager.GetTranslation(LocalizationDataId.UI_Stage),
                        TitleEn = "stage",
                        Stage = int.Parse(stage.Id)
                    },
                    int.Parse(stage.Id) == currentCategory.Stage
                );
            }

            btnGO = Instantiate(SpacerItemPrefab);
            btnGO.transform.SetParent(SubmenuContainer.transform, false);

            foreach (WordDataCategory cat in GenericHelper.SortEnums<WordDataCategory>()) {
                //if (cat == WordDataCategory.None) continue;
                btnGO = Instantiate(CategoryItemPrefab);
                btnGO.transform.SetParent(SubmenuContainer.transform, false);
                CategoryData = LocalizationManager.GetWordCategoryData(cat);
                btnGO.GetComponent<MenuItemCategory>().Init(
                    this,
                    new GenericCategoryData
                    {
                        area = VocabularyChapter.Words,
                        wordCategory = cat,
                        Id = cat.ToString(),
                        Title = CategoryData.LearningText,
                        TitleEn = CategoryData.InstructionText,
                        Stage = 0
                    },
                    currentCategory.wordCategory == cat
                );
            }
        }

        public void DetailWord(WordInfo _currentWord)
        {
            currentWordInfo = _currentWord;
            DetailPanel.SetActive(true);
            HighlightWordItem(currentWordInfo.data.Id);
            PlayWord();

            // empty spelling container
            foreach (Transform t in SpellingContainer.transform) {
                Destroy(t.gameObject);
            }
            var splittedLetters = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).SplitWord(AppManager.I.DB, currentWordInfo.data, false, false);
            foreach (var letter in splittedLetters) {
                btnGO = Instantiate(SpellingLetterItemPrefab);
                btnGO.transform.SetParent(SpellingContainer.transform, false);
                btnGO.transform.SetAsFirstSibling();
                btnGO.GetComponent<ItemSpellingLetter>().Init(letter.letter);
            }

            WordArabicText.text = currentWordInfo.data.Arabic;

            if (currentWordInfo.data.Drawing != "") {
                WordDrawingText.text = AppManager.I.VocabularyHelper.GetWordDrawing(currentWordInfo.data);
                if (currentWordInfo.data.Category == WordDataCategory.Color) {
                    WordDrawingText.SetColor(GenericHelper.GetColorFromString(currentWordInfo.data.Value));
                }
            } else {
                WordDrawingText.text = "";
            }

            if (AppConfig.DebugLogEnabled) {
                Debug.Log("Detail Word(): " + currentWordInfo.data.Id);
                Debug.Log("word unicodes: " + LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetStringUnicodes(currentWordInfo.data.Arabic));
                Debug.Log("word unicodes forms: " + LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetStringUnicodes(WordArabicText.RenderedText));
            }
            //ScoreText.text = "Score: " + currentWord.score;
        }

        public void SelectSubCategory(GenericCategoryData _category)
        {
            switch (_category.area) {
                case VocabularyChapter.Words:
                    WordsPanel(_category);
                    break;
            }
        }

        public void BtnClickWord()
        {
            PlayWord();
        }

        void PlayWord()
        {
            AudioManager.I.PlayWord(currentWordInfo.data);
        }

        void HighlightWordItem(string id)
        {
            foreach (Transform t in ListContainer.transform) {
                t.GetComponent<ItemWord>().Select(id);
            }
        }

        void HighlightMenutCategory(string id)
        {
            foreach (Transform t in SubmenuContainer.transform) {
                t.GetComponent<MenuItemCategory>().Select(id);
            }
        }

        void emptyListContainers()
        {
            foreach (Transform t in ListContainer.transform) {
                Destroy(t.gameObject);
            }
            // reset vertical position
            ListPanel.GetComponent<UnityEngine.UI.ScrollRect>().verticalNormalizedPosition = 1.0f;

            foreach (Transform t in SubmenuContainer.transform) {
                Destroy(t.gameObject);
            }
        }

    }
}