using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Antura.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class InfoPoint : MonoBehaviour
    {
        [Header("Homer")]
        public bool IsInteractable;
        public string HomerNodeId;
        [Header("References")]
        public TextRender Label;
        [Header("Content")]
        public string Text;

        void Start()
        {
            if (Text != "")
            {
                Label.SetText(Text);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (IsInteractable)
            {
                QuestManager.I.OnInfoPoint(this, HomerNodeId);
            }
        }
    }
}
