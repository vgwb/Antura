using Antura.Dog;
using Antura.Core;
using Antura.Keeper;
using Antura.Profile;
using Antura.Tutorial;
using Antura.UI;
using DG.Tweening;
using System.Collections;
using Antura.Debugging;
using Antura.Helpers;
using UnityEngine;

namespace Antura.Rewards
{
    /// <summary>
    /// Manages the Rewards scene.
    /// Accessed after a learning block is completed.
    /// </summary>
    [RequireComponent(typeof(RewardsAnimator))]
    public class RewardsScene : SceneBase
    {
        [Header("Setup")]
        public AnturaAnimationStates AnturaAnimation = AnturaAnimationStates.sitting;

        [Header("References")]
        public AnturaAnimationController AnturaAnimController;
        //public Button AnturaSpaceBtton;

        //Tween btAnturaTween;

        protected override void Start()
        {
            base.Start();
            GlobalUI.ShowPauseMenu(false);
            //Debug.Log("RewardsManager playsession: " + AppManager.I.Player.CurrentJourneyPosition.PlaySession);

            AnturaAnimController.State = AnturaAnimation;
            //AnturaSpaceBtton.gameObject.SetActive(false);
            ShowReward();

            //AnturaSpaceBtton.onClick.AddListener(() => AppManager.I.NavigationManager.GoToAnturaSpace());

            tutorialManager = gameObject.GetComponentInChildren<RewardsTutorialManager>();
            tutorialManager.HandleStart();

            DebugManager.OnSkipCurrentScene += HandleSceneSkip;
        }

        void HandleSceneSkip()
        {
            Continue();
        }

        void OnDestroy()
        {
            DebugManager.OnSkipCurrentScene -= HandleSceneSkip;

            //btAnturaTween.Kill();
        }

        public void ShowReward()
        {
            StartCoroutine(StartReward());
        }

        IEnumerator StartReward()
        {
            if (FirstContactManager.I.IsSequenceFinished())
            {
                int rnd = Random.Range(1, 3);
                switch (rnd)
                {
                    case 1:
                        KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Reward_Big_1);
                        break;
                    case 3:
                        KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Reward_Big_2);
                        break;
                    default:
                        KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Reward_Big_3);
                        break;
                }
            }

            // Wait animation ending before show continue button
            yield return new WaitForSeconds(8.0f);
            ContinueScreen.Show(Continue, ContinueScreenMode.Button, true);
            //if (FirstContactManager.I.IsFinished()) {
            //    AnturaSpaceBtton.gameObject.SetActive(true);
            //    btAnturaTween = AnturaSpaceBtton.transform.DOScale(0.1f, 0.4f).From().SetEase(Ease.OutBack);
            //}
            yield return null;
        }

        #region API for animation driven

        public void ClearLoadedRewardsOnAntura()
        {
            // Clean and load antura reward.
            AnturaModelManager.I.ClearLoadedRewardPacks();
        }

        /// <summary>
        /// Gets the reward to instantiate.
        /// </summary>
        /// <returns></returns>
        public RewardPack GetRewardPackToInstantiate()
        {
            if (FirstContactManager.I.IsPhaseUnlockedAndNotCompleted(FirstContactPhase.Reward_FirstBig))
            {
                // Get the first prop reward (already unlocked)
                var firstRewardPack = AppManager.I.RewardSystemManager.GetUnlockedRewardPacksOfBaseType(RewardBaseType.Prop).RandomSelectOne();
                return firstRewardPack;
            }
            else
            {
                // Unlock the rewards for this JP (should be one, since this is an Assessment)
                var newRewardPacks = AppManager.I.RewardSystemManager.UnlockAllRewardPacksForJourneyPosition(AppManager.I.Player.CurrentJourneyPosition);

                RewardPack newRewardPack = null;
                if (newRewardPacks != null && newRewardPacks.Count > 0)
                {
                    newRewardPack = newRewardPacks[0];
                }

                // Also advance the MaxJP
                AppManager.I.Player.AdvanceMaxJourneyPosition();    // TODO: move this out of here and into the NavigationManager instead

                return newRewardPack;
            }
        }

        /// <summary>
        /// Instantiates the reward, mount on antura and return gameobject.
        /// </summary>
        /// <param name="_rewardToInstantiate">The reward to instantiate.</param>
        /// <returns></returns>
        public GameObject InstantiateReward(RewardPack _rewardToInstantiate)
        {
            return AnturaModelManager.I.LoadRewardPackOnAntura(_rewardToInstantiate);
        }

        #endregion

        public void Continue()
        {
            if (FirstContactManager.I.IsPhaseUnlockedAndNotCompleted(FirstContactPhase.Reward_FirstBig))
                FirstContactManager.I.CompleteCurrentPhaseInSequence();

            AppManager.I.NavigationManager.GoToNextScene();
        }
    }
}
