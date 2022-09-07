using Antura.UI;
using UnityEngine;
using TMPro;

namespace Antura.Minigames.Maze
{
    public class MazeTimer : MonoBehaviour
    {
        public static bool ENABLE_TIMER = false;

        [HideInInspector]
        public float time;
        public TextMeshProUGUI timerText;

        private bool isRunning;
        private bool playedSfx;
        private float timeRemaining;

        public void initTimer()
        {
            if (!ENABLE_TIMER) return;

            if (!MazeGame.instance.isTutorialMode)
            {
                MinigamesUI.Timer.Setup(MazeGame.instance.gameTime);
            }
        }

        public void Update()
        {
            if (!ENABLE_TIMER) return;
            if (!MazeGame.instance.isTutorialMode &&
                MinigamesUI.Timer != null &&
                MinigamesUI.Timer.Duration == MinigamesUI.Timer.Elapsed)
            {
                StopTimer();
                MazeGame.instance.onTimeUp();
            }
        }

        public void StartTimer()
        {
            if (!ENABLE_TIMER) return;
            if (!MazeGame.instance.isTutorialMode)
            { MinigamesUI.Timer.Play(); }

        }

        public void StopTimer()
        {
            if (!ENABLE_TIMER) return;
            if (!MazeGame.instance.isTutorialMode)
            { MinigamesUI.Timer.Pause(); }
        }

    }
}
