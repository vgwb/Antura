using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Antura.Minigames.DiscoverCountry
{
    public class InfoPoint : MonoBehaviour
    {
        public string HomerNodeId;
        public TextMeshPro label;

        void Start()
        {

        }

        public void OnTriggerEnter(Collider other)
        {
            QuestManager.I.OnInfoPoint(HomerNodeId);
        }
    }
}
