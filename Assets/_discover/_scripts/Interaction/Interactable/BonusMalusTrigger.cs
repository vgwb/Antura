using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// Attach to any GameObject. Call Trigger() to apply a Bonus/Malus to card progress.
    /// You can target specific cards OR apply by filters (Country/Category).
    /// </summary>
    public class BonusMalusTrigger : MonoBehaviour
    {

        [Tooltip("The Bonus/Malus to apply when Trigger() is called.")]
        public BonusMalusData BonusMalus;

        void Awake()
        {
        }

        public void Trigger()
        {
            if (BonusMalus == null)
            {
                Debug.LogWarning("BonusMalus is not set on " + gameObject.name);
                return;
            }
            Debug.Log($"Triggering Bonus/Malus: {BonusMalus.Title.GetLocalizedString()} ({BonusMalus.Type}) with {BonusMalus.Points} points.");
        }
    }
}
