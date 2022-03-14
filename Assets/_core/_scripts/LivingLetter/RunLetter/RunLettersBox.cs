using Antura.LivingLetters;
using UnityEngine;
using System.Collections.Generic;

namespace Antura.Minigames
{
    public class RunLettersBox : MonoBehaviour
    {
        public Transform[] leftOutPositions;
        public Transform[] rightOutPositions;

        private List<RunLetter> runLetters = new List<RunLetter>();

        private GameObject letterObjectPrefab;
        private GameObject shadowPrefab;

        public void Initialize(GameObject letterObjectPrefab, GameObject shadowPrefab)
        {
            this.letterObjectPrefab = letterObjectPrefab;
            this.shadowPrefab = shadowPrefab;
        }

        public RunLetter AddRunLetter(ILivingLetterData letterData, Vector3 scale = default)
        {
            if (scale == default)
                scale = Vector3.one;
            Vector3 leftOutPosition = leftOutPositions[runLetters.Count].position;
            Vector3 rightOutPosition = rightOutPositions[runLetters.Count].position;

            RunLetter runLetter = new RunLetter(letterObjectPrefab, shadowPrefab, letterData, transform, leftOutPosition, rightOutPosition, scale);
            runLetter.Run();

            runLetters.Add(runLetter);
            return runLetter;
        }

        public void RemoveAllRunLetters()
        {
            for (int i = 0; i < runLetters.Count; i++)
            {
                runLetters[i].DestroyRunLetter();
            }

            runLetters.Clear();
        }

        public void RunAll()
        {
            foreach (var runLetter in runLetters)
            {
                runLetter.Stop();
                runLetter.Run();
            }
        }

        public void AnimateAll(LLAnimationStates anim)
        {
            foreach (var runLetter in runLetters)
            {
                runLetter.Stop();
                runLetter.PlayAnimation(anim);
            }
        }
    }
}
