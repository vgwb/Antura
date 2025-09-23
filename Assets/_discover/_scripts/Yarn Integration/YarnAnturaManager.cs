using Antura.Discover.Activities;
using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;

namespace Antura.Discover
{
    public class YarnAnturaManager : MonoBehaviour
    {
        public static YarnAnturaManager I { get; private set; }

        [Header("Yarn References")]
        [SerializeField] private DialogueRunner runner;
        [SerializeField] private DiscoverDialoguePresenter presenter;
        [SerializeField] private DiscoverLineProvider lineProvider;

        public event Action<string> OnNodeStarted;
        public event Action<QuestNode> OnQuestNode;
        public event Action<QuestNode> OnQuestOptions;
        public event Action OnDialogueComplete;

        public DialogueRunner Runner => runner;
        public DiscoverLineProvider LineProvider => lineProvider;

        private void Awake()
        {
            if (I != null && I != this)
            {
                Destroy(gameObject);
                return;
            }
            I = this;
        }

        private void OnValidate()
        {
            if (!runner)
            {
                runner = FindFirstObjectByType<DialogueRunner>(FindObjectsInactive.Include);

            }
            if (!presenter)
            {
                presenter = FindFirstObjectByType<DiscoverDialoguePresenter>(FindObjectsInactive.Include);
            }
        }

        private void Start()
        {
            if (runner != null)
            {
                runner.onNodeStart.AddListener(nodeName => { OnNodeStarted?.Invoke(nodeName); presenter?.SetCurrentNodeName(nodeName); });
                runner.onDialogueComplete.AddListener(() => OnDialogueComplete?.Invoke());

                //Hook Commands into Yarn

                // runner.AddCommandHandler<string>("camera_focus", (arg) =>
                // {
                //     var focus = gameObject.FindCameraFocus(arg);
                //     CameraManager.I.FocusOn(focus.LookAt, focus.Origin);
                // });
                // runner.AddCommandHandler("camera_reset", () =>
                // {
                //     CameraManager.I.ResetFocus();
                // });
            }

            if (presenter != null)
            {
                presenter.Manager = this;
            }
        }

        public void Setup()
        {
            // any setup goes here

        }

        public void InitNode(string nodeName)
        {
            StartDialogue(nodeName);
        }

        public void StartDialogue(string nodeName)
        {
            if (runner == null || string.IsNullOrEmpty(nodeName))
                return;
            if (!runner.IsDialogueRunning)
            {
                runner.StartDialogue(nodeName);
            }
            else
            {
                runner.RequestNextLine();
            }
        }

