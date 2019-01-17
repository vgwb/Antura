using Antura.LivingLetters;
using Antura.Tutorial;
using DG.DeExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    public class ReadingGameAnswerState : FSM.IState
    {
        public bool TutorialMode = false;
        ReadingGameGame game;

        ILivingLetterData correct;

        public float ReadTime;
        public float MaxTime;
        CircleButton correctButton;
        CircleButtonBox box;

        float rightButtonTimer = 0;

        public ReadingGameAnswerState(ReadingGameGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            game.antura.AllowSitting = false;
            game.isTimesUp = false;

            game.circleBox.SetActive(true);
            box = game.circleBox.GetComponent<CircleButtonBox>();
            box.Clear();
            box.ImageMode = true;

            correct = game.CurrentQuestion.GetCorrectAnswers().First();
            var wrongs = game.CurrentQuestion.GetWrongAnswers();

            var choices = new List<ILivingLetterData>();

            int maxWrongs = Mathf.RoundToInt(2 + 4 * ReadingGameConfiguration.Instance.Difficulty);

            choices.AddRange(wrongs);

            for (int i = maxWrongs, count = choices.Count; i < count; ++i) {
                choices.RemoveAt(choices.Count - 1);
            }

            choices.Add(correct);
            choices.Shuffle();

            float delay = 0;
            foreach (var c in choices) {
                var button = box.AddButton(c, OnAnswered, delay);
                delay += 0.2f;

                if (c == correct) {
                    correctButton = button;
                }
            }

            box.Active = true;

            if (!TutorialMode) {
                game.radialWidget.Show();
                game.radialWidget.Reset(ReadTime / MaxTime);
                game.radialWidget.inFront = true;
                game.radialWidget.pulsing = true;
            }
        }

        public void ExitState()
        {
            game.circleBox.GetComponent<CircleButtonBox>().Clear();
            game.radialWidget.inFront = false;

            if (TutorialMode) {
                TutorialUI.Clear(true);
            }
        }

        public void Update(float delta)
        {
            rightButtonTimer -= delta;

            if (correctButton != null && TutorialMode && rightButtonTimer < 0 && box.IsReady()) {
                rightButtonTimer = 3;
                var uicamera = game.uiCamera;
                TutorialUI.SetCamera(uicamera);
                TutorialUI.Click(correctButton.transform.position);
            }
        }

        public void UpdatePhysics(float delta)
        {

        }

        void OnAnswered(CircleButton button)
        {
            bool result = button.Answer == correct;

            game.Context.GetAudioManager().PlaySound(result ? Sfx.OK : Sfx.KO);

            if (!TutorialMode) {
                game.Context.GetLogManager().OnAnswered(correct, result);
            }
            if (button.Answer == correct) {
                // Assign score
                if (!TutorialMode) {
                    game.AddScore((int)(ReadTime) + 1);
                    game.radialWidget.percentage = 0;
                    game.radialWidget.pulsing = false;
                }

                game.StartCoroutine(DoEndAnimation(true, correctButton));

                game.antura.Mood = ReadingGameAntura.AnturaMood.HAPPY;
            } else {
                if (TutorialMode) {
                    //button.SetColor(UnityEngine.Color.red);
                    if (box.IsReady())
                        TutorialUI.MarkNo(button.transform.position);
                } else {
                    if (!TutorialMode) {
                        game.radialWidget.PoofAndHide();
                    }
                    game.StartCoroutine(DoEndAnimation(false, correctButton));

                    game.antura.animator.DoShout(() => { ReadingGameConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DogBarking); });
                }
            }
        }

        IEnumerator DoEndAnimation(bool correct, CircleButton correctButton)
        {
            box.Active = false;
            if (correct) {
                correctButton.SetColor(UnityEngine.Color.green);
            } else {
                correctButton.SetColor(UnityEngine.Color.red);
            }
            yield return new UnityEngine.WaitForSeconds(1.0f);

            if (!correct && game.RemoveLife()) {
                yield break;
            }

            game.circleBox.GetComponent<CircleButtonBox>().Clear(() => {
                game.SetCurrentState(game.QuestionState);
            }, 0.5f);
        }
    }
}