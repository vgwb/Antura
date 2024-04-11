using System;
using Homer;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class EdAgent : MonoBehaviour
    {
        [Header("Homer")]
        public HomerActors.Actors ActorId;
        public Action<GameObject> OnInteraction;

        public void OnInteractionWith(GameObject otherGo)
        {
            OnInteraction?.Invoke(otherGo);
            QuestManager.I.OnInteract(ActorId);
        }
    }
}
