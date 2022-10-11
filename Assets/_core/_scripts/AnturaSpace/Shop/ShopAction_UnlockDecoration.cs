
using System;
using Antura.Core;
using Antura.Keeper;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class ShopAction_UnlockDecoration : ShopAction
    {
        public ShopDecorationObject UnlockableDecorationObject;
        protected override string ActionKey => LocID;
        public override GameObject ObjectToRender
        {
            get { return UnlockableDecorationObject.gameObject; }
        }

        public override void PerformDrag()
        {
            if (!AnturaSpaceScene.I.TutorialMode)
                KeeperManager.I.PlayDialogue("UI_Antura_Shop_" + LocID, keeperMode: KeeperMode.LearningNoSubtitles);

            ShopDecorationsManager.I.CreateAndStartDragPlacement(UnlockableDecorationObject, bonesCost);
            ShopDecorationsManager.I.OnPurchaseComplete += CommitAction;
            ShopDecorationsManager.I.OnPurchaseCancelled += CancelAction;
        }

        protected override void CommitAction()
        {
            base.CommitAction();
            ShopDecorationsManager.I.OnPurchaseComplete -= CommitAction;
            ShopDecorationsManager.I.OnPurchaseCancelled -= CancelAction;
        }

        public override void CancelAction()
        {
            base.CancelAction();
            ShopDecorationsManager.I.OnPurchaseComplete -= CommitAction;
            ShopDecorationsManager.I.OnPurchaseCancelled -= CancelAction;
        }

        public override bool IsLocked
        {
            get
            {
                if (base.IsLocked)
                    return base.IsLocked;
                return !ShopDecorationsManager.I.HasSlotsForDecoration(UnlockableDecorationObject);
            }
        }

    }
}
