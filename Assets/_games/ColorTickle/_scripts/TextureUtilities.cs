using UnityEngine;

namespace Antura.Minigames.ColorTickle
{
    public class TextureUtilities
    {

        /// <summary>
        /// Returns the resulting pixels of the given texture after scaling.
        /// Note: pixels processed sequentially, potentially heavy.
        /// </summary>
        /// <param name="source">The original texture to scale</param>
        /// <param name="iTargetWidth">The wanted size for the width</param>
        /// <param name="iTargetHeight">The wanted size for the height</param>
        /// <returns>Resulting pixels</returns>
        static public Color[] ScaleTexture(Texture2D source, int iTargetWidth, int iTargetHeight)
        {
            float _fXSampleUnit = 1f / iTargetWidth;
            float _fYSampleUnit = 1f / iTargetHeight;

            Color[] resultMatrix = new Color[iTargetWidth * iTargetHeight];

            //Fill the color by extracting the bilinear value in the original texture for each sample
            for (int j = 0; j < iTargetHeight; ++j)
            {
                for (int i = 0; i < iTargetWidth; ++i)
                {
                    resultMatrix[i + j * iTargetWidth] = source.GetPixelBilinear(_fXSampleUnit * i, _fYSampleUnit * j);
                }
            }

            return resultMatrix;
        }

        /// <summary>
        /// Returns the resulting pixels of the given texture after filling all of them with the same color.
        /// Note: pixels processed sequentially, potentially heavy.
        /// </summary>
        /// <param name="source">The original texture to fill</param>
        /// <param name="newValue">The color to apply</param>
        /// <returns>Resulting pixels</returns>
        static public Color[] FillTextureWithColor(Texture2D source, Color newValue)
        {
            Color[] resultMatrix = new Color[source.width * source.height];

            for (int idx = 0; idx < resultMatrix.Length; ++idx)
            {
                resultMatrix[idx] = newValue;
            }

            return resultMatrix;
        }

        /// <summary>
        /// Given the uvs coordinates of a point relative to a rectangle, calculates the resulting
        /// uvs for a container of that rectangle.
        /// </summary>
        /// <param name="v2CoordinatesOfInnerPoint">Inner point uvs</param>
        /// <param name="v2BottomLeftCornerOfInnerRect">Bottom left corner uvs of inner rectangle relative to the container</param>
        /// <param name="fHeightOfect">Inner rectangle height or distance between bottom-top corners</param>
        /// <param name="fWidthOfRect">Inner rectangle whidth or distance between left-right corners</param>
        /// <returns>UVs coordinates of inner point relative to the container</returns>
        static public Vector2 CombineSubUV(Vector2 v2CoordinatesOfInnerPoint, Vector2 v2BottomLeftCornerOfInnerRect, float fHeightOfect, float fWidthOfRect)
        {
            Vector2 finalOuterUV = Vector2.zero;
            finalOuterUV.x = v2BottomLeftCornerOfInnerRect.x + fWidthOfRect * (v2CoordinatesOfInnerPoint.x);
            finalOuterUV.y = v2BottomLeftCornerOfInnerRect.y + fHeightOfect * (v2CoordinatesOfInnerPoint.y);
            return finalOuterUV;
        }
    }

}
