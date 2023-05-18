using System.Linq;
using Antura.Core;

namespace Antura.Rewards
{
    /// <summary>
    /// Structure focused to communicate about items from e to UI.
    /// </summary>
    public class RewardBaseItem
    {
        // Data
        public RewardBase data;
        public bool IsNew;

        // State
        public bool IsSelected;

        public bool IsBought
        {
            get
            {
                // An item is considered bought if it was either unlocked (old mode), or if the shop state is present
                if (data.Cost == 0) return true; // Any item with cost 0 is already unlocked too
                var shopState = AppManager.I.Player.CustomizationShopState.CustomizationShopStates.FirstOrDefault(x => x.SharedID == data.SharedID);
                bool unlocked = AppManager.I.RewardSystemManager.IsRewardBaseUnlocked(data, AppManager.I.Player.PetData.SelectedPet);
                return unlocked || (shopState != null && shopState.Bought);
            }
        }
    }
}
