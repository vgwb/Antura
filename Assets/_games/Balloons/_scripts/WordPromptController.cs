using UnityEngine;
using System.Collections.Generic;
using Antura.LivingLetters;

namespace Antura.Minigames.Balloons
{
    public class WordPromptController : MonoBehaviour
    {
        public LetterPromptController[] letterPrompts;

        [HideInInspector]
        public List<LetterPromptController> IdleLetterPrompts
        {
            get
            {
                return new List<LetterPromptController>(letterPrompts).FindAll(
                prompt => prompt.isActiveAndEnabled && prompt.State == LetterPromptController.PromptState.IDLE);
            }
        }
        [HideInInspector]
        public int activePromptsCount;


        public void DisplayWord(List<LL_LetterData> wordLetters)
        {
            for (int i = 0; i < wordLetters.Count; i++)
            {
                letterPrompts[i].gameObject.SetActive(true);
                letterPrompts[i].Init(wordLetters[i]);
            }
            activePromptsCount = wordLetters.Count;
        }

        public void Reset()
        {
            foreach (var prompt in letterPrompts)
            {
                prompt.State = LetterPromptController.PromptState.IDLE;
                prompt.gameObject.SetActive(false);
            }
            activePromptsCount = 0;
        }
    }
}
