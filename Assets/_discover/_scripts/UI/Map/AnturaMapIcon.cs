using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class AnturaMapIcon : AbstractMapIcon
    {
        public override bool IsEnabled => ActionManager.I.Target_AnturaLocation != null;
        
        #region Methods

        protected override Vector3 GetPosition()
        {
            return IsEnabled ? ActionManager.I.Target_AnturaLocation.position : Vector3.zero;
        }

        #endregion
    }
}
