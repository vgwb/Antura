using UnityEngine;
using System;
using Antura.UI;

namespace Antura.Minigames.ColorTickle
{
    public class SurfaceColoring : MonoBehaviour
    {

        #region EXPOSED MEMBERS
#pragma warning disable 649
        [SerializeField]
        private ColoringParameters m_oBrush; //The brush used to color
        [SerializeField]
        private MeshCollider m_oBody; //The touchable collider
        [SerializeField]
        private SkinnedMeshRenderer m_oSurfaceRenderer; //The face that will render the body texture
        [SerializeField]
        private int m_iPixelPerUnit = 5; //The number of pixels to fit in 1 unit
        [SerializeField]
        private Color m_oBaseColor = Color.white; //The base color for the texture to color
        [SerializeField]
        private bool m_bEnableColor = true; //Flag used to enable the coloring functions
#pragma warning restore 649
        #endregion

        #region PRIVATE MEMBERS
        private Texture2D m_tBodyTexture; //Generated texture to color on the surface
        private RaycastHit m_oRayHit; //Store the data on the last collision
        #endregion

        #region EVENTS
        public event Action<bool> OnBodyHit; //event launched upon touching the face/letter
        #endregion

        #region GETTER/SETTER
        public ColoringParameters brush
        {
            get { return m_oBrush; }
            set { m_oBrush = value; }
        }

        public int pixelPerUnit
        {
            get { return m_iPixelPerUnit; }
            set { m_iPixelPerUnit = value; }
        }

        public bool enableColor
        {
            get { return m_bEnableColor; }
            set { m_bEnableColor = value; }
        }
        #endregion

        #region INTERNALS
        void Start()
        {
            SetupBodyColorTexture(); //prepare the texture of the body
        }

        void Update()
        {
            if (GlobalUI.I.IsFingerOverUI()) return;
            if (Input.GetMouseButton(0) && !PauseMenu.I.IsMenuOpen) //On touch
            {

                Ray _mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); //Ray with direction camera->screenpoint

                //Debug.DrawRay(_mouseRay.origin, _mouseRay.direction * 100, Color.yellow, 10);

                //check for ray collision
                if (m_oBody.Raycast(_mouseRay, out m_oRayHit, Mathf.Infinity))
                {
                    if (OnBodyHit != null)
                    {
                        OnBodyHit(true);
                    }
                    ColorBodyTexturePoint(m_oRayHit.textureCoord);
                }
                else
                {
                    if (OnBodyHit != null)
                    {
                        OnBodyHit(false);
                    }
                }
            }
            else
            {
                if (OnBodyHit != null)
                {
                    OnBodyHit(false);
                }
            }
        }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Reset the component by reinitializing it.
        /// </summary>
        public void Reset()
        {
            SetupBodyColorTexture(); //prepare the texture of the body
        }
        #endregion

        #region PRIVATE FUNCTIONS
        /// <summary>
        /// Builds and link the texture used to color the body.
        /// </summary>
        private void SetupBodyColorTexture()
        {
            //Generate the body texture to color using the face actual size
            m_tBodyTexture = new Texture2D(
                Mathf.FloorToInt(m_oBody.sharedMesh.bounds.size.x * m_iPixelPerUnit),
                Mathf.FloorToInt(m_oBody.sharedMesh.bounds.size.z * m_iPixelPerUnit),
                TextureFormat.ARGB32,
                false);
            m_tBodyTexture.SetPixels(TextureUtilities.FillTextureWithColor(m_tBodyTexture, m_oBaseColor)); //initialiaze it to white
            m_tBodyTexture.Apply();

            //link the body texture as the material's main texture
            m_oSurfaceRenderer.material.SetTexture("_MainTex", m_tBodyTexture);
        }

