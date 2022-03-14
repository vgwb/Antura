using UnityEngine;

namespace Antura.Minigames
{
    /// <summary>
    /// Utility script that automatically destroys a game object after a given duration.
    /// </summary>
    public class AutoDestroy : MonoBehaviour
    {
        public float duration;

        void Update()
        {
            duration -= Time.deltaTime;

            if (duration <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
