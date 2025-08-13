using System;
using Antura.LivingLetters;
using DG.DeExtensions;
using UnityEngine;

namespace Antura.Discover
{
    public enum HeadPropID
    {
        None,
        Ribbon,
        PunkHair,
        TubeHat,
        Guard,
        Wizard,
        Guide,
        Cook,
        Tutor
    }

    public class EdLivingLetter : EdAgent
    {
        [Header("Starting Appearance")]
        public HeadPropID HeadProp = HeadPropID.None;
        public WordData Word;
        public Color Color = Color.white;
        public LLAnimationStates Animation = LLAnimationStates.LL_idle;

        [Header("References")]
        public LivingLetterController letterObjectView;
        public GameObject HeadProp_Ribbon;
        public GameObject HeadProp_PunkHair;
        public GameObject HeadProp_TubeHat;
        public GameObject HeadProp_Guard;
        public GameObject HeadProp_Wizard;
        public GameObject HeadProp_Guide;
        public GameObject Prop_Cook;
        public GameObject Prop_Tutor;

        private ILivingLetterData letterData { get; set; }

        public override void Start()
        {
            base.Start();
            PlayAnimation(Animation);

            if (Word != null)
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
            HeadProp_Guard.SetActive(false);
            HeadProp_Wizard.SetActive(false);
            HeadProp_Guide.SetActive(false);
            Prop_Cook.SetActive(false);
            Prop_Tutor.SetActive(false);

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
                case HeadPropID.Guard:
                    HeadProp_Guard.SetActive(choice);
                    break;
                case HeadPropID.Wizard:
                    HeadProp_Wizard.SetActive(choice);
                    break;
                case HeadPropID.Guide:
                    HeadProp_Guide.SetActive(choice);
                    break;
                case HeadPropID.Cook:
                    Prop_Cook.SetActive(choice);
                    break;
                case HeadPropID.Tutor:
                    Prop_Tutor.SetActive(choice);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propID), propID, null);
            }
        }

        public void ShowImage(WordData word)
        {
            letterData = new LL_ImageData(word.Id);
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
