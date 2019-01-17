using UnityEngine;
using DG.Tweening;

namespace Antura.Test
{ 
    /// <summary>
    /// Follow the gameobject on selected axes with selected delay.
    /// </summary>
    public class GOFollower : MonoBehaviour
    {
        [Header("Follow configuration")]
        [Tooltip("Object to follow")]
        public GameObject GOToFollow;
        [Tooltip("Time to move to followed object")]
        public float FollowDelay;
        [Tooltip("Axis user for follow")]
        public bool X, Y, Z;

        Vector3 initialPosition;
        Tween tw;

        void Start() {
            initialPosition = transform.position;

        }

        void Update() {

            tw.Kill();
            tw = transform.DOMove(new Vector3(
                X ? GOToFollow.transform.position.x : initialPosition.x,
                Y ? GOToFollow.transform.position.y : initialPosition.y,
                Z ? GOToFollow.transform.position.z : initialPosition.z
                ), FollowDelay);
        }

        void OnDestroy() {
            tw.Kill();
        }

    }
}
