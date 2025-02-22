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

        public string DebugArea;

        [Tooltip("The starting location of the player")]
        public GameObject PlayerSpawnPoint;

        public ActionData[] Actions;

        [Header("Specific")]
        private Transform target_AnturaLocation;
        public Transform Target_AnturaLocation { get => target_AnturaLocation; set => target_AnturaLocation = value; }

        public GameObject WinFx;
        public GameObject AnturaDog;

        public GameObject Player;

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
            Player = GameObject.FindWithTag("Player");

            if (!QuestManager.I.DebugQuest)
            {
                foreach (var action in Actions)
                {
                    if (action.Type == ActionType.Area)
                    {
                        if (action.Area != null)
                            action.Area?.SetActive(false);
                        if (action.Beam != null)
                            action.Beam.SetActive(false);
                        if (action.Walls != null)
                            action.Walls.SetActive(false);
                    }
                }
            }

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

            if (DebugArea != "")
            {
                ResolveAction(DebugArea);
                Player.GetComponent<EdPlayer>().SpawnToNewLocation(GetActionData(ActionType.Area, DebugArea.Substring(5)).DebugSpawn.transform);
            }
            else
            {
                ResolveAction("area_init");
            }
        }

        public void RespawnPlayer()
        {
            if (PlayerSpawnPoint != null)
            {
                Player.GetComponent<EdPlayer>().SpawnToNewLocation(PlayerSpawnPoint.transform);
            }
        }

        private ActionData GetActionData(string actionCode)
        {
            return Actions.FirstOrDefault(action => action.ActionCode == actionCode);
        }

        private ActionData GetActionData(ActionType type, string actionCode)
        {
            return Actions.FirstOrDefault(action => action.Type == type && action.ActionCode == actionCode);
        }

        public void CameraShowTarget(string targetArea)
        {
            if (QuestManager.I.DebugQuest)
                Debug.Log("CameraShowTarget targetArea:" + targetArea);

            var actionData = GetActionData(ActionType.Area, targetArea);

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
            var actionData = GetActionData(ActionType.Area, targetArea);
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
                    Player.GetComponent<EdPlayer>().SpawnToNewLocation(actionData.SpawnPlayer.transform);
                }

            }
            else
            {
                Debug.Log("ActivateArea: Could not find targetArea: " + targetArea);
            }
        }

        private void Spawn(string spawnCode)
        {
            var actionData = GetActionData(ActionType.Spawn, spawnCode);
            // Debug.Log("Spawn spawnCode: " + spawnCode);
            // Debug.Log("Spawn actionData: " + actionData.ActionCode);
            // Debug.Log("Spawn EdPlayer: " + Player.GetComponent<EdPlayer>().name);
            // Debug.Log("Spawn actionData.Target.transform: " + actionData.Target.name);

            Player.GetComponent<EdPlayer>().SpawnToNewLocation(actionData.SpawnPlayer.transform);
        }

        private void Collect(string collectCode)
        {
            //var actionData = GetActionData(ActionType.Spawn, spawnCode);
            // Debug.Log("Spawn spawnCode: " + spawnCode);
            // Debug.Log("Spawn actionData: " + actionData.ActionCode);
            // Debug.Log("Spawn EdPlayer: " + Player.GetComponent<EdPlayer>().name);
            // Debug.Log("Spawn actionData.Target.transform: " + actionData.Target.name);

            //Player.GetComponent<EdPlayer>().SpawnToNewLocation(actionData.Target.transform);
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
                        WinFx.SetActive(true);
                        WinFx.GetComponent<ParticleSystem>().Play();
                        AudioManager.I.PlaySound(Sfx.Win);
                        AnturaDog.SetActive(true);
                        break;
                    default:
                        Trigger(action);
                        break;
                }
            }
        }

        #region DEBUG
        public void DebugActionEnd()
        {
            ResolveAction("win");
        }

        #endregion

    }
}
