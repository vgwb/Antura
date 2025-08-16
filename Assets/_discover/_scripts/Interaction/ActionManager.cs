using Antura.Audio;
using Antura.Helpers;
using Antura.Discover.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using TMPro;

namespace Antura.Discover
{
    public class ActionManager : MonoBehaviour
    {
        public static ActionManager I;

        [Tooltip("if set, it will be resolved at the start of the game")]
        public string DebugAction;

        [Tooltip("The starting location of the player")]
        public GameObject PlayerSpawnPoint;

        public QuestActionData[] QuestActions;

        [Header("Specific")]
        private Transform target_AnturaLocation;
        public Transform Target_AnturaLocation { get => target_AnturaLocation; set => target_AnturaLocation = value; }

        public GameObject WinFx;
        public GameObject AnturaDog;

        private PlayerController PlayerController;

        private GameObject currentArea;

        void Awake()
        {
            if (I == null)
            {
                I = this;
            }
            else
            {
                Debug.LogError("ActionManager DUPLICATED!");
                Destroy(gameObject);
            }
        }

        IEnumerator Start()
        {
            PlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            Target_AnturaLocation = null;
            if (AnturaDog != null)
            {
                AnturaDog.SetActive(false);
            }
            if (WinFx != null)
            {
                WinFx.SetActive(false);
            }

            RespawnPlayer();
            yield return null;

            if (DebugAction != "")
            {
                ResolveQuestAction(DebugAction);
                //PlayerController.SpawnToNewLocation(GetActionData(CommandType.Area, DebugAction.Substring(5)).DebugSpawn.transform);
            }
            else
            {
                ResolveQuestAction("init");
            }
            // TODO RUN INIT
            //InteractionManager.I.DisplayNode(QuestManager.I.GetQuestNode("init"));
        }

        private void SetPlayerSpawnPoint(GameObject spawnPoint)
        {
            PlayerSpawnPoint = spawnPoint;
        }
        public void RespawnPlayer()
        {
            if (PlayerSpawnPoint != null)
            {
                PlayerController.SpawnToNewLocation(PlayerSpawnPoint.transform);
            }
        }

        public void ResolveNodeCommandAssetHide()
        {
            UIManager.I.dialogues.HidePostcardFromDialog();
        }

        public void ResolveNodeCommandAsset(string assetCode)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandAsset: {assetCode}");
            if (string.IsNullOrEmpty(assetCode))
                return;
            var db = DatabaseProvider.Instance;
            //var assetImage = db.Get<ItemData>("assetCode");
            if (db.TryGet<AssetData>(assetCode, out var assetImage))
            {
                UIManager.I.dialogues.ShowPostcardFromDialog(assetImage.Image);
            }
        }

        public void ResolveNodeCommandTaskStart(string taskCode)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandTaskStart: {taskCode}");
            if (string.IsNullOrEmpty(taskCode))
                return;

            //var task = QuestManager.I.GetTaskByCode(taskCode);
            //if (task == null)
            //{
            //    Debug.LogError($"ActionManager: Task not found for command {taskCode}");
            //    return;
            //}

