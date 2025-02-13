using Antura.Minigames.DiscoverCountry.Interaction;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class InteractableIcon : AbstractMapIcon
    {
        public override bool IsEnabled => true;

        #region Methods

        protected override Vector3 GetPosition()
        {
            return Vector3.zero;
        }

        #endregion
    }
}