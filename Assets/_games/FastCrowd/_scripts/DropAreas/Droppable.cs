using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.FastCrowd
{

    /// <summary>
    /// Add functionality to be droppable on DropSingleArea.
    /// </summary>
    [RequireComponent(typeof(LivingLetterController))]
    [RequireComponent(typeof(Collider))]
    public class Droppable : MonoBehaviour
    {

        DropSingleArea dropAreaActive;


        public delegate void DropEvent(LivingLetterController _letterView);

        void OnTriggerEnter(Collider other)
        {
            DropSingleArea da = other.GetComponent<DropSingleArea>();
            if (da)
            {
                dropAreaActive = da;

            }
        }

        void OnTriggerExit(Collider other)
        {
            DropSingleArea da = other.GetComponent<DropSingleArea>();
            if (da && da == dropAreaActive)
            {
                dropAreaActive.DeactivateMatching();
                dropAreaActive = null;
            }
        }

    }
}
