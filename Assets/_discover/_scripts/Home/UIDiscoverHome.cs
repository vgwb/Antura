using Antura.Core;
using Antura.Discover.UI;
using Antura.Utilities;
using DG.DemiLib.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    public class UIDiscoverHome : SingletonMonoBehaviour<UIDiscoverHome>
    {
        [Header("References")]
        public QuestData DomusQuest;
        public Button btDomus;
        public Button btDiscoArcade;

        [Header("UI Elements")]

        public UIQuestListPanel QuestListPanel;
        public DiscoArcade DiscoArcade;
        public QuestInfoPanel InfoPanel;

        private LocationData currentLocation;
        private LocationPin currentLocationPin;

        protected override void Init()
        {
            btDomus.onClick.AddListener(OpenDomus);
            btDiscoArcade.onClick.AddListener(OpenDiscoArcade);
        }

        private void OnEnable()
        {

        }

        void Start()
        {
            InfoPanel.Close();
        }

        void OnDestroy()
        {
            btDomus.onClick.RemoveListener(OpenDomus);
            btDiscoArcade.onClick.RemoveListener(OpenDiscoArcade);
        }

        public void ShowCountry(Countries country, bool forceReload = false)
        {
            QuestListPanel.ShowCountry(country, forceReload);
        }

        public void ShowLocation(LocationData location)
        {
            if (location.Id == "artic")
            {
                // special quest to open the Domus quest
                SelectQuest(DomusQuest);
            }
            else
            {
                QuestListPanel.ShowLocation(location, currentLocationPin);
            }
        }

        public void OpenDiscoArcade()
        {
            DiscoArcade.Open();
        }

        public void OpenDomus()
        {
            SelectQuest(DomusQuest);
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
