using Antura.Core;
using Antura.Audio;
using Antura.Discover.Activities;
using Antura.Profile;
using Antura.UI;
using Antura.Utilities;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using DG.DeExtensions;

namespace Antura.Discover
{
    public class QuestManager : SingletonMonoBehaviour<QuestManager>
    {
        public QuestData CurrentQuest;
        public List<ActivityConfig> ActivityConfigs;

        public QuestTask[] QuestTasks;

        private QuestTask CurrentTask;

        public InventoryManager Inventory;
        public ProgressManager Progress;
        public QuestTaskManager TaskManager;

        [Header("DEBUG")]
        public bool DebugQuest = false;
        public string DebugLanguage = "";
        private bool _debugQuestApplied;

        [Tooltip("Force the EASY_MODE var")]
        public bool EasyMode = false;

        [Header("Readonly")]
        public string LanguageCode = "";
        public string NativeLanguageCode = "";
        private GameObject currentNPC;
        private PlayerController playerController;
        private int total_coins = 0;
        private int collected_items = 0;


        protected override void Init()
        {
            Inventory = new InventoryManager();
            Progress = new ProgressManager();
            TaskManager = new QuestTaskManager(this);
        }

        void Start()
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            total_coins = 0;

            LanguageCode = (DebugQuest && DebugLanguage != "") ? DebugLanguage : "FR";
            NativeLanguageCode = LocalizationManager.IsoLangFromLangCode(AppManager.I.AppSettings.NativeLanguage);

            var yarnManager = YarnAnturaManager.I;
            if (yarnManager == null)
            {
                yarnManager = FindFirstObjectByType<YarnAnturaManager>(FindObjectsInactive.Include);
            }
            yarnManager?.Setup(LanguageCode, NativeLanguageCode);

            // Initialize inventory target from Yarn variables if present
            int questItemsTarget = GetIntVar("$QUEST_ITEMS", 0);
            Inventory.Init(questItemsTarget);
            // Initialize and register tasks for this quest
            if (QuestTasks != null)
            {
                foreach (var t in QuestTasks)
                {
                    if (t != null)
                        t.Setup();
                }
            }

            TaskManager.RegisterTasks(QuestTasks);

            // Initialize progress via local tasks
            Progress.Init(QuestTasks);
            updateCounters();

            // Task registration moved to ActionManager.Start()

            // Subscribe to Yarn manager events to run actions and show UI
            if (yarnManager != null)
            {
                yarnManager.OnNodeStarted += OnYarnNodeStarted;
                yarnManager.OnQuestNode += OnYarnQuestNode;
                yarnManager.OnQuestOptions += OnYarnQuestOptions;
                yarnManager.OnDialogueComplete += OnYarnDialogueComplete;
            }

            // DiscoverGameManager is responsible for starting the intro Yarn node

            ApplyInteractableDebugLabels(DebugQuest);

            // TODO, maybe in taskmanager
            UIManager.I.ProgressDisplay.Setup(30);
        }

        void Update()
        {
            if (DebugQuest != _debugQuestApplied)
            {
                ApplyInteractableDebugLabels(DebugQuest);
            }
        }

        void OnDestroy()
        {
            var yarnManager = YarnAnturaManager.I;
            if (yarnManager != null)
            {
                yarnManager.OnNodeStarted -= OnYarnNodeStarted;
                yarnManager.OnQuestNode -= OnYarnQuestNode;
                yarnManager.OnQuestOptions -= OnYarnQuestOptions;
                yarnManager.OnDialogueComplete -= OnYarnDialogueComplete;
            }
        }

        public void QuestEnd()
        {
            if (DebugConfig.I.VerboseAntura)
                Debug.Log("QuestEnd");

            QuestEnd questResult = new QuestEnd();
            questResult.questId = CurrentQuest.Id;
            questResult.stars = Progress.GetCurrentStarsAchieved();
            DiscoverGameManager.I.GameEnd(questResult, Inventory.GetCookies());
            //YarnAnturaManager.I?.StartDialogue("quest_end");
        }

        public void StartDialogue(string nodeName)
        {
            //Debug.Log($"YarnGetQuestNode: {nodeName}");
            //conversation?.DebugMetadata(nodeName);
            YarnAnturaManager.I?.StartDialogue(nodeName);
        }

