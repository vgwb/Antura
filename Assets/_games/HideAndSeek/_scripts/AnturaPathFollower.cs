using System;
using System.Collections.Generic;
using Antura.Dog;
using Antura.Helpers;
using UnityEngine;

namespace Antura.Minigames.HideAndSeek
{
    public class AnturaPathFollower : MonoBehaviour
    {
        private List<Vector3> path;

        int currentNode = 0;
        float speed = 10f;

        float randomSniffTime;
        bool isSniffing = false;

        public bool IsFollowing { get { return path != null; } }

        AnturaAnimationController animationController;

        void Awake()
        {
            animationController = GetComponent<AnturaAnimationController>();
        }

        public void FollowPath(AnturaPath path)
        {
            this.path = path.GetPath();
            currentNode = 0;
            transform.position = this.path[0];
            animationController.State = AnturaAnimationStates.walking;
            randomSniffTime = UnityEngine.Random.Range(3, 6);
            isSniffing = false;
        }

        void Update()
        {
            if (path != null)
            {
                var target = path[currentNode];

                var distance = target - transform.position;
                float distanceSqrMagnitude = distance.sqrMagnitude;

                distance.y = 0;

                if (distanceSqrMagnitude < 2)
                {
                    // reached
                    ++currentNode;
                    if (currentNode >= path.Count)
                    {
                        animationController.State = AnturaAnimationStates.idle;
                        path = null;
                    }
                }
                else
                {
                    if (isSniffing)
                        return;

                    randomSniffTime -= Time.deltaTime;

                    if (randomSniffTime < 0)
                    {
                        randomSniffTime = UnityEngine.Random.Range(3, 6);
                        isSniffing = true;
                        speed = 0;
                        animationController.DoSniff(() => { isSniffing = false; });
                        Audio.AudioManager.I.PlaySound(Sfx.DogSnorting);
                    }
                    else
                    {
                        distance.Normalize();

                        speed = Mathf.Min(12, speed + 20 * Time.deltaTime);

                        //transform.position += distance * Mathf.Abs(Vector3.Dot(distance, transform.forward)) * speed * Time.deltaTime;
                        Vector3 direction = Vector3.Slerp(distance, transform.forward, Mathf.Sqrt(distanceSqrMagnitude) / 2);

                        transform.position += direction * speed * Time.deltaTime;
                        GameplayHelper.LerpLookAtPlanar(transform, target, Time.deltaTime * 4);
                    }
                }
            }
        }
    }
}
