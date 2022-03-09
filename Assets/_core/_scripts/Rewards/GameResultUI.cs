using Antura.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Rewards
{
    /// <summary>
    /// Controls the UI that shows the end results of a minigame or play session.
    /// - Shown when a minigame ends
    /// - Shown when a play session ends
    /// </summary>
    public class GameResultUI : MonoBehaviour
    {
        public static GameResultUI I;

        public EndgameResultPanel EndgameResultPanel;
        public EndsessionResultPanel EndsessionResultPanel;
        public BonesCounter BonesCounter;
        public Vector2 BonesCounterEndgamePos, BonesCounterEndsessionPos;

        const string GameResultUIResourcesPath = "Prefabs/UI/GameResultUI";

        #region Unity + Init

        static void Init()
        {
            if (I != null)
            { return; }

            I = Instantiate(Resources.Load<GameResultUI>(GameResultUIResourcesPath));
            I.BonesCounter.Hide();
        }

        void Awake()
        {
            I = this;
        }

        void OnDestroy()
        {
            if (I == this)
            { I = null; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Never use this directly! Use the <code>Minigames Interface</code> instead.
        /// </summary>
        public static void ShowEndgameResult(int _numStars)
        {
            Init();
            GlobalUI.ShowPauseMenu(false);

            I.BonesCounter.GetComponent<RectTransform>().anchoredPosition = I.BonesCounterEndgamePos;
            I.BonesCounter.Hide();
            I.EndgameResultPanel.Show(_numStars);
        }

        /// <summary>
        /// Returns an array with 2 gameObjects inside which to place the rewards prefab (0 is first reward, 1 is second).
        /// <para><code>IMPORTANT:</code> do not modify the returned array, just use its data</para>
        /// </summary>
        /// <param name="_sessionData">A list of data for each minigame that was part of the session</param>
        /// <param name="_alreadyUnlockedRewards">Total rewards unlocked in previous sessions (0 to 2)</param>
        /// <param name="_immediate">If TRUE shows the screen immediately with no animations. You can ignore this</param>
        /// <returns></returns>
        public static GameObject[] ShowEndsessionResult(List<EndsessionResultData> _sessionData, int _alreadyUnlockedRewards,
            bool _immediate = false)
        {
            Init();
            GlobalUI.ShowPauseMenu(false);
            //I.BonesCounter.GetComponent<RectTransform>().anchoredPosition = I.BonesCounterEndsessionPos;
            //I.BonesCounter.Hide();
            I.EndsessionResultPanel.Show(_sessionData, _alreadyUnlockedRewards, _immediate);
            return I.EndsessionResultPanel.RewardsGos;
        }

        /// <summary>
        /// Never use this directly! Use the <code>Minigames Interface</code> instead.
        /// </summary>
        public static void HideEndgameResult(bool _immediate = false)
        {
            //if (I == null) return;
            GlobalUI.ShowPauseMenu(true);
            I.BonesCounter.Hide();
            I.EndgameResultPanel.Hide(_immediate);
        }

        /// <summary>
        /// Never use this directly! Use the <code>Minigames Interface</code> instead.
        /// </summary>
        public static void HideEndsessionResult(bool _immediate = false)
        {
            if (I == null)
            { return; }
            GlobalUI.ShowPauseMenu(true);
            //I.BonesCounter.Hide();
            I.EndsessionResultPanel.Hide(_immediate);
        }

        #endregion
    }
}
