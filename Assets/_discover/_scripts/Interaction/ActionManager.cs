using Antura.Audio;
using Antura.Helpers;
using Antura.Discover.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

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

        public ActionData[] Actions;

        [Header("Specific")]
        private Transform target_AnturaLocation;
        public Transform Target_AnturaLocation { get => target_AnturaLocation; set => target_AnturaLocation = value; }

        public GameObject WinFx;
        public GameObject AnturaDog;

        private EdPlayer PlayerController;

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
            PlayerController = GameObject.FindWithTag("Player").GetComponent<EdPlayer>();

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
            InteractionManager.I.DisplayNode(QuestManager.I.GetQuestNode("init"));
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

        private ActionData GetActionData(string actionCode)
        {
            return Actions.FirstOrDefault(action => action.ActionCode == actionCode);
        }

        private ActionData GetActionData(CommandType type, string actionCode)
        {
            return Actions.FirstOrDefault(action => action.Type == type && action.ActionCode == actionCode);
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
                        Debug.Log("Command is disabled: " + command.Command);
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
