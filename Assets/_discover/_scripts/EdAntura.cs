using Antura.Dog;
using DG.Tweening;
using System;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class EdAntura : MonoBehaviour
    {
        GameObject anturaPrefab;
        private AnturaAnimationController anturaAnimation => anturaPetSwitcher.AnimController;
        public AnturaPetSwitcher anturaPetSwitcher;
        public float speed = 5f;

        public void Initialize()
        {
            Transform tr;
            (tr = anturaPetSwitcher.transform).SetParent(transform);
            tr.localEulerAngles = new Vector3(0f, 90f);
            tr.localScale = Vector3.one;
        }

        void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput);
            movementDirection.Normalize();

            transform.Translate(movementDirection * speed * Time.deltaTime);
        }

        public void PlayAnimation(AnturaAnimationStates anim)
        {
            anturaAnimation.State = anim;
        }

        public void GoTo(Transform posTr, Action callback = null)
        {
            anturaAnimation.State = AnturaAnimationStates.walking;
            Move(posTr.position, 1f, callback);
        }

        Tween moveTween;
        Action moveEndCallback;
        void Move(Vector3 position, float duration, Action callback)
        {
            moveTween?.Kill();
            moveEndCallback = callback;
            moveTween = transform.DOMove(position, duration).OnComplete(() =>
            {
                moveEndCallback?.Invoke();
            });
        }
    }
}
