using Antura.Minigames.DiscoverCountry.Interaction;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class PlayerMapIcon : AbstractMapIcon
    {
        #region Unity

        void Update()
        {
            if (ActionManager.I.Target_AnturaLocation != null) UpdatePosition(InteractionManager.I.player.transform.position);
        }

        #endregion
        
        #region Public Methods

        public override void Show()
        {
            DoShow();
        }

        #endregion
    }
}