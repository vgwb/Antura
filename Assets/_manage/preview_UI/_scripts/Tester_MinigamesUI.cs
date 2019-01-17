using System.Collections;
using Antura.Minigames;
using Antura.UI;
using UnityEngine;

namespace Antura.Test
{
    /// <summary>
    /// Helper class to test the MinigamesUI
    /// <seealso cref="MinigamesUI"/>
    /// </summary>
    public class Tester_MinigamesUI : MonoBehaviour
    {
        public RadialGadget RadialGadget;
        public Tester_MinigamesUIPanel[] Panels;

        #region Unity

        void Awake()
        {
            foreach (Tester_MinigamesUIPanel panel in Panels) panel.gameObject.SetActive(false);
        }

        #endregion

        #region Init

        public void Init(int _id)
        {
            switch (_id) {
            case 0: MinigamesUI.Init(MinigamesUIElement.Starbar);
                break;
            case 1: MinigamesUI.Init(MinigamesUIElement.Starbar | MinigamesUIElement.Timer);
                break;
            case 2: MinigamesUI.Init(MinigamesUIElement.Starbar | MinigamesUIElement.Lives);
                break;
            case 3: MinigamesUI.Init(MinigamesUIElement.Starbar | MinigamesUIElement.Timer | MinigamesUIElement.Lives);
                break;
            }

            foreach (Tester_MinigamesUIPanel panel in Panels) {
                switch (panel.PanelType) {
                case Tester_MinigamesUIPanel.UIPanelType.Timer:
                    panel.gameObject.SetActive(_id == 1 || _id == 3);
                    break;
                case Tester_MinigamesUIPanel.UIPanelType.Lives:
                    panel.gameObject.SetActive(_id == 2 || _id == 3);
                    break;
                case Tester_MinigamesUIPanel.UIPanelType.Starbar:
                    panel.gameObject.SetActive(true);
                    break;
                default:
                    continue;
                }
                if (panel.gameObject.activeSelf) panel.Refresh();
            }
        }

        #endregion

        #region Lives

        public void Lives_Setup(int _totLives)
        { MinigamesUI.Lives.Setup(_totLives); }

        public void Lives_SetCurrLives(int _to)
        { MinigamesUI.Lives.SetCurrLives(_to, true); }

        public void Lives_ResetToMax()
        { MinigamesUI.Lives.ResetToMax(); }
        public void Lives_GainALife(bool _canExceedMax)
        { MinigamesUI.Lives.GainALife(_canExceedMax); }

        public void Lives_LoseALife()
        { MinigamesUI.Lives.LoseALife(); }

        #endregion

        #region Starbar

        public void Starbar_Goto(float _percentage)
        { MinigamesUI.Starbar.Goto(_percentage); }

        public void Starbar_GotoStar(int _starIndex)
        { MinigamesUI.Starbar.GotoStar(_starIndex); }

        #endregion

        #region Timer

        public void Timer_Setup(float _timerDuration)
        {
            this.StopAllCoroutines();
            MinigamesUI.Timer.Setup(_timerDuration);
        }

        public void Timer_Play()
        {
            this.StopAllCoroutines();
            MinigamesUI.Timer.Play();
        }

        public void Timer_PlayWithGoto()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(CO_Timer_PlayWithGoto());
        }

        public void Timer_Pause()
        {
            this.StopAllCoroutines();
            MinigamesUI.Timer.Pause();
        }

        public void Timer_ReStart()
        {
            this.StopAllCoroutines();
            MinigamesUI.Timer.Restart();
        }

        public void Timer_Rewind()
        {
            this.StopAllCoroutines();
            MinigamesUI.Timer.Rewind();
        }

        public void Timer_Complete()
        {
            this.StopAllCoroutines();
            MinigamesUI.Timer.Complete();
        }

        public void Timer_Goto(float _time)
        {
            this.StopAllCoroutines();
            MinigamesUI.Timer.Goto(_time, true);
        }

        public void Timer_GotoAdvance(float _elapsed)
        {
            this.StopAllCoroutines();
            MinigamesUI.Timer.Goto(MinigamesUI.Timer.Elapsed + _elapsed, false);
        }

        public void Timer_GotoPercentage(float _percentage)
        {
            this.StopAllCoroutines();
            MinigamesUI.Timer.GotoPercentage(_percentage, true);
        }

        #endregion

        #region RadialGadget

        public void TimerGadget_SetRandomPercentage()
        {
            RadialGadget.SetPercentage(UnityEngine.Random.value);
        }

        #endregion

        #region Methods

        IEnumerator CO_Timer_PlayWithGoto()
        {
            float time = Time.realtimeSinceStartup;
            while (true) {
                yield return null;
                float elapsed = Time.realtimeSinceStartup - time;
                time = Time.realtimeSinceStartup;
                MinigamesUI.Timer.Goto(MinigamesUI.Timer.Elapsed + elapsed);
            }
        }

        #endregion
    }
}