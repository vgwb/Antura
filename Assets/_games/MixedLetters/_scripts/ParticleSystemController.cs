using UnityEngine;

namespace Antura.Minigames.MixedLetters
{
    public class ParticleSystemController : MonoBehaviour
    {
        public static ParticleSystemController instance;

        public ParticleSystem MyParticleSystem;

        void Awake()
        {
            instance = this;
            Debug.Log("ParticleSystemController");
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            MyParticleSystem.time = 0;
        }

        public void SetPosition(Vector3 position)
        {
            SetPositionWithOffset(position, Vector3.zero);
        }

        public void SetPositionWithOffset(Vector3 position, Vector3 offset)
        {
            transform.position = position + offset;
        }
    }
}
