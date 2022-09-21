using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Antura.Rewards
{
    public class GroupSpawner : MonoBehaviour
    {
        public int nToSpawn = 10;
        public GameObject originalGo;
        public float scale = 1f;
        public Vector3 Span;
        public Vector3 gravity;

        public Vector3 speed;

        private Vector3 pastGravity;

        void Awake()
        {
            pastGravity = Physics.gravity;
            Physics.gravity = gravity;
        }

        /*public void OnEnable()
        {
            Spawn(nToSpawn);
        }*/

        public void OnDisable()
        {
            Physics.gravity = pastGravity;
        }

        public void Spawn(int nToSpawn)
        {
            originalGo.SetActive(true);
            for (int i = 0; i < nToSpawn; i++)
            {
                var spawnedGo = Instantiate(originalGo);
                spawnedGo.transform.position = this.transform.position + Span * i + Random.insideUnitSphere * 0.1f;
                spawnedGo.transform.localEulerAngles = Random.insideUnitSphere.normalized * 90;
                spawnedGo.transform.localScale = Vector3.one * (scale + Random.Range(-0.1f, 0.1f));
                spawnedGo.GetComponent<Rigidbody>().velocity = speed;
            }
            originalGo.SetActive(false);
        }
    }
}
