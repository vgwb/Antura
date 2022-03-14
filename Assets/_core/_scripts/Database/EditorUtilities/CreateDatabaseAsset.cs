#if UNITY_EDITOR
using Antura.EditorUtilities;

namespace Antura.Database
{
    /// <summary>
    /// Utility class that generates a complete empty static database in the given folder.
    /// </summary>
    public class CreateDatabaseAsset
    {
        public static void CreateAssets(string targetPath, string targetName)
        {
            // @todo: also create the folder?
            CustomAssetUtility.CreateAsset<StageDatabase>(targetPath, targetName + "_" + "Stage");
            CustomAssetUtility.CreateAsset<LearningBlockDatabase>(targetPath, targetName + "_" + "LearningBlock");
            CustomAssetUtility.CreateAsset<PlaySessionDatabase>(targetPath, targetName + "_" + "PlaySession");
            CustomAssetUtility.CreateAsset<MiniGameDatabase>(targetPath, targetName + "_" + "MiniGame");
            CustomAssetUtility.CreateAsset<LetterDatabase>(targetPath, targetName + "_" + "Letter");
            CustomAssetUtility.CreateAsset<WordDatabase>(targetPath, targetName + "_" + "Word");
            CustomAssetUtility.CreateAsset<PhraseDatabase>(targetPath, targetName + "_" + "Phrase");
            CustomAssetUtility.CreateAsset<LocalizationDatabase>(targetPath, targetName + "_" + "Localization");
            CustomAssetUtility.CreateAsset<RewardDatabase>(targetPath, targetName + "_" + "Reward");
        }

    }
}
#endif
