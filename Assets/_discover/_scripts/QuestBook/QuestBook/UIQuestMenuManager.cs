using Antura.Core;
using Antura.Utilities;
using DG.DemiLib.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Antura.Discover.UI
{
    public class UIQuestMenuManager : MonoBehaviour
    {
        public static UIQuestMenuManager I;
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

        [Header("UI Elements")]

        public UIQuestListPanel QuestListPanel;
        public GameObject BookPanel;
        public QuestInfoPanel InfoPanel;

        private GameObject btnGO;
        private LocationData currentLocation;
        private LocationPin currentLocationPin;

        void Start()
        {
            //            Debug.Log("UIQuestMenuManager START");
            InfoPanel.Close();
        }

        public void ShowCountry(Countries country)
        {
            QuestListPanel.ShowCountry(country);
        }

        public void ShowLocation(LocationData location)
        {
            QuestListPanel.ShowLocation(location, currentLocationPin);
        }

        public void OpenBook()
        {
            //BookPanel.Show();
            BookPanel.SetActive(true);
        }

        public void SelectQuest(QuestData questData)
        {
            InfoPanel.Show(questData);
        }

        public void OpenQuest(QuestData questData)
        {
            DiscoverAppManager.I.OpenQuest(questData);
        }


    }

}
