using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class MazeDot : MonoBehaviour
    {

        public MazeCharacter character;
        public bool isClicked = false;

        void Start()
        {
        }

        void Update()
        {
        }

        void OnMouseDown()
        {
            isClicked = true;
            MazeGame.instance.timer.StopTimer();
            MazeGame.instance.currentTutorial.stopCurrentTutorial();
            character.setClickedDot();
        }
    }
}
