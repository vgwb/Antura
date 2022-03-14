using Antura.Core;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class ShopActionsManager : MonoBehaviour
    {
        public ShopPanelUI ShopPanelUi;
        public ShopDecorationsManager ShopDecorationsManager;

        private ShopAction[] shopActions;

        public void Initialise()
        {
            // Setup the decorations manager
            var shopState = AppManager.I.Player.CurrentShopState;
            ShopDecorationsManager.Initialise(shopState);
            ShopDecorationsManager.OnContextChange += (x) => UpdateAllActions();

            // Setup actions
            shopActions = GetComponentsInChildren<ShopAction>();
            foreach (var shopAction in shopActions)
            {
                shopAction.OnActionCommitted += HandleActionPerformed;
                shopAction.OnActionRefreshed += HandleActionRefreshed;
            }

            ShopPanelUi.SetActions(shopActions);
            ShopPanelUi.UpdateAllActionButtons();
        }

        private void HandleActionPerformed()
        {
            UpdateAllActions();
        }

        private void HandleActionRefreshed()
        {
            UpdateAllActions();
        }

        void UpdateAllActions()
        {
            ShopPanelUi.UpdateAllActionButtons();
        }

    }
}
