using UnityEngine;
using System.Collections;
using Antura.LivingLetters;
using TMPro;
using System.Collections.Generic;

namespace Antura.Minigames.MakeFriends
{
    public class MakeFriendsLivingLetter : MonoBehaviour
    {
        public LivingLetterController LLPrefab;
        public bool focusOnTouch;
        public float focusOnTouchTime;
        public Animator animator;
        public Collider letterCollider;
        public LL_LetterData letterData;
        public TMP_Text tmpText;
        [HideInInspector]
        public GameObject container;
        [HideInInspector]
        public LL_WordData wordData;

        //private Vector3 standingPosition;
        //private Vector3 offscreenPosition;
        private Vector3 initialRotation;
        private Vector3 entranceRotation;
        private bool isFocusing;
        private bool isWalking;

        private struct WalkParameters
        {
            public Vector3 from;
            public Vector3 to;
            public Vector3 rotation;
            public float duration;
            public float delay;
            public LLAnimationStates walkAnimation;
            public float walkSpeed;
            public LLAnimationStates afterWalkAnimation;
            public float afterWalkSpeed;
            public bool speak;
            public bool rotateAfterWalk;
            public Vector3 afterWalkRotation;

            public WalkParameters(Vector3 from, Vector3 to, Vector3 rotation, float duration, float delay = 0f, LLAnimationStates walkAnimation = LLAnimationStates.LL_walking, float walkSpeed = 0f, LLAnimationStates afterWalkAnimation = LLAnimationStates.LL_idle, float afterWalkSpeed = 0f, bool speak = false, bool rotateAfterWalk = false, Vector3 afterWalkRotation = default(Vector3))
            {
                this.from = from;
                this.to = to;
                this.rotation = rotation;
                this.duration = duration;
                this.delay = delay;
                this.walkAnimation = walkAnimation;
                this.walkSpeed = walkSpeed;
                this.afterWalkAnimation = afterWalkAnimation;
                this.afterWalkSpeed = afterWalkSpeed;
                this.speak = speak;
                this.rotateAfterWalk = rotateAfterWalk;
                this.afterWalkRotation = afterWalkRotation;
            }
        }


        public void Init(LL_WordData _data)
        {
            wordData = _data;
            LLPrefab.Init(_data);

            //var text = ArabicFixer.Fix(_data.Data.Arabic);
            //tmpText.text = text;
        }

        void OnMouseDown()
        {
            if (!isWalking && !isFocusing)
            {
                SpeakWord();
                if (focusOnTouch)
                {
                    Focus();
                }
            }
        }

        #region Public Methods

        public void Dance()
        {
            LLPrefab.ToggleDance();
            LLPrefab.SetDancingSpeed(LLPrefab.DancingSpeed * Random.Range(0.75f, 1.25f));
        }

        public void MakeEntrance(Vector3 offscreenPosition, Vector3 startingPosition, Vector3 entranceRotation, float entranceDuration, Vector3 afterWalkRotation)
        {
            Walk(offscreenPosition, startingPosition, entranceRotation, entranceDuration, speak: true, rotateAfterWalk: true, afterWalkRotation: afterWalkRotation);
        }

        public void MakeFriendlyExit(Vector3 position, Vector3 rotation, float duration)
        {
            var from = transform.position;
            var to = position;

            Walk(from, to, rotation, duration, walkAnimation: LLAnimationStates.LL_walking, walkSpeed: 1f);
        }

        public void GoToFriendsZone(FriendsZone zone, bool left)
        {
            var side = left ? zone.left : zone.right;
            var from = zone.entrancePosition;
            var to = side.transform.position;
            var rotation = zone.entranceRotation;
            var duration = zone.entranceDuration;
            var afterWalkRotation = left ? zone.finalRotationLeft : zone.finalRotationRight;

            this.transform.SetParent(side.transform);
            this.transform.localPosition = Vector3.zero;
            Walk(from, to, rotation, duration, walkAnimation: LLAnimationStates.LL_walking, walkSpeed: 1f, rotateAfterWalk: true, afterWalkRotation: afterWalkRotation);
        }

