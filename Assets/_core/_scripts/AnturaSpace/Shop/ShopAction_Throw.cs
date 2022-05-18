using Antura.Core;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class ShopAction_Throw : ShopAction
    {
        public GameObject objectToRender;
        public ThrowableObject throwingObjectPrefabGO;
        protected override string ActionKey => "bone";
        public override GameObject ObjectToRender
        {
            get { return objectToRender; }
        }

        public AnturaSpaceScene AnturaSpaceScene
        {
            get { return ((AnturaSpaceScene)AnturaSpaceScene.I); }
        }

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
