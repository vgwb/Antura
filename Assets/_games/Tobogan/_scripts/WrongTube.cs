using UnityEngine;
using System.Collections.Generic;

namespace Antura.Minigames.Tobogan
{
    public class WrongTube : MonoBehaviour
    {
        List<System.Action> dropRequest = new List<System.Action>();

        public Transform spawnTransform;
        public GameObject letterPrefab;

        public TremblingTube tube;
        float spittingTimer = 0;
        bool sfxPlayed = false;

        GameObject spittingLetter;

        public bool doSpawn = false;

        bool dropping = false;

        void Update()
        {
            if (!dropping && (doSpawn || dropRequest.Count > 0))
            {
                doSpawn = false;
                dropping = true;

                System.Action toCall = null;

                if (dropRequest.Count > 0)
                {
                    toCall = dropRequest[dropRequest.Count - 1];
                    dropRequest.RemoveAt(dropRequest.Count - 1);
                }

                spittingLetter = Instantiate(letterPrefab);
                spittingLetter.SetActive(false);

                var ragdoll = spittingLetter.GetComponent<LivingLetterRagdoll>();
                ragdoll.maxPoofCountdownAfterHit = 0.15f;

                spittingTimer = 0.5f;
                ragdoll.SetRagdoll(true, spawnTransform.forward * 75);
                ragdoll.onPoofed += () =>
                {
                    if (toCall != null)
                        toCall();

                    dropping = false;
                };

            }

            if (spittingTimer > 0)
            {
                tube.Trembling = true;
                spittingTimer -= Time.deltaTime;

                if (!sfxPlayed && spittingTimer < 0.3f)
                {
                    sfxPlayed = true;
                    ToboganConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.UIPauseIn);
                }

                if (spittingTimer <= 0)
                {
                    spittingLetter.transform.position = spawnTransform.position;
                    spittingLetter.transform.forward = spawnTransform.right;

                    spittingLetter.transform.RotateAround(spittingLetter.transform.position + spittingLetter.transform.up * 3.5f, spittingLetter.transform.forward, Random.value * 360);
                    spittingLetter.SetActive(true);
                    spittingLetter = null;
                }
            }
            else
            {
                sfxPlayed = false;
                tube.Trembling = false;
            }
        }

        public void DropLetter(System.Action callback)
        {
            dropRequest.Add(callback);
        }
    }
}
