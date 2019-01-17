using UnityEngine;
using UnityEngine.UI;

namespace Antura.Map
{
    /// <summary>
    /// UI that tells the player where Antura is
    /// </summary>
    public class AnturaHint : MonoBehaviour
    {
        public GameObject panelPivotGO;
        public Camera cam;
        public CanvasScaler canvasScaler;
        public RectTransform rectTr;
        public PlayerPin playerPin;

        public Image goLeftImage;
        public Image goRightImage;

        public int offsetX = 116;

        void Update()
        {
            if (playerPin != null) {
                // Follow pin (on the sides)
                var playerOnScreen = cam.WorldToScreenPoint(playerPin.transform.position);
                float resolutionRatio = Screen.height / canvasScaler.referenceResolution.y;
                rectTr.anchoredPosition = (playerOnScreen - new Vector3(Screen.width / 2, Screen.height / 2)) / resolutionRatio;

                bool playerIsLeft = playerOnScreen.x < 0;
                bool playerIsRight = playerOnScreen.x > Screen.width;

                goLeftImage.gameObject.SetActive(playerIsLeft);
                goRightImage.gameObject.SetActive(playerIsRight);

                if (playerIsRight) {
                    rectTr.anchorMin = new Vector2(1, rectTr.anchorMin.y);
                    rectTr.anchorMax = new Vector2(1, rectTr.anchorMax.y);
                    rectTr.anchoredPosition = new Vector3(-offsetX, rectTr.anchoredPosition.y, 0);
                }
                if (playerIsLeft) {
                    rectTr.anchorMin = new Vector2(0, rectTr.anchorMin.y);
                    rectTr.anchorMax = new Vector2(0, rectTr.anchorMax.y);
                    rectTr.anchoredPosition = new Vector3(offsetX, rectTr.anchoredPosition.y, 0);
                }

                // Auto show/hide
                panelPivotGO.SetActive(playerIsLeft || playerIsRight);
            }
        }

    }
}