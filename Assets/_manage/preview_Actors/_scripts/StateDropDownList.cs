using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Antura.LivingLetters;

namespace Antura.Test
{
    public class StateDropDownList : Dropdown {

        new void Start() {
            options.Clear();
            options.AddRange(addOptionsFromEnum<LLAnimationStates>());
            onValueChanged.AddListener(delegate {
                foreach (var l in FindObjectsOfType<LivingLetterController>()) {
                    l.SetState((LLAnimationStates)Enum.Parse(typeof(LLAnimationStates), options[value].text));
                }
            });

        }

        List<OptionData> addOptionsFromEnum<T>() {
            List<OptionData> optionsToAdd = new List<OptionData>();
            foreach (var val in Enum.GetValues(typeof(T))) {
                optionsToAdd.Add(new OptionData() { text = val.ToString() });
            }
            return optionsToAdd;
        }

        protected override void OnDisable() {
            base.OnDisable();

            onValueChanged.RemoveAllListeners();
        }
    }
}