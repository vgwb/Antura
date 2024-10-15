using System;
using System.Collections;
using System.Collections.Generic;
using Antura.Minigames.DiscoverCountry.Interaction;
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
                // QuestManager.I.OnInfoPoint(this, HomerNodeId);
                if (other.gameObject == InteractionManager.I.player.gameObject) DiscoverNotifier.Game.OnInfoPointTriggerEnteredByPlayer.Dispatch(this, HomerNodeId);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == InteractionManager.I.player.gameObject) DiscoverNotifier.Game.OnInfoPointTriggerExitedByPlayer.Dispatch(this);
        }
    }
}
