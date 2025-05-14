
using UnityEngine;

namespace PetanqueGame.Physics
{
    public class JackThrower : MonoBehaviour
    {
        [SerializeField] private GameObject _jackPrefab;
        [SerializeField] private Transform _throwPoint;
        private bool _hasThrown;

        private void Start()
        {
            ThrowJack();
        }

        private void ThrowJack()
        {
            if (_hasThrown) return;

            GameObject jack = Instantiate(_jackPrefab, _throwPoint.position, Quaternion.identity);
            Rigidbody rb = jack.GetComponent<Rigidbody>();
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(2f, 4f));
            rb.AddForce(randomDir * 3f, ForceMode.Impulse);
            _hasThrown = true;
        }
    }
}
