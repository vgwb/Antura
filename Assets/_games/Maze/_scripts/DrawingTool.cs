using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class DrawingTool : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.IndexOf("fruit_") == 0)
            {
                MazeGame.instance.OnFruitGotDrawnOver(other.gameObject.GetComponent<MazeArrow>());
            }
        }
    }
}
