using Antura.Discover.Interaction;
using UnityEngine;

namespace Antura.Discover
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