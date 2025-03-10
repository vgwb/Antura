using Antura.Core;
using Antura.Profile;

namespace Antura.UI
{
    public static class ClassroomHelper
    {
        #region Public Methods

        public static void SaveProfile(PlayerIconData profile)
        {
            PlayerProfile playerProfile = AppManager.I.PlayerProfileManager.GetPlayerProfileByUUID(profile.Uuid);
            AppManager.I.PlayerProfileManager.SavePlayerProfile(playerProfile);
            AppManager.I.PlayerProfileManager.UpdatePlayerIconDataInSettings(profile);
        }

        #endregion
    }
}