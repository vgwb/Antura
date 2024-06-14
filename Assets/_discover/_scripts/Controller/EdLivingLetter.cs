using Antura.LivingLetters;
using DG.DeExtensions;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public enum HeadPropID
    {
        None,
        Ribbon,
        PunkHair,
        TubeHat
    }

    public class EdLivingLetter : EdAgent
    {
        [Header("Starting Appearance")]
        public HeadPropID HeadProp = HeadPropID.None;
        public string Word;
        public Color Color = Color.white;
        public LLAnimationStates Animation = LLAnimationStates.LL_idle;

        [Header("References")]
        public LivingLetterController letterObjectView;
        public GameObject HeadProp_Ribbon;
        public GameObject HeadProp_PunkHair;
        public GameObject HeadProp_TubeHat;

        private ILivingLetterData letterData { get; set; }

        public override void Start()
        {
            base.Start();
            PlayAnimation(Animation);

            if (!Word.IsNullOrEmpty())
            {
                ShowImage(Word);
            }

            var smr = letterObjectView.GetComponentInChildren<SkinnedMeshRenderer>();
            smr.material.color = Color;

            ShowHeadProp(HeadProp, true);
        }

        public void PlayAnimation(LLAnimationStates anim)
        {
            letterObjectView.SetState(anim);
        }

        public void ShowHeadProp(HeadPropID propID, bool choice)
        {
            HeadProp_Ribbon.SetActive(false);
            HeadProp_PunkHair.SetActive(false);
            HeadProp_TubeHat.SetActive(false);

            switch (propID)
            {
                case HeadPropID.None:
                    break;
                case HeadPropID.Ribbon:
                    HeadProp_Ribbon.SetActive(choice);
                    break;
                case HeadPropID.PunkHair:
                    HeadProp_PunkHair.SetActive(choice);
                    break;
                case HeadPropID.TubeHat:
                    HeadProp_TubeHat.SetActive(choice);
                    break;
            }
        }

        public void ShowImage(string id)
        {
            letterData = new LL_ImageData(id);
            letterObjectView.Init(letterData);
        }

        public override void Update()
        {
            base.Update();
            if (NavMeshAgent.enabled)
            {
                letterObjectView.SetState(LLAnimationStates.LL_walking);
                letterObjectView.SetWalkingSpeed(NavMeshAgent.speed);
            }
            else
            {
                letterObjectView.SetState(LLAnimationStates.LL_idle);
            }
        }

    }
}
