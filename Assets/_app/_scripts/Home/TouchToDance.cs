using System.Collections;
using System.Collections.Generic;
using Antura.Audio;
using Antura.Core;
using UnityEngine;

namespace Antura.LivingLetters
{
    public class TouchToDance : MonoBehaviour
    {
        public void OnMouseDown()
        {
            var view = GetComponent<LivingLetterController>();
            view.ToggleDance();
            var letter = AppManager.I.Teacher.GetRandomTestLetterLL(useMaxJourneyData: true);
            view.Init(letter);

            AudioManager.I.PlayLetter(letter.Data, true);
        }
    }
}