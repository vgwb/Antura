using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class TrackBounds : MonoBehaviour
    {
        public static TrackBounds instance;

        private MazeLetter mazeLetter;

        private void Awake()
        {
            instance = this;
        }
    }
}
