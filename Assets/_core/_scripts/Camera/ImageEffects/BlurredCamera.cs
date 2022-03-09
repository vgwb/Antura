using UnityEngine;

namespace Antura.CameraEffects
{
    public class BlurredCamera : PostEffectsBase
    {
        public RenderTexture normalTextureOutput;

        [Range(1, 4)]
        public int blurIterations = 1;

        public int textureSize = 512;

        [Range(0.0f, 10.0f)]
        public float blurSize = 3.0f;

        public Shader blurShader = null;
        private Material blurMaterial = null;

        protected override bool CheckResources()
        {
            CheckSupport(false);

            blurMaterial = CheckShaderAndCreateMaterial(blurShader, blurMaterial);

            if (!isSupported)
            {
                ReportAutoDisable();
            }
            return isSupported;
        }

        void OnDisable()
        {
            if (blurMaterial)
            {
                DestroyImmediate(blurMaterial);
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (normalTextureOutput != null)
            {
                normalTextureOutput.DiscardContents();
                Graphics.Blit(source, normalTextureOutput);
            }
            if (CheckResources() == false)
            {
                Graphics.Blit(source, destination);
                return;
            }

            int rtW = textureSize;
            int rtH = textureSize >> 1;

            blurMaterial.SetVector("_Parameter", new Vector4(blurSize, -blurSize, 0.0f, 0.0f));
            source.filterMode = FilterMode.Bilinear;

            // downsample
            RenderTexture rt = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);

            rt.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, rt, blurMaterial, 0);

            for (int i = 0; i < blurIterations; i++)
            {
                float iterationOffs = (i * 1.0f);
                blurMaterial.SetVector("_Parameter",
                    new Vector4(blurSize + iterationOffs, -blurSize - iterationOffs, 0.0f, 0.0f));

                // vertical blur
                RenderTexture rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
                rt2.filterMode = FilterMode.Bilinear;
                Graphics.Blit(rt, rt2, blurMaterial, 1);
                RenderTexture.ReleaseTemporary(rt);
                rt = rt2;

                // horizontal blur
                rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
                rt2.filterMode = FilterMode.Bilinear;
                Graphics.Blit(rt, rt2, blurMaterial, 2);
                RenderTexture.ReleaseTemporary(rt);
                rt = rt2;
            }

            Graphics.Blit(rt, destination);

            RenderTexture.ReleaseTemporary(rt);
        }
    }
}
