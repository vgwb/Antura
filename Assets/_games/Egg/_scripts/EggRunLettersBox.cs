using Antura.LivingLetters;
using UnityEngine;
using System.Collections.Generic;

namespace Antura.Minigames.Egg
{
    public class EggRunLettersBox : MonoBehaviour
    {
        public Transform[] leftOutPositions;
        public Transform[] rightOutPositions;

        private List<EggRunLetter> runLetters = new List<EggRunLetter>();

        private GameObject letterObjectPrefab;
        private GameObject shadowPrefab;

        public void Initialize(GameObject letterObjectPrefab, GameObject shadowPrefab)
        {
            this.letterObjectPrefab = letterObjectPrefab;
            this.shadowPrefab = shadowPrefab;
        }

        public void AddRunLetter(ILivingLetterData letterData)
        {
            Vector3 leftOutPosition = leftOutPositions[runLetters.Count].position;
            Vector3 rightOutPosition = rightOutPositions[runLetters.Count].position;

            EggRunLetter runLetter = new EggRunLetter(letterObjectPrefab, shadowPrefab, letterData, transform, leftOutPosition, rightOutPosition);

            runLetter.Run();

            runLetters.Add(runLetter);
        }

        public void RemoveAllRunLetters()
        {
            for (int i = 0; i < runLetters.Count; i++) {
                runLetters[i].DestroyRunLetter();
            }

            runLetters.Clear();
        }
    }
}