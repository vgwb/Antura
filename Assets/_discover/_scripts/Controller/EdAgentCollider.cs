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
            DiscoverNotifier.Game.OnAgentTriggerEnter.Dispatch(Agent);
        }

        public void OnTriggerExit(Collider other)
        {
            DiscoverNotifier.Game.OnAgentTriggerExit.Dispatch(Agent);
        }
    }
}
