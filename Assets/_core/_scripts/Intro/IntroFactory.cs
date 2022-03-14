using UnityEngine;
using System.Collections.Generic;
using Antura.LivingLetters;
using Antura.Minigames.FastCrowd;
using Antura.Core;
using Antura.Helpers;

namespace Antura.Intro
{
    /// <summary>
    /// Controls the instantiation of game objects in the Intro scene.
    /// </summary>
    public class IntroFactory : MonoBehaviour
    {
        public event System.Action<ILivingLetterData, bool> onDropped;

        public LettersWalkableArea walkableArea;
        public AnturaRunnerController antura;

        public LivingLetterController livingLetterPrefab;
        public GameObject puffPrefab;

        public int MaxConcurrentLetters = 5;

        protected List<IntroStrollingLetter> letters = new List<IntroStrollingLetter>();
        List<GameObject> letterGOs = new List<GameObject>();

        Queue<IntroStrollingLetter> toDestroy = new Queue<IntroStrollingLetter>();

        [HideInInspector]
        public bool StartSpawning = false;

        public void GetNearLetters(List<IntroStrollingLetter> output, Vector3 position, float radius)
        {
            for (int i = 0, count = letters.Count; i < count; ++i)
            {
                if (Vector3.Distance(letters[i].transform.position, position) < radius)
                {
                    output.Add(letters[i]);
                }
            }
        }

        public void Clean()
        {
            //toAdd.Clear();

            foreach (var l in letters)
            {
                toDestroy.Enqueue(l);
            }

            letters.Clear();
            letterGOs.Clear();
        }

        protected virtual LivingLetterController SpawnLetter()
        {
            // Spawn!
            LivingLetterController LLController = Instantiate(livingLetterPrefab);
            LLController.gameObject.SetActive(true);
            LLController.transform.SetParent(transform, true);

            Vector3 newPosition = walkableArea.GetFurthestSpawn(letterGOs); // Find isolated spawn point

            LLController.transform.position = newPosition;
            LLController.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.value * 360, 0);
            LLController.Init(AppManager.I.Teacher.GetAllTestLetterDataLL().GetRandom());

            LLController.gameObject.AddComponent<Rigidbody>().isKinematic = true;

            foreach (var collider in LLController.gameObject.GetComponentsInChildren<Collider>())
            {
                collider.isTrigger = true;
            }

            var characterController = LLController.gameObject.AddComponent<CharacterController>();
            characterController.height = 6;
            characterController.center = Vector3.up * 3;
            characterController.radius = 1.5f;
            LLController.gameObject.AddComponent<LetterCharacterController>();

            var livingLetter = LLController.gameObject.AddComponent<IntroStrollingLetter>();
            livingLetter.factory = this;

            var pos = LLController.transform.position;
            pos.y = 10;
            LLController.transform.position = pos;

            letters.Add(livingLetter);
            letterGOs.Add(livingLetter.gameObject);

            livingLetter.onDropped += (result) =>
            {
                if (result)
                {
                    letters.Remove(livingLetter);
                    letterGOs.Remove(livingLetter.gameObject);
                    toDestroy.Enqueue(livingLetter);
                }

                if (onDropped != null)
                {
                    onDropped(LLController.Data, result);
                }
            };

            return LLController;
        }

        void Update()
        {
            if (StartSpawning)
            {
                if (letters.Count < MaxConcurrentLetters)
                {
                    SpawnLetter();
                }
            }

            //if (toDestroy.Count > 0)
            //{
            //    destroyTimer -= Time.deltaTime;

            //    if (destroyTimer <= 0)
            //    {
            //        destroyTimer = 0.1f;

            //        var puffGo = GameObject.Instantiate(puffPrefab);
            //        puffGo.AddComponent<AutoDestroy>().duration = 2;
            //        puffGo.SetActive(true);

            //        var t = toDestroy.Dequeue();
            //        puffGo.transform.position = t.transform.position + Vector3.up * 3;
            //        Destroy(t.gameObject);
            //    }
            //}
        }
    }
}
