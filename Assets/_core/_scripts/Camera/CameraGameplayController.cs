using Antura.Core;
using UnityEngine;

namespace Antura.CameraControl
{
    /// <summary>
    /// Controller for the camera used during gameplay and to show the 3D world.
    /// Movement is mainly used in the map scene.
    /// </summary>
    public class CameraGameplayController : MonoBehaviour
    {
        // TODO refactor: remove the static access
        public static CameraGameplayController I;

        public bool FxEnabled { get; private set; }

        void Awake()
        {
            I = this;
        }

        void Start()
        {
            EnableFX(AppManager.I.AppSettings.HighQualityGfx);
        }

        public void EnableFX(bool status)
        {
            FxEnabled = status;
            // Debug.Log("CameraGameplayController EnableFX " + status);
            //if (gameObject.GetComponent<VignetteAndChromaticAberration>() != null) {
            //    FxEnabled = status;
            //    gameObject.GetComponent<VignetteAndChromaticAberration>().enabled = status;
            //}
            //gameObject.GetComponent<ColorCorrectionCurves>().enabled = status;
        }

    }
}
