using UnityEngine;
using System.Collections.Generic;

namespace Antura.Book
{
    /// <summary>
    /// Shows a graph detailing the journey of the player.
    /// </summary>
    public class GraphJourney : MonoBehaviour
    {
        public BookGraph Graph;

        public void Show(List<Database.PlaySessionInfo> allPsInfo, List<Database.PlaySessionInfo> unlockedPlaySessionInfos)
        {
            float[] journeyValues = allPsInfo.ConvertAll(x => x.score).ToArray();
            //string[] journeyLabels = allPsInfo.ConvertAll(x => x.data.Id).ToArray();
            Graph.SetValues(unlockedPlaySessionInfos.Count, 1f, journeyValues);
        }
    }
}
