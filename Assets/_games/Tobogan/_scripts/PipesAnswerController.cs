using UnityEngine;
using System.Collections.Generic;
using Antura.LivingLetters;

namespace Antura.Minigames.Tobogan
{
    public class PipesAnswerController : MonoBehaviour
    {
        public PipeAnswer[] pipeAnswers;
        public Transform basePosition;

        ToboganGame game;
        float hidingProbability;

        PipeAnswer currentPipeAnswer;
        float hideSignsTimer = 0;

        List<PipeAnswer> toHide = new List<PipeAnswer>();

        float maxLetterDistanceX = 3.0f;
        float maxLetterDistanceY = 3.5f;

        public void SetSignHidingProbability(float hidingProbability)
        {
            this.hidingProbability = hidingProbability;
        }

        public void Initialize(ToboganGame game)
        {
            this.game = game;

            currentPipeAnswer = null;

            HidePipes();

            game.Context.GetInputManager().onPointerDown += OnPointerDown;
        }

        public void SetPipeAnswers(IEnumerable<ILivingLetterData> wrongAnswers, ILivingLetterData correctAnswers, bool sunMoonQuestion)
        {
            // Selecting auto-hiding signs
            toHide.Clear();
            for (int i = 0; i < pipeAnswers.Length; ++i)
            {
                if (Random.value + 0.0001f < hidingProbability)
                {
                    toHide.Add(pipeAnswers[i]);
                }
            }

            hideSignsTimer = 1.5f + 0.5f * toHide.Count;
            HidePipes();

            currentPipeAnswer = null;

            List<ILivingLetterData> wrongs = new List<ILivingLetterData>();

            foreach (ILivingLetterData answer in wrongAnswers)
                wrongs.Add(answer);

            int answersCount = wrongs.Count + 1;

            if (answersCount > 4)
                answersCount = 4;

            if (sunMoonQuestion)
            {
                int correctIndex = 0;
                int wrongIndex = 1;

                if (correctAnswers.Id == "the_sun")
                {
                    correctIndex = 1;
                    wrongIndex = 0;
                }

                pipeAnswers[correctIndex].SetAnswer(correctAnswers, true, game.drawingMaterial, Color.black);
                pipeAnswers[correctIndex].active = true;
                pipeAnswers[correctIndex].ShowSign = true;

                pipeAnswers[wrongIndex].SetAnswer(wrongs[0], false, game.drawingMaterial, Color.black);
                pipeAnswers[wrongIndex].active = true;
                pipeAnswers[wrongIndex].ShowSign = true;

            }
            else
            {
                int correctPosition = Random.Range(0, answersCount);

                for (int i = 0; i < answersCount; i++)
                {
                    if (i == correctPosition)
                    {
                        pipeAnswers[i].SetAnswer(correctAnswers, true,
                            ToboganGame.I.Difficulty <= 0.1f ? game.markedTextMaterial : game.textMaterial,
                            ToboganGame.I.Difficulty <= 0.1f ? (Color)ToboganGame.LETTER_MARK_PIPE_COLOR : Color.black);
                    }
                    else
                    {
                        int wrongIndex = Random.Range(0, wrongs.Count);

                        pipeAnswers[i].SetAnswer(wrongs[wrongIndex], false, game.textMaterial, Color.black);

                        wrongs.RemoveAt(wrongIndex);
                    }

                    pipeAnswers[i].active = true;
                    pipeAnswers[i].ShowSign = true;
                }
            }
        }

        public void MarkCorrect()
        {
            for (int i = 0; i < pipeAnswers.Length; i++)
            {
                if (pipeAnswers[i].IsCorrectAnswer)
                {
                    Color color = ToboganGame.LETTER_MARK_PIPE_COLOR;

                    pipeAnswers[i].SetAppearance(game.markedTextMaterial, game.markedDrawingMaterial, color);
                }
            }

        }

        public void HidePipes()
        {
            for (int i = 0; i < pipeAnswers.Length; i++)
            {
                pipeAnswers[i].active = false;
            }
        }

        public PipeAnswer GetCurrentPipeAnswer()
        {
            return currentPipeAnswer;
        }

        public PipeAnswer GetCorrectPipeAnswer()
        {
            for (int i = 0; i < pipeAnswers.Length; i++)
            {
                if (pipeAnswers[i].IsCorrectAnswer)
                {
                    return pipeAnswers[i];
                }
            }

            return null;
        }

        void OnPointerDown()
        {
            // If not hiding, play letter sound when touching tube
            for (int i = 0; i < pipeAnswers.Length; ++i)
            {
                var pipe = pipeAnswers[i];
                if (pipe.active && pipe.ShowSign && pipe.Data != null)
                {
                    var pointerPosition = game.Context.GetInputManager().LastPointerPosition;
                    var screenRay = game.tubesCamera.ScreenPointToRay(pointerPosition);

                    RaycastHit hitInfo;
                    if (pipe.signCollider.Raycast(screenRay, out hitInfo, game.tubesCamera.farClipPlane))
                    {
                        game.Context.GetAudioManager().PlayVocabularyData(pipe.Data, true, soundType: ToboganConfiguration.Instance.GetVocabularySoundType());
                    }
                }
            }
        }

        void Update()
        {
            if (hideSignsTimer > 0)
            {
                hideSignsTimer -= Time.deltaTime;

                if (hideSignsTimer <= 0)
                {
                    int len = toHide.Count;

                    for (int i = 0; i < len; i++)
                    {
                        toHide[i].ShowSign = false;
                    }
                }
            }

            UpdateCurrentPipeAnswer();
        }

        void UpdateCurrentPipeAnswer()
        {
            var currentLivingLetter = game.questionsManager.GetQuestionLivingLetter();
            if (currentLivingLetter == null)
            {
                currentPipeAnswer = null;
                return;
            }

            PipeAnswer newPipeAnswer = null;

            Vector3? letterPosition = currentLivingLetter.TargetContentDragPosition;

            if (!letterPosition.HasValue)
            {
                currentPipeAnswer = null;
                currentLivingLetter.NearTube = null;
                return;
            }

            float pipeDistanceX = float.PositiveInfinity;
            float pipeDistanceY = float.PositiveInfinity;

            for (int i = 0; i < pipeAnswers.Length; i++)
            {
                if (pipeAnswers[i].active)
                {
                    Vector3 pipePosition = pipeAnswers[i].tutorialPoint.position;
                    float newPipeDistance = Mathf.Abs(pipePosition.x - letterPosition.Value.x);

                    if (newPipeDistance < pipeDistanceX)
                    {
                        newPipeAnswer = pipeAnswers[i];
                        pipeDistanceX = newPipeDistance;
                        pipeDistanceY = Mathf.Abs(pipePosition.y - letterPosition.Value.y);
                    }
                }
            }

            if (pipeDistanceX > maxLetterDistanceX || pipeDistanceY > maxLetterDistanceY)
            {
                currentLivingLetter.NearTube = null;

                if (currentPipeAnswer != null)
                {
                    currentPipeAnswer.StopSelectedAnimation();
                    currentPipeAnswer = null;
                }
            }
            else
            {
                if (currentPipeAnswer != null && currentPipeAnswer != newPipeAnswer)
                {
                    currentPipeAnswer.StopSelectedAnimation();
                    currentPipeAnswer = null;
                }

                currentLivingLetter.NearTube = currentPipeAnswer;

                if (currentPipeAnswer == null)
                {
                    if (newPipeAnswer != null)
                        newPipeAnswer.PlaySelectedAnimation();

                    currentPipeAnswer = newPipeAnswer;
                }
            }
        }
    }
}
