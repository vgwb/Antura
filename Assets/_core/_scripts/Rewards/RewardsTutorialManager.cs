using Antura.Core;
using Antura.Keeper;
using Antura.Profile;
using Antura.Tutorial;

namespace Antura.Rewards
{
    public class RewardsTutorialManager : TutorialManager
    {
        //private RewardsScene _mScene;

        protected override void InternalHandleStart()
        {
            //_mScene = FindObjectOfType<RewardsScene>();

            switch (FirstContactManager.I.CurrentPhaseInSequence)
            {
                case FirstContactPhase.Reward_FirstBig:
                    //_mScene.AnturaSpaceBtton.gameObject.SetActive(false);
                    KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Reward_Intro);
                    break;
            }
        }

        protected override AppScene CurrentAppScene
        {
            get { return AppScene.Rewards; }
        }
    }
}
