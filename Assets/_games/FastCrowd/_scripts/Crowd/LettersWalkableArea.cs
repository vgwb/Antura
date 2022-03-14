using System.Collections.Generic;
using Antura.Helpers;
using UnityEngine;

namespace Antura.Minigames.FastCrowd
{
    public class LettersWalkableArea : MonoBehaviour
    {
        public GameObject[] spawnPoints;
        BoxCollider[] colliders;

        // Use a different collider for random targets
        public BoxCollider[] walkingTargets;

        public Transform tutorialPosition;
        public Transform focusPosition;

        void Awake()
        {
            colliders = GetComponentsInChildren<BoxCollider>(false);
        }

        public Vector3 GetFurthestSpawn(IList<GameObject> letters)
        {
            float bestDistance = -1;
            Vector3 bestSpawn = Vector3.zero;

            var shuffledSpawnPoints = spawnPoints.ShuffleCopy();

            if (letters == null || letters.Count == 0)
                return shuffledSpawnPoints[0].transform.position;

            foreach (var spawn in shuffledSpawnPoints)
            {
                var spawnPos = spawn.transform.position;

                float minDistance = float.PositiveInfinity;
                foreach (var letter in letters)
                {
                    var letterPosition = letter.transform.position;
                    minDistance = Mathf.Min(minDistance, Vector2.Distance(new Vector2(spawnPos.x, spawnPos.z), new Vector2(letterPosition.x, letterPosition.z)));
                }

                if (minDistance > bestDistance)
                {
                    bestDistance = minDistance;
                    bestSpawn = spawn.transform.position;
                }
            }

            return bestSpawn;
        }

        public bool IsInside(Vector3 pos, bool limitToWalkingTargets)
        {
            var colliderSet = limitToWalkingTargets ? walkingTargets : colliders;

            for (int i = 0, count = colliderSet.Length; i < count; ++i)
            {
                var localPos = colliderSet[i].transform.InverseTransformPoint(pos) - colliderSet[i].center;
                var colliderSize = colliderSet[i].size;

                if (localPos.x >= -colliderSize.x * 0.5f &&
                    localPos.x <= colliderSize.x * 0.5f &&
                    localPos.y >= -colliderSize.y * 0.5f &&
                    localPos.y <= colliderSize.y * 0.5f)
                    return true;
            }

            return false;
        }

        public Vector3 GetRandomPosition()
        {
            BoxCollider randomCollider = walkingTargets[UnityEngine.Random.Range(0, walkingTargets.Length)];

            Vector3 randomLocalPos = (Vector3.right * (UnityEngine.Random.value - 0.5f) * randomCollider.size.x + Vector3.up * (UnityEngine.Random.value - 0.5f) * randomCollider.size.y) + randomCollider.center;
            Vector3 randomPosition = randomCollider.transform.TransformPoint(randomLocalPos);
            randomPosition.y = 0;

            return randomPosition;
        }


        public Vector3 GetNearestPoint(Vector3 pos, bool limitToWalkingTargets = false)
        {
            Vector3 nearest = pos;
            float nearestDistance = float.PositiveInfinity;

            var colliderSet = limitToWalkingTargets ? walkingTargets : colliders;

            for (int i = 0, count = colliderSet.Length; i < count; ++i)
            {
                var localPos = colliderSet[i].transform.InverseTransformPoint(pos) - colliderSet[i].center;
                var colliderSize = colliderSet[i].size;
                localPos.x = Mathf.Clamp(localPos.x, -colliderSize.x * 0.5f, colliderSize.x * 0.5f);
                localPos.y = Mathf.Clamp(localPos.y, -colliderSize.y * 0.5f, colliderSize.y * 0.5f);
                var nearPos = colliderSet[i].transform.TransformPoint(localPos + colliderSet[i].center);

                float distance = Vector3.Distance(nearPos, pos);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = nearPos;
                }
            }

            return nearest;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            foreach (var spawn in spawnPoints)
                Gizmos.DrawSphere(spawn.transform.position, 0.4f);
        }
#endif
    }
}
