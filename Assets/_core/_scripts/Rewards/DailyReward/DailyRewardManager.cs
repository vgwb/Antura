using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Antura.Rewards
{
    public class DailyRewardManager
    {
        public class DailyReward
        {
            public DailyRewardType rewardType;
            public int amount;

            public DailyReward(DailyRewardType rewardType, int amount)
            {
                this.rewardType = rewardType;
                this.amount = amount;
            }
        }

        private List<DailyReward> rewards;

        public IEnumerable<DailyReward> GetRewards(int startIndex, int stopIndex)
        {
            for (var index = startIndex; index < stopIndex; index++)
            {
                if (index >= rewards.Count)
                {
                    // Fallback to the last one
                    yield return rewards.Last();
                }
                else
                {
                    yield return rewards[index];
                }
            }
        }

        public DailyReward GetReward(int i)
        {
            if (i < 0)
                i = 0;
            if (i >= rewards.Count)
                i = rewards.Count - 1;
            //            Debug.Log("i= " + i + " / length rewards=" + rewards.Count);
            return rewards[i];
        }

        public DailyRewardManager()
        {
            rewards = new List<DailyReward>
            {
                new DailyReward(DailyRewardType.Bones, 1),      // for 1 combo day
                new DailyReward(DailyRewardType.Bones, 3),
                new DailyReward(DailyRewardType.Bones, 5),
                new DailyReward(DailyRewardType.Bones, 7),
                new DailyReward(DailyRewardType.Bones, 10),     // last one is forever
            };
        }

    }
}
