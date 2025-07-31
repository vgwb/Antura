using Antura.Audio;
using Antura.Helpers;
using Antura.Minigames.DiscoverCountry.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
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

        private GameObject currentInvisibleWalls;

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

            // if (!QuestManager.I.DebugQuest)
            // {
            //     foreach (var action in Actions)
            //     {
            //         if (action.Type == CommandType.Area)
            //         {
            //             if (action.Area != null)
            //                 action.Area?.SetActive(false);
            //             if (action.Beam != null)
            //                 action.Beam.SetActive(false);
            //             if (action.Walls != null)
            //                 action.Walls.SetActive(false);
            //         }
            //     }
            // }

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
                ResolveAction(DebugAction);
                PlayerController.SpawnToNewLocation(GetActionData(CommandType.Area, DebugAction.Substring(5)).DebugSpawn.transform);
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

            var actionData = QuestActions.FirstOrDefault(a => a.ActionCode.ToLower() == action);
            if (actionData == null)
            {
                Debug.LogError("Action not found: " + action);
                return;
            }

            if (QuestManager.I.DebugQuest)
                Debug.Log("Resolve QuestAction Data: " + actionData.ActionCode);

            foreach (var command in actionData.Commands)
            {
                if (command.Disabled)
                {
                    if (QuestManager.I.DebugQuest)
                        Debug.Log("Command is disabled: " + command.Command);
                    continue;
                }
                switch (command.Command)
                {
                    case CommandType.UnityAction:
                        if (command.unityAction != null)
                            command.unityAction.Invoke();
                        break;
                    case CommandType.InventoryAdd:
                        QuestManager.I.OnCollectItemCode(command.mainObject.ToString());
                        break;
                    case CommandType.InventoryRemove:
                        QuestManager.I.RemoveItemCode(command.mainObject.ToString());
                        break;
                    case CommandType.Bones:
                        QuestManager.I.OnCollectBones(1);
                        break;
                    case CommandType.Trigger:
                        command.mainObject.GetComponent<ActionAbstract>().Trigger();
                        break;
                    case CommandType.Area:
                        ActivateArea(command.mainObject.name);
                        break;
                    case CommandType.SetRespawn:
                        SetPlayerSpawnPoint(command.mainObject);
                        break;
                    case CommandType.PlayerSpawn:
                        RespawnPlayer();
                        break;
                    case CommandType.Collect:
                        Collect(command.mainObject.name);
                        break;
                    case CommandType.Activity:
                        command.mainObject.GetComponent<ActivityPanel>().Open();
                        break;
                    case CommandType.End:
                        command.mainObject.GetComponent<ActivityPanel>().Open();
                        break;
                    default:
                        Debug.LogError("Unknown command type: " + command.Command);
                        break;
                }

            }
        }

        public void ResolveObjective(QuestNode node)
        {
            if (QuestManager.I.DebugQuest)
                Debug.Log("Resolve Objective: " + node.Objective);

            if (node.Objective == null)
                return;

            UIManager.I.ObjectiveDisplay.Show(node.Objective, 0);

        }

        public void ResolveNextTarget(string targetArea)
        {
            if (QuestManager.I.DebugQuest)
                Debug.Log("CameraShowTarget targetArea:" + targetArea);

            var actionData = GetActionData(CommandType.Area, targetArea);

            if (QuestManager.I.DebugQuest)
                Debug.Log("CameraShowTarget ActionCode:" + actionData.ActionCode);

            InteractionManager.I.FocusCameraOn(actionData.Target.transform);

            if (actionData.Beam != null)
            {
                actionData.Beam.SetActive(true);
            }

            if (actionData.Target.transform != null)
            {
                InteractionManager.I.ActivateWorldTargetIcon(true, actionData.Target.transform);
            }
        }

        private void ActivateArea(string targetArea)
        {
            var actionData = GetActionData(CommandType.Area, targetArea);
            if (actionData != null)
            {
                if (QuestManager.I.DebugQuest)
                    Debug.Log("ActivateArea: " + actionData.ActionCode);

                if (actionData.Area != null)
                {
                    actionData.Area.SetActive(true);
                }

                if (actionData.Beam != null)
                {
                    actionData.Beam.SetActive(true);
                }

                if (actionData.Target != null)
                {
                    InteractionManager.I.ActivateWorldTargetIcon(true, actionData.Target.transform);
                }
                else
                {
                    InteractionManager.I.ActivateWorldTargetIcon(false);
                }

                if (actionData.Walls != null)
                {
                    if (currentInvisibleWalls != null)
                    {
                        currentInvisibleWalls.SetActive(false);
                    }
                    currentInvisibleWalls = actionData.Walls;
                    currentInvisibleWalls.SetActive(true);
                }

                if (actionData.SpawnPlayer != null)
                {
                    PlayerController.GetComponent<EdPlayer>().SpawnToNewLocation(actionData.SpawnPlayer.transform);
                }

            }
            else
            {
                Debug.Log("ActivateArea: Could not find targetArea: " + targetArea);
            }
        }

        private void Spawn(string spawnCode)
        {
            var actionData = GetActionData(CommandType.PlayerSpawn, spawnCode);

            PlayerController.GetComponent<EdPlayer>().SpawnToNewLocation(actionData.SpawnPlayer.transform);
        }

        private void Collect(string collectCode)
        {
            QuestManager.I.OnCollectItemCode(collectCode);
        }

        private void Trigger(string actionCode)
        {
            var actionData = GetActionData(actionCode);
            if (actionData != null)
            {
                if (QuestManager.I.DebugQuest)
                    Debug.Log("Trigger: " + actionData.ActionCode);
                actionData.mainObject.GetComponent<ActionAbstract>().Trigger();
            }
            else
            {
                Debug.LogError("Trigger: Could not find actionCode: " + actionCode);
            }
        }

        public void ResolveAction(string action, string permalink = "")
        {
            action = action.ToLower();
            if (QuestManager.I.DebugQuest)
                Debug.Log("ResolveAction: " + action);

            if (action.SafeSubstring(0, 5) == "area_")
            {
                ActivateArea(action.Substring(5));
            }
            else if (action.SafeSubstring(0, 8) == "collect_")
            {
                if (permalink != "")
                {
                    Collect(permalink);
                }
                else
                {
                    Collect(action.Substring(8));
                }
            }
            else if (action.SafeSubstring(0, 6) == "spawn_")
            {
                Spawn(action.Substring(6));
            }
            else
            {
                switch (action)
                {
                    case "update_items":
                        QuestManager.I.UpateItemsCounter();
                        break;
                    case "updatecoins":
                    case "update_coins":
                        QuestManager.I.UpateCoinsCounter();
                        break;
                    case "win":
                    case "game_end":
                        QuestEnd();
                        break;
                    default:
                        Trigger(action);
                        break;
                }
            }
        }

        private void QuestEnd()
        {
            WinFx.SetActive(true);
            WinFx.GetComponent<ParticleSystem>().Play();
            AudioManager.I.PlaySound(Sfx.Win);
            AnturaDog.SetActive(true);
            QuestManager.I.OnQuestEnd();
        }

        #region DEBUG
        public void DebugActionEnd()
        {
            ResolveAction("win");
        }

        #endregion

    }
}
