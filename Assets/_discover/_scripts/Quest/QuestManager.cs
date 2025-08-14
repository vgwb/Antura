using Antura.Core;
using Antura.Audio;
using Antura.Discover.Activities;
using Antura.Language;
using Antura.Profile;
using Antura.UI;
using Antura.Utilities;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;
namespace Antura.Discover
{
    public class QuestManager : SingletonMonoBehaviour<QuestManager>
    {
        [Header("Yarn")]
        [SerializeField] private YarnConversationController conversation;
        [SerializeField] private YarnAnturaManager yarnManager;

        public QuestListData Quests;
        public QuestData CurrentQuest;
        public Task[] QuestTasks;
        private Task CurrentTask;
        private string CurrentActivity;

        private Inventory inventory;
        private Progress progress;
        private readonly List<QuestNode> tmpQuestNodes = new List<QuestNode>();

        [Header("Quest Settings")]
        [Tooltip("Force the EASY_MODE var")]
        public bool EasyMode = false;
        public string LanguageCode = "";
        public string NativeLanguageCode = "";
        private GameObject currentNPC;
        private PlayerController playerController;
        public int total_coins = 0;
        public int total_bones = 0;
        public int collected_items = 0;

        [Header("DEBUG")]
        public bool DebugQuest = false;
        public string DebugLanguage = "";

        void Start()
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            total_coins = 0;
            if (QuestTasks != null)
            {
                foreach (var task in QuestTasks)
                    task.Setup();
            }

            inventory = new Inventory();
            progress = new Progress();

            LanguageCode = (DebugQuest && DebugLanguage != "") ? DebugLanguage : "FR";
            NativeLanguageCode = LocalizationManager.IsoLangFromLangCode(AppManager.I.AppSettings.NativeLanguage);

            yarnManager = yarnManager ? yarnManager : YarnAnturaManager.I;
            if (yarnManager == null)
            {
#if UNITY_2023_1_OR_NEWER
                yarnManager = UnityEngine.Object.FindFirstObjectByType<YarnAnturaManager>(FindObjectsInactive.Include);
#else
                yarnManager = UnityEngine.Object.FindObjectOfType<YarnAnturaManager>(true);
#endif
            }
            yarnManager?.Setup(LanguageCode, NativeLanguageCode);

            // Initialize inventory target from Yarn variables if present
            int questItemsTarget = GetIntVar("$QUEST_ITEMS", 0);
            inventory.Init(questItemsTarget);
            progress.Init(QuestTasks);
            updateCounters();

            // Subscribe to Yarn manager events to run actions and show UI
            if (yarnManager != null)
            {
                yarnManager.OnNodeStarted += OnYarnNodeStarted;
                yarnManager.OnQuestNode += OnYarnQuestNode;
                yarnManager.OnQuestOptions += OnYarnQuestOptions;
                yarnManager.OnDialogueComplete += OnYarnDialogueComplete;
            }

            // Start the quest's starting node via Yarn (convention: "init")
            yarnManager?.InitNode("init");
        }

        public void OnQuestEnd()
        {
            if (DebugConfig.I.VerboseAntura)
                Debug.Log("OnQuestEnd");

            yarnManager?.StartDialogue("quest_end");

            DiscoverQuestSaved questStatus = new DiscoverQuestSaved
            {
                QuestCode = CurrentQuest.Id
            };
            AppManager.I.Player.SaveQuest(questStatus);
        }

        public void YarnGetQuestNode(string nodeName)
        {
            Debug.Log($"YarnGetQuestNode: {nodeName}");
            conversation?.DebugMetadata(nodeName);
            conversation?.StartDialogue(nodeName);
        }

        public void TaskStart(string taskCode)
        {
            if (QuestTasks == null)
                return;
            foreach (var task in QuestTasks)
            {
                if (task.Code == taskCode)
                {
                    CurrentTask = task;
                    CurrentTask.Activate();
                    return;
                }
            }
        }

        public void TaskSuccess(string taskCode = "")
        {
            if (CurrentTask == null)
                return;

            if (taskCode != "" && CurrentTask.Code != taskCode)
            {
                Debug.LogError($"TaskSuccess called with taskCode {taskCode}, but current task is {CurrentTask.Code}");
                return;
            }

            UIManager.I.TaskDisplay.Hide();
            progress.AddProgressPoints(CurrentTask.GetSuccessPoints());

            if (!string.IsNullOrEmpty(CurrentTask.NodeSuccess))
                yarnManager?.StartDialogue(CurrentTask.NodeSuccess);

            CurrentTask = null;
        }

        public void AddProgressPoints(int points)
        {
            progress.AddProgressPoints(points);
        }

        public void TaskFail(string taskCode = "")
        {
            if (CurrentTask != null)
            {
                UIManager.I.TaskDisplay.Hide();
                if (!string.IsNullOrEmpty(CurrentTask.NodeFail))
                    yarnManager?.StartDialogue(CurrentTask.NodeFail);
                CurrentTask = null;
            }
        }

