using UnityEngine;
using System.Collections;
using Antura.LivingLetters;
using Antura.UI;
using TMPro;

namespace Antura.Minigames.MakeFriends
{
    public class LetterBalloonController : MonoBehaviour
    {
        public TextRender displayedText;
        public Rigidbody rigidBody;
        public Renderer balloonRenderer;
        public Animator animator;
        [HideInInspector]
        public ILivingLetterData letterData;

        private struct MoveParameters
        {
            public Vector3 from;
            public Vector3 to;
            public float duration;

            public MoveParameters(Vector3 from, Vector3 to, float duration)
            {
                this.from = from;
                this.to = to;
                this.duration = duration;
            }
        }


        public void Init(ILivingLetterData _data)
        {
            letterData = _data;
            displayedText.SetLetterData(letterData);
        }

        public void SetColor(Color color)
        {
            balloonRenderer.material.color = color;
        }

        public void EnterScene(bool correctChoice)
        {
            if (correctChoice)
            {
                SetColor(Color.green);
            }
            else
            {
                SetColor(Color.red);
            }

            StopCoroutine("EnterScene_Coroutine");
            StartCoroutine("EnterScene_Coroutine", correctChoice);
        }

        public IEnumerator EnterScene_Coroutine(bool correctChoice)
        {
            var from = transform.position;
            var center = Vector3.up * 7.5f;
            var duration = 1f;
            Move(from, center, duration);

            yield return new WaitForSeconds(duration);

            if (correctChoice)
            {
                var exitFrom = center;
                var exitTo = Vector3.up * 30f;
                var exitDuration = 1f;
                Move(exitFrom, exitTo, exitDuration);

                yield return new WaitForSeconds(exitDuration);
            }
            else
            {
                Pop();
            }
            this.gameObject.SetActive(false);
            Destroy(this.gameObject, 3f);
        }

        private void Pop()
        {
            MakeFriendsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.BalloonPop);
            animator.SetBool("Pop", true);
            var poof = Instantiate(MakeFriendsGame.Instance.FxParticlesPoof, 3f * Vector3.up, Quaternion.identity) as GameObject;
            Destroy(poof, 10);
        }

        private void Move(Vector3 from, Vector3 to, float duration)
        {
            var parameters = new MoveParameters(from, to, duration);
            StopCoroutine("Move_Coroutine");
            StartCoroutine("Move_Coroutine", parameters);
        }

        private IEnumerator Move_Coroutine(MoveParameters parameters)
        {
            var from = parameters.from;
            var to = parameters.to;
            var duration = parameters.duration;

            var interpolant = 0f;
            var lerpProgress = 0f;
            var lerpLength = duration;

            while (lerpProgress < lerpLength)
            {
                rigidBody.position = Vector3.Lerp(from, to, interpolant);
                lerpProgress += Time.deltaTime;
                interpolant = lerpProgress / lerpLength;
                interpolant = Mathf.Sin(interpolant * Mathf.PI * 0.5f);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
