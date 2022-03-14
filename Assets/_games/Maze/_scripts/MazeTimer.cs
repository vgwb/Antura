using Antura.UI;
using UnityEngine;
using TMPro;

namespace Antura.Minigames.Maze
{
    public class MazeTimer : MonoBehaviour
    {

        [HideInInspector]
        public float time;
        public TextMeshProUGUI timerText;

        private bool isRunning;
        private bool playedSfx;
        private float timeRemaining;

        public void initTimer()
        {
            /*time = MazeGameManager.Instance.gameTime;
			timeRemaining = time;
			DisplayTime ();*/

            // this.StopAllCoroutines();
            if (!MazeGame.instance.isTutorialMode)
            {
                MinigamesUI.Timer.Setup(MazeGame.instance.gameTime);
            }
        }

        public void Update()
        {
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
            //isRunning = true;
            //   this.StopAllCoroutines();
            if (!MazeGame.instance.isTutorialMode)
            { MinigamesUI.Timer.Play(); }

        }

        public void StopTimer()
        {
            // this.StopAllCoroutines();
            if (!MazeGame.instance.isTutorialMode)
            { MinigamesUI.Timer.Pause(); }
        }

        public void DisplayTime()
        {
            /*var text = Mathf.Floor(timeRemaining).ToString();
			timerText.text = text;*/
        }
    }
}
