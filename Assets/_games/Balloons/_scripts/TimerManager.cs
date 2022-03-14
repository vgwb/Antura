using Antura.UI;
using UnityEngine;

namespace Antura.Minigames.Balloons
{
    public class TimerManager : MonoBehaviour
    {
        [HideInInspector]
        public float time;

        private bool isRunning;
        private float timeRemaining;

        void Update()
        {
            if (isRunning)
            {
                if (timeRemaining > 0f)
                {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime();
                }

                if (timeRemaining <= 0f)
                {
                    StopTimer(true);
                    BalloonsGame.instance.OnTimeUp();
                }
            }
        }

        public void InitTimer()
        {
            time = BalloonsGame.instance.RoundTime;
            if (MinigamesUI.Timer != null)
            {
                MinigamesUI.Timer.Setup(time);
            }
        }

        public void StartTimer()
        {
            isRunning = true;
            MinigamesUI.Timer.Play();
            MinigamesUI.Timer.gameObject.SetActive(true);
        }

        public void StopTimer(bool forceCompletion = false)
        {
            isRunning = false;
            if (MinigamesUI.Timer != null)
            {
                if (forceCompletion)
                {
                    MinigamesUI.Timer.Complete();
                }
                else
                {
                    MinigamesUI.Timer.Pause();
                }
            }
            //AudioManager.I.StopSfx(Sfx.DangerClockLong);

            // Hide the timer
            if (MinigamesUI.Timer != null)
                MinigamesUI.Timer.gameObject.SetActive(false);
        }

        public void ResetTimer()
        {
            if (MinigamesUI.Timer == null)
            {
                return;
            }
            if (!MinigamesUI.Timer.IsSetup)
            {
                InitTimer();
            }
            StopTimer();
            timeRemaining = time;
            MinigamesUI.Timer.Rewind();
        }

        public void DisplayTime()
        {
            //textvar text = Mathf.Floor(timeRemaining).ToString();
            //timerText.text = text;
        }
    }
}
