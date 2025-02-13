﻿using Antura.Minigames.DiscoverCountry.Interaction;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class PlayerMapIcon : AbstractMapIcon
    {
        public override bool IsEnabled => true;
        
        #region Methods

        protected override Vector3 GetPosition()
        {
            return InteractionManager.I.player.transform.position;
        }

        #endregion
    }
}