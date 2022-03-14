using UnityEngine;

namespace Antura.Minigames.MakeFriends
{
    public class FriendsZonesManager : MonoBehaviour
    {
        public FriendsZone[] zones;
        [HideInInspector]
        public FriendsZone currentZone;

        public static FriendsZonesManager instance;

        private int currentZoneIndex;


        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            currentZoneIndex = 0;
            currentZone = zones[currentZoneIndex];
        }

        public void IncrementCurrentZone()
        {
            if (currentZoneIndex >= zones.Length - 1)
            {
                Debug.Log("No more Friends Zones!");
                return;
            }
            currentZoneIndex++;
            currentZone = zones[currentZoneIndex];
        }

        public void EverybodyDance()
        {
            for (int i = 0; i < zones.Length; i++)
            {
                var leftLivingLetter = zones[i].left.GetComponentInChildren<MakeFriendsLivingLetter>();
                var rightLivingLetter = zones[i].right.GetComponentInChildren<MakeFriendsLivingLetter>();

                if (leftLivingLetter != null)
                {
                    leftLivingLetter.Dance();
                }
                if (rightLivingLetter != null)
                {
                    rightLivingLetter.Dance();
                }
            }
        }
    }
}
