using Antura.Minigames.DiscoverCountry.Interaction;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class PlayerMapIcon : AbstractMapIcon
    {
        public override bool IsEnabled => true;
        public bool Rotate;

        #region Methods

        protected override Vector3 GetPosition()
        {
            return InteractionManager.I.player.transform.position;
        }

        void Update()
        {
            if (Rotate)
            {
                transform.Rotate(Vector3.up * 10 * Time.fixedDeltaTime);
            }
        }

        #endregion
    }
}
