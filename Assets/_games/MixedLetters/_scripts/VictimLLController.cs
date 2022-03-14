using UnityEngine;
using System.Collections;
using Antura.LivingLetters;

namespace Antura.Minigames.MixedLetters
{
    public class VictimLLController : MonoBehaviour
    {
        private const float LOOK_TOWARDS_ANTURA_TIME = 0.33f;
        private const float BIG_SCALE_VALUE = 1.5f;

        public static VictimLLController instance;
        public LivingLetterController letterObjectView;

        public GameObject victoryRays;

        void Awake()
        {
            instance = this;
        }

        public void SetBigScale()
        {
            letterObjectView.Scale = BIG_SCALE_VALUE;
        }

        public void SetCustomText(string text)
        {
            if (letterObjectView.LabelRender.enabled == false)
            {
                letterObjectView.LabelRender.enabled = true;
            }

            letterObjectView.LabelRender.SetText(text);
        }

        public void LookTowardsAntura()
        {
            StartCoroutine(LookTowardsAnturaCoroutine());
        }

        private IEnumerator LookTowardsAnturaCoroutine()
        {
            yield return new WaitForSeconds(0.1f);

            letterObjectView.SetState(LLAnimationStates.LL_still);

            float targetAngle = 180 + 80 * (AnturaController.instance.LastEnteredFromTheLeft ? 1 : -1);

            float timeElapsed = 0;
            float sinFactor = 2 * Mathf.PI * Mathf.Pow(LOOK_TOWARDS_ANTURA_TIME * 4, -1);

            while (timeElapsed < LOOK_TOWARDS_ANTURA_TIME)
            {
                transform.rotation = Quaternion.Euler(0, Mathf.Lerp(180, targetAngle, Mathf.Sin(sinFactor * timeElapsed)), 0);
                timeElapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            letterObjectView.DoSmallJump();
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
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
            transform.rotation = Quaternion.Euler(0, 180, 0);
            letterObjectView.SetState(LLAnimationStates.LL_idle);
        }

        public void DoHooray()
        {
            letterObjectView.DoHorray();
        }

        public void ShowVictoryRays()
        {
            victoryRays.SetActive(true);
        }

        public void HideVictoryRays()
        {
            victoryRays.SetActive(false);
        }
    }
}
