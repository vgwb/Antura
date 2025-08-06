using Unity.Cinemachine;
using UnityEngine;

namespace Antura.Discover
{
    public class DialogueCamera : AbstractCineCamera
    {
        #region Serialized

        [Range(0, -0.3f)]
        [SerializeField] float composerScreenPositionXWChoices = -0.22f;

        #endregion

        float defComposerScreenPositionX;
        CinemachinePositionComposer positionComposer;

        void Awake()
        {
            positionComposer = CineMain.GetComponent<CinemachinePositionComposer>();
            defComposerScreenPositionX = positionComposer.Composition.ScreenPosition.x;
        }

        #region Public Methods

        public void SetTarget(Transform target, bool hasChoices = false)
        {
            cineMain.Target.TrackingTarget = target;
            positionComposer.Composition.ScreenPosition.x = hasChoices ? composerScreenPositionXWChoices : defComposerScreenPositionX;
        }

        #endregion
    }
}
