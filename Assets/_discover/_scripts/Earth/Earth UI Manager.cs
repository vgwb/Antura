using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class EarthUIManager : MonoBehaviour
    {
        public static EarthUIManager I;

        public QuestInfoPanel InfoPanel;
        public GameObject MenuItemPrefab;
        public GameObject Container;
        public Quests QuestsData;

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

        private GameObject btnGO;
        void Start()
        {
            Container.SetActive(false);
            InfoPanel.Close();
        }

        private void LoadCountry(string country)
        {
            emptyContainer(Container);
            foreach (var questData in QuestsData.AvailableQuests)
            {
                btnGO = Instantiate(MenuItemPrefab);
                btnGO.transform.SetParent(Container.transform, false);
                btnGO.GetComponent<QuestMenuItem>().Init(questData);
            }

        }

        public void ShowCountry(string country)
        {
            LoadCountry(country);
            Container.SetActive(true);
        }

        public void SelectQuest(QuestData questData)
        {
            InfoPanel.Show(questData);
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
