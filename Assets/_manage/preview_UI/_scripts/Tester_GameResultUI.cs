using System.Collections.Generic;
using Antura.Database;
using Antura.Rewards;
using UnityEngine;

namespace Antura.Test
{
    /// <summary>
    /// Helper class to test the GameResultUI
    /// <seealso cref="GameResultUI"/>
    /// </summary>
    public class Tester_GameResultUI : MonoBehaviour
    {
        #region EndgameResult

        public void EndgameResult_Show(int _numStars)
        {
            GameResultUI.HideEndsessionResult();

            GameResultUI.ShowEndgameResult(_numStars);
        }

        #endregion

        #region EndsessionResult

        public void EndsessionResult_Show(int _totMinigamesStars)
        {
            GameResultUI.HideEndgameResult();

            MiniGameData d0 = new MiniGameData() { Main = MiniGameCode.Maze_lettername.ToString(), Variation = "letters" };
            MiniGameData d1 = new MiniGameData() { Main = MiniGameCode.DancingDots_lettername.ToString(), Variation = "alphabet" };
            MiniGameData d2 = new MiniGameData() { Main = MiniGameCode.MakeFriends_letterinword.ToString(), Variation = "counting" };
            List<EndsessionResultData> res = new List<EndsessionResultData>() {
                new EndsessionResultData(2, d0),
                new EndsessionResultData(0, d1),
                new EndsessionResultData(3, d2),
            };
            for (int i = 0; i < 3; ++i) {
                int num = Mathf.Min(3, _totMinigamesStars);
                _totMinigamesStars -= num;
                res[i].Stars = num;
            }
            int rndUnlocked = UnityEngine.Random.Range(0, 3);
            Debug.Log("ShowEndsessionResult > " + rndUnlocked);
            GameResultUI.ShowEndsessionResult(res, rndUnlocked);
        }

        #endregion
    }
}