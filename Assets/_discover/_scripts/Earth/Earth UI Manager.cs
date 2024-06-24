using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class EarthUIManager : MonoBehaviour
    {
        public static EarthUIManager I;

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

        private void emptyContainer(GameObject container)
        {
            foreach (Transform t in container.transform)
            {
                Destroy(t.gameObject);
            }
        }
    }

}
