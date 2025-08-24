using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace Antura.Discover
{
    /// <summary>
    /// Static class containing Yarn functions for Discover.
    /// These functions can be called from Yarn scripts to access Discover-specific data and functionality.
    /// </summary>
    public static class YarnDiscoverFunctions
    {
        // --- Helpers ---
        static DiscoverPlayerProfile PlayerProfile => DiscoverAppManager.I != null ? DiscoverAppManager.I.CurrentProfile : null;
        static CardData GetCard(string cardId) => DiscoverAppManager.I != null ? DiscoverAppManager.I.GetCardById(cardId) : null;
        static CardState GetState(string cardId)
        {
            if (PlayerProfile == null || string.IsNullOrEmpty(cardId) || PlayerProfile.cards == null)
                return null;
            CardState state;
            return PlayerProfile.cards.TryGetValue(cardId, out state) ? state : null;
        }

        // ------------------------------------------------------------
        // CARDS
        // ------------------------------------------------------------

        /*
        ///  EXAMPLE
        /// <<if card_unlocked("CARD_FR_BAGUETTE") == false>>
        ///      line: "You don't have the Baguette card yet!"
        /// <<endif>>
        */
        [YarnFunction("card_unlocked")]
        public static bool CardUnlocked(string cardId)
        {
            var st = GetState(cardId);
            return st != null && st.unlocked;
        }

        // ------------------------------------------------------------
        // INVENTORY
        // ------------------------------------------------------------


    }
}
