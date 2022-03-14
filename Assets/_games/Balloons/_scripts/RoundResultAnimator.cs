using UnityEngine;
using System.Collections;
using Antura.LivingLetters;
using Antura.Tutorial;

namespace Antura.Minigames.Balloons
{
    public class RoundResultAnimator : MonoBehaviour
    {
        public LivingLetterController LLPrefab;
        public ParticleSystem vfx;
        public Vector3 wrongMarkPosition1;
        public Vector3 wrongMarkPosition2;
        public Vector3 onscreenPosition;
        public Vector3 offscreenPosition;
        public float moveDuration;

        public void ShowWin(ILivingLetterData livingLetterData)
        {
            this.transform.position = offscreenPosition;
            LLPrefab.gameObject.SetActive(true);
            vfx.gameObject.SetActive(true);
            if (livingLetterData != null)
            {
                LLPrefab.Init(livingLetterData);
            }
            LLPrefab.DoHorray();
            vfx.Play();

            StartCoroutine(Move_Coroutine(offscreenPosition, onscreenPosition, moveDuration));
        }

        public void ShowLose(ILivingLetterData livingLetterData)
        {
            this.transform.position = offscreenPosition;
            LLPrefab.gameObject.SetActive(true);
            vfx.gameObject.SetActive(false);
            if (livingLetterData != null)
            {
                LLPrefab.Init(livingLetterData);
            }
            LLPrefab.DoAngry();
            TutorialUI.MarkNo(Random.value <= 0.5f ? wrongMarkPosition1 : wrongMarkPosition2, TutorialUI.MarkSize.Huge);
            BalloonsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.KO);

            StartCoroutine(Move_Coroutine(offscreenPosition, onscreenPosition, moveDuration));
        }

        public void Hide()
        {
            StartCoroutine(Move_Coroutine(onscreenPosition, offscreenPosition, moveDuration, true));
        }

        private IEnumerator Move_Coroutine(Vector3 from, Vector3 to, float duration, bool hide = false)
        {
            var interpolant = 0f;
            var lerpProgress = 0f;
            var lerpLength = duration;

            while (lerpProgress < lerpLength)
            {
                transform.position = Vector3.Lerp(from, to, interpolant);
                lerpProgress += Time.deltaTime;
                interpolant = lerpProgress / lerpLength;
                interpolant = Mathf.Sin(interpolant * Mathf.PI * 0.5f);
                yield return new WaitForFixedUpdate();
            }

            if (hide)
            {
                LLPrefab.LabelRender.text = "";
                LLPrefab.gameObject.SetActive(false);
                vfx.gameObject.SetActive(false);
            }
        }
    }
}