        public void ActivityStart(GameObject activityObject)
        {
            Debug.Log("ActivityStart: " + activityObject.name);
            CurrentActivity = activityObject.GetComponent<ActivityBase>().ActivityCode;
            activityObject.GetComponent<ActivityPanel>().Open();
        }

        public void OnNodeStart(QuestNode node)
        {
            if (!string.IsNullOrEmpty(node.Action))
                ActionManager.I.ResolveQuestAction(node.Action, node);
        }

        public void OnNodeEnd(QuestNode node)
        {
            if (!string.IsNullOrEmpty(node.Task))
                TaskStart(node.Task);
            if (!string.IsNullOrEmpty(node.ActionPost))
                ActionManager.I.ResolveQuestAction(node.ActionPost, node);
        }

        public void OnCollectItemCode(string itemCode)
        {
            if (inventory.CollectItem(itemCode))
            {
                Debug.Log("Collect item " + itemCode);
                collected_items++;
                SetIntVar("$COLLECTED_ITEMS", collected_items);
                UpateItemsCounter();
            }
        }

        public void RemoveItemCode(string itemCode)
        {
            if (inventory != null && inventory.RemoveItem(itemCode))
            {
                Debug.Log("Remove item " + itemCode);
                collected_items--;
                SetIntVar("$COLLECTED_ITEMS", collected_items);
                UpateItemsCounter();
            }
        }

        public void OnCollectItem(string tag)
        {
            collected_items++;
            SetIntVar("$COLLECTED_ITEMS", collected_items);
            if (!string.IsNullOrEmpty(tag) && CurrentTask != null)
                CurrentTask.ItemCollected();
            AudioManager.I.PlaySound(Sfx.ScaleUp);
        }

        public void OnCollectBone()
        {
            total_bones++;
            UIManager.I.BonesCounter.SetValue(total_bones);
            AppManager.I.Player.AddBones(1);
        }

        public void OnCollectBones(int bones)
        {
            total_bones += bones;
            UIManager.I.BonesCounter.SetValue(total_bones);
            AppManager.I.Player.AddBones(bones);
        }

        private void updateCounters()
        {
            UpateCoinsCounter();
            UpateItemsCounter();
        }

        public void UpdateProgressScore(int counter)
        {
            UIManager.I.ProgressDisplay.SetCurrentScore(counter);
        }

        public void UpateItemsCounter()
        {
            int questItemsTarget = GetIntVar("$QUEST_ITEMS", 0);
            if (questItemsTarget > 0)
            {
                UIManager.I.TaskDisplay.gameObject.SetActive(true);
                UIManager.I.TaskDisplay.SetTargetItems(questItemsTarget);
                UIManager.I.TaskDisplay.SetTotItemsCollected(collected_items);
            }
        }

        public void UpateCoinsCounter()
        {
            total_coins = GetIntVar("$TOTAL_COINS", total_coins);
            UIManager.I.CoinsCounter.SetValue(total_coins);
        }

        public void OnCollectCoin()
        {
            total_coins++;
            SetIntVar("$TOTAL_COINS", total_coins);
            UIManager.I.CoinsCounter.IncreaseByOne();
            Debug.Log("ANTURA COLLECTS coin nr " + total_coins);
        }

        // Yarn bindings
        private QuestNode _lastYarnNode;
        private void OnYarnNodeStarted(string nodeName)
        {
            if (DebugQuest)
                Debug.Log($"[Yarn] Node started: {nodeName}");
        }
        private void OnYarnQuestNode(QuestNode node)
        {
            _lastYarnNode = node;
            OnNodeStart(node);
        }
        private void OnYarnQuestOptions(QuestNode node)
        {
            _lastYarnNode = node;
        }
        private void OnYarnDialogueComplete()
        {
            if (_lastYarnNode != null)
            {
                OnNodeEnd(_lastYarnNode);
                _lastYarnNode = null;
            }
        }

        // Helpers for Yarn variable access
        private int GetIntVar(string name, int fallback)
        {
            var storage = yarnManager?.Runner?.VariableStorage;
            if (storage != null && storage.TryGetValue<float>(name, out var num))
                return Mathf.RoundToInt(num);
            return fallback;
        }

        private void SetIntVar(string name, int value)
        {
            var storage = yarnManager?.Runner?.VariableStorage;
            if (storage == null)
                return;
            storage.SetValue(name, value);
        }

        #region Debug
        public void PrintDebugInfo()
        {
            var output = "";
            output += "DEBUG INFO";
            output += "\nQuest: " + CurrentQuest.Id;
            output += "\nQuest Score: " + AppManager.I.Player.GetQuestStatus(CurrentQuest.Id).Score;
            output += "\nNative Language: " + AppManager.I.AppSettings.NativeLanguage;
            output += "\nLearning Language: " + AppManager.I.ContentEdition.LearningLanguage;
            Debug.Log(output);
        }
        #endregion

    }
}
