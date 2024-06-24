using DG.DeInspektor.Attributes;
using Unity.Cinemachine;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class MapCamera : AbstractCineCamera
    {
        #region Serialized

        [Header("Options")]
        [Range(10, 300)]
        [SerializeField] int zoomOutFactor = 150;
        [SerializeField] bool alignToPlayerCam = true;

        #endregion

        Transform camTarget;
        Transform playerCamT;
        CinemachinePositionComposer cineComposer;

        #region Unity

        void Awake()
        {
            camTarget = FindObjectOfType<PlayerCameraTarget>(true).transform;
            cineMain.Target.TrackingTarget = camTarget;
            playerCamT = this.GetComponent<PlayerCameraController>().CineMain.transform;
            cineComposer = CineMain.GetComponent<CinemachinePositionComposer>();
        }

        #endregion

        #region Public Methods

        public override void Activate(bool activate)
        {
            base.Activate(activate);

            if (activate)
            {
                cineComposer.CameraDistance = zoomOutFactor;
                if (alignToPlayerCam)
                {
                    Vector3 playerCamEuler = playerCamT.eulerAngles;
                    Vector3 euler = cineMain.transform.eulerAngles;
                    euler.y = playerCamEuler.y;
                    cineMain.transform.rotation = Quaternion.Euler(euler);
                }
                DiscoverNotifier.Game.OnMapCameraActivated.Dispatch(true);
            }
            else
            {
                DiscoverNotifier.Game.OnMapCameraActivated.Dispatch(false);
            }
        }

        #endregion
    }
}
