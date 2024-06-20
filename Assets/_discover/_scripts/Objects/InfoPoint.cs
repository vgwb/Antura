using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Antura.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class InfoPoint : MonoBehaviour
    {
        public string HomerNodeId;
        public string Text;
        public TextRender Label;

        void Start()
        {
            if (Text != "")
            {
                Label.SetText(Text);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            QuestManager.I.OnInfoPoint(this, HomerNodeId);
        }
    }
}
