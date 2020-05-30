using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using Antura.Audio;
using Antura.Core;
using Antura.Language;
using Antura.LivingLetters;
using Antura.UI;

namespace Antura.Minigames.FastCrowd
{

    public class WordComposer : MonoBehaviour
    {
        public Transform innerTransform;
        WordFlexibleContainer WordLabel;
        List<LL_LetterData> CompletedLetters = new List<LL_LetterData>();
        public bool splitMode = false;

        Tweener shake;
        void Awake()
        {
            WordLabel = GetComponent<WordFlexibleContainer>();
            UpdateWord();
        }


        const string invisiblePlaceholder = "<color=#0000>ﺟ</color>";

        void UpdateWord(bool onlyIfActive = true)
        {
            if (onlyIfActive && !isActiveAndEnabled)
                return;

            string word = string.Empty;

            for (int i = 0; i < CompletedLetters.Count; ++i)
            {
                LL_LetterData letter = CompletedLetters[i];

                if (splitMode)
                {
                    if (i == 0)
                        word = $"<size=130%>{letter.Data.GetStringForDisplay(letter.Form, true)}</size>";
                    else if (i == 1)
                        word += $"\n{letter.Data.GetStringForDisplay(letter.Form, true)}";
                    else
                        word += $" {letter.Data.GetStringForDisplay(letter.Form, true)}";
                }
                else
                {
                    if (letter.Id == " ")
                    {
                        word += invisiblePlaceholder;
                    }
                    else
                    {
                        word += letter.Data.GetStringForDisplay(letter.Form, true);
                    }
                }
            }

            if (splitMode)
            {
                // Hack to fix space
                for (int i = CompletedLetters.Count; i < 4; ++i)
                {
                    if (i == 0)
                        word = $"<size=130%>{invisiblePlaceholder}</size>";
                    else if (i == 1)
                        word += $"\n{invisiblePlaceholder}";
                    else
                        word += $" {invisiblePlaceholder}";
                }
            }

            WordLabel.SetTextUnfiltered(word);
        }

        public void AddLetter(ILivingLetterData data)
        {
            if (!isActiveAndEnabled)
                return;

            StartCoroutine(AddLetter(data, 1.3f));
        }

        public void Clean()
        {
            CompletedLetters = new List<LL_LetterData>();
            UpdateWord(onlyIfActive:false);

            StopAllCoroutines();
        }

        IEnumerator AddLetter(ILivingLetterData data, float _delay)
        {
            yield return new WaitForSeconds(_delay);
            CompletedLetters.Add(data as LL_LetterData);
            AudioManager.I.PlaySound(Sfx.Hit);
            shake = innerTransform.DOShakeScale(1.5f, 0.5f);
            UpdateWord();
        }

        private void OnDisable()
        {
            if (shake != null)
            {
                shake.Complete();
                shake = null;
                innerTransform.localScale = Vector3.one;
            }
        }

        private void DropContainer_OnObjectiveBlockCompleted()
        {
            Clean();
        }
    }
}