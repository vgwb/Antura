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
    public class UIQuestListPanel : MonoBehaviour
    {

        [Header("UI Elements")]

        public GameObject MenuItemPrefab;
        public GameObject Container;
        public QuestListData QuestsData;

        // Title for the list of quests (country name or Location name)
        public TextMeshProUGUI ListTitleText;

        private Countries currentCountry;
        private LocationData currentLocation;
        private GameObject btnGO;

        void Start()
        {
            Container.SetActive(false);
            currentCountry = Countries.Global;
            ListTitleText.text = "";
        }

        private void LoadCountry(Countries country)
        {
            emptyContainer(Container);

            if (QuestsData == null || QuestsData.QuestList == null)
            {
                Debug.LogWarning("UIQuestListPanel: QuestsData or QuestList is null.");
                return;
            }

            foreach (var questData in QuestsData.QuestList)
            {
                if (questData == null)
                    continue;
                // If a specific country is selected, include both that country's quests and Global ones.
                // If Global is selected, include only Global quests.
                bool match = country == Countries.Global
                    ? questData.Country == Countries.Global
                    : (questData.Country == country || questData.Country == Countries.Global);
                if (match)
                    addQuestMenuItem(questData);
            }
            currentCountry = country;
            ListTitleText.text = country.ToString();
        }

        private void addQuestMenuItem(QuestData questData)
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
                    addQuestMenuItem(questData);
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

            // if (currentLocationPin != null && currentLocation == location)
            // {
            //     return; // Already showing this location
            // }

            // if (currentLocationPin != null)
            // {
            //     currentLocationPin.DeSelect();
            // }

            // currentLocationPin = locationPin;
            LoadLocation(location);
            Container.SetActive(true);
        }

        public void SelectQuest(QuestData questData)
        {
            UIQuestMenuManager.I.SelectQuest(questData);
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
