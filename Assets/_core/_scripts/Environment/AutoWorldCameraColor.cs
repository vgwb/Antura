using Antura.CameraEffects;
using UnityEngine;

namespace Antura.Environment
{
    [ExecuteInEditMode]
    public class AutoWorldCameraColor : MonoBehaviour
    {
        public WorldColorSet backgroundColorSet;

#if UNITY_EDITOR
        private WorldColorSet lastColorSet;
        private WorldID lastTestWorld = WorldID.Default;
        public WorldID testWorld;
#endif

        void UpdateCamera(Color color)
        {
            var camera = GetComponent<Camera>();
            var cameraFog = GetComponent<CameraFog>();

            camera.backgroundColor = color;

            if (cameraFog != null)
            {
                cameraFog.color = color;
            }
        }

        public void Start()
        {
            if (backgroundColorSet != null)
            {
                var color = WorldManager.I.GetColor(backgroundColorSet);

                UpdateCamera(color);
            }
        }


#if UNITY_EDITOR
        void Update()
        {
            if (!Application.isPlaying && backgroundColorSet != null)
            {
                if (testWorld != lastTestWorld || backgroundColorSet != lastColorSet)
                {
                    lastTestWorld = testWorld;
                    lastColorSet = backgroundColorSet;

                    var color = WorldManager.I.GetColor(backgroundColorSet, testWorld);

                    UpdateCamera(color);
                }
            }
        }
#endif
    }
}
