using UnityEngine;

namespace Antura.CameraEffects
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class RenderTextureOutput : MonoBehaviour
    {
        public RenderTexture output;

        int lastWidth;
        int lastHeight;

        bool UpdateTexture()
        {
            if (output == null || Screen.width != lastWidth || Screen.height != lastHeight)
            {
                output = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
                output.hideFlags = HideFlags.HideAndDontSave;
                lastWidth = Screen.width;
                lastHeight = Screen.height;
                return true;
            }
            return false;
        }

        void Start()
        {
            Update();
        }

        void Update()
        {
            if (UpdateTexture())
            { GetComponent<Camera>().targetTexture = output; }
        }
    }
}

// TODO refactor: Helpers need to be standardized
