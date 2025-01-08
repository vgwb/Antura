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
        public Transform Target_AnturaLocation;

        public GameObject TutorialElevator;
        public GameObject Eiffel_Guide;
        public GameObject NotreDame_Major;

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

        private ActionData GetActionDataByCode(string actionCode)
        {
            return Actions.FirstOrDefault(action => action.ActionCode == actionCode);
        }

        public void CameraShowTarget(string targetArea)
        {
            Debug.Log("CameraShowTarget targetArea:" + targetArea);


            var actionData = GetActionDataByCode(targetArea);

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
            var actionData = GetActionDataByCode(targetArea);
            actionData.Area.SetActive(true);
        }

        public void ResolveAction(string action)
        {
            Debug.Log("ResolveAction" + action);

            switch (action)
            {
                case "updatecoins":
                    QuestManager.I.UpateCoinsCounter();
                    break;
                case "area_tutorial":
                    ActivateArea("tutorial");
                    break;
                case "area_eiffel":
                    ActivateArea("eiffel");
                    break;
                case "area_notredame":
                    ActivateArea("notredame");
                    break;
                case "area_louvre":
                    ActivateArea("louvre");
                    break;
                case "area_bakery":
                    ActivateArea("bakery");
                    break;
                case "area_seine":
                    ActivateArea("seine");
                    break;
                case "game_end":
                    WinFx.SetActive(true);
                    WinFx.GetComponent<ParticleSystem>().Play();
                    //InteractionManager.I.FocusCameraOn(Target_Eiffel);
                    AudioManager.I.PlaySound(Sfx.Win);
                    AnturaDog.SetActive(true);
                    break;
                case "louvre_exit":
                    Player.GetComponent<EdPlayer>().SpawnToNewLocation(Spawn_Louvre_Exit);
                    break;
                case "louvre_enter":
                    ActivateArea("louvre");
                    Player.GetComponent<EdPlayer>().SpawnToNewLocation(Spawn_Louvre_Enter);
                    break;
            }

        }

    }
}