        public void MoveAwayAngrily(Vector3 position, Vector3 rotation, float duration, float delay)
        {
            var parameters = new WalkParameters(from: transform.position, to: position, rotation: rotation, duration: duration, delay: delay);
            StopCoroutine("MoveAwayAngrily_Coroutine");
            ResetAnimation();
            StartCoroutine("MoveAwayAngrily_Coroutine", parameters);
        }

        public void Celebrate(Vector3 celebrationPosition, Vector3 rotation, float celebrationDuration)
        {
            StopCoroutine("MoveAwayAngrily_Coroutine");
            StopCoroutine("Walk_Coroutine");
            StopCoroutine("Focus_Coroutine");
            ResetAnimation();

            var from = transform.position;
            var to = celebrationPosition;
            var duration = celebrationDuration;

            Walk(from, to, rotation, duration, walkAnimation: LLAnimationStates.LL_walking, walkSpeed: 1f, afterWalkAnimation: LLAnimationStates.LL_dancing);
        }

        public void HighFive(float delay, bool rotate = false, Vector3 rotation = default(Vector3))
        {
            StartCoroutine(HighFive_Coroutine(delay, rotate, rotation));
        }

        public IAudioSource SpeakWord()
        {
            IAudioSource s = null;
            if (wordData != null && wordData.Id != null)
            {
                s = MakeFriendsConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(wordData, true, soundType: MakeFriendsConfiguration.Instance.GetVocabularySoundType());
            }
            LookAngry();
            return s;
        }

        public void LookAngry()
        {
            LLPrefab.DoAngry();
        }

        public void ResetAnimation()
        {
            LLPrefab.Reset();
        }

        #endregion

        #region Private Methods

        private void Focus()
        {
            StartCoroutine("Focus_Coroutine");
        }

        private IEnumerator MoveAwayAngrily_Coroutine(WalkParameters parameters)
        {
            var from = parameters.from;
            var to = parameters.to;
            var rotation = parameters.rotation;
            var duration = parameters.duration;
            var delay = parameters.delay;

            LookAngry();
            yield return new WaitForSeconds(0.75f);
            Walk(from, to, rotation, duration: duration, delay: delay);
        }

        private IEnumerator Focus_Coroutine()
        {
            isFocusing = true;
            var originalRotation = transform.rotation.eulerAngles;

            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            yield return new WaitForSeconds(focusOnTouchTime);

            var focusedRotation = transform.rotation.eulerAngles;
            var interpolant = 0f;
            var lerpProgress = 0f;
            var lerpLength = 0.5f;

            while (lerpProgress < lerpLength)
            {
                transform.rotation = Quaternion.Euler(Vector3.Lerp(focusedRotation, originalRotation, interpolant));
                lerpProgress += Time.deltaTime;
                interpolant = lerpProgress / lerpLength;
                interpolant = Mathf.Sin(interpolant * Mathf.PI * 0.5f);
                yield return new WaitForFixedUpdate();
            }
            isFocusing = false;
        }

        private IEnumerator HighFive_Coroutine(float delay, bool rotate, Vector3 rotation)
        {
            yield return new WaitForSeconds(delay);
            if (rotate)
            {
                var initialRotation = transform.rotation.eulerAngles;
                var finalRotation = rotation;

                var rotationInterpolant = 0f;
                var rotationLerpProgress = 0f;
                var rotationLerpLength = 0.25f;

                while (rotationLerpProgress < rotationLerpLength)
                {
                    transform.rotation = Quaternion.Euler(Vector3.Lerp(initialRotation, finalRotation, rotationInterpolant));
                    rotationLerpProgress += Time.deltaTime;
                    rotationInterpolant = rotationLerpProgress / rotationLerpLength;
                    rotationInterpolant = Mathf.Sin(rotationInterpolant * Mathf.PI * 0.5f);
                    yield return new WaitForFixedUpdate();
                }
            }
            LLPrefab.DoHighFive();
        }

