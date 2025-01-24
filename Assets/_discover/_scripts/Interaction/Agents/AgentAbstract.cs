using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Antura.Minigames.DiscoverCountry
{
    public class InteractableAbstract : MonoBehaviour
    {
        [Header("Quest")]
        public bool IsInteractable;
        public string HomerNodeId;
        public string Command;

    }
}
