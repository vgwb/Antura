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

        public GameObject Beam_Eiffel;
        public GameObject Beam_NotreDame;
        public GameObject Beam_Louvre;
        public GameObject Beam_Baguette;

        public GameObject TutorialElevator;

        public GameObject WinFx;

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
            Beam_Eiffel.SetActive(false);
        }

        public void CameraShowTarget(string targetName)
        {
            switch (targetName)
            {
                case "tutorial":
                    InteractionManager.I.FocusCameraOn(Target_TutorialTreasure);
                    break;
                case "toureiffel":
                    InteractionManager.I.FocusCameraOn(Target_Eiffel);
                    Beam_Eiffel.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void ShowNextTarget(string targetName)
        {

        }

        public void ResolveAction(string action)
        {
            Debug.Log("ResolveAction" + action);
            if (action == "tutorial_elevator")
            {
                TutorialElevator.GetComponent<MovingPlatform>().Activate(true);
            }
        }

    }
}
