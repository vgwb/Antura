using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.MakeFriends
{
    public class LivingLetterArea : MonoBehaviour
    {
        public GameObject livingLetterPrefab;
        public bool left;
        public Vector3 offscreenPosition;
        public Vector3 startingPosition;
        public Vector3 celebrationPosition;
        public Vector3 friendlyExitPosition;
        public Vector3 entranceRotation;
        public Vector3 exitRotation_readable;
        public Vector3 exitRotation_unreadable;
        public Vector3 friendlyExitRotation;
        public float entranceDuration;
        public float celebrationDuration;
        public float friendlyExitDuration;
        public float movingAwayDuration;

        [HideInInspector]
        public MakeFriendsLivingLetter livingLetter;
        [SerializeField]
        private Vector3 exitRotation;

        private int movingAwayCounter = 1;

        private Vector3 NextMovingAwayPosition
        {
            get
            {
                switch (movingAwayCounter)
                {
                    case 1:
                        return startingPosition + (offscreenPosition - startingPosition).normalized * 4f;
                    case 2:
                        return startingPosition + (offscreenPosition - startingPosition).normalized * 8f;
                    case 3:
                        return offscreenPosition;
                    default:
                        return startingPosition;
                }
            }
        }


        private void AdjustForDifficulty()
        {
            exitRotation = exitRotation_readable;

            if (MakeFriendsGame.Instance.DifficultyChoice != MakeFriendsDifficulty.EASY)
            {
                exitRotation = exitRotation_unreadable;
            }

            if (livingLetter != null)
            {
                livingLetter.focusOnTouch = true;
                livingLetter.focusOnTouchTime = MakeFriendsGame.Instance.DifficultyChoice == MakeFriendsDifficulty.HARD ? 1f : 3f; // At normal, more
            }
        }

        public MakeFriendsLivingLetter SpawnLivingLetter(LL_WordData wordData)
        {
            AdjustForDifficulty();

            var instance = Instantiate(livingLetterPrefab, offscreenPosition, Quaternion.Euler(entranceRotation), this.transform) as GameObject;
            instance.SetActive(true);
            livingLetter = instance.GetComponent<MakeFriendsLivingLetter>();
            livingLetter.Init(wordData);
            livingLetter.container = this.gameObject;

            AdjustForDifficulty();

            return livingLetter;
        }

        public void MakeEntrance()
        {
            livingLetter.MakeEntrance(offscreenPosition, startingPosition, entranceRotation, entranceDuration, exitRotation);
        }

        public void MakeFriendlyExit()
        {
            AdjustForDifficulty();

            livingLetter.MakeFriendlyExit(friendlyExitPosition, friendlyExitRotation, friendlyExitDuration);
        }

        public void GoToFriendsZone(FriendsZone zone)
        {
            livingLetter.GoToFriendsZone(zone, left);
        }

        public void MoveAwayAngrily()
        {
            livingLetter.MoveAwayAngrily(NextMovingAwayPosition, exitRotation, movingAwayDuration, 1f);
            movingAwayCounter++;
        }

        public void Celebrate()
        {
            livingLetter.Celebrate(celebrationPosition, entranceRotation, celebrationDuration);
        }

        public void HighFive(float delay)
        {
            var leftLivingLetterRotation = exitRotation_unreadable * -1f;
            livingLetter.HighFive(delay, left, leftLivingLetterRotation);
        }

        public void Reset()
        {
            livingLetter = null;
            var lingeringLetter = GetComponentInChildren<MakeFriendsLivingLetter>();
            if (lingeringLetter != null)
            {
                Destroy(lingeringLetter.gameObject);
            }
            movingAwayCounter = 1;
        }
    }
}
