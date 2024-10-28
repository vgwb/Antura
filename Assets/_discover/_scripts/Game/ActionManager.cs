using Antura.Audio;
using Antura.Minigames.DiscoverCountry.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class ActionManager : MonoBehaviour
    {
        public static ActionManager I;

        public Transform Target_TutorialTreasure;
        public Transform Target_Eiffel;
        public Transform Target_NotreDame;
        public Transform Target_Louvre;
        public Transform Target_Bakery;
        public Transform Target_Seine;
        public Transform Target_AnturaLocation;

        public GameObject Beam_Tutorial;
        public GameObject Beam_Eiffel;
        public GameObject Beam_NotreDame;
        public GameObject Beam_Louvre;
        public GameObject Beam_Bakery;
        public GameObject Beam_Seine;

        public GameObject Area_Tutorial;
        public GameObject Area_Eiffel;
        public GameObject Area_NotreDame;
        public GameObject Area_Louvre;
        public GameObject Area_Bakery;
        public GameObject Area_Seine;

        public GameObject TutorialElevator;
        public GameObject Eiffel_Guide;
        public GameObject NotreDame_Major;

        public GameObject Player;
        public Transform Spawn_Louvre_Enter;
        public Transform Spawn_Louvre_Exit;

        public GameObject WinFx;
        public GameObject AnturaDog;

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
            Area_Tutorial.SetActive(false);
            Area_Eiffel.SetActive(false);
            Area_NotreDame.SetActive(false);
            Area_Louvre.SetActive(false);
            Area_Bakery.SetActive(false);
            Area_Seine.SetActive(false);

            Beam_Tutorial.SetActive(false);
            Beam_Eiffel.SetActive(false);
            Beam_NotreDame.SetActive(false);
            Beam_Louvre.SetActive(false);
            Beam_Bakery.SetActive(false);
            Beam_Seine.SetActive(false);
            Target_AnturaLocation = null;

            Eiffel_Guide.SetActive(true);
            NotreDame_Major.SetActive(true);

            AnturaDog.SetActive(false);
        }

        public void CameraShowTarget(string targetName)
        {
            switch (targetName)
            {
                case "tutorial":
                    InteractionManager.I.FocusCameraOn(Target_TutorialTreasure);
                    Target_AnturaLocation = null;
                    break;
                case "toureiffel":
                    InteractionManager.I.FocusCameraOn(Target_Eiffel);
                    Beam_Eiffel.SetActive(true);
                    Target_AnturaLocation = Beam_Eiffel.transform;
                    break;
                case "notredame":
                    InteractionManager.I.FocusCameraOn(Target_NotreDame);
                    Beam_NotreDame.SetActive(true);
                    Target_AnturaLocation = Beam_NotreDame.transform;
                    break;
                case "louvre":
                    InteractionManager.I.FocusCameraOn(Target_Louvre);
                    Beam_Louvre.SetActive(true);
                    Target_AnturaLocation = Beam_Louvre.transform;
                    break;
                case "bakery":
                    InteractionManager.I.FocusCameraOn(Target_Bakery);
                    Beam_Bakery.SetActive(true);
                    Target_AnturaLocation = Beam_Bakery.transform;
                    break;
                case "sein":
                    InteractionManager.I.FocusCameraOn(Target_Seine);
                    Beam_Seine.SetActive(true);
                    Target_AnturaLocation = Beam_Seine.transform;
                    break;
                default:
                    break;
            }
            if (Target_AnturaLocation != null)
                InteractionManager.I.ActivateWorldTargetIcon(true, Target_AnturaLocation);
        }

        public void ShowNextTarget(string targetName)
        {

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
                    Area_Tutorial.SetActive(true);
                    TutorialElevator.GetComponent<MovingPlatform>().Activate(true);
                    break;
                case "area_eiffel":
                    Area_Eiffel.SetActive(true);
                    Eiffel_Guide.SetActive(false);
                    break;
                case "area_notredame":
                    Area_NotreDame.SetActive(true);
                    NotreDame_Major.SetActive(false);
                    break;
                case "area_louvre":
                    Area_Louvre.SetActive(true);
                    break;
                case "area_bakery":
                    Area_Bakery.SetActive(true);
                    break;
                case "area_seine":
                    Area_Seine.SetActive(true);
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
                    Area_Louvre.SetActive(true);
                    Player.GetComponent<EdPlayer>().SpawnToNewLocation(Spawn_Louvre_Enter);
                    break;
            }

        }

    }
}
