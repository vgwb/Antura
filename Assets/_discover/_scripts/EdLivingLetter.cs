using Antura.LivingLetters;
using Antura.UI;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class EdLivingLetter : MonoBehaviour
    {
        [Header("References")]
        public LivingLetterController letterObjectView;
        public GameObject HeadProp;

        public ILivingLetterData letterData { get; private set; }

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
    }
}
