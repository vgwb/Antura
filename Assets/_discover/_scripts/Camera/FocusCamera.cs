using UnityEngine;

namespace Antura.Discover
{
    public class FocusCamera : AbstractCineCamera
    {
        #region Serialized

        [Range(0, 30)]
        public int YOffset = 4;

        #endregion

        #region Public Methods

        public void SetTarget(Transform target)
        {
            cineMain.Target.TrackingTarget = target;
        }

        #endregion
    }
}