            //QuestManager.I.TaskStart(task);
            ResolveQuestAction(taskCode);
        }

        public void ResolveNodeCommandTaskEnd(string taskCode)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandTaskEnd: {taskCode}");
            if (string.IsNullOrEmpty(taskCode))
                return;

            //var task = QuestManager.I.GetTaskByCode(taskCode);
            //if (task == null)
            //{
            //    Debug.LogError($"ActionManager: Task not found for command {taskCode}");
            //    return;
            //}

            //QuestManager.I.TaskEnd(task);
            ResolveQuestAction(taskCode);
        }

        public void ResolveNodeCommandEndquest(int finalStars = 0)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandEndquest: {finalStars}");

            QuestEnd questResult = new QuestEnd();
            questResult.questId = QuestManager.I.CurrentQuest.Id;
            questResult.stars = finalStars;
            DiscoverAppManager.I.RecordQuestEnd(questResult);
        }

        public void ResolveNodeCommandInventory(string itemCode, string action = "add")
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

        public void ResolveNodeCommandCard(string cardId)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandCard: {cardId}");
            DiscoverAppManager.I.RecordCardInteraction(cardId, true);
        }

        public void ResolveNodeCommandActivity(string activityCode, string difficulty = null)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandActivity: {activityCode} {difficulty}");
            if (string.IsNullOrEmpty(activityCode))
                return;

            //var activity = QuestManager.I.GetActivityByCode(command);
            // if (activity == null)
            //{
            //    Debug.LogError($"ActionManager: Activity not found for command {command}");
            //    return;
            //}

            //QuestManager.I.ActivityStart(activity);
            ResolveQuestAction(activityCode);
        }

        public void ResolveQuestAction(string action, QuestNode node = null)
        {
            action = action.ToLower();

            if (action == "update_ui")
            {
                QuestManager.I.UpateItemsCounter();
                QuestManager.I.UpateCoinsCounter();
                return;
            }
            else if (action == "game_end" || action == "win")
            {
                QuestEnd();
                return;
            }
            else
            {
                var actionData = QuestActions.FirstOrDefault(a => a.ActionCode.ToLower() == action);
                if (actionData == null)
                {
                    Debug.LogError("Action not found: " + action);
                    return;
                }

                if (QuestManager.I.DebugQuest)
                    Debug.Log("Resolve QuestAction Data: " + actionData.ActionCode);

                if (actionData.Commands == null || actionData.Commands.Count == 0)
                {
                    Debug.LogError("No commands found for action: " + actionData.ActionCode);
                    return;
                }
                ResolveCommands(actionData.Commands);

            }
        }

        public void ResolveCommands(List<CommandData> commands)
        {
            foreach (var command in commands)
            {
                if (command.Bypass)
                {
                    if (QuestManager.I.DebugQuest)
                        //                        Debug.Log("Command is disabled: " + command.Command);
                        continue;
                }
                switch (command.Command)
                {
                    case CommandType.Activity:
                        QuestManager.I.ActivityStart(command.mainObject);
                        break;
                    case CommandType.Area:
                        ChangeArea(command.mainObject);
                        break;
                    case CommandType.Bones:
                        QuestManager.I.OnCollectBones(1);
                        break;
                    case CommandType.Collect:
                        QuestManager.I.OnCollectItemCode(command.Parameter);
                        break;
                    case CommandType.InventoryAdd:
                        QuestManager.I.OnCollectItemCode(command.mainObject.ToString());
                        break;
                    case CommandType.InventoryRemove:
                        QuestManager.I.RemoveItemCode(command.mainObject.ToString());
                        break;
                    case CommandType.PlaySfx:
                        command.mainObject.GetComponent<ActionAbstract>().Trigger();
                        break;
                    case CommandType.ProgressPoints:
                        QuestManager.I.AddProgressPoints(int.Parse(command.Parameter));
                        break;
                    case CommandType.QuestEnd:
                        QuestManager.I.OnQuestEnd();
                        break;
                    case CommandType.SetActive:
                        if (command.mainObject != null)
                        {
                            if (command.Parameter == "0")
                                command.mainObject.SetActive(false);
                            else
                                command.mainObject.SetActive(true);
                        }
                        break;
                    case CommandType.SpawnSet:
                        SetPlayerSpawnPoint(command.mainObject);
                        break;
                    case CommandType.SpawnPlayer:
                        RespawnPlayer();
                        break;
                    case CommandType.Target:
                        FocusTarget(command.mainObject.transform);
                        break;
                    case CommandType.Trigger:
                        command.mainObject.GetComponent<ActionAbstract>().Trigger();
                        break;
                    case CommandType.TaskStart:
                        QuestManager.I.TaskStart(command.Parameter);
                        break;
                    case CommandType.TaskSuccess:
                        QuestManager.I.TaskSuccess(command.Parameter);
                        break;
                    case CommandType.TaskFail:
                        QuestManager.I.TaskFail(command.Parameter);
                        break;
                    case CommandType.UnityAction:
                        if (command.unityAction != null)
                            command.unityAction.Invoke();
                        break;
                    default:
                        Debug.LogError("Unknown command type: " + command.Command);
                        break;
                }
            }
        }

        public void FocusTarget(Transform targetGO)
        {
            if (targetGO == null)
            {
                Debug.LogError("Target GameObject is null.");
                return;
            }
            InteractionManager.I.FocusCameraOn(targetGO);
            InteractionManager.I.ActivateWorldTargetIcon(true, targetGO);
        }

        private void ChangeArea(GameObject area)
        {
            if (currentArea != null)
            {
                currentArea.SetActive(false);
            }
            currentArea = area;
            currentArea.SetActive(true);
        }

        private void QuestEnd()
        {
            WinFx.SetActive(true);
            WinFx.GetComponent<ParticleSystem>().Play();
            AudioManager.I.PlaySound(Sfx.Win);
            AnturaDog.SetActive(true);
            QuestManager.I.OnQuestEnd();
        }

        #region Debug Methods
        public void TestQuestEnd()
        {
            QuestEnd();
        }
        #endregion
    }
}
