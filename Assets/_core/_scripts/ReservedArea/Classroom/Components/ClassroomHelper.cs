using Antura.Core;
using Antura.Profile;

namespace Antura.UI
{
    public static class ClassroomHelper
    {

        public static string GetClassroomName(int classRoomIndex)
        {
            switch (classRoomIndex)
            {
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "D";
                case 5:
                    return "E";
                case 6:
                    return "F";
                default:
                    return "";
            }
        }

        public static void SaveProfile(PlayerProfilePreview profile)
        {
            PlayerProfile playerProfile = AppManager.I.PlayerProfileManager.GetPlayerProfileByUUID(profile.Uuid);
            playerProfile.PlayerName = profile.PlayerName;
            playerProfile.Classroom = profile.Classroom;
            playerProfile.TalkToPlayerStyle = profile.TalkToPlayerStyle;
            AppManager.I.PlayerProfileManager.SavePlayerProfile(playerProfile);
            AppManager.I.PlayerProfileManager.UpdatePlayerIconDataInSettings(profile);
        }

    }
}
