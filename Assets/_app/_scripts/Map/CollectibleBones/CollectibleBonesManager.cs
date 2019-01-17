using Antura.Core;
using Antura.Helpers;
using Antura.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Collectible
{
    public class CollectibleBonesManager : MonoBehaviour
    {
        public BonesCounter bonesCounter;
        public GameObject collectibleBonePrefabGO;

        public float delay = 1.0f;
        public float duration = 5.0f;

        private List<CollectibleBoneSpawnPivot> spawnPivots;

        void Start()
        {
            bonesCounter.Hide();
            spawnPivots = FindObjectsOfType<CollectibleBoneSpawnPivot>().ToList();
            InvokeRepeating("SpawnNewBone", delay, delay);
        }

        private void SpawnNewBone()
        {
            if (spawnPivots.Count == 0) {
                // No more spawn pivots left
                CancelInvoke("SpawnNewBone");
                return;
            }

            var go = Instantiate(collectibleBonePrefabGO);
            var bone = go.GetComponent<CollectibleBone>();

            var pivot = RandomHelper.RandomSelectOne(spawnPivots);
            spawnPivots.Remove(pivot); // Disable it

            bone.transform.position = pivot.transform.position;
            bone.transform.eulerAngles = pivot.transform.eulerAngles;
            bone.Initialise(duration);
            bone.OnPickUpBone += HandlePickUpBone;
        }

        private bool _isHandlingBone = false;
        private void HandlePickUpBone()
        {
            if (_isHandlingBone) {
                StopAllCoroutines();
                bonesCounter.IncreaseByOne();
                AppManager.I.Player.AddBones(1);
            }
            StartCoroutine(HandlePickupBoneCO());
        }

        private IEnumerator HandlePickupBoneCO()
        {
            _isHandlingBone = true;
            bonesCounter.Show();
            yield return new WaitForSeconds(0.1f);
            _isHandlingBone = false;
            bonesCounter.IncreaseByOne();
            AppManager.I.Player.AddBones(1);
            yield return new WaitForSeconds(2.0f);
            bonesCounter.Hide();
        }
    }
}