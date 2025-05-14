
using System.Collections.Generic;
using UnityEngine;

namespace PetanqueGame.Utils
{
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _initialSize = 10;

        private Queue<GameObject> _pool = new Queue<GameObject>();

        private void Start()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                GameObject obj = Instantiate(_prefab);
                obj.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        public GameObject GetFromPool(Vector3 position, Quaternion rotation)
        {
            GameObject obj = _pool.Count > 0 ? _pool.Dequeue() : Instantiate(_prefab);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}
