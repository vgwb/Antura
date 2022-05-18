using Antura.Core;

namespace Antura.AnturaSpace
{
    public class ShopAction_Photo : ShopAction
    {
        protected override string ActionKey => "photo";

        void Start()
        {
            var shopActionUI = GetComponent<ShopActionUI>();
            shopActionUI.SetAction(this);
            ShopPhotoManager.I.OnPurchaseSuccess += CommitActionCheck;

            if (!AppManager.I.Services.Gallery.HasWriteAccess)
                bonesCost = 0;

            ShopPhotoManager.I.CurrentPhotoCost = bonesCost;
        }

        public override void PerformAction()
        {
            ShopPhotoManager.I.TakePhoto();
        }

        // TODO: optionally set a limit for photos to be saved
        //public int photoLimit = 10;

        public override bool IsLocked
        {
            get
            {
                if (base.IsLocked)
                {
                    return true;
                }
                return false;
            }
        }

        public override bool CanPurchaseAnywhere
        {
            get { return true; }
        }

        public override bool IsOnTheSide
        {
            get { return true; }
        }

        public override bool IsClickButton
        {
            get { return true; }
        }
    }
}
