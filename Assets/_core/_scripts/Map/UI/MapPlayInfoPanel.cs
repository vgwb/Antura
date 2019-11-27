using Antura.Core;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Map
{
    /// <summary>
    /// UI that appears when you select a Pin.
    /// </summary>
    public class MapPlayInfoPanel : MonoBehaviour
    {
        public Camera cam;
        public CanvasScaler canvasScaler;
        public RectTransform rectTr;

        public GameObject panelPivotGO;

        public Button playBtn;
        public Button lockedBtn;

        public TextRender psNumberTextUI;
        public TextRender psNameTextUI;

        private Pin targetPin = null;

        void Awake()
        {
            RemovePin();
        }

        void Update()
        {
            if (targetPin != null) {
                // Follow pin
                var pinOnScreen = cam.WorldToScreenPoint(targetPin.transform.position);
                float resolutionRatio = Screen.height / canvasScaler.referenceResolution.y;

                Vector2 newAnchor = (pinOnScreen - new Vector3(Screen.width / 2, Screen.height / 2)) / resolutionRatio;
                newAnchor.y = rectTr.anchoredPosition.y;
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
            psNumberTextUI.SetText(pin.journeyPosition.ToDisplayedString(true));
            var lbDatas = AppManager.I.DB.FindLearningBlockData(x => x.Stage == pin.journeyPosition.Stage && x.LearningBlock == pin.journeyPosition.LearningBlock);
            if (lbDatas.Count != 0 && lbDatas[0] != null) psNameTextUI.SetText(lbDatas[0].Title_LearningLang);
            //playBtn.gameObject.SetActive(!pin.isLocked);
            //lockedBtn.gameObject.SetActive(pin.isLocked);
        }

    }
}