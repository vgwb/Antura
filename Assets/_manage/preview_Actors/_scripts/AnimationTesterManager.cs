using Antura.Dog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Test
{
    public class AnimationTesterManager : MonoBehaviour
    {
        AnturaAnimationTester AnimationTester;
        public GridLayoutGroup StateList;
        public GameObject ElementPrefab;

        public Toggle angryToggle;
        public Slider walkSlider;

        public List<Button> buttons;

        void Start()
        {
            AnimationTester = GetComponent<AnturaAnimationTester>();
            angryToggle.onValueChanged.AddListener(delegate { AngryChanged(); });
            walkSlider.onValueChanged.AddListener(delegate { WalkValueChanged(); });

            var allStates = AnturaAnimationStates.GetValues(typeof(AnturaAnimationStates));
            foreach(AnturaAnimationStates state in allStates)
            {
                Button b = Instantiate<Button>(ElementPrefab.GetComponent<Button>());
                b.transform.SetParent(StateList.transform);
                b.GetComponentInChildren<Text>().text = state.ToString();
                b.onClick.AddListener(delegate { ChangeState(b.GetComponentInChildren<Text>().text); });
            }
        }

        void ChangeState(string _name)
        {
            AnimationTester.targetState = (AnturaAnimationStates)Enum.Parse(typeof(AnturaAnimationStates), _name);
        }

        public void AngryChanged()
        {
            AnimationTester.angry = angryToggle.isOn;
        }

        public void WalkValueChanged()
        {
            AnimationTester.walkSpeed = walkSlider.value;
        }

        public void AlterAnimation(int target)
        {
            var col = Color.gray;
            switch (target)
            {
                case 0:
                    AnimationTester.onJumpStart = !AnimationTester.onJumpStart;
                    if(!AnimationTester.onJumpStart)
                        col = Color.white;
                    break;
                case 1:
                    AnimationTester.onJumpGrab = !AnimationTester.onJumpGrab;
                    if (!AnimationTester.onJumpGrab)
                        col = Color.white;
                    break;
                case 2:
                    AnimationTester.onJumpMiddle = !AnimationTester.onJumpMiddle;
                    if (!AnimationTester.onJumpMiddle)
                        col = Color.white;
                    break;
                case 3:
                    AnimationTester.onJumpEnd = !AnimationTester.onJumpEnd;
                    if (!AnimationTester.onJumpEnd)
                        col = Color.white;
                    break;
                case 4:
                    AnimationTester.doCharge = !AnimationTester.doCharge;
                    if (!AnimationTester.doCharge)
                        col = Color.white;
                    break;
                case 5:
                    AnimationTester.doBurp = !AnimationTester.doBurp;
                    if (!AnimationTester.doBurp)
                        col = Color.white;
                    break;
                case 6:
                    AnimationTester.doBite = !AnimationTester.doBite;
                    if (!AnimationTester.doBite)
                        col = Color.white;
                    break;
                case 7:
                    AnimationTester.doShout = !AnimationTester.doShout;
                    if (!AnimationTester.doShout)
                        col = Color.white;
                    break;
                case 8:
                    AnimationTester.doSniff = !AnimationTester.doSniff;
                    if (!AnimationTester.doSniff)
                        col = Color.white;
                    break;
                case 9:
                    AnimationTester.doSpitOpen = !AnimationTester.doSpitOpen;
                    if (!AnimationTester.doSpitOpen)
                        col = Color.white;
                    break;
                case 10:
                    AnimationTester.doSpitClosed = !AnimationTester.doSpitClosed;
                    if (!AnimationTester.doSpitClosed)
                        col = Color.white;
                    break;
                case 11:
                    AnimationTester.onSlipStart = !AnimationTester.onSlipStart;
                    if (!AnimationTester.onSlipStart)
                        col = Color.white;
                    break;
                case 13:
                    AnimationTester.onSlipEnd = !AnimationTester.onSlipEnd;
                    if (!AnimationTester.onSlipEnd)
                        col = Color.white;
                    break;

            }
            buttons[target].GetComponent<Image>().color = col;
        }
    }
}
