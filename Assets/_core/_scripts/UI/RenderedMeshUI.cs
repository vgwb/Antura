using Antura.Extensions;
using Antura.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// Renders a 3D object at startup and shows it as a UI item.
    /// </summary>
    public class RenderedMeshUI : MonoBehaviour
    {
        public GameObject renderedGo;
        public LayerMask RenderOnUILayer;
        public Transform containerPivotTR;

        private RenderTexture renderTexture;

        public Vector3 eulOffset = new Vector3(0, 0, 0);
        public float scaleMultiplier = 2f;

        private RawImage rawImage;

        public void AssignObjectToRender(GameObject go)
        {
            renderedGo = Instantiate(go);
            renderedGo.transform.SetParent(containerPivotTR);
            renderedGo.transform.localScale = Vector3.one;
            renderedGo.transform.localPosition = Vector3.zero;

            renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            renderTexture.Create();
            var camera = GetComponentInChildren<Camera>(true);
            camera.targetTexture = renderTexture;
            rawImage = GetComponentInChildren<RawImage>(true);
            rawImage.texture = renderTexture;

            renderedGo.SetLayerRecursive(GenericHelper.LayerMaskToIndex(RenderOnUILayer));
            CameraHelper.FitShopDecorationToUICamera(renderedGo.transform, camera, scaleMultiplier, eulOffset);
            camera.Render();
            camera.enabled = false;
            renderedGo.gameObject.SetActive(false);
        }

        public RawImage GetRawImage()
        {
            return rawImage;
        }

        public Texture GetTexture()
        {
            return rawImage.mainTexture;
        }

        void OnDestroy()
        {
            if (renderTexture != null)
            {
                renderTexture.Release();
                renderTexture.DiscardContents();
            }
        }

    }
}
