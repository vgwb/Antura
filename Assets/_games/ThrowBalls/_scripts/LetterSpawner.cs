using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class LetterSpawner
    {
        public static LetterSpawner instance;

        public const float MIN_X = -17f;
        public const float MAX_X = 17f;

        public const float MIN_Y = 0.51f;
        public const float MAX_Y = 0.51f;

        public const float MIN_Z = -8.25f;
        public const float MAX_Z = 13.25f;

        private const float MIN_DISTANCE_SQUARED = 450f;

        public readonly Vector3 TUTORIAL_POSITION = new Vector3((MIN_X + MAX_X) / 2 + (MAX_X - MIN_X) * 0.5f, (MIN_Y + MAX_Y) / 2, (MIN_Z + MAX_Z) / 2);

        private readonly Vector3 V11 = new Vector3(4.826691f, 8.097503f, -25.57817f);
        private readonly Vector3 V12 = new Vector3(2.457545f, 7.25744f, -25.8511f);
        private readonly Vector3 V21 = new Vector3(-4.826691f, 8.097503f, -25.57817f);
        private readonly Vector3 V22 = new Vector3(-2.457545f, 7.25744f, -25.8511f);

        public LetterSpawner()
        {
            instance = this;
        }

        public Vector3[] GenerateRandomPositions(int numPositions, bool isTutorialLevel)
        {
            Vector3[] randomPositions = new Vector3[numPositions];

            for (int i = 0; i < numPositions; i++)
            {
                Vector3 randomPosition;

                float minDistanceSquared = MIN_DISTANCE_SQUARED;
                int numTries = 0;
                bool isRandomPositionInvalid = true;

                while (isRandomPositionInvalid)
                {
                    if (i == 0 && isTutorialLevel)
                    {
                        randomPosition = TUTORIAL_POSITION;
                    }

                    else
                    {
                        randomPosition = new Vector3(Random.Range(MIN_X, MAX_X), Random.Range(MIN_Y, MAX_Y), Random.Range(MIN_Z, MAX_Z));
                    }

                    isRandomPositionInvalid = false;

                    for (int j = 0; j < i; j++)
                    {
                        if ((randomPosition - randomPositions[j]).sqrMagnitude <= minDistanceSquared)
                        {
                            isRandomPositionInvalid = true;
                            break;
                        }
                    }

                    if (!isRandomPositionInvalid)
                    {
                        randomPositions[i] = randomPosition;
                    }

                    else
                    {
                        numTries++;
                        if (numTries > 15)
                        {
                            minDistanceSquared *= 0.8f;
                            numTries = 0;
                        }
                    }
                }
            }

            return randomPositions;
        }

        public Vector3 BiLerpForTutorialUI(Vector3 targetPosition)
        {
            var X = targetPosition.x;
            var Z = targetPosition.z;
            var VXZ1 = ((MAX_X - X) / (MAX_X - MIN_X)) * V11 + ((X - MIN_X) / (MAX_X - MIN_X)) * V21;
            var VXZ2 = ((MAX_X - X) / (MAX_X - MIN_X)) * V12 + ((X - MIN_X) / (MAX_X - MIN_X)) * V22;

            return ((MAX_Z - Z) / (MAX_Z - MIN_Z)) * VXZ1 + ((Z - MIN_Z) / (MAX_Z - MIN_Z)) * VXZ2;
        }
    }
}
