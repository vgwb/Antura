using Antura.Core;
using Antura.Utilities;
using DG.DemiLib.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public Quests QuestsData;

        private Countries currentCountry;
        private GameObject btnGO;
        private LocationDefinition currentLocation;

        void Start()
        {
            //            Debug.Log("UIQuestMenuManager START");
            Container.SetActive(false);
            InfoPanel.Close();
            currentCountry = Countries.None;
        }

        private void LoadCountry(Countries country)
        {
            emptyContainer(Container);
            foreach (var questData in QuestsData.AvailableQuests)
            {
                if (questData.Country == country)
                {
                    btnGO = Instantiate(MenuItemPrefab);
                    btnGO.transform.SetParent(Container.transform, false);
                    btnGO.GetComponent<QuestMenuItem>().Init(questData);
                }
            }
            currentCountry = country;
        }

        private void LoadLocation(LocationDefinition location)
        {
            emptyContainer(Container);
            foreach (var questData in QuestsData.AvailableQuests)
            {
                if (questData.Location == location)
                {
                    btnGO = Instantiate(MenuItemPrefab);
                    btnGO.transform.SetParent(Container.transform, false);
                    btnGO.GetComponent<QuestMenuItem>().Init(questData);
                }
            }
            currentLocation = location;
        }

        public void ShowCountry(Countries country)
        {
            if (country != currentCountry)
            {
                LoadCountry(country);
            }
            Container.SetActive(true);
        }

        public void ShowLocation(LocationDefinition location)
        {
            if (location != currentLocation)
            {
                LoadLocation(location);
            }
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
