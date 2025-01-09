using Antura.Audio;
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

        public ActionData[] Actions;

        [Header("Specific")]
        private Transform target_AnturaLocation;
        public Transform Target_AnturaLocation { get => target_AnturaLocation; set => target_AnturaLocation = value; }

        public Transform Spawn_Louvre_Enter;
        public Transform Spawn_Louvre_Exit;

        public GameObject WinFx;
        public GameObject AnturaDog;

        private GameObject Player;


        void Awake()
        {
            if (I == null)
            {
                I = this;
            }
            else
            {
                Debug.Log("ActionManager DUPLICATED!");
                Destroy(gameObject);
            }
        }

        void Start()
        {
            Player = GameObject.FindWithTag("Player");

            foreach (var action in Actions)
            {
                if (action.Type == ActionType.Area)
                {
                    action.Area.SetActive(false);
                    action.Beam.SetActive(false);
                }
            }

            Target_AnturaLocation = null;
            AnturaDog.SetActive(false);
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
            Debug.Log("CameraShowTarget targetArea:" + targetArea);

            var actionData = GetActionData(ActionType.Area, targetArea);

            Debug.Log("CameraShowTarget ActionCode:" + actionData.ActionCode);
            InteractionManager.I.FocusCameraOn(actionData.Target.transform);
            actionData.Beam.SetActive(true);
            Target_AnturaLocation = actionData.Target.transform;

            if (Target_AnturaLocation != null)
            {
                InteractionManager.I.ActivateWorldTargetIcon(true, Target_AnturaLocation);
            }
        }

        private void ActivateArea(string targetArea)
        {
            var actionData = GetActionData(ActionType.Area, targetArea);
            if (actionData != null)
            {
                Debug.Log("ActivateArea: " + actionData.ActionCode);
                actionData.Area.SetActive(true);
            }
            else
            {
                Debug.Log("ActivateArea: Could not find targetArea: " + targetArea);
            }
        }

        private void Spawn(string spawnCode)
        {
            var actionData = GetActionData(ActionType.Spawn, spawnCode);
            Player.GetComponent<EdPlayer>().SpawnToNewLocation(actionData.Target.transform);
        }

        public void ResolveAction(string action)
        {
            Debug.Log("ResolveAction: " + action);

            if (action.Substring(0, 5) == "area_")
            {
                ActivateArea(action.Substring(5));
            }
            else if (action.Substring(0, 6) == "spawn_")
            {
                Spawn(action.Substring(6));
            }
            else
            {
                switch (action)
                {
                    case "updatecoins":
                        QuestManager.I.UpateCoinsCounter();
                        break;
                    case "game_end":
                        WinFx.SetActive(true);
                        WinFx.GetComponent<ParticleSystem>().Play();
                        AudioManager.I.PlaySound(Sfx.Win);
                        AnturaDog.SetActive(true);
                        break;
                }
            }
        }

    }
}