        /// <summary>
        /// Sets the DialogueRunner's Yarn Project.
        /// </summary>
        public void SetYarnProject(YarnProject project)
        {
            try
            {
                if (runner == null)
                {
                    runner = FindFirstObjectByType<DialogueRunner>(FindObjectsInactive.Include);
                }
                if (runner != null)
                {
                    runner.SetProject(project);
                    Debug.Log($"[Yarn] Project set: {(project != null ? project.name : "<null>")}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"YarnAnturaManager: SetYarnProject failed: {ex.Message}");
            }
        }

        internal void EmitQuestNode(QuestNode node)
        {
            OnQuestNode?.Invoke(node);
        }

        internal void EmitOptionsNode(QuestNode node)
        {
            OnQuestOptions?.Invoke(node);
        }

        // ------------------------------------------------------------
        // YARN COMMANDS
        // ------------------------------------------------------------

        [YarnCommand("custom_wait")]
        public static IEnumerator CustomWait()
        {
            // Because this method returns IEnumerator, it's a coroutine.
            // Yarn Spinner will wait until onComplete is called.

            // Debug.Log("YarnAnturaManager: CustomWait");
            // Wait for 1 second
            yield return new WaitForSeconds(4.0f);

        }

        // ------------------------------------------------------------
        // ACTION
        // ------------------------------------------------------------

        [YarnCommand("action")]
        public static void CommandAction(string actionCode)
        {
            if (string.IsNullOrEmpty(actionCode))
                return;
            ActionManager.I.ResolveQuestAction(actionCode);
        }

        // ------------------------------------------------------------
        // ACTIVITY
        // ------------------------------------------------------------

        [YarnCommand("activity")]
        public static void CommandActivity(string activitySettingsCode, string nodeReturn = "")
        {
            //Debug.Log($"Yarn: activity {activityCode} -> {nodeReturn}");
            if (string.IsNullOrEmpty(activitySettingsCode))
                return;
            ActivityManager.I?.Launch(activitySettingsCode, nodeReturn);
        }

        [YarnFunction("GetActivityResult")]
        public static int FunctionGetActivityResult(string activitySettingsCode)
        {
            if (QuestManager.I == null || string.IsNullOrEmpty(activitySettingsCode))
                return 0;
            return ActivityManager.I?.GetResult(activitySettingsCode) ?? 0;
        }

        // ------------------------------------------------------------
        // AREA
        // ------------------------------------------------------------

        [YarnCommand("area")]
        public static void CommandArea(string actionCode)
        {
            if (string.IsNullOrEmpty(actionCode))
                return;
            ActionManager.I.ResolveAreaCommand(actionCode);
        }

        // ------------------------------------------------------------
        // ASSET
        // ------------------------------------------------------------

        [YarnCommand("asset")]
        public static void CommandAsset(string assetCode)
        {
            //Debug.Log($"ActionManager: ResolveNodeCommandAsset: {assetCode}");
            if (string.IsNullOrEmpty(assetCode))
                return;
            var db = DatabaseProvider.I;
            //var assetImage = db.Get<ItemData>("assetCode");
            if (db.TryGet<AssetData>(assetCode, out var assetImage))
            {
                UIManager.I.dialogues.ShowPostcard(assetImage.Image);
            }
        }

        [YarnCommand("asset_hide")]
        public static void CommandAssetHide()
        {
            UIManager.I.dialogues.HidePostcard();
        }

        // ------------------------------------------------------------
        // CAMERA
        // ------------------------------------------------------------

        [YarnCommand("camera_focus")]
        public static IEnumerator CommandCameraFocus(string cameraCode)
        {
            Debug.Log("YarnAnturaManager: camera_focus");
            if (string.IsNullOrEmpty(cameraCode))
                yield break;

            var focus = ActionManager.I.FindCameraFocus(cameraCode);
            if (focus != null)
            {
                I.StartCoroutine(CameraManager.I.FocusOn(focus));
            }
            else
            {
                Debug.LogWarning($"CommandCameraFocus: focus not found for code {cameraCode}");
            }
        }

        [YarnCommand("camera_reset")]
        public static void CommandCameraReset()
        {
            CameraManager.I.ResetFocus();
        }


        // ------------------------------------------------------------
        // CARDS / ASSETS
        // ------------------------------------------------------------

        [YarnCommand("card")]
        public static void CommandCard(string cardId, string zoom = "")
        {
            if (string.IsNullOrEmpty(cardId))
                return;
            // Debug.Log($"ActionManager: ResolveNodeCommandCard: {cardId}");
            DatabaseProvider.TryGet<CardData>(cardId, out var c);
            CardData card = c;

            bool openZoomed = (zoom.ToLower() == "zoom");
            UIManager.I.dialogues.ShowPostcard(card.ImageAsset.Image, card.Title.GetLocalizedString(), openZoomed);
            DiscoverAppManager.I.RecordCardInteraction(card, true);
        }

        [YarnCommand("card_hide")]
        public static void CommandCardHide()
        {
            UIManager.I.dialogues.HidePostcard();
        }

        // ------------------------------------------------------------
        // COOKIES
        // ------------------------------------------------------------

        /// <summary>
        /// Usage in Yarn: <<cookies_add 5>>
        /// </summary>
        [YarnCommand("cookies_add")]
        public static void CommandCookiesAdd(int amount)
        {
            QuestManager.I.OnCollectCookie(amount, true);
        }

        [YarnFunction("GetCookies")]
        public static int FunctionGetCookies()
        {
            return QuestManager.I.Inventory.GetCookies();
        }

        // ------------------------------------------------------------
        // PARTY
        // ------------------------------------------------------------
        //  <<party_join "LETTER_A_ID">>
        //  <<party_release "LETTER_A_ID">> // if no id, remove all
        //  <<party_formation "circle">>

        private static PartyManager GetPartyManager()
        {
            return FindFirstObjectByType<PartyManager>();
        }

        [YarnCommand("party_join")]
        public static void PartyJoinById(string id)
        {
            var pm = GetPartyManager();
            if (pm == null || string.IsNullOrEmpty(id))
                return;

            // TODO improve
            foreach (var m in FindObjectsByType<PartyMember>(FindObjectsSortMode.None))
            {
                if (m.GetComponent<Interactable>().Id == id)
                {
                    pm.AddMember(m);
                    return;
                }
            }
            Debug.LogWarning($"[PartyYarn] party_join: member with Id '{id}' not found in scene.");
        }

        [YarnCommand("party_release")]
        public static void PartyReleaseById(string id = "")
        {
            var pm = GetPartyManager();
            if (pm == null)
                return;
            if (string.IsNullOrEmpty(id))
                pm.RemoveAllFollowers();
            else
                pm.RemoveMemberById(id);
        }

        [YarnCommand("party_formation")]
        public static void PartySetFormation(string name)
        {
            var pm = GetPartyManager();
            if (pm == null)
                return;
            pm.SetFormation(name);
        }

        // ------------------------------------------------------------
        // TARGET
        // ------------------------------------------------------------

        [YarnCommand("target")]
        public static void CommandTarget(string targetCode)
        {
            if (string.IsNullOrEmpty(targetCode))
                return;
            ActionManager.I.ResolveTargetCommand(targetCode);
        }

        // ------------------------------------------------------------
        // TASK
        // ------------------------------------------------------------

        [YarnCommand("task_start")]
        public static void CommandTaskStart(string taskCode, string nodeReturn)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandTaskStart: {taskCode}");
            if (string.IsNullOrEmpty(taskCode))
                return;
            QuestManager.I?.TaskManager?.StartTask(taskCode, nodeReturn);
        }

        [YarnCommand("task_end")]
        public static void CommandTaskEnd(string taskCode)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandTaskEnd: {taskCode}");
            if (string.IsNullOrEmpty(taskCode))
                return;
            QuestManager.I?.TaskManager.EndTask(taskCode, true);
        }

