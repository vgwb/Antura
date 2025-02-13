using Antura.Minigames.DiscoverCountry.Interaction;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class AnturaMapIcon : AbstractMapIcon
    {
        #region Unity

        void Update()
        {
            if (ActionManager.I.Target_AnturaLocation != null) UpdatePosition(ActionManager.I.Target_AnturaLocation.position);
        }

        #endregion

        #region Public Methods

        public override void Show()
        {
            if (ActionManager.I.Target_AnturaLocation != null) DoShow();
            else Hide(true);
        }

        #endregion
    }
}
