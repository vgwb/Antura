using Antura.Core;
using Antura.Audio;
using Antura.Discover.Activities;
using Antura.Utilities;
using AdventurEd;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;
using System;
using System.Collections;

namespace Antura.Discover
{
    [Serializable]
    public class QuestDebugConfig
    {
        [Tooltip("Dialogue node to start")]
        public DialogueReference DebugNode = new();
        public GameObject DebugSpawnPoint;

        [Header("Override player settings")]
        public TalkToPlayerMode TalkToPlayerMode = TalkToPlayerMode.Default;
        public LanguageCode NativeLanguage;
        public LanguageCode LearningLanguage;
    }

    public class QuestManager : SingletonMonoBehaviour<QuestManager>
    {
        public QuestData CurrentQuest;
        public bool DebugMode;
        private bool _debugQuestApplied;
        public QuestDebugConfig DebugConfig;

        public List<ActivityConfig> ActivityConfigs;
        public QuestTask[] QuestTasks;
        public InventoryManager Inventory;
        public ProgressManager Progress;
        public QuestTaskManager TaskManager;

        private GameObject currentNPC;
        private int total_coins = 0;
        public int TotalCoins => total_coins;
        private int collected_items = 0;

        public TalkToPlayerMode TalkToPlayerMode { get; private set; }
        public bool HasTranslation => TalkToPlayerMode == TalkToPlayerMode.LearningThenNative || TalkToPlayerMode == TalkToPlayerMode.NativeThenLearning;
        public bool LearningLangFirst => TalkToPlayerMode == TalkToPlayerMode.LearningLanguageOnly || TalkToPlayerMode == TalkToPlayerMode.LearningThenNative;

        protected override void Init()
        {
            Inventory = new InventoryManager();
            Progress = new ProgressManager();
            TaskManager = new QuestTaskManager();
        }

        void Start()
        {

            var yarnManager = YarnAnturaManager.I;
            if (yarnManager == null)
            {
                yarnManager = FindFirstObjectByType<YarnAnturaManager>(FindObjectsInactive.Include);
            }
            yarnManager?.Setup();

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

            ApplyInteractableDebugLabels(DebugMode);

            // TODO, maybe in taskmanager
            UIManager.I.ProgressDisplay.Setup(CurrentQuest.ProgressPoints > 0 ? CurrentQuest.ProgressPoints : TaskManager.GetMaxPoints());
        }

        void OnEnable()
        {
            // setup World
            var root = WorldManager.I ? WorldManager.I.Current : FindFirstObjectByType<WorldController>();
            if (!root)
                return;

            var effective = root.GetEffectiveSetup(CurrentQuest ? CurrentQuest.WorldSetup : null);
            if (effective)
                root.ApplySetup(effective);
        }

        void Update()
        {
            if (DebugMode != _debugQuestApplied)
            {
                ApplyInteractableDebugLabels(DebugMode);
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

        public void QuestStart()
        {
            TalkToPlayerMode = DiscoverAppManager.I.CurrentProfile.profile.talkToPlayerMode;
            // Debug.Log("QuestStart with TalkToPlayerMode: " + TalkToPlayerMode);
            if (DebugMode)
            {
                if (DebugConfig.TalkToPlayerMode != TalkToPlayerMode.Default)
                    TalkToPlayerMode = DebugConfig.TalkToPlayerMode;
                // if (DebugConfig.LearningLanguage != "")
                // if (DebugConfig.NativeLanguage != "")
                // {
                //     // DiscoverAppManager.I.CurrentProfile.
                //     // AppManager.I..SetNativeLanguage(DebugConfig.NativeLanguageCode);
                // }
            }

            YarnAnturaManager.I.Variables.IS_DESKTOP = AppConfig.IsDesktopPlatform();
            YarnAnturaManager.I.Variables.EASY_MODE = DiscoverAppManager.I.CurrentProfile.profile.easyMode;

            var currentItemCode = Inventory?.CurrentItem != null ? Inventory.CurrentItem.Code : string.Empty;
            YarnAnturaManager.I.Variables.CURRENT_ITEM = currentItemCode ?? string.Empty;

            if (DebugMode && DebugConfig.DebugNode != null)
            {
                StartDialogue(DebugConfig.DebugNode);
            }
            else
            {
                StartDialogue("quest_start");
            }
        }

        public void QuestEnd()
        {
            if (DebugMode)
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


        public void OnInteractCard(CardData card)
        {
            TaskManager.OnInteractCard(card);
        }

        public void AddProgressPoints(int points)
        {
            Progress.AddProgressPoints(points);
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
                //Debug.Log("Collect item " + itemCode);
                collected_items++;
                // YarnAnturaManager.I.Variables.COLLECTED_ITEMS = collected_items;
                // UpateItemsCounter();
            }
        }

        public void RemoveItemCode(string itemCode)
        {
            if (Inventory != null && Inventory.RemoveItem(itemCode))
            {
                Debug.Log("Remove item " + itemCode);
                collected_items--;
                //YarnAnturaManager.I.Variables.COLLECTED_ITEMS = collected_items;
            }
        }

        public void OnCollectItem(string tag)
        {
            collected_items++;
            //YarnAnturaManager.I.Variables.COLLECTED_ITEMS = collected_items;
            // route to task manager per-task logic
            TaskManager?.OnCollectItemTag(tag);
            AudioManager.I.PlaySound(Sfx.ScaleUp);
        }

        public void OnCollectCookie(int quantity = 1, bool animate = false)
        {
            Inventory.AddCookies(quantity, animate);
        }

        private void updateCounters()
        {
            UpateCoinsCounter();
        }

        public void UpdateProgressScore(int counter)
        {
            UIManager.I.ProgressDisplay.SetCurrentScore(counter);
        }

        public void UpateCoinsCounter()
        {
            // YarnAnturaManager.I.Variables.TOTAL_COINS = total_coins;
            UIManager.I.CoinsCounter.SetValue(total_coins);
        }

        public void OnCollectCoin()
        {
            total_coins++;
            // YarnAnturaManager.I.Variables.TOTAL_COINS = total_coins;
            UIManager.I.CoinsCounter.IncreaseByOne();
            //Debug.Log("ANTURA COLLECTS coin nr " + total_coins);
        }

        // Yarn bindings
        private QuestNode _lastYarnNode;
        private void OnYarnNodeStarted(string nodeName)
        {
            if (DebugMode)
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
            _debugQuestApplied = DebugMode;
            var interactables = FindObjectsByType<Interactable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
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
