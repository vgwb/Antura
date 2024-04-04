using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class DialogueCamera : AbstractCineCamera
    {
        #region Public Methods

        public void SetTarget(Transform target)
        {
            cineMain.Target.TrackingTarget = target;
        }

        #endregion
    }
}