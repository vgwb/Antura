using UnityEngine;

namespace Antura.Minigames.Egg
{
    public class EggBox : MonoBehaviour
    {
        public Transform[] eggPositions;

        public Transform[] lettersMaxPositions;

        public Vector3[] GetEggLocalPositions()
        {
            Vector3[] eggLocalPositions = new Vector3[eggPositions.Length];

            for (int i = 0; i < eggPositions.Length; i++)
            {
                eggLocalPositions[i] = eggPositions[i].localPosition;
            }

            return eggLocalPositions;
        }

        public Vector3[] GetLocalLettersMaxPositions()
        {
            Vector3[] localLettersMaxPositions = new Vector3[lettersMaxPositions.Length];

            for (int i = 0; i < lettersMaxPositions.Length; i++)
            {
                localLettersMaxPositions[i] = lettersMaxPositions[i].localPosition;
            }

            return localLettersMaxPositions;
        }
    }
}
