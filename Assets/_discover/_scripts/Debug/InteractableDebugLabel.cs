using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Antura.Discover.Debugging
{
    [DisallowMultipleComponent]
    public class InteractableDebugLabel : MonoBehaviour
    {
        [SerializeField] private Canvas worldCanvas;
        [SerializeField] private TextMeshPro text;
        [SerializeField] private Vector3 offset = new Vector3(0, .01f, 0);
        [SerializeField] private bool alwaysVisible = true;

        private Camera mainCam;
        private Interactable interactable;

        void Awake()
        {
            mainCam = Camera.main;
            interactable = GetComponentInParent<Antura.Discover.Interactable>();

            if (worldCanvas == null)
            {
                var go = new GameObject("DebugLabelCanvas");
                go.transform.SetParent(transform, false);
                worldCanvas = go.AddComponent<Canvas>();
                worldCanvas.renderMode = RenderMode.WorldSpace;
                worldCanvas.sortingOrder = 5000;
                var scaler = go.AddComponent<CanvasScaler>();
                scaler.dynamicPixelsPerUnit = 10;
            }
            if (text == null)
            {
                var txtGO = new GameObject("Label");
                txtGO.transform.SetParent(worldCanvas.transform, false);
                text = txtGO.AddComponent<TextMeshPro>();
                text.alignment = TextAlignmentOptions.Center;
                //text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.fontSize = 1.5f;
                text.color = Color.yellow;
                var rt = text.rectTransform;
                rt.sizeDelta = new Vector2(200, 30);
            }
        }

        void OnEnable()
        {
            UpdateText();
        }

        void LateUpdate()
        {
            if (mainCam == null)
            { mainCam = Camera.main; if (mainCam == null) return; }
            var target = interactable != null && interactable.IconTransform != null ? interactable.IconTransform : transform;
            transform.position = target.position + offset;
            transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
            if (!alwaysVisible && worldCanvas != null)
                worldCanvas.enabled = QuestManager.I != null && QuestManager.I.DebugQuest;
        }

        public void UpdateText()
        {
            if (text == null || interactable == null)
                return;
            var nodeName = interactable.DialogueNodeName;
            var permalink = interactable.NodePermalink;
            var label = !string.IsNullOrEmpty(nodeName) ? nodeName : (!string.IsNullOrEmpty(permalink) ? permalink : name);
            text.text = label;
        }
    }
}
