using Antura.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class EarthManager : MonoBehaviour
    {
        public static EarthManager I;

        void Awake()
        {
            if (I == null)
            {
                I = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {

        }

        public void SelectFrance()
        {
            EarthUIManager.I.ShowCountry("france");
        }

        public void SelectQuest(QuestData questData)
        {
            //            Debug.Log("Selected " + questData.Title);
            AppManager.I.NavigationManager.GoToCountryFrance();
        }
    }
}
