using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class MazeLL : MonoBehaviour
    {
        public LivingLetterController letter;

        void Start()
        {
            letter = GetComponent<LivingLetterController>();
            letter.SetState(LLAnimationStates.LL_rocketing);
        }

        void Update()
        {

        }
    }
}
