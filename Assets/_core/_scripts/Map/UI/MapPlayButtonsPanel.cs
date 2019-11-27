using UnityEngine;
using UnityEngine.UI;

namespace Antura.Map
{
    /// <summary>
    /// UI that appears when you select a Pin.
    /// Has the button for play.
    /// </summary>
    public class MapPlayButtonsPanel : MonoBehaviour
    {
        public Camera cam;
        public CanvasScaler canvasScaler;
        public RectTransform rectTr;

        public GameObject panelPivotGO;

        public Button playBtn;
        public Button lockedBtn;

        private Pin targetPin = null;

        void Awake()
        {
            RemovePin();
        }

        void Update()
        {
            if (targetPin != null)
            {
                // Follow pin
                var pinOnScreen = cam.WorldToScreenPoint(targetPin.transform.position);
                float resolutionRatio = Screen.height / canvasScaler.referenceResolution.y;

                Vector2 newAnchor = (pinOnScreen - new Vector3(Screen.width / 2, Screen.height / 2)) / resolutionRatio;
                rectTr.anchoredPosition = newAnchor;

                // Auto show/hide
                panelPivotGO.SetActive(pinOnScreen.x >= 0 && pinOnScreen.x < Screen.width);
            }
        }

        public void RemovePin()
        {
            targetPin = null;
            panelPivotGO.SetActive(false);
        }

        public void SetPin(Pin pin)
        {
            targetPin = pin;
            playBtn.gameObject.SetActive(!pin.isLocked);
            lockedBtn.gameObject.SetActive(pin.isLocked);
        }

    }
}