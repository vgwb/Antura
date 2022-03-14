using Antura.AnturaSpace.UI;
using Antura.Core;
using Antura.Helpers;
using Antura.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.AnturaSpace
{
    public class ShopDragIconUI : MonoBehaviour
    {
        public RawImage iconUI;
        public CanvasScaler canvasScaler;

        private void Start()
        {
            ShopDecorationsManager.I.OnDragStart += HandleDragStart;
            ShopDecorationsManager.I.OnDragStop += HandleDragStop;
        }

        private bool isDragging = false;

        private void HandleDragStart(ShopDecorationObject decorationObject)
        {
            isDragging = true;
            iconUI.texture = decorationObject.RawImage.texture;
        }

        private void HandleDragStop()
        {
            isDragging = false;
        }

        private void Update()
        {
            if (isDragging)
            {
                var mousePos = AnturaSpaceUI.I.ScreenToUIPoint(Input.mousePosition);
                iconUI.rectTransform.anchoredPosition = mousePos;
            }
            else
            {
                iconUI.rectTransform.anchoredPosition = Vector3.right * 10000;
            }
        }

    }
}
