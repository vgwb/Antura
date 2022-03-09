using UnityEngine;
using UnityEngine.UI;

namespace Antura.Core.Services.Gallery
{

    public class GalleryManager : MonoBehaviour
    {
        public GameObject PhotoFrame;

        public RawImage PhotoImage;
        public Image PhotoBorder;

        public void ShowPreview(Texture texture)
        {
            float frameAspectRatio = 640f / 480f;
            float aspectRatio = Screen.width * 1f / Screen.height;
            PhotoImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 640 * aspectRatio / frameAspectRatio);
            PhotoBorder.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 640 * aspectRatio / frameAspectRatio);

            PhotoImage.texture = texture;
            PhotoFrame.SetActive(true);
        }
        public void HidePreview()
        {
            PhotoFrame.SetActive(false);
        }
    }
}
