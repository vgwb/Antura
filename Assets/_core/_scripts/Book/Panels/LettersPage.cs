using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using Antura.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Book
{
    public class LettersPage : MonoBehaviour, BookDataManager
    {
        [Header("Prefabs")]
        public GameObject LetterItemPrefab;
        public GameObject DiacriticSymbolItemPrefab;
        public GameObject WordItemPrefab;

        [Header("References")]
        public GameObject DetailPanel;
        public GameObject MainLetterPanel;
        public GameObject ListPanelLTR;
        public GameObject ListPanelRTL;
        public GameObject ListContainerLTR;
        public GameObject ListContainerRTL;

        public LetterAllForms MainLetterDisplay;
        public GameObject DiacriticsContainer;
        public GameObject RelatedWordsContainer;

        private GameObject ListPanel;
        private GameObject ListContainer;
        private LetterInfo myLetterInfo;
        private LetterData myLetterData;
        private GameObject btnGO;

        #region Letters

        private void OnEnable()
        {
            LettersPanel();
        }

        private void LettersPanel()
        {
            if (LanguageSwitcher.I.IsLearningLanguageRTL())
            {
                ListPanel = ListPanelRTL;
                ListContainer = ListContainerRTL;
                ListPanelLTR.SetActive(false);
                ListPanelRTL.SetActive(true);
            }
            else
            {
                ListPanel = ListPanelLTR;
                ListContainer = ListContainerLTR;
                ListPanelLTR.SetActive(true);
                ListPanelRTL.SetActive(false);
            }

            ListPanel.SetActive(true);
            DetailPanel.SetActive(false);
            emptyContainer(ListContainer);

            List<LetterData> letters = AppManager.I.DB.FindLetterData((x) => (x.Kind == LetterDataKind.Letter && x.InBook));
            letters.Sort((x, y) => x.Number.CompareTo(y.Number));

            //adds Letter Vaiations
            List<LetterData> lettersVariations = AppManager.I.DB.FindLetterData((x) => (x.Kind == LetterDataKind.LetterVariation && x.InBook));
            lettersVariations.Sort((x, y) => x.Number.CompareTo(y.Number));
            letters.AddRange(lettersVariations);

            //adds Symbols
            List<LetterData> symbols = AppManager.I.DB.FindLetterData((x) => (x.Kind == LetterDataKind.Symbol && x.InBook));
            symbols.Sort((x, y) => x.Number.CompareTo(y.Number));
            letters.AddRange(symbols);

            List<LetterInfo> allLetterInfos = AppManager.I.ScoreHelper.GetAllLetterInfo();
            foreach (var letter in letters)
            {
                LetterInfo myLetterinfo = allLetterInfos.Find(value => value.data == letter);

                btnGO = Instantiate(LetterItemPrefab);
                btnGO.transform.SetParent(ListContainer.transform, false);
                if (LanguageSwitcher.I.IsLearningLanguageRTL())
                {
                    btnGO.transform.SetAsFirstSibling();
                }
                btnGO.GetComponent<ItemLetter>().Init(this, myLetterinfo, false);
            }

            MainLetterPanel.GetComponent<Button>().onClick.AddListener(BtnClickMainLetterPanel);
        }

        #endregion
        public void DetailLetter(LetterInfo letterInfo)
        {
            DetailPanel.SetActive(true);
            myLetterInfo = letterInfo;
            myLetterData = letterInfo.data;

            if (DebugConfig.I.VerboseBook)
                Debug.Log("[DetailLetter] number: " + myLetterData.Number + "  - Id: " + myLetterData.Id);

            HighlightLetterItem(myLetterInfo.data.Id);

            if (AppManager.I.ContentEdition.LearnMethod.ShowLinkedWordsInBook)
            {
                // show related words
                RelatedWordsContainer.SetActive(true);
                DiacriticsContainer.SetActive(false);
                emptyContainer(RelatedWordsContainer);
                foreach (var word in letterInfo.data.LinkedWords)
                {
                    WordData wdata = AppManager.I.DB.GetWordDataById(word);
                    var winfo = new WordInfo
                    {
                        data = wdata
                    };

                    btnGO = Instantiate(WordItemPrefab);
                    btnGO.transform.SetParent(RelatedWordsContainer.transform, false);
                    btnGO.GetComponent<ItemWord>().Init(this, winfo, false);
                }
            }
            else
            {
                // show related diacritics
                RelatedWordsContainer.SetActive(false);
                DiacriticsContainer.SetActive(true);
                emptyContainer(DiacriticsContainer);
                var letterbase = myLetterInfo.data.Id;
                var variationsletters = AppManager.I.DB.FindLetterData(
                    (x) => (x.BaseLetter == letterbase && (x.Kind == LetterDataKind.DiacriticCombo && x.Active))
                );
                variationsletters.Sort((x, y) => x.Number - y.Number);

                // diacritics box
                var letterGO = Instantiate(DiacriticSymbolItemPrefab);
                letterGO.transform.SetParent(DiacriticsContainer.transform, false);
                letterGO.GetComponent<ItemDiacriticSymbol>().Init(this, myLetterInfo, true);

                List<LetterInfo> info_list = AppManager.I.ScoreHelper.GetAllLetterInfo();
                info_list.Sort((x, y) => x.data.Number - y.data.Number);
                foreach (var info_item in info_list)
                {
                    if (variationsletters.Contains(info_item.data))
                    {
                        if (AppConfig.DisableShaddah && info_item.data.Symbol == "shaddah")
                        {
                            continue;
                        }
                        btnGO = Instantiate(DiacriticSymbolItemPrefab);
                        btnGO.transform.SetParent(DiacriticsContainer.transform, false);
                        btnGO.GetComponent<ItemDiacriticSymbol>().Init(this, info_item, false);
                        //debug_output += info_item.data.GetDebugDiacriticFix();
                    }
                }
            }

            //Debug.Log(debug_output);
            ShowLetter(myLetterInfo);
        }

        private void ShowLetter(LetterInfo letterInfo, LetterDataSoundType soundType = LetterDataSoundType.Name)
        {
            myLetterInfo = letterInfo;
            myLetterData = letterInfo.data;

            string positionsString = "";
            foreach (var p in letterInfo.data.GetAvailableForms())
            {
                positionsString = positionsString + " " + p;
            }
            MainLetterDisplay.Init(myLetterData);
            //LetterScoreText.text = "Score: " + myLetterInfo.score;

            HighlightDiacriticItem(myLetterData.Id);
            playSound(soundType);

            // Debug.Log(myLetterData.GetDebugDiacriticFix());
        }

        public void DetailWord(WordInfo _word)
        {
            AudioManager.I.PlayWord(_word.data);
        }

        private void BtnClickMainLetterPanel()
        {
            AudioManager.I.PlayLetter(myLetterData, true, LetterDataSoundType.Phoneme);
        }

        private void playSound(LetterDataSoundType soundType)
        {
            AudioManager.I.PlayLetter(myLetterData, true, soundType);
        }

        public void ShowDiacriticCombo(LetterInfo newLetterInfo)
        {
            ShowLetter(newLetterInfo, LetterDataSoundType.Phoneme);
        }

        private void HighlightLetterItem(string id)
        {
            foreach (Transform t in ListContainer.transform)
            {
                t.GetComponent<ItemLetter>().Select(id);
            }
        }

        private void HighlightDiacriticItem(string id)
        {
            foreach (Transform t in DiacriticsContainer.transform)
            {
                t.GetComponent<ItemDiacriticSymbol>().Select(id);
            }
        }

        private void emptyContainer(GameObject container)
        {
            foreach (Transform t in container.transform)
            {
                Destroy(t.gameObject);
            }
            // reset vertical position
            //ListPanel.GetComponent<UnityEngine.UI.ScrollRect>().verticalNormalizedPosition = 1.0f;
        }
    }
}