        public void TaskStart(string taskCode)
        {
            if (string.IsNullOrEmpty(taskCode))
                return;
            // Prefer lookup via TaskManager registration
            if (TaskManager.TryGetTask(taskCode, out var tmTask) && tmTask != null)
            {
                CurrentTask = tmTask;
                CurrentTask.Activate();
                return;
            }
            // Fallback: search in locally-serialized tasks
            var list = QuestTasks;
            if (list == null)
                return;
            foreach (var t in list)
            {
                if (t != null && t.Code == taskCode)
                {
                    CurrentTask = t;
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
            Progress.AddProgressPoints(CurrentTask.GetSuccessPoints());

            if (!string.IsNullOrEmpty(CurrentTask.NodeSuccess))
                YarnAnturaManager.I?.StartDialogue(CurrentTask.NodeSuccess);

            CurrentTask = null;
        }

        public void AddProgressPoints(int points)
        {
            Progress.AddProgressPoints(points);
        }

        public void TaskFail(string taskCode = "")
        {
            if (CurrentTask != null)
            {
                UIManager.I.TaskDisplay.Hide();
                if (!string.IsNullOrEmpty(CurrentTask.NodeFail))
                    YarnAnturaManager.I?.StartDialogue(CurrentTask.NodeFail);
                CurrentTask = null;
            }
        }

        // public void ActivityStart(GameObject activityObject)
        // {
        //     // Debug.Log("ActivityStart: " + activityObject.name);
        //     var activityBase = activityObject.GetComponent<ActivityBase>();
        //     CurrentActivity = activityBase.ActivityCode;
        //     activityBase.Open();
        // }

        public void OnNodeStart(QuestNode node)
        {
            if (!string.IsNullOrEmpty(node.Action))
                ActionManager.I.ResolveQuestAction(node.Action, node);
        }

        public void OnNodeEnd(QuestNode node)
        {
            if (!string.IsNullOrEmpty(node.Task))
                TaskManager?.StartTask(node.Task);
            if (!string.IsNullOrEmpty(node.ActionPost))
                ActionManager.I.ResolveQuestAction(node.ActionPost, node);
        }

        public void OnCollectItemCode(string itemCode)
        {
            if (Inventory.CollectItem(itemCode))
            {
                Debug.Log("Collect item " + itemCode);
                collected_items++;
                SetIntVar("$COLLECTED_ITEMS", collected_items);
                UpateItemsCounter();
            }
        }

        public void RemoveItemCode(string itemCode)
        {
            if (Inventory != null && Inventory.RemoveItem(itemCode))
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
            // route to task manager per-task logic
            TaskManager?.OnCollectItemTag(tag);
            AudioManager.I.PlaySound(Sfx.ScaleUp);
        }

        public void OnCollectCookie(int quantity = 1)
        {
            Inventory.AddCookies(quantity);
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
            var storage = YarnAnturaManager.I?.Runner?.VariableStorage;
            if (storage != null && storage.TryGetValue<float>(name, out var num))
                return Mathf.RoundToInt(num);
            return fallback;
        }

        private void SetIntVar(string name, int value)
        {
            var storage = YarnAnturaManager.I?.Runner?.VariableStorage;
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
            // output += "\nQuest Score: " + AppManager.I.Player.GetQuestStatus(CurrentQuest.Id).Score;
            output += "\nNative Language: " + AppManager.I.AppSettings.NativeLanguage;
            output += "\nLearning Language: " + AppManager.I.ContentEdition.LearningLanguage;
            Debug.Log(output);
        }

        private void ApplyInteractableDebugLabels(bool enable)
        {
            _debugQuestApplied = DebugQuest;
            var interactables = Object.FindObjectsByType<Interactable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var it in interactables)
            {
                if (enable)
                {
                    var label = it.GetComponentInChildren<Debugging.InteractableDebugLabel>(true);
                    if (label == null)
                    {
                        var go = new GameObject("_DebugLabel");
                        go.transform.SetParent(it.transform, false);
                        label = go.AddComponent<Debugging.InteractableDebugLabel>();
                    }
                    label.gameObject.SetActive(true);
                    label.UpdateText();
                }
                else
                {
                    var label = it.GetComponentInChildren<Debugging.InteractableDebugLabel>(true);
                    if (label != null)
                        label.gameObject.SetActive(false);
                }
            }
        }
        #endregion

    }
}
