using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Keeper;
using Antura.Language;
using Antura.LivingLetters;
using Antura.Helpers;
using Antura.Teacher;
using Antura.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Book
{
    public class WordsPage : MonoBehaviour, IBookPanel, BookDataManager
    {
        [Header("Prefabs")]
        public GameObject WordItemPrefab;
        public GameObject SpacerItemPrefab;
        public GameObject CategoryItemPrefab;
        public GameObject SpellingLetterItemPrefab;

        [Header("References")]

        public GameObject DetailPanel;
        public GameObject DetailPanel_Inner;
        public GameObject ListPanel;
        public GameObject ListContainer;
        public GameObject SpellingContainer;
        public GameObject Submenu;
        public GameObject SubmenuContainer;

        public TextRender WordTitleText;
        public TextRender WordDrawingText;

        private WordInfo currentWordInfo;
        private GenericCategoryData currentCategory;
        private GameObject btnGO;

        public Transform[] SubPanels;

        private string debugSingleWord;

        private void OnEnable()
        {
            debugSingleWord = "";

            var cat = new GenericCategoryData
            {
                area = VocabularyChapter.Words,
                Id = "1",
                wordCategory = WordDataCategory.None,
                Stage = 0
            };
            WordsPanel(cat);

            if (AppManager.I.LanguageSwitcher.IsLearningLanguageRTL())
            {
                for (int i = 0; i < SubPanels.Length; i++)
                {
                    SubPanels[i].SetSiblingIndex(0);
                }
            }
        }

        private List<WordData> GetWordsByCategory(WordDataCategory _category)
        {
            if (_category != WordDataCategory.None)
            {
                return AppManager.I.DB.FindWordData((x) => x.Category == _category);
            }
            else
            {
                return new List<WordData>();
            }
        }

        private void WordsPanel(GenericCategoryData _category)
        {
            ListPanel.SetActive(true);
            DetailPanel_Inner.SetActive(false);
            currentCategory = _category;

            //Debug.Log("current word cat: " + _category);

            List<WordData> wordsList;
            if (currentCategory.Stage > 0)
            {
                var contents = TeacherAI.I.VocabularyAi.GetContentsOfStage(currentCategory.Stage);
                var hashList = contents.GetHashSet<WordData>();
                wordsList = new List<WordData>();

                if (debugSingleWord != "")
                {
                    var customWord = AppManager.I.DB.GetWordDataById(debugSingleWord);
                    wordsList.Add(customWord);
                }
                else
                {
                    wordsList.AddRange(hashList);
                }
            }
            else
            {
                wordsList = GetWordsByCategory(currentCategory.wordCategory);
            }

            wordsList.Sort(WordComparison);

            emptyListContainers();

            List<WordInfo> info_list = AppManager.I.ScoreHelper.GetAllWordInfo();
            foreach (var wordData in wordsList)
            {
                var findMyInfo = info_list.FirstOrDefault((x) => x.data == wordData);
                if (findMyInfo != null)
                {
                    if (findMyInfo.data.Active)
                    {
                        btnGO = Instantiate(WordItemPrefab);
                        btnGO.transform.SetParent(ListContainer.transform, false);
                        btnGO.GetComponent<ItemWord>().Init(this, findMyInfo, false);
                    }
                }
            }

            //var listStages = AppManager.I.DB.GetAllStageData();
            //foreach (var stage in listStages) {
            //    btnGO = Instantiate(CategoryItemPrefab);
            //    btnGO.transform.SetParent(SubmenuContainer.transform, false);
            //    btnGO.GetComponent<MenuItemCategory>().Init(
            //        this,
            //        new GenericCategoryData
            //        {
            //            area = VocabularyChapter.Words,
            //            Id = stage.Id,
            //            TitleLearning = LocalizationManager.GetTranslation(LocalizationDataId.UI_Stage),
            //            TitleNative = "stage",
            //            Stage = int.Parse(stage.Id)
            //        },
            //        int.Parse(stage.Id) == currentCategory.Stage
            //    );
            //}

            //btnGO = Instantiate(SpacerItemPrefab);
            //btnGO.transform.SetParent(SubmenuContainer.transform, false);

            List<GenericCategoryData> categoriesList = new List<GenericCategoryData>();
            GenericCategoryData categoryData;
            foreach (WordDataCategory wordCat in GenericHelper.SortEnums<WordDataCategory>())
            {
                if (wordCat == WordDataCategory.None)
                    continue;
                // TODO hack disable these word categories
                //if (wordCat == WordDataCategory.Expressions
                //    || wordCat == WordDataCategory.NumbersOrdinal
                //    || wordCat == WordDataCategory.StateOfBeing
                //    || wordCat == WordDataCategory.Jobs) continue;

                if (GetWordsByCategory(wordCat).Count < 1)
                {
                    continue;
                }

                var catLocData = LocalizationManager.GetWordCategoryData(wordCat);
                if (catLocData == null)
                    continue;
                categoryData = new GenericCategoryData
                {
                    area = VocabularyChapter.Words,
                    wordCategory = wordCat,
                    Id = catLocData.Id,
                    TitleLearning = catLocData.LearningText.ToUpper(),
                    TitleNative = catLocData.NativeText.ToUpper(),
                    TitleHelp = catLocData.HelpText,
                    Stage = 0
                };
                categoriesList.Add(categoryData);
                //               Debug.Log(categoryData.Id + " - " + catLocData.NativeText + " - " + catLocData.LearningText);
            }
            categoriesList.Sort((x, y) => string.Compare(x.TitleLearning, y.TitleLearning));

            foreach (var category in categoriesList)
            {
                btnGO = Instantiate(CategoryItemPrefab);
                btnGO.transform.SetParent(SubmenuContainer.transform, false);
                btnGO.GetComponent<MenuItemCategory>().Init(
                    this,
                    category,
                    currentCategory.wordCategory == category.wordCategory
                );
            }
        }

        private int WordComparison(WordData x, WordData y)
        {
            bool compareUsingValues = !string.IsNullOrEmpty(x.SortValue) && !string.IsNullOrEmpty(y.SortValue);
            if (compareUsingValues)
            {
                return int.Parse(x.SortValue) - int.Parse(y.SortValue);
            }
            else
            {
                return string.Compare(x.Text, y.Text);
            }
        }

        public void DetailWord(WordInfo _currentWord)
        {
            currentWordInfo = _currentWord;
            DetailPanel_Inner.SetActive(true);
            HighlightWordItem(currentWordInfo.data.Id);
            PlayWord();

            // empty spelling container
            foreach (Transform t in SpellingContainer.transform)
            {
                Destroy(t.gameObject);
            }
            var splittedLetters = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).SplitWord(AppManager.I.DB, currentWordInfo.data, false);
            foreach (var letter in splittedLetters)
            {
                btnGO = Instantiate(SpellingLetterItemPrefab);
                btnGO.transform.SetParent(SpellingContainer.transform, false);
                if (LanguageSwitcher.I.IsLearningLanguageRTL())
                {
                    btnGO.transform.SetAsFirstSibling();
                }
                btnGO.GetComponent<ItemSpellingLetter>().Init(letter.letter);
            }

            WordTitleText.SetText(currentWordInfo.data.Text, Font2Use.Learning);

            if (currentWordInfo.data.DrawingId != "")
            {
                WordDrawingText.SetLetterData(new LL_ImageData(currentWordInfo.data));
            }
            else
            {
                WordDrawingText.text = "";
            }

            if (DebugConfig.I.DebugLogEnabled)
            {
                Debug.Log("Detail Word(): " + currentWordInfo.data.Id);
                Debug.Log("drawing code: " + currentWordInfo.data.DrawingId);
                Debug.Log("word unicodes: " + LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetStringUnicodes(currentWordInfo.data.Text));
                Debug.Log("word unicodes forms: " + LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetStringUnicodes(WordTitleText.RenderedText));
            }
            //ScoreText.text = "Score: " + currentWord.score;
        }

        public void Update()
        {
            if (Book.I.EditDiacritics)
            {
                // Clear so it will be re-rendered again
                WordTitleText.text = "";
                WordTitleText.text = currentWordInfo.data.Text;
            }
        }

        public void SelectSubCategory(GenericCategoryData _category)
        {
            switch (_category.area)
            {
                case VocabularyChapter.Words:
                    KeeperManager.I.PlayDialogue(LocalizationManager.GetWordCategoryData(_category.wordCategory).Id, KeeperMode.LearningNoSubtitles);
                    //AudioManager.I.PlayDialogue(LocalizationManager.GetWordCategoryData(_category.wordCategory));
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
            foreach (Transform t in ListContainer.transform)
            {
                t.GetComponent<ItemWord>().Select(id);
            }
        }

        void HighlightMenutCategory(string id)
        {
            foreach (Transform t in SubmenuContainer.transform)
            {
                t.GetComponent<MenuItemCategory>().Select(id);
            }
        }

        void emptyListContainers()
        {
            foreach (Transform t in ListContainer.transform)
            {
                Destroy(t.gameObject);
            }
            // reset vertical position
            ListPanel.GetComponent<UnityEngine.UI.ScrollRect>().verticalNormalizedPosition = 1.0f;

            foreach (Transform t in SubmenuContainer.transform)
            {
                Destroy(t.gameObject);
            }
        }

    }
}
