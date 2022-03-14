using Antura.Audio;
using Antura.Database;
using Antura.UI;
using Antura.Core;
using System.Collections.Generic;
using Antura.Keeper;
using UnityEngine;

namespace Antura.Book
{
    /// <summary>
    /// Displays information on all learning items the player has unlocked.
    /// </summary>
    public class JourneyPanel : MonoBehaviour, IBookPanel
    {
        [Header("Prefabs")]
        public GameObject CategoryItemPrefab;

        public GameObject LearningBlockItemPrefab;

        [Header("References")]
        public GameObject DetailPanel;

        public GameObject Submenu;
        public GameObject SubmenuContainer;
        public GameObject ListPanel;
        public GameObject ElementsContainer;

        public TextRender DetailCodeText;
        public TextRender DetailTitleText;
        public TextRender DetailDescriptionEn;
        public TextRender DetailDescriptionAr;
        public TextRender ScoreText;

        private int currentStage;
        private GameObject btnGO;
        private LearningBlockInfo currentLearningBlock;

        void Start()
        {
        }

        void OnEnable()
        {
            OpenArea();
        }

        void OpenArea()
        {
            KeeperManager.I.PlayDialogue(LocalizationDataId.UI_LearningBlock);
            DetailPanel.SetActive(false);
            LearningBlockPanel(AppManager.I.Player.CurrentJourneyPosition.Stage);
        }

        void LearningBlockPanel(int _stage = 1)
        {
            currentStage = _stage;

            Submenu.SetActive(true);
            ListPanel.SetActive(true);
            emptyListContainers();

            List<LearningBlockData> list = AppManager.I.DB.FindLearningBlockData((x) => (x.Stage == currentStage));

            List<LearningBlockInfo> info_list = AppManager.I.ScoreHelper.GetAllLearningBlockInfo();
            foreach (var info_item in info_list)
            {
                if (list.Contains(info_item.data))
                {
                    btnGO = Instantiate(LearningBlockItemPrefab);
                    btnGO.transform.SetParent(ElementsContainer.transform, false);
                    btnGO.GetComponent<ItemLearningBlock>().Init(this, info_item);
                }
            }

            var listStages = AppManager.I.DB.GetAllStageData();
            listStages.Reverse();
            foreach (var stage in listStages)
            {
                btnGO = Instantiate(CategoryItemPrefab);
                btnGO.transform.SetParent(SubmenuContainer.transform, false);
                btnGO.GetComponent<MenuItemCategory>().Init(
                    this,
                    new GenericCategoryData
                    {
                        area = VocabularyChapter.LearningBlock,
                        Id = stage.Id,
                        TitleLearning = stage.Id,
                        TitleNative = stage.Id
                    },
                    int.Parse(stage.Id) == currentStage
                );
            }
        }

        public void SelectSubCategory(GenericCategoryData _stage)
        {
            LearningBlockPanel(int.Parse(_stage.Id));
        }

        public void DetailLearningBlock(LearningBlockInfo _currentLearningBlock)
        {
            currentLearningBlock = _currentLearningBlock;
            DetailPanel.SetActive(true);
            AudioManager.I.PlayLearningBlock(currentLearningBlock.data.AudioFile);

            DetailCodeText.text = currentLearningBlock.data.Id;
            DetailTitleText.text = currentLearningBlock.data.Title_LearningLang;
            DetailDescriptionEn.text = currentLearningBlock.data.Description_NativeLang;
            DetailDescriptionAr.text = currentLearningBlock.data.Description_LearningLang;

            HighlightItem(currentLearningBlock.data.Id);
            ScoreText.text = "Score: " + currentLearningBlock.score;
        }

        void emptyListContainers()
        {
            foreach (Transform t in ElementsContainer.transform)
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

        void HighlightItem(string id)
        {
            foreach (Transform t in ElementsContainer.transform)
            {
                t.GetComponent<ItemLearningBlock>().Select(id);
            }
        }
    }
}
