using System;
using Antura.LivingLetters;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    // A living letter to be used in a MiniGame
    public class GameLivingLetter
    {
        public LivingLetterController livingLetter;
        private Transform shadowTransform;

        private Vector3 startPosition;
        private Vector3 endPosition;

        private float startScale = 0.4f;
        private float endScale = 1f;

        private Vector3 shadowLocalPosition;

        private float delay;
        private float danceSpeed;

        private Action endCallback;
        public bool dropped;

        public GameLivingLetter(Transform parent, GameObject letterObjectViewPrefab, GameObject shadowPrefab, ILivingLetterData livingLetterData, Vector3 startPosition, Vector3 shadowPosition, Vector3 endPosition, float delay, Action endCallback,
            float endScale, float danceSpeed)
        {
            livingLetter = UnityEngine.Object.Instantiate(letterObjectViewPrefab).GetComponent<LivingLetterController>();

            livingLetter.transform.SetParent(parent);
            livingLetter.transform.localPosition = startPosition;

            livingLetter.Init(livingLetterData);
            livingLetter.transform.localScale *= startScale;
            livingLetter.gameObject.SetActive(false);

            shadowTransform = UnityEngine.Object.Instantiate(shadowPrefab).transform;
            shadowTransform.SetParent(parent);
            shadowLocalPosition = shadowPosition;
            shadowTransform.localPosition = shadowLocalPosition;
            shadowTransform.localScale *= 0.7f;
            shadowTransform.gameObject.SetActive(false);

            this.startPosition = startPosition;
            this.endPosition = endPosition;

            this.delay = delay;
            this.danceSpeed = danceSpeed;

            this.endCallback = endCallback;
            this.endScale = endScale;

            JumpToEnd();
        }

        public void Update(float delta)
        {
            shadowLocalPosition.x = livingLetter.transform.localPosition.x;
            shadowLocalPosition.z = livingLetter.transform.localPosition.z;
            shadowTransform.localPosition = shadowLocalPosition;
        }

        public void DestroyLetter()
        {
            UnityEngine.Object.Destroy(livingLetter.gameObject);
            UnityEngine.Object.Destroy(shadowTransform.gameObject);
        }

        public void JumpToEnd()
        {
            float duration = 0.4f;

            livingLetter.transform.DOLocalMove(startPosition, delay).OnComplete(delegate ()
            {
                shadowTransform.gameObject.SetActive(true);
                livingLetter.gameObject.SetActive(true);

                livingLetter.transform.DOScale(endScale, duration * 0.5f);

                //livingLetter.Poof();
                livingLetter.OnJumpStart();

                float timeToJumpStart = 0f;// 0.15f;
                float timeToJumpEnd = 0.2f;

                Sequence animationSequence = DOTween.Sequence();
                animationSequence.AppendInterval(timeToJumpStart);
                animationSequence.AppendCallback(delegate ()
                { livingLetter.transform.DOLocalJump(endPosition, 7f, 1, duration).OnComplete(delegate () { if (endCallback != null) endCallback(); }).SetEase(Ease.Linear); });
                animationSequence.AppendInterval(duration - timeToJumpEnd);
                animationSequence.AppendCallback(delegate ()
                { livingLetter.OnJumpEnded(); });
                animationSequence.Play();
            });
        }
    }
}
