using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class MazeLetter : MonoBehaviour
    {
        public MazeCharacter mazeCharacter;
        public bool isDrawing;
        public float idleSeconds = 0;
        public float anturaSeconds;
        private bool didAnturaBark = false;

        void Start()
        {
            anturaSeconds = 0;
            isDrawing = false;
            mazeCharacter.toggleVisibility(false);
            //character.gameObject.SetActive (false);
        }

        void Update()
        {
            if (MazeGame.instance.gameEnded)
            {
                return;
            }

            if (mazeCharacter.characterIsMoving)
            {
                anturaSeconds = 0;
                return;
            }

            float anturaAppearTime = Mathf.Lerp(10f, 6f, MazeGame.instance.Difficulty);

            //should we replay tutorial?
            if (!isDrawing)
            {
                if (!MazeGame.instance.currentCharacter || MazeGame.instance.currentCharacter.isFleeing || MazeGame.instance.currentCharacter.isAppearing)
                    return;

                if (!MazeGame.instance.isTutorialMode && MazeGame.instance.currentTutorial && MazeGame.instance.currentTutorial.isShownOnce && MazeGame.instance.isShowingAntura == false)
                {
                    anturaSeconds += Time.deltaTime;

                    if (anturaSeconds >= anturaAppearTime - 2f && !didAnturaBark)
                    {
                        MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DogBarking);
                        didAnturaBark = true;
                    }

                    if (anturaSeconds >= anturaAppearTime)
                    {
                        anturaSeconds = 0;
                        MazeGame.instance.onIdleTime();
                    }

                }

                if (MazeGame.instance.currentTutorial != null &&
                    MazeGame.instance.currentTutorial.isStopped == false &&
                    MazeGame.instance.currentTutorial.isShownOnce == true)
                {

                    idleSeconds += Time.deltaTime;

                    if (idleSeconds >= 5)
                    {
                        idleSeconds = 0;
                        MazeGame.instance.currentTutorial.showCurrentTutorial();
                    }
                }
            }

            if (isDrawing)
            {
                anturaSeconds = 0;
                mazeCharacter.calculateMovementAndRotation();
            }
        }

        public void OnPointerDown()
        {
            if (mazeCharacter.characterIsMoving || !mazeCharacter.canMouseBeDown() || mazeCharacter.finishedRound)
            {
                return;
            }

            Debug.Log("started Drawing!");

            MazeGame.instance.drawingTool.SetActive(true);

            idleSeconds = 0;
            MazeGame.instance.currentTutorial.stopCurrentTutorial();
            anturaSeconds = 0;

            mazeCharacter.ChangeStartingFXHighlight();

            // Inform that we are inside the collision:
            isDrawing = true;
        }

        public void OnPointerUp()
        {
            if (CanLaunchRocket())
            {
                MazeGame.instance.drawingTool.SetActive(false);
                LaunchRocket();
            }
        }

        public void OnPointerOverTrackBounds(Vector3 pointOfImpact)
        {
            if (CanLaunchRocket())
            {
                mazeCharacter.loseState = MazeCharacter.LoseState.OutOfBounds;

                MazeGame.instance.ColorCurrentLinesAsIncorrect();

                Tutorial.TutorialUI.MarkNo(pointOfImpact);
                MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.KO);

                if (!MazeGame.instance.isTutorialMode)
                {
                    MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Lose);
                }

                LaunchRocket();
            }
        }

        private bool CanLaunchRocket()
        {
            return MazeGame.instance.tutorialForLetterisComplete() && isDrawing;
        }

        private void LaunchRocket()
        {
            isDrawing = false;
            mazeCharacter.toggleVisibility(true);
            mazeCharacter.initMovement();



            MazeGame.instance.timer.StopTimer();
        }

        public void NotifyFruitGotMouseOver(MazeArrow fruit)
        {
            if (isDrawing && fruit.gameObject != mazeCharacter._fruits[0])
            {
                fruit.HighlightAsReached();
            }
        }

        public void NotifyDrawnLetterWrongly()
        {
            if (CanLaunchRocket())
            {
                LaunchRocket();
            }
        }
    }
}