        /// <summary>
        /// Just like the letter case, but we don't need to check for completation of the shape.
        /// </summary>
        /// <param name="targetUV">Where the brush must be painted</param>
        private void ColorBodyTexturePoint(Vector2 targetUV)
        {

            if (!m_bEnableColor)
            {
                return;
            }

            //Before color with the brush's texture we need to verify that it fits
            //in the target's bound and eventually split it

            //store pixel touched in the texture
            int _iXPixelTouched = Mathf.FloorToInt(targetUV.x * m_tBodyTexture.width);
            int _iYPixelTouched = Mathf.FloorToInt(targetUV.y * m_tBodyTexture.height);

            //extract pixels coordinates of brush limits and clamp to fit the texture bounds
            int _iLBrushBound = Mathf.Clamp(_iXPixelTouched - m_oBrush.brushShapeScaled.width / 2, 0, m_tBodyTexture.width);
            int _iRBrushBound = Mathf.Clamp(_iXPixelTouched + m_oBrush.brushShapeScaled.width / 2, 0, m_tBodyTexture.width);
            int _iBBrushBound = Mathf.Clamp(_iYPixelTouched - m_oBrush.brushShapeScaled.height / 2, 0, m_tBodyTexture.height);
            int _iTBrushBound = Mathf.Clamp(_iYPixelTouched + m_oBrush.brushShapeScaled.height / 2, 0, m_tBodyTexture.height);

            //brush's dimensions in pixels for this frame
            int _iBrushWidth = _iRBrushBound - _iLBrushBound;
            int _iBrushHeight = _iTBrushBound - _iBBrushBound;

            //offset for the new center of the brush from the base
            int _iXCenterOffset = -(_iXPixelTouched - (_iRBrushBound - _iBrushWidth / 2));
            int _iYCenterOffset = -(_iYPixelTouched - (_iTBrushBound - _iBrushHeight / 2));

            //Retrive the current array of pixels from the texture to color fitting the brush shape and position
            Color[] _aColors_CurrentDynamicTex = m_tBodyTexture.GetPixels(_iLBrushBound, _iBBrushBound, _iBrushWidth, _iBrushHeight);

            //Retrieve the current array of pixels from the clamped brush's shape
            Color[] _aColors_BrushClampedShape = m_oBrush.brushShapeScaled.GetPixels(
                Mathf.Clamp(m_oBrush.brushShapeScaled.width / 2 + _iXCenterOffset - _iBrushWidth / 2, 0, m_oBrush.brushShapeScaled.width - _iBrushWidth), //for rounding errors this is clamped to fit the bounds
                Mathf.Clamp(m_oBrush.brushShapeScaled.height / 2 + _iYCenterOffset - _iBrushHeight / 2, 0, m_oBrush.brushShapeScaled.height - _iBrushHeight), // --
                _iBrushWidth,
                _iBrushHeight);

            //Note that these operations can become quite heavy; we are operating on pixels using the cpu, essentially writing a pixel-shader
            //Adjust the parameters of pixels per unit, brush scale and texture dimensions to process a reasonable number of pixels
            //Alternatively delegate this to a coroutine or split the calculations on more frames

            float _fAlphaOver = 0f;
            float _fAlphaBack = 0f;

            for (int idx = 0; idx < _aColors_BrushClampedShape.Length; ++idx)
            {
                //Blend brush color with texture
                _fAlphaOver = _aColors_BrushClampedShape[idx].a;
                _fAlphaBack = _aColors_CurrentDynamicTex[idx].a;
                _aColors_BrushClampedShape[idx] = _aColors_BrushClampedShape[idx] * _fAlphaOver + _aColors_CurrentDynamicTex[idx] * _fAlphaBack * (1 - _fAlphaOver);
                _aColors_BrushClampedShape[idx].a = _fAlphaOver + _fAlphaBack * (1 - _fAlphaOver);
            }

            //finally color the texture with the resulting matrix
            m_tBodyTexture.SetPixels(_iLBrushBound, _iBBrushBound, _iBrushWidth, _iBrushHeight, _aColors_BrushClampedShape);
            m_tBodyTexture.Apply();

        }
        #endregion

    }
}
