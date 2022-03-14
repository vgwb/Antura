using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Book
{
    public class PhrasesPage : MonoBehaviour, IBookPanel
    {
        [Header("Prefabs")]
        public GameObject PhraseItemPrefab;
        public GameObject CategoryItemPrefab;

        [Header("References")]
        public GameObject DetailPanel;
        public GameObject ListPanel;
        public GameObject ListContainer;
        public GameObject Submenu;
        public GameObject SubmenuContainer;

        private PhraseDataCategory currentPhraseCategory;
        private PhraseInfo currentPhrase;
        private string currentCategory;
        private LocalizationData CategoryData;
        private GameObject btnGO;

        private void OnEnable()
        {
            PhrasesPanel();
            //PhrasesPanel(PhraseDataCategory.Expression);
        }

        void PhrasesPanel(PhraseDataCategory _category = PhraseDataCategory.None)
        {
            ListPanel.SetActive(true);
            DetailPanel.SetActive(false);
            currentPhraseCategory = _category;

            List<PhraseData> list;
            switch (currentPhraseCategory)
            {
                case PhraseDataCategory.None:
                    list = new List<PhraseData>();
                    break;
                default:
                    list = AppManager.I.DB.FindPhraseData((x) => (x.Category == currentPhraseCategory));
                    break;
            }
            emptyListContainers();

            List<PhraseInfo> info_list = AppManager.I.ScoreHelper.GetAllPhraseInfo();
            foreach (var info_item in info_list)
            {
                if (list.Contains(info_item.data))
                {
                    btnGO = Instantiate(PhraseItemPrefab);
                    btnGO.transform.SetParent(ListContainer.transform, false);
                    btnGO.GetComponent<ItemPhrase>().Init(this, info_item);
                }
            }

            foreach (PhraseDataCategory cat in GenericHelper.SortEnums<PhraseDataCategory>())
            {
                if (cat == PhraseDataCategory.None)
                { continue; }
                btnGO = Instantiate(CategoryItemPrefab);
                btnGO.transform.SetParent(SubmenuContainer.transform, false);
                CategoryData = LocalizationManager.GetPhraseCategoryData(cat);
                btnGO.GetComponent<MenuItemCategory>().Init(
                    this,
                    new GenericCategoryData
                    {
                        area = VocabularyChapter.Phrases,
                        phraseCategory = cat,
                        Id = cat.ToString(),
                        TitleLearning = CategoryData.LearningText,
                        TitleNative = CategoryData.NativeText
                    },
                    currentPhraseCategory == cat
                );
            }
        }

        public void DetailPhrase(PhraseInfo _currentPhrase)
        {
            currentPhrase = _currentPhrase;
            DetailPanel.SetActive(true);

            Debug.Log("Detail Phrase :" + currentPhrase.data.Id);
            AudioManager.I.PlayPhrase(currentPhrase.data);

            //ArabicText.text = currentPhrase.data.Arabic;
            //ScoreText.text = "Score: " + currentPhrase.score;
        }

        public void SelectSubCategory(GenericCategoryData _category)
        {
            switch (_category.area)
            {
                case VocabularyChapter.Words:
                    PhrasesPanel(_category.phraseCategory);
                    break;
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
