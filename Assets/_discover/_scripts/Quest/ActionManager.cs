using Antura.Audio;
using System;
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

        [Tooltip("The starting location of the player, if set, goes here")]
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

                if (QuestManager.I.DebugMode)
                    Debug.Log("Resolve QuestAction Data: " + actionData.ActionCode);

                if (actionData.Commands == null || actionData.Commands.Count == 0)
                {
                    Debug.LogError("No commands found for action: " + actionData.ActionCode);
                    return;
                }
                ResolveCommands(actionData.Commands);

            }
        }

        public void ResolveAreaCommand(string areaCode)
        {
            areaCode = areaCode.ToLower();

            if (areaCode == "off")
            {
                if (currentArea != null)
                {
                    currentArea.SetActive(false);
                    currentArea = null;
                }
                return;
            }

            var actionData = QuestActions.FirstOrDefault(a => a.ActionCode.ToLower() == areaCode);
            if (actionData == null)
            {
                Debug.LogError("Action not found: " + areaCode);
                return;
            }
            ResolveCommands(actionData.Commands);

        }

        public void ResolveTargetCommand(string targetCode)
        {
            targetCode = targetCode.ToLower();

            if (targetCode == "off")
            {
                TargetOff();
                return;
            }

            var actionData = QuestActions.FirstOrDefault(a => a.ActionCode.ToLower() == targetCode);
            if (actionData == null)
            {
                Debug.LogError("Action not found: " + targetCode);
                return;
            }
            ResolveCommands(actionData.Commands);

        }

        public void CommandSetActive(string name, bool active)
        {
            // Debug.LogWarning($"CommandSetActive '{name}' set to {active}.");
            var triggerable = FindActableInChildren(name);
            if (triggerable != null)
            {
                triggerable.gameObject.SetActive(active);
            }
            else
            {
                Debug.LogWarning($"Triggerable '{name}' not found under ActionManager hierarchy.");
            }
        }

        public void ResolveCommands(List<CommandData> commands)
        {
            foreach (var command in commands)
            {
                if (command.Bypass)
                {
                    continue;
                }
                switch (command.Command)
                {
                    // case CommandType.Activity:
                    //     QuestManager.I.ActivityStart(command.mainObject);
                    //     break;
                    case CommandType.Area:
                        ChangeArea(command.mainObject);
                        break;
                    case CommandType.Cookie:
                        QuestManager.I.OnCollectCookie(1);
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
                        command.mainObject.GetComponent<ActableAbstract>().Trigger();
                        break;
                    case CommandType.ProgressPoints:
                        QuestManager.I.AddProgressPoints(int.Parse(command.Parameter));
                        break;
                    case CommandType.QuestEnd:
                        QuestManager.I.QuestEnd();
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
                        Target(command.mainObject.transform);
                        break;
                    case CommandType.TargetOff:
                        TargetOff();
                        break;
                    case CommandType.Trigger:
                        command.mainObject.GetComponent<ActableAbstract>().Trigger();
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

        public void TargetOff()
        {
            InteractionManager.I.ActivateWorldTargetIcon(false);
        }
        public void Target(Transform targetGO)
        {
            if (targetGO == null)
            {
                Debug.LogError("Target GameObject is null.");
                return;
            }
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
            QuestManager.I.QuestEnd();
        }

        public CameraFocusData FindCameraFocus(string id, bool includeInactive = true)
        {
            Debug.Log("ActionManager: FindCameraFocus: " + id);
            var items = this.GetComponentsInChildren<CameraFocusData>(includeInactive);
            Debug.Log("ActionManager: FindCameraFocus: items found " + items.Length);
            foreach (var it in items)
            {
                if (string.Equals(it.Id, id, StringComparison.Ordinal))
                    return it;
            }
            Debug.LogWarning("ActionManager: FindCameraFocus: focus not found for code " + id);
            return null;

        }

        private ActableAbstract FindActableInChildren(string name)
        {
            var list = GetComponentsInChildren<ActableAbstract>(includeInactive: true);
            if (list == null || list.Length == 0)
                return null;

            if (string.IsNullOrEmpty(name))
                return list.FirstOrDefault();

            return list.FirstOrDefault(t => string.Equals(t.gameObject.name, name, StringComparison.OrdinalIgnoreCase)
                                          || string.Equals(t.name, name, StringComparison.OrdinalIgnoreCase)
                                          || string.Equals(t.GetComponent<ActableAbstract>().Id, name, StringComparison.OrdinalIgnoreCase));
        }

        #region Debug Methods
        public void TestQuestEnd()
        {
            QuestEnd();
        }
        #endregion
    }
}
