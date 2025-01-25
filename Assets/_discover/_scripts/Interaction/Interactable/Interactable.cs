using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public enum InteractionType
    {
        None = 0,
        HomerCommand = 1,
        HomerNode = 2,
        UnityAction = 3
    }
    public abstract class Interactable : MonoBehaviour
    {
        [Header("Interaction")]
        public bool IsInteractable;
        public string HomerNodeId;
        public string Command;

    }
}
