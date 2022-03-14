using Antura.Keeper;
using UnityEngine;

namespace Antura.Minigames.SickLetters
{
    public class PlayGameState : FSM.IState
    {
        SickLettersGame game;
        Vector3 correctDotPos;

        float timer = 2;
        int alarmIsTriggered;

        //IAudioSource clockSound;

        public PlayGameState(SickLettersGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            timer = game.gameDuration;

            SickLettersConfiguration.Instance.Context.GetAudioManager().MusicEnabled = true;
            if (game.roundsCount == 0)
            {
                SickLettersConfiguration.Instance.Context.GetAudioManager().PlayMusic(Music.Relax);
            }
            else
            {
                SickLettersConfiguration.Instance.Context.GetAudioManager().PlayMusic(Music.Theme10);
                game.Context.GetOverlayWidget().Initialize(true, true, false);
                game.Context.GetOverlayWidget().SetClockDuration(game.gameDuration);
            }

            game.LLPrefab.jumpIn();

        }

        public void ExitState()
        {
            game.disableInput = true;
        }

        public void Update(float delta)
        {
            game.ProcessDifficulty(game.Difficulty);

            if (game.roundsCount > 0)
            {
                timer -= delta;
                game.Context.GetOverlayWidget().SetClockTime(timer);
            }
            if (timer < 0)
            {
                /*
                if (clockSound != null)
                {
                    clockSound.Stop();
                    clockSound = null;
                }
                */

                game.Context.GetOverlayWidget().OnClockCompleted();
                game.SetCurrentState(game.ResultState);
                KeeperManager.I.PlayDialogue(Database.LocalizationDataId.Keeper_TimeUp);
            }

            if (alarmIsTriggered == 0 && timer < 2)
            {
                alarmIsTriggered = 1;
                KeeperManager.I.PlayDialogue("Keeper_Time_" + Random.Range(1, 4));
            }
            if (alarmIsTriggered == 1 && timer < 4)
            {
                alarmIsTriggered = 2;
                //clockSound = SickLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DangerClockLong);
            }

            /*if (Input.GetKeyDown(KeyCode.A)) {
                game.LLPrefab.jumpOut();
            }*/

            if (game.LLPrefab.correctDot.mesh.vertices.Length > 0)
                correctDotPos = game.LLPrefab.correctDot.transform.TransformPoint(Vector3.Lerp(game.LLPrefab.correctDot.mesh.vertices[0], game.LLPrefab.correctDot.mesh.vertices[2], 0.5f));

            if (game.LLPrefab.correctDotCollider.transform.childCount == 0)
                game.LLPrefab.correctDotCollider.transform.position = correctDotPos;

            if (game.LLPrefab.letterView.LabelRender.color.a > 0)
                game.LLPrefab.letterView.LabelRender.color = new Color32(0, 0, 0, 0);

            Debug.DrawRay(correctDotPos, -Vector3.forward * 10, Color.red);
            Debug.DrawRay(correctDotPos, -Vector3.right * 10, Color.yellow);

        }

        public void UpdatePhysics(float delta)
        {

        }
    }
}
