using System.Collections.Generic;
using Antura.CameraEffects;
using TMPro;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    public class HiddenText : MonoBehaviour
    {
        public Camera mainCamera;
        public Camera textCamera;
        public GameObject target;

        RenderTexture textRenderTexture;
        RenderTexture blurredTextRenderTexture;

        Vector2 lastMin = new Vector2(Screen.width, Screen.height);
        Vector2 lastMax = Vector2.zero;

        Vector3 boundsMin;
        Vector3 boundsMax;

        bool hasElements = false;
        bool needsRender = false;

        Dictionary<TextMeshPro, float> previousAlpha = new Dictionary<TextMeshPro, float>();
        Dictionary<TextMeshPro, string> previousString = new Dictionary<TextMeshPro, string>();

        List<MeshRenderer> renderers = new List<MeshRenderer>();
        List<TextMeshPro> texts = new List<TextMeshPro>();

        private Material magnifyingGlassMaterial
        {
            get { return ((ReadingGameGame)ReadingGameGame.I).magnifyingGlassMaterial; }
        }

        private Material blurredTextMaterial
        {
            get { return ((ReadingGameGame)ReadingGameGame.I).blurredTextMaterial; }
        }

        public void Clear()
        {
            if (textCamera != null)
                textCamera.Render();
        }

        private void UpdateTarget()
        {
            hasElements = true;

            renderers.Clear();
            texts.Clear();

            target.GetComponentsInChildren<MeshRenderer>(true, renderers);
            target.GetComponentsInChildren<TextMeshPro>(true, texts);

            bool textChanged = false;
            for (int i = 0, count = texts.Count; i < count; ++i)
            {
                var textMesh = texts[i];
                var currentAlpha = textMesh.alpha;
                var currentString = textMesh.text;
                float previousAlpha;

                if (this.previousAlpha.TryGetValue(textMesh, out previousAlpha))
                {
                    textChanged = textChanged || (previousAlpha != currentAlpha);
                }
                else
                {
                    textChanged = true;
                }

                this.previousAlpha[textMesh] = currentAlpha;

                string previousString;

                if (this.previousString.TryGetValue(textMesh, out previousString))
                {
                    textChanged = textChanged || (previousString != currentString);
                }
                else
                {
                    textChanged = true;
                }

                this.previousString[textMesh] = currentString;
            }

            Vector3 min = new Vector3(Screen.width, Screen.height, 0);
            Vector3 max = Vector3.zero;

            boundsMin = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            boundsMax = new Vector3(-float.PositiveInfinity, -float.PositiveInfinity, -float.PositiveInfinity);

            hasElements = renderers.Count > 0;
            for (int i = 0, count = renderers.Count; i < count; ++i)
            {
                if (!renderers[i].enabled || !renderers[i].gameObject.activeInHierarchy)
                    continue; // skip

                var screenBoundsMin = mainCamera.WorldToScreenPoint(renderers[i].bounds.min) - new Vector3(32.0f, 32.0f, 0.0f);
                var screenBoundsMax = mainCamera.WorldToScreenPoint(renderers[i].bounds.max) + new Vector3(32.0f, 32.0f, 0.0f);

                // Define a minimum size
                screenBoundsMin.x = Mathf.Min(screenBoundsMin.x, 0.25f * Screen.width);
                screenBoundsMin.y = Mathf.Min(screenBoundsMin.y, 0.5f * Screen.height);
                screenBoundsMax.x = Mathf.Max(screenBoundsMax.x, 0.75f * Screen.width);
                screenBoundsMax.y = Mathf.Max(screenBoundsMax.y, 1.0f * Screen.height);

                boundsMin = Vector3.Min(boundsMin, mainCamera.ScreenToWorldPoint(screenBoundsMin));
                boundsMax = Vector3.Max(boundsMax, mainCamera.ScreenToWorldPoint(screenBoundsMax));

                min = Vector3.Min(min, screenBoundsMin);
                max = Vector3.Max(max, screenBoundsMax);
            }

            if (!hasElements)
            {

            }

            if ((Vector2)min == lastMin && (Vector2)max == lastMax && !textChanged)
                return;

            lastMin = (Vector2)min;
            lastMax = (Vector2)max;

            int width = (int)(max.x - min.x);
            int height = (int)(max.y - min.y);

            if (width <= 0 || height <= 0)
                return;

            if (textRenderTexture != null)
            {
                magnifyingGlassMaterial.SetTexture("_BackTex", null);
                textCamera.GetComponent<BlurredCamera>().normalTextureOutput = null;
                textRenderTexture.Release();
                DestroyImmediate(textRenderTexture);
                textRenderTexture = null;
            }

            textRenderTexture = new RenderTexture(width, height, 0);

            magnifyingGlassMaterial.SetTexture("_BackTex", textRenderTexture);

            if (blurredTextRenderTexture != null)
            {
                blurredTextMaterial.SetTexture("_MainTex", null);
                textCamera.targetTexture = null;
                blurredTextRenderTexture.Release();
                DestroyImmediate(blurredTextRenderTexture);
                blurredTextRenderTexture = null;
            }

            blurredTextRenderTexture = new RenderTexture(width, height, 0);

            blurredTextMaterial.SetTexture("_MainTex", blurredTextRenderTexture);

            textCamera.targetTexture = blurredTextRenderTexture;
            textCamera.GetComponent<BlurredCamera>().normalTextureOutput = textRenderTexture;

            var oldCameraPos = textCamera.transform.position;
            oldCameraPos.x = boundsMin.x + (boundsMax.x - boundsMin.x) / 2;
            oldCameraPos.y = boundsMin.y + (boundsMax.y - boundsMin.y) / 2;
            textCamera.transform.position = oldCameraPos;
            textCamera.orthographicSize = (boundsMax.y - boundsMin.y) / 2;

            Vector2 uvMin;
            uvMin.x = min.x / Screen.width;
            uvMin.y = min.y / Screen.height;

            Vector2 uvMax;
            uvMax.x = max.x / Screen.width;
            uvMax.y = max.y / Screen.height;

            magnifyingGlassMaterial.SetVector("_BackOffset", uvMin);
            magnifyingGlassMaterial.SetVector("_BackScale", (Vector2)(uvMax - uvMin));
            needsRender = true;

            GetComponentInChildren<MeshRenderer>().sharedMaterial = blurredTextMaterial;
        }

        void Update()
        {
            UpdateTarget();

            if (hasElements && needsRender)
            {
                transform.position = boundsMin + 0.5f * Vector3.forward;
                transform.localScale = new Vector3(boundsMax.x - boundsMin.x, boundsMax.y - boundsMin.y, 1);
                textCamera.Render();
                needsRender = false;
            }
        }

        void Start()
        {
            textCamera.enabled = false;
        }

        void OnDestroy()
        {
            if (textRenderTexture != null)
            {
                magnifyingGlassMaterial.SetTexture("_BackTex", null);
                textCamera.GetComponent<BlurredCamera>().normalTextureOutput = null;
                textRenderTexture.Release();
                DestroyImmediate(textRenderTexture);
                textRenderTexture = null;
            }

            if (blurredTextRenderTexture != null)
            {
                blurredTextMaterial.SetTexture("_MainTex", null);
                textCamera.targetTexture = null;
                blurredTextRenderTexture.Release();
                DestroyImmediate(blurredTextRenderTexture);
                blurredTextRenderTexture = null;
            }
        }
    }
}
