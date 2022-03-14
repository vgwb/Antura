using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Antura.LivingLetters;
using Antura.UI;

namespace Antura.Minigames.Tobogan
{
    public class PipeAnswer : MonoBehaviour
    {
        public TMP_Text answerText;
        public TextRender answerRender;
        public TextMeshPro answerWordDrawings;

        public GameObject aspirationParticle;
        public GameObject graphics;
        public TremblingTube trembling;

        public Transform signTransform;

        public Collider signCollider;

        public Transform tutorialPoint;

        public bool IsCorrectAnswer { get; private set; }
        public ILivingLetterData Data { get; private set; }

        List<Material> tubeMaterials = new List<Material>();

        public bool active;

        bool showSign = true;

        float easeTimer;
        const float EASE_DURATION = 4.0f;
        public bool ShowSign
        {
            get
            {
                return showSign;
            }
            set
            {
                if (value == showSign)
                    return;

                easeTimer = EASE_DURATION;
                showSign = value;
            }
        }

        public float disappearHeight = 6.5f;
        float disappearSpeed;

        public AnimationCurve easeCurve;

        void Start()
        {
            StopSelectedAnimation();

            foreach (var particles in aspirationParticle.GetComponentsInChildren<ParticleSystem>(true))
            {
                particles.Clear();
            }

            foreach (var renderer in graphics.GetComponentsInChildren<MeshRenderer>(true))
                tubeMaterials.Add(renderer.material);

            aspirationParticle.SetActive(true);
            graphics.transform.localPosition = Vector3.up * disappearHeight;
            disappearSpeed = 4.0f + 2 * UnityEngine.Random.value;
        }

        public void Update()
        {
            Vector3 targetPosition = Vector3.zero;

            if (!active)
                targetPosition = Vector3.up * disappearHeight;

            graphics.transform.localPosition = Vector3.Lerp(graphics.transform.localPosition, targetPosition, disappearSpeed * Time.deltaTime);


            if (showSign)
            {
                answerText.alpha = Mathf.Lerp(answerText.alpha, 1, Time.deltaTime * 5.0f);
                signTransform.localRotation = Quaternion.Slerp(signTransform.localRotation, Quaternion.identity, Time.deltaTime * 5.0f);
            }
            else
            {
                easeTimer -= Time.deltaTime;
                if (easeTimer < 0)
                    easeTimer = 0;

                float t = easeCurve.Evaluate(1 - (easeTimer / EASE_DURATION));
                answerText.alpha = 1 - t;

                signTransform.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, 90, 0), t);
            }
        }

        public void SetAnswer(ILivingLetterData livingLetterData, bool correct, Material fontMaterial, Color color)
        {
            Data = livingLetterData;

            if (livingLetterData.DataType == LivingLetterDataType.Image)
            {
                answerText.gameObject.SetActive(false);
                answerWordDrawings.gameObject.SetActive(true);

                answerWordDrawings.fontMaterial = fontMaterial;
                answerWordDrawings.color = color;
                answerWordDrawings.text = livingLetterData.DrawingCharForLivingLetter;
            }
            else
            {
                answerText.fontMaterial = fontMaterial;
                answerText.color = color;
                answerRender.gameObject.SetActive(true);
                answerWordDrawings.gameObject.SetActive(false);

                answerRender.SetLetterData(livingLetterData);
            }

            IsCorrectAnswer = correct;
        }

        public void SetAppearance(Material textMaterial, Material drawingMaterial, Color color)
        {
            answerText.fontMaterial = textMaterial;
            answerWordDrawings.fontMaterial = drawingMaterial;
            answerText.color = color;
            answerWordDrawings.color = color;
        }

        public void EnterAnimation()
        {

        }

        public void ExitAnimation()
        {

        }

        public void PlaySelectedAnimation()
        {
            foreach (var particles in aspirationParticle.GetComponentsInChildren<ParticleSystem>(true))
            {
                particles.Play();
            }

            trembling.Trembling = true;

            for (int i = 0, count = tubeMaterials.Count; i < count; ++i)
                tubeMaterials[i].SetFloat("_OpeningAnimation", 1);
        }

        public void StopSelectedAnimation()
        {
            foreach (var particles in aspirationParticle.GetComponentsInChildren<ParticleSystem>(true))
            {
                particles.Stop();
            }

            trembling.Trembling = false;

            for (int i = 0, count = tubeMaterials.Count; i < count; ++i)
                tubeMaterials[i].SetFloat("_OpeningAnimation", 0);
        }
    }
}
