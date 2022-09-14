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
            mazeCharacter.ToggleParticlesVisibility(false);
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
                //mazeCharacter.Draw();
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

            // Auto-end if surpassing the last fruit
            if (isDrawing && mazeCharacter.HasCompletedPath)
            {
                var dist = Vector3.Distance(MazeGame.instance.drawingTool.transform.position, mazeCharacter._fruits[mazeCharacter._fruits.Count - 1].transform.position);
                //Debug.LogWarning(dist);
                if (dist > 1f)
                {
                    OnPointerUp();
                }
            }
        }

        public void OnPointerDown()
        {
            bool shouldCheckIfCanStartDrawing = !hasStartedDrawing && !isDrawing;
            if (shouldCheckIfCanStartDrawing && !mazeCharacter.canStartDrawing()) return;
            if (mazeCharacter.characterIsMoving) return;
            if (mazeCharacter.finishedRound) return;

            //Debug.Log("started Drawing!");

            MazeGame.instance.drawingTool.SetActive(true);

            // Init drawing tool position
            float distance = (0.1f) - Camera.main.transform.position.y;
            var targetPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -distance);
            targetPos = Camera.main.ScreenToWorldPoint(targetPos);
            var newDrawingToolPosition = targetPos + new Vector3(0, 0.5f, 0);
            MazeGame.instance.drawingTool.transform.position = newDrawingToolPosition;

            idleSeconds = 0;
            MazeGame.instance.currentTutorial.stopCurrentTutorial();
            anturaSeconds = 0;

            mazeCharacter.HighlightStartFruit();


            // Inform that we are inside the collision:
            isDrawing = true;

            if (hasStartedDrawing)
            {
                // Create a new line
                MazeGame.instance.addLine(MazeGame.instance.drawingColor);
            }

            hasStartedDrawing = true;
        }

        private bool hasStartedDrawing = false;
        public void OnPointerUp()
        {
            if (!isDrawing) return;

            if (!MazeCharacter.LOSE_ON_SINGLE_PATH)
            {
                if (!mazeCharacter.HasCompletedPath)
                {
                    if (CanLaunchRocket())
                    {
                        MazeGame.instance.drawingTool.SetActive(false);
                        LaunchRocket(finished:false, wrong:false);
                    }

                    isDrawing = false;

                    return;
                }
            }

            if (CanLaunchRocket())
            {
                MazeGame.instance.drawingTool.SetActive(false);
                LaunchRocket(wrong:false);
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

                LaunchRocket(finished:false, wrong:true);
            }
        }

        private bool CanLaunchRocket()
        {
            return MazeGame.instance.tutorialForLetterisComplete() && isDrawing;
        }

        private void LaunchRocket(bool finished = true, bool wrong = false)
        {
            if (finished) hasStartedDrawing = false;
            isDrawing = false;
            mazeCharacter.ToggleParticlesVisibility(true);

            if (wrong)
            {
                mazeCharacter.PerformMovement(true);
            }
            else
            {
                // Recreate character waypoints from passed fruits
                mazeCharacter.MoveOnCurrentFruits();
            }
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
                mazeCharacter.loseState = MazeCharacter.LoseState.OutOfBounds;
                MazeGame.instance.ColorCurrentLinesAsIncorrect();
                MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.KO);
                if (!MazeGame.instance.isTutorialMode)
                {
                    MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Lose);
                }

                LaunchRocket(finished:false, wrong:true);
            }
        }
    }
}
