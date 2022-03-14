using Antura.Core;
using UnityEngine;

namespace Antura.Book
{
    /// <summary>
    /// Shows a graph with the history of moods of the player.
    /// </summary>
    public class GraphMood : MonoBehaviour
    {
        public BookGraph Graph;

        public void OnEnable()
        {
            int nMoods = 10;
            var latestMoods = AppManager.I.Teacher.GetLastMoodData(nMoods);
            float[] moodValues = latestMoods.ConvertAll(x => x.MoodValue).ToArray();
            Graph.SetValues(nMoods, AppConfig.MaxMoodValue, moodValues);
        }
    }
}