        // Yarn functions for querying task state
        [YarnFunction("GetCurrentTask")]
        public static string FunctionGetCurrentTask()
        {
            return QuestManager.I != null ? (QuestManager.I.TaskManager?.CurrentTaskCode ?? string.Empty) : string.Empty;
        }

        [YarnFunction("GetCollectedItem")]
        public static int FunctionGetCollectedItem(string taskCode)
        {
            if (QuestManager.I == null || string.IsNullOrEmpty(taskCode))
                return 0;
            return QuestManager.I.TaskManager?.GetCollectedCount(taskCode) ?? 0;
        }

        [YarnFunction("HasCompletedTask")]
        public static bool FunctionHasCompletedTask(string taskCode)
        {
            if (QuestManager.I == null || string.IsNullOrEmpty(taskCode))
                return false;
            return QuestManager.I.TaskManager?.IsTaskCompleted(taskCode) ?? false;
        }

        // ------------------------------------------------------------
        // QUEST
        // ------------------------------------------------------------

        [YarnCommand("quest_end")]
        public static void CommandQuestEnd()
        {
            //Debug.Log($"ActionManager: ResolveNodeCommandEndquest: {finalStars}");
            QuestManager.I.QuestEnd();
        }

        // ------------------------------------------------------------
        // INVENTORY
        // ------------------------------------------------------------

        [YarnCommand("inventory")]
        public static void CommandInventory(string itemCode, string action = "add")
        {
            Debug.Log($"ActionManager: ResolveNodeCommandInventory: {itemCode} {action}");
            if (string.IsNullOrEmpty(itemCode))
                return;

            if (action == "add")
            {
                QuestManager.I.Inventory.CollectItem(itemCode);
            }
            else if (action == "remove")
            {
                QuestManager.I.Inventory.RemoveItem(itemCode);
            }
            else
            {
                Debug.LogError($"ActionManager: Unknown inventory action: {action}");
            }
        }

        [YarnFunction("item_count")]
        public static float FunctionItemCount(string itemCode)
        {
            var inv = QuestManager.I?.Inventory;
            if (inv == null || string.IsNullOrEmpty(itemCode))
                return 0f;
            return inv.GetItemCount(itemCode);
        }

        [YarnFunction("has_item")]
        public static bool FunctionHasItem(string itemCode)
        {
            var inv = QuestManager.I?.Inventory;
            if (inv == null || string.IsNullOrEmpty(itemCode))
                return false;
            return inv.HasItem(itemCode);
        }

        [YarnFunction("has_item_at_least")]
        public static bool FunctionHasItemAtLeast(string itemCode, float minQty)
        {
            var inv = QuestManager.I?.Inventory;
            if (inv == null || string.IsNullOrEmpty(itemCode))
                return false;
            int min = Mathf.Max(1, Mathf.RoundToInt(minQty));
            return inv.HasItem(itemCode, min);
        }

        [YarnFunction("can_collect")]
        public static bool FunctionCanCollect(string itemCode)
        {
            var inv = QuestManager.I?.Inventory;
            if (inv == null || string.IsNullOrEmpty(itemCode))
                return false;
            return inv.CanCollect(itemCode);
        }

        // ------------------------------------------------------------
        // TRIGGERS
        // ------------------------------------------------------------
        [YarnCommand("SetActive")]
        public static void CommandSetActive(string triggerable, bool active = true)
        {
            // Debug.Log($"YarnAnturaManager: SetActive {triggerable} {active}");
            ActionManager.I.CommandSetActive(triggerable, active);
        }

        // ------------------------------------------------------------
        // UTILITIES
        // ------------------------------------------------------------

        public void DebugMetadata(string nodeName)
        {
            if (runner == null || string.IsNullOrEmpty(nodeName))
                return;

            var metadata = runner.Dialogue.GetHeaderValue(nodeName, "tags");


            Debug.Log($"TAGS for {nodeName}: {metadata}");
        }
    }
}
