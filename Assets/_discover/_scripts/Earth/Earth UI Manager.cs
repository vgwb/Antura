using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class EarthUIManager : MonoBehaviour
    {
        public GameObject MenuItemPrefab;
        public GameObject Container;
        public Quests QuestsData;

        private GameObject btnGO;
        void Start()
        {
            emptyContainer(Container);
            foreach (var questData in QuestsData.AvailableQuests)
            {
                btnGO = Instantiate(MenuItemPrefab);
                btnGO.transform.SetParent(Container.transform, false);
                btnGO.GetComponent<QuestMenuItem>().Init(questData);
            }
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
