using UnityEngine;

namespace Antura.CameraEffects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class VignettingSimple : MonoBehaviour
    {
        public float fadeOut = 0;

        public float vignetting
        {
            get { return intensity; }
            set { intensity = value; }
        }

        public float intensity = 0.375f;
        public Color color = Color.black;

        public Shader vignetteShader;
        private Material vignetteMaterial;

        void Start()
        {
            vignetteMaterial = new Material(vignetteShader);
            fadeOut = 0;
        }

        void OnPostRender()
        {
            if (vignetteMaterial == null)
            { return; }

            fadeOut = Mathf.Clamp01(fadeOut);

            vignetteMaterial.SetFloat("_Intensity", intensity);
            vignetteMaterial.SetFloat("_FadeOut", fadeOut);
            vignetteMaterial.SetColor("_Color", color);

            GL.PushMatrix();
            vignetteMaterial.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);

            GL.TexCoord3(0, 0, 0);
            GL.Vertex3(0, 0, 0);

            GL.TexCoord3(0, 1, 0);
            GL.Vertex3(0, 1, 0);

            GL.TexCoord3(1, 1, 0);
            GL.Vertex3(1, 1, 0);

            GL.TexCoord3(1, 0, 0);
            GL.Vertex3(1, 0, 0);

            GL.End();
            GL.PopMatrix();
        }
    }
}
