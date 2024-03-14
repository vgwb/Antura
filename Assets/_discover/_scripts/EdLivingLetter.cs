using System;
using Antura.LivingLetters;
using Antura.UI;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class EdLivingLetter : MonoBehaviour
    {
        [Header("Homer")]
        public string UniqueID;

        [Header("References")]
        public LivingLetterController letterObjectView;
        public GameObject HeadProp;

        public Action<GameObject> OnInteraction;
        private ILivingLetterData letterData { get; set; }

        void Start()
        {
            PlayAnimation(LLAnimationStates.LL_idle);
        }

        public void PlayAnimation(LLAnimationStates anim)
        {
            letterObjectView.SetState(anim);
        }

        public void ShowHeadProp(bool choice)
        {
            HeadProp.SetActive(choice);
        }

        public void ShowImage(string id)
        {
            letterData = new LL_ImageData(id);
            letterObjectView.Init(letterData);
        }


        public void OnInteractionWith(GameObject otherGo)
        {
            OnInteraction?.Invoke(otherGo);
        }
    }
}
