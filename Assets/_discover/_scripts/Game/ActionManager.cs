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

        public GameObject Beam_Eiffel;
        public GameObject Beam_NotreDame;
        public GameObject Beam_Louvre;
        public GameObject Beam_Baguette;

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
            InteractionManager.I.FocusCameraOn(Target_TutorialTreasure);
        }

        public void ShowNextTarget(string targetName)
        {
            Beam_Eiffel.SetActive(true);
        }


    }
}
