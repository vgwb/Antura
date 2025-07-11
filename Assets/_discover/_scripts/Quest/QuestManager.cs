using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Homer;
using Antura.Core;
using Antura.Audio;
using Antura.Utilities;
using Antura.UI;
using Antura.Language;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using Antura.Profile;

namespace Antura.Minigames.DiscoverCountry
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class QuestManager : SingletonMonoBehaviour<QuestManager>
    {
        public EdPlayer PlayerController;

        public Quests Quests;
        public QuestData CurrentQuest;
        public BonesCounter bonesCounter;
        public BonesCounter coinsCounter;
        public ItemsCounter itemsCounter;

        public ProgressCounter progressCounter;
        public string LanguageCode = "";
        public string NativeLanguageCode = "";
        private GameObject currentNPC;
        public int total_coins = 0;
        public int total_bones = 0;
        public int collected_items = 0;

        private Inventory inventory;
        private Progress progress;
        private readonly List<QuestNode> tmpQuestNodes = new List<QuestNode>(); // Used to get all QuestNodes with old system, and return a single one

        [Header("DEBUG")]
        public bool DebugQuest = false;
        public string DebugLanguage = "";

        void Start()
        {
            // INSTATIATE LEVEL PREFAB
            // Vector3 spawnPosition = new Vector3(0, 1, 0);
            // GameObject levelInstance = Instantiate(CurrentQuest.GameLevel, spawnPosition, Quaternion.identity);
            // levelInstance.transform.SetParent(null);

            PlayerController = GameObject.FindWithTag("Player").GetComponent<EdPlayer>();
            total_coins = 0;
            inventory = new Inventory();
            progress = new Progress();
            if (coinsCounter == null)
            {
                coinsCounter = GameObject.Find("CoinsCounter").GetComponent<BonesCounter>();
            }
            if (bonesCounter == null)
            {
                bonesCounter = GameObject.Find("BonesCounter").GetComponent<BonesCounter>();
            }
            if (itemsCounter == null)
            {
                itemsCounter = GameObject.Find("ItemsCounter").GetComponent<ItemsCounter>();
                itemsCounter.gameObject.SetActive(false);
            }

            if (progressCounter == null)
            {
                progressCounter = GameObject.Find("ProgressCounter").GetComponent<ProgressCounter>();
            }

            if (DebugQuest && DebugLanguage != "")
            {
                LanguageCode = DebugLanguage;
            }
            else
            {
                LanguageCode = "FR";
            }

            NativeLanguageCode = LocalizationManager.IsoLangFromLangCode(AppManager.I.AppSettings.NativeLanguage);
            //            Debug.Log("native = " + AppManager.I.AppSettings.NativeLanguage + " / " + NativeLangCode);
            HomerAnturaManager.I.Setup(LanguageCode, NativeLanguageCode);
            HomerAnturaManager.I.InitNode(CurrentQuest.QuestId);

            if (DebugQuest)
            {
                HomerVars.MET_MONALISA = true;
            }

            HomerVars.IS_DESKTOP = AppConfig.IsDesktopPlatform();
            inventory.Init(HomerVars.QUEST_ITEMS);
            progress.Init(CurrentQuest.TotalProgress);
            updateCounters();
        }

        public void OnQuestEnd()
        {
            if (DebugConfig.I.VerboseAntura)
                Debug.Log("OnQuestEnd");

            DiscoverQuestSaved questStatus = new DiscoverQuestSaved();
            questStatus.QuestCode = CurrentQuest.Code;
            questStatus.Score = 1;

            AppManager.I.Player.SaveQuest(questStatus);
        }

        public QuestNode GetQuestNode(string permalink, string command)
        {
            if (permalink != "")
            {
                return HomerAnturaManager.I.GetNodeFromPermalink(permalink, CurrentQuest.QuestId, "");
            }
            else
            {
                return HomerAnturaManager.I.GetContentByCommand(CurrentQuest.QuestId, command, true);
            }
        }

        public QuestNode GetNextNode(int choiceIndex = 0)
        {
            return HomerAnturaManager.I.NextNode(choiceIndex);
        }

        public void OnInteract(EdAgent agent)
        {
            // Debug.Log("ANTURA INTERACTS WITH LL " + agent.ActorId);
        }

        public void OnNodeStart(QuestNode node)
        {
            if (node.Action != null)
                ActionManager.I.ResolveAction(node.Action, node.Permalink);

            if (node.Permalink != "")
                progress.VisitNode(node.Permalink);
        }

        public void OnNodeEnd(QuestNode node)
        {
            if (node.NextTarget != null)
                ActionManager.I.CameraShowTarget(node.NextTarget);
            if (node.ActionPost != null)
                ActionManager.I.ResolveAction(node.ActionPost, node.Permalink);
        }

        public void OnCollectItemCode(string itemCode)
        {
            if (inventory.CollectItem(itemCode))
            {
                Debug.Log("Collect item " + itemCode);

                collected_items++;
                HomerVars.COLLECTED_ITEMS = collected_items;
                UpateItemsCounter();
            }
        }

        public void OnCollectItem(GameObject go)
        {
            collected_items++;
            HomerVars.COLLECTED_ITEMS = collected_items;
            Destroy(go);
            UpateItemsCounter();
        }

        public void OnCollectBone(GameObject go)
        {
            total_bones++;
            bonesCounter.IncreaseByOne();
            AppManager.I.Player.AddBones(1);
            Destroy(go);
        }

        private void updateCounters()
        {
            UpateCoinsCounter();
            UpateItemsCounter();
            //UpateProgressCounter();
        }
        public void UpateProgressCounter(int counter, int maxSteps)
        {
            progressCounter.UpdateProgress(counter, maxSteps);
        }
        public void UpateItemsCounter()
        {
            if (HomerVars.QUEST_ITEMS > 0)
            {
                itemsCounter.gameObject.SetActive(true);
                itemsCounter.SetMax(HomerVars.QUEST_ITEMS);
                itemsCounter.SetValue(HomerVars.COLLECTED_ITEMS);
            }
        }
        public void UpateCoinsCounter()
        {
            coinsCounter.SetValue(HomerVars.TOTAL_COINS);
        }
        public void OnCollectCoin(GameObject go)
        {
            total_coins++;
            HomerVars.TOTAL_COINS = total_coins;
            coinsCounter.IncreaseByOne();
            Debug.Log("ANTURA COLLECTS coin nr " + HomerVars.TOTAL_COINS);
            Destroy(go);
        }

        #region Debug
        public void PrintDebugInfo()
        {
            var output = "";
            output += "DEBUG INFO";
            output += "\nQuest: " + CurrentQuest.Code;
            output += "\nQuest Score: " + AppManager.I.Player.GetQuestStatus(CurrentQuest.Code).Score;
            output += "\nNative Language: " + AppManager.I.AppSettings.NativeLanguage;
            output += "\nLearning Language: " + AppManager.I.ContentEdition.LearningLanguage;
            //config = LanguageSwitcher.I.GetLangConfig(languageUse);

            Debug.Log(output);
        }
        #endregion

    }
}
