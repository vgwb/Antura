using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class EdLivingLetterCollider : MonoBehaviour
    {
        public EdLivingLetter LL;

        public void OnTriggerEnter(Collider other)
        {
            LL.OnInteractionWith(other.gameObject);
        }
    }
}
