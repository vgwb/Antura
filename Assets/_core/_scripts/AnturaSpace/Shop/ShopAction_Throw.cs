using System;
using Antura.Core;
using Antura.Dog;
using UnityEngine;

namespace Antura.AnturaSpace
{

    public class ShopAction_Throw : ShopAction
    {
        public GameObject objectsToRender;
        public ThrowableObject throwingObjectPrefabGO;
        protected override string ActionKey => "bone";

        public override Sprite IconSprite => Resources.Load<Sprite>($"{AppManager.I.Player.PetData.SelectedPet}/UI/bone");

        public override GameObject ObjectToRender => objectsToRender;

        public AnturaSpaceScene AnturaSpaceScene => ((AnturaSpaceScene)AnturaSpaceScene.I);

        public override void PerformAction()
        {
            ThrowableObject thrownObject = AnturaSpaceScene.ThrowObject(throwingObjectPrefabGO);
            if (thrownObject != null)
            {
                thrownObject.OnDeath += RefreshAction;
            }
            CommitAction();
        }

        public override void PerformDrag()
        {
            ThrowableObject thrownObject = AnturaSpaceScene.DragObject(throwingObjectPrefabGO);
            if (thrownObject != null)
            {
                thrownObject.OnDeath += RefreshAction;
            }
            CommitAction();
        }

        public override bool IsLocked
        {
            get
            {
                if (base.IsLocked)
                    return base.IsLocked;
                return !AnturaSpaceScene.CanSpawnMoreObjects;
            }
        }

    }
}
