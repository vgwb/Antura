using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;

namespace Antura.Discover
{
    /// <summary>
    /// Facade/proxy over Yarn's DialogueRunner to keep a stable API for Quest/Node systems.
    /// Not a LineProvider. Works with DiscoverDialoguePresenter to emit QuestNode events.
    /// </summary>
    public class YarnAnturaManager : MonoBehaviour
    {
        public static YarnAnturaManager I { get; private set; }

        [Header("Yarn References")]
        [SerializeField] private DialogueRunner runner;
        [SerializeField] private DiscoverDialoguePresenter presenter;

        [Header("Locale")] public string CurrentLanguage = "EN";
        public string NativeLanguage = "EN";

        public event Action<string> OnNodeStarted;
        public event Action<QuestNode> OnQuestNode;         // for text lines
        public event Action<QuestNode> OnQuestOptions;      // for choices
        public event Action OnDialogueComplete;

        public DialogueRunner Runner => runner;

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



        public void Setup(string language = "EN", string native = "EN")
        {
            CurrentLanguage = language;
            NativeLanguage = native;
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
            UIManager.I.dialogues.ShowPostcard(card.ImageAsset.Image, openZoomed);
            DiscoverAppManager.I.RecordCardInteraction(card, true);
        }

        [YarnCommand("card_hide")]
        public static void CommandCardHide()
        {
            UIManager.I.dialogues.HidePostcard();
        }

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
        // TASK
        // ------------------------------------------------------------

        [YarnCommand("task_start")]
        public static void CommandTaskStart(string taskCode)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandTaskStart: {taskCode}");
            if (string.IsNullOrEmpty(taskCode))
                return;
            TaskManager.I?.StartTask(taskCode);
        }

        [YarnCommand("task_end")]
        public static void CommandTaskEnd(string taskCode)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandTaskEnd: {taskCode}");
            if (string.IsNullOrEmpty(taskCode))
                return;
            TaskManager.I?.EndTask(taskCode, true);
        }

        // ------------------------------------------------------------
        // QUEST
        // ------------------------------------------------------------

        [YarnCommand("quest_end")]
        public static void CommandEndquest(int finalStars = -1)
        {
            // if finalStars is -1, calculate based on progress
            // if finalStars is 0, end as failed
            // if finalStars is 1,2,3 end with that many stars
            Debug.Log($"ActionManager: ResolveNodeCommandEndquest: {finalStars}");

            QuestEnd questResult = new QuestEnd();
            questResult.questId = QuestManager.I.CurrentQuest.Id;
            questResult.stars = finalStars;
            DiscoverAppManager.I.RecordQuestEnd(questResult);
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
                QuestManager.I.inventory.CollectItem(itemCode);
            }
            else if (action == "remove")
            {
                QuestManager.I.inventory.RemoveItem(itemCode);
            }
            else
            {
                Debug.LogError($"ActionManager: Unknown inventory action: {action}");
            }
        }

        // ------------------------------------------------------------
        // ACTIVITY
        // ------------------------------------------------------------

        [YarnCommand("activity")]
        public static void ResolveNodeCommandActivity(string activityCode, string difficulty = null)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandActivity: {activityCode} {difficulty}");
            if (string.IsNullOrEmpty(activityCode))
                return;
            ActionManager.I.ResolveQuestAction(activityCode);
        }

    }
}
