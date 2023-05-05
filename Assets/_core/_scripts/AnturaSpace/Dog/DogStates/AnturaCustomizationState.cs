using Antura.Core;
using Antura.Dog;
using Antura.Rewards;
using DG.Tweening;
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
            Camera.main.DOFieldOfView(50, 0.5f);
            Camera.main.transform.DOLocalRotate(new Vector3(10, -8, 0), 0.5f);

            UI.AnturaSpaceUI.onRewardCategorySelectedInCustomization += AnturaSpaceUI_onRewardCategorySelectedInCustomization;
            controller.Antura.SetTarget(controller.SceneCenter, true, controller.RotatingBase.transform);
            controller.RotatingBase.Activated = true;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            controller.Antura.AnimController.State = AnturaAnimationStates.sitting;
        }

        public override void ExitState()
        {
            Camera.main.DOFieldOfView(60, 0.5f);
            Camera.main.transform.DOLocalRotate(new Vector3(8, 0, 0), 0.5f);

            UI.AnturaSpaceUI.onRewardCategorySelectedInCustomization -= AnturaSpaceUI_onRewardCategorySelectedInCustomization;
            controller.RotatingBase.Angle = 0;
            controller.RotatingBase.Activated = false;
            controller.Antura.AnimController.State = AnturaAnimationStates.idle;
            controller.Antura.SetTarget(null, false);
            base.ExitState();
        }

        /// <summary>
        /// Happens when category selected in antura space customization mode.
        /// </summary>
        /// <param name="_category">The category.</param>
        private void AnturaSpaceUI_onRewardCategorySelectedInCustomization(string _category)
        {
            float rotation = AppManager.I.RewardSystemManager.GetAnturaRotationAngleViewForRewardCategory(_category, AppManager.I.Player.PetData.SelectedPet);
            float offSet = rotation == 0 ? 0 : 40;
            controller.RotatingBase.Angle = rotation + offSet;
        }
    }
}
