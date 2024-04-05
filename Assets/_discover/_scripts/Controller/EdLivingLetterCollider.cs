using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class EdLivingLetterCollider : MonoBehaviour
    {
        public EdLivingLetter LL;

        public void OnTriggerEnter(Collider other)
        {
            LL.OnInteractionWith(other.gameObject);
            DiscoverNotifier.Game.OnLivingLetterTriggerEnter.Dispatch(LL);
        }
        
        public void OnTriggerExit(Collider other)
        {
            DiscoverNotifier.Game.OnLivingLetterTriggerExit.Dispatch(LL);
        }
    }
}
