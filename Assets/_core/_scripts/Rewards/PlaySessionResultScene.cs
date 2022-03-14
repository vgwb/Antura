using Antura.Core;
using System.Linq;
using Antura.Debugging;

namespace Antura.Rewards
{
    /// <summary>
    /// Manager for the Play Session Result scene.
    /// Accessed when a play session is completed.
    /// </summary>
    public class PlaySessionResultScene : SceneBase
    {
        private static bool UNLOCK_AT_EACH_PS = false;  // if true, we unlock something at the end of each PS

        protected override void Start()
        {
            base.Start();

            DebugManager.OnSkipCurrentScene += HandleSceneSkip;

            var jp = AppManager.I.Player.CurrentJourneyPosition;
            var nEarnedStars = AppManager.I.NavigationManager.CalculateEarnedStarsCount();
            if (NavigationManager.TEST_SKIP_GAMES)
            { nEarnedStars = 3; }

            // Log various data
            LogManager.I.LogPlaySessionScore(AppManager.I.JourneyHelper.GetCurrentPlaySessionData().Id, nEarnedStars);
            AppManager.I.Teacher.logAI.UnlockVocabularyDataForJourneyPosition(AppManager.I.Player.CurrentJourneyPosition);

            // Advance journey if we earned enough stars
            if (nEarnedStars > 0)
            {
                AppManager.I.Player.AdvanceMaxJourneyPosition();
            }

            if (UNLOCK_AT_EACH_PS)
            {
                // Compute numbers we need to unlock
                var nTotalRewardPacksToUnlock = AppManager.I.NavigationManager.CalculateRewardPacksUnlockCount();

                var rewardPacksForJourneyPosition =
                    AppManager.I.RewardSystemManager.GetOrGenerateAllRewardPacksForJourneyPosition(jp);
                var rewardPacksUnlocked = rewardPacksForJourneyPosition.Where(x => x.IsUnlocked).ToList();
                var rewardPacksLocked = rewardPacksForJourneyPosition.Where(x => x.IsLocked).ToList();

                int nRewardPacksAlreadyUnlocked = rewardPacksUnlocked.Count();
                int nNewRewardPacksToUnlock = nTotalRewardPacksToUnlock - nRewardPacksAlreadyUnlocked;

                // Unlock the selected set of locked rewards
                AppManager.I.RewardSystemManager.UnlockPacksSelection(rewardPacksLocked, nNewRewardPacksToUnlock);

                // Show UI result and unlock transform parent where show unlocked items
                var uiGameObjects =
                    GameResultUI.ShowEndsessionResult(AppManager.I.NavigationManager.UseEndSessionResults(),
                        nRewardPacksAlreadyUnlocked);

                // For any rewards mount them model on parent transform object (objs)
                for (int i = 0; i < rewardPacksUnlocked.Count() && i < uiGameObjects.Length; i++)
                {
                    var matPair = rewardPacksUnlocked[i].GetMaterialPair();
                    ModelsManager.MountModel(rewardPacksUnlocked[i].BaseId, uiGameObjects[i].transform, matPair);
                }

            }
            else
            {
                GameResultUI.ShowEndsessionResult(AppManager.I.NavigationManager.UseEndSessionResults(), 1);
            }

        }

        void OnDestroy()
        {
            DebugManager.OnSkipCurrentScene -= HandleSceneSkip;
        }

        private void HandleSceneSkip()
        {
            GameResultUI.I.EndsessionResultPanel.Continue();
        }
    }
}
