using Antura.Minigames.DiscoverCountry.Interaction;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Minigames.DiscoverCountry
{
    public class EdAgentCollider : MonoBehaviour
    {
        public EdAgent Agent;

        public void OnTriggerEnter(Collider other)
        {
            Agent.OnInteractionWith(other.gameObject);
            if (other.gameObject == InteractionManager.I.player.gameObject) DiscoverNotifier.Game.OnAgentTriggerEnteredByPlayer.Dispatch(Agent);
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject == InteractionManager.I.player.gameObject) DiscoverNotifier.Game.OnAgentTriggerExitedByPlayer.Dispatch(Agent);
        }
    }
}
