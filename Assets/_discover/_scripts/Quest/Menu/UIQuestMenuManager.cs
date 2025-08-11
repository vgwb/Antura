using Antura.Core;
using Antura.Utilities;
using DG.DemiLib.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Antura.Discover
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

        public GameObject BookPanel;
        public QuestInfoPanel InfoPanel;
        public GameObject MenuItemPrefab;
        public GameObject Container;
        public QuestListData QuestsData;

        public TextMeshProUGUI ListTitleText; // Title for the list of quests
        public QuestData TutorialQuest; // The tutorial quest, always available

        private Countries currentCountry;
        private GameObject btnGO;
        private LocationData currentLocation;
        private LocationPin currentLocationPin;

        void Start()
        {
            //            Debug.Log("UIQuestMenuManager START");
            Container.SetActive(false);
            InfoPanel.Close();
            currentCountry = Countries.Global;
            ListTitleText.text = "";
        }

        private void LoadCountry(Countries country)
        {
            emptyContainer(Container);
            addMenuItem(TutorialQuest);

            foreach (var questData in QuestsData.QuestList)
            {
                if (questData.Country == country)
                {
                    addMenuItem(questData);
                }
            }
            currentCountry = country;
            ListTitleText.text = country.ToString();
        }

        private void addMenuItem(QuestData questData)
        {
            btnGO = Instantiate(MenuItemPrefab);
            btnGO.transform.SetParent(Container.transform, false);
            btnGO.GetComponent<QuestMenuItem>().Init(questData);
        }

        private void LoadLocation(LocationData location)
        {
            emptyContainer(Container);
            foreach (var questData in QuestsData.QuestList)
            {
                if (questData.Location == location)
                {
                    addMenuItem(questData);
                }
            }
            currentLocation = location;
            ListTitleText.text = location.Country.ToString() + " - " + location.Name.GetLocalizedString();
        }

        public void ShowCountry(Countries country)
        {
            Debug.Log($"UIQuestMenuManager: ShowCountry called with {country}");
            if (country != currentCountry)
            {
                LoadCountry(country);

            }
            Container.SetActive(true);
        }

        public void ShowLocation(LocationData location, LocationPin locationPin = null)
        {
            if (location == null)
            {
                Debug.LogWarning("UIQuestMenuManager: ShowLocation called with null location.");
                return;
            }

            if (currentLocationPin != null && currentLocation == location)
            {
                return; // Already showing this location
            }

            if (currentLocationPin != null)
            {
                currentLocationPin.DeSelect();
            }

            currentLocationPin = locationPin;
            LoadLocation(location);
            Container.SetActive(true);
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
            //Debug.Log("Load scene " + questData.scene);
            AppManager.I.NavigationManager.GoToDiscoverQuest(questData.scene);
        }

        private void emptyContainer(GameObject container)
        {
            foreach (Transform t in container.transform)
            {
                Destroy(t.gameObject);
            }
        }

    }

}