        private void Walk(Vector3 from, Vector3 to, Vector3 rotation, float duration, float delay = 0f, LLAnimationStates walkAnimation = LLAnimationStates.LL_walking, float walkSpeed = 0f, LLAnimationStates afterWalkAnimation = LLAnimationStates.LL_idle, float afterWalkSpeed = 0f, bool speak = false, bool rotateAfterWalk = false, Vector3 afterWalkRotation = default(Vector3))
        {
            var parameters = new WalkParameters(from, to, rotation, duration, delay, walkAnimation, walkSpeed, afterWalkAnimation, afterWalkSpeed, speak, rotateAfterWalk, afterWalkRotation);
            StopCoroutine("Walk_Coroutine");
            StartCoroutine("Walk_Coroutine", parameters);
        }

        private IEnumerator Walk_Coroutine(WalkParameters parameters)
        {
            isWalking = true;
            var from = parameters.from;
            var to = parameters.to;
            var rotation = parameters.rotation;
            var duration = parameters.duration;
            var delay = parameters.delay;
            var walkAnimation = parameters.walkAnimation;
            var walkSpeed = parameters.walkSpeed;
            var afterWalkAnimation = parameters.afterWalkAnimation;
            var afterWalkSpeed = parameters.afterWalkSpeed;
            var speak = parameters.speak;
            var rotateAfterWalk = parameters.rotateAfterWalk;
            var afterWalkRotation = parameters.afterWalkRotation;

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            transform.rotation = Quaternion.Euler(rotation);
            //animator.SetTrigger(walkAnimation);
            LLPrefab.SetState(walkAnimation);
            LLPrefab.SetWalkingSpeed(walkSpeed);

            var interpolant = 0f;
            var lerpProgress = 0f;
            var lerpLength = duration;

            while (lerpProgress < lerpLength)
            {
                transform.localPosition = Vector3.Lerp(from, to, interpolant);
                lerpProgress += Time.deltaTime;
                interpolant = lerpProgress / lerpLength;
                interpolant = Mathf.Sin(interpolant * Mathf.PI * 0.5f);
                yield return new WaitForFixedUpdate();
            }

            //animator.SetTrigger(afterWalkAnimation);
            LLPrefab.SetState(afterWalkAnimation);
            LLPrefab.SetWalkingSpeed(afterWalkSpeed);

            if (speak)
            {
                yield return new WaitForSeconds(0.25f);

                // Dirty Hack to fix an audio synch bug
                while (MakeFriendsGame.Instance.IsIntroducingLetter)
                    yield return null;

                MakeFriendsGame.Instance.IsIntroducingLetter = true;
                var audioSource = SpeakWord();
                while (!audioSource.IsLoaded)
                {
                    yield return null;
                }

                while (audioSource.IsPlaying)
                {
                    yield return null;
                }
                MakeFriendsGame.Instance.IsIntroducingLetter = false;
                yield return new WaitForSeconds(0.25f);
                MakeFriendsGame.Instance.SpokenWords++;
            }

            if (rotateAfterWalk)
            {
                var initialRotation = transform.rotation.eulerAngles;
                var finalRotation = afterWalkRotation;

                var rotationInterpolant = 0f;
                var rotationLerpProgress = 0f;
                var rotationLerpLength = 0.5f;

                while (rotationLerpProgress < rotationLerpLength)
                {
                    transform.rotation = Quaternion.Euler(Vector3.Lerp(initialRotation, finalRotation, rotationInterpolant));
                    rotationLerpProgress += Time.deltaTime;
                    rotationInterpolant = rotationLerpProgress / rotationLerpLength;
                    rotationInterpolant = Mathf.Sin(rotationInterpolant * Mathf.PI * 0.5f);
                    yield return new WaitForFixedUpdate();
                }
            }
            isWalking = false;
        }

        public void MarkLetters(List<ILivingLetterData> commonLetters, Color color)
        {
            LLPrefab.MarkLetters(commonLetters, color);
        }

        #endregion
    }
}
