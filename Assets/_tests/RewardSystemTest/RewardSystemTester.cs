using System.Linq;
using Antura.Core;
using Antura.Dog;
using Antura.Helpers;
using Antura.Rewards;
using UnityEngine;
using DG.DeInspektor.Attributes;

namespace Antura.Test.Rewards
{ 
    public class RewardSystemTester : MonoBehaviour
    {
        public AnturaModelManager AnturaModelManager;

        [DeMethodButton("Print Reward Check")]
        void PrintRewardCheck()
        {
            int allRewardsWeHave = AppManager.I.RewardSystemManager.GetTotalRewardPacksCount(true);
            int allRewardsWeNeed = AppManager.I.RewardSystemManager.CountTotalNeededRewards();

            string s = "";
            s += "Total rewards we can generate: " + allRewardsWeHave;
            s += "\nTotal rewards we need for the whole journey: " + allRewardsWeNeed;
            Debug.Log(s);
        }

        [DeMethodButton("Print Reward Stats")]
        void PrintRewardStats()
        {
            string s = "";
            s += "All rewards: " + AppManager.I.RewardSystemManager.GetTotalRewardPacksCount();
            s += "\n All rewards (ignoring color): " + AppManager.I.RewardSystemManager.GetTotalRewardPacksCount(true);
            s += "\n Decal Packs: " + AppManager.I.RewardSystemManager.GetAllRewardPacksOfBaseType(RewardBaseType.Decal).Count;
            s += "\n Decal Packs (ignoring color):  " + AppManager.I.RewardSystemManager.GetAllRewardPacksOfBaseType(RewardBaseType.Decal, true).Count;
            s += "\n Texture Packs: " + AppManager.I.RewardSystemManager.GetAllRewardPacksOfBaseType(RewardBaseType.Texture).Count;
            s += "\n Texture Packs (ignoring color): " + AppManager.I.RewardSystemManager.GetAllRewardPacksOfBaseType(RewardBaseType.Texture, true).Count;
            s += "\n Prop Packs: " + AppManager.I.RewardSystemManager.GetAllRewardPacksOfBaseType(RewardBaseType.Prop).Count;
            s += "\n Prop Packs (ignoring color): " + AppManager.I.RewardSystemManager.GetAllRewardPacksOfBaseType(RewardBaseType.Prop, true).Count;
            Debug.Log(s);
        }

        [DeMethodButton("Print Unlocks")]
        void PrintCurrentUnlocks()
        {
            string s = "We unlocked " + AppManager.I.RewardSystemManager.GetUnlockedRewardPacksCount(true);
            foreach (var unlockedRewardPack in AppManager.I.RewardSystemManager.GetUnlockedRewardPacks())
            {
                s += "\n- " + unlockedRewardPack;
            }
            Debug.Log(s);
        }


        [DeMethodButton("Reset Unlocks")]
        void ResetUnlocks()
        {
            AppManager.I.RewardSystemManager.ResetRewardsUnlockData();
            Debug.Log("Unlocks reset!");
        }

        [DeMethodButton("Load Unlocks")]
        void LoadUnlocks()
        {
            AppManager.I.Player.LoadRewardPackUnlockDataList();
            Debug.Log("Unlocks loaded!");
        }



        [DeMethodButton("Unlock First Set")]
        void UnlockFirstSet()
        {
            AppManager.I.RewardSystemManager.UnlockFirstSetOfRewards();
        }

        [DeMethodButton("Unlock 1 for PS 1.1.100", 0, 1, 1, 100, 1)]
        [DeMethodButton("Unlock 1 for PS 1.2.100", 1, 1, 2, 100, 1)]
        void UnlockJPSelection(int stage, int lb, int ps, int number)
        {
            var jp = new JourneyPosition(stage, lb, ps);
            var lockedPacks = AppManager.I.RewardSystemManager.GetOrGenerateAllRewardPacksForJourneyPosition(jp).Where(x => x.IsLocked).ToList();
            if (lockedPacks.Count == 0) Debug.LogError("Already unlocked all packs for JP " + jp);
            AppManager.I.RewardSystemManager.UnlockPacksSelection(lockedPacks, number);
        }

        [DeMethodButton("Unlock PS 1.1.100", 2, 1, 1, 100)]
        [DeMethodButton("Unlock PS 1.2.100", 2, 1, 2, 100)]
        [DeMethodButton("Unlock PS 1.3.100", 2, 1, 3, 100)]
        void UnlockJP (int stage, int lb, int ps)
        {
            var jp = new JourneyPosition(stage, lb, ps);
            AppManager.I.RewardSystemManager.UnlockAllRewardPacksForJourneyPosition(jp);
        }

        [DeMethodButton("Unlock Everything")]
        void UnlockEverything()
        {
            AppManager.I.RewardSystemManager.UnlockAllPacks();
        }

        [DeMethodButton("Unlock Missing")]
        void UnlockMissing()
        {
            AppManager.I.RewardSystemManager.UnlockAllMissingExtraPacks();
        }

        [DeMethodButton("Load Packs on Antura")]
        void LoadPacksOnAntura()
        {
            var propPack = AppManager.I.RewardSystemManager.GetUnlockedRewardPacksOfBaseType(RewardBaseType.Prop).RandomSelectOne();
            var texturePack = AppManager.I.RewardSystemManager.GetUnlockedRewardPacksOfBaseType(RewardBaseType.Texture).RandomSelectOne();
            var decalPack = AppManager.I.RewardSystemManager.GetUnlockedRewardPacksOfBaseType(RewardBaseType.Decal).RandomSelectOne();

            AnturaModelManager.LoadRewardPackOnAntura(propPack);
            AnturaModelManager.LoadRewardPackOnAntura(texturePack);
            AnturaModelManager.LoadRewardPackOnAntura(decalPack);
        }

    }
}