using Antura.Dog;
using Antura.Audio;
using Antura.Helpers;
using UnityEngine;

namespace Antura.Minigames.FastCrowd
{
    public class AnturaRunnerController : MonoBehaviour
    {
        public GameObject[] targetPositions;
        public Transform HidePosition;
        Vector3 target;
        public const float ANTURA_SPEED = 15.0f;

        public AnturaAnimationController antura;

        public bool IsAnturaTime { get; private set; }

        float nextAnturaBarkTimer;

        void Start()
        {
            transform.position = HidePosition.position;
            target = HidePosition.position;
            antura.State = AnturaAnimationStates.walking;
            antura.SetWalkingSpeed(AnturaAnimationController.RUN_SPEED);
        }

        void Update()
        {
            var distance = target - transform.position;
            distance.y = 0;

            if (IsAnturaTime)
            {
                if (nextAnturaBarkTimer <= 0)
                {
                    PrepareNextAnturaBark();
                    antura.DoShout(() =>
                    {
                        AudioManager.I.PlaySound(Sfx.DogBarking);
                    });

                }
                else
                    nextAnturaBarkTimer -= Time.deltaTime;
            }

            if (distance.sqrMagnitude < 0.1f)
            {
                // reached
                if (IsAnturaTime)
                    SetRandomTarget();
            }
            else
            {
                distance.Normalize();
                transform.position += distance * Mathf.Abs(Vector3.Dot(distance, transform.forward)) * ANTURA_SPEED * Time.deltaTime;
                GameplayHelper.LerpLookAtPlanar(transform, target, Time.deltaTime * 4);
            }
        }

        void SetRandomTarget()
        {
            target = targetPositions[Random.Range(0, targetPositions.Length)].transform.position;
        }

        public void SetAnturaTime(bool _isAnturaTime)
        {
            IsAnturaTime = _isAnturaTime;
            if (IsAnturaTime)
            {
                SetRandomTarget();
                PrepareNextAnturaBark();
            }
            else
                target = HidePosition.position;
        }

        void PrepareNextAnturaBark()
        {
            nextAnturaBarkTimer = Random.Range(1, 3);
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            foreach (var spawn in targetPositions)
                Gizmos.DrawSphere(spawn.transform.position, 0.8f);
        }
#endif
    }
}
