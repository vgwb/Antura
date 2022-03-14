using Antura.Core;
using Antura.Dog;
using Antura.Rewards;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class AnturaCustomizationState : AnturaState
    {
        public AnturaCustomizationState(AnturaSpaceScene controller) : base(controller)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            UI.AnturaSpaceUI.onRewardCategorySelectedInCustomization += AnturaSpaceUI_onRewardCategorySelectedInCustomization;
            controller.Antura.SetTarget(controller.SceneCenter, true, controller.RotatingBase.transform);
            controller.RotatingBase.Activated = true;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            controller.Antura.AnimationController.State = AnturaAnimationStates.sitting;
        }

        public override void ExitState()
        {
            UI.AnturaSpaceUI.onRewardCategorySelectedInCustomization -= AnturaSpaceUI_onRewardCategorySelectedInCustomization;
            controller.RotatingBase.Angle = 0;
            controller.RotatingBase.Activated = false;
            controller.Antura.AnimationController.State = AnturaAnimationStates.idle;
            controller.Antura.SetTarget(null, false);
            base.ExitState();
        }

        /// <summary>
        /// Happens when category selected in antura space customization mode.
        /// </summary>
        /// <param name="_category">The category.</param>
        private void AnturaSpaceUI_onRewardCategorySelectedInCustomization(string _category)
        {
            float rotation = AppManager.I.RewardSystemManager.GetAnturaRotationAngleViewForRewardCategory(_category);
            float offSet = rotation == 0 ? 0 : 40;
            controller.RotatingBase.Angle = rotation + offSet;
        }
    }
}
