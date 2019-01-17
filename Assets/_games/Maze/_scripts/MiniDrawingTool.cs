using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class MiniDrawingTool : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "MazeLetter")
            {
                MazeGame.instance.OnDrawnLetterWrongly();
            }
        }
    }
}