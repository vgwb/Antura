using UnityEngine;

namespace Antura.Minigames.ColorTickle
{
    public class ColoringParameters : MonoBehaviour
    {

        #region EXPOSED MEMBERS
        [Header("Brush")]
        [SerializeField]
        private string m_szBrushName; //Name of the brush, used just to distinguish multiple brushes
        [SerializeField]
        private Texture2D m_tBrushShape; //The texture defining the brush shape (alpha>0 are the shape)
        [SerializeField]
        private float m_fBrushScaling = 1; //The scaling of the brush texture
        [SerializeField]
        private Color m_oDrawingColor = Color.red; //The color used by the brush
        #endregion

        #region PRIVATE MEMBERS
        private Texture2D m_tScaledBrush; //Generated texture of the scaled brush
        #endregion

        #region GETTER/SETTER
        public string brushName
        {
            get { return m_szBrushName; }
            set { m_szBrushName = value; }
        }

        public Texture2D brushShape
        {
            get { return m_tBrushShape; }
            set { m_tBrushShape = value; }
        }

        public Texture2D brushShapeScaled
        {
            get { return m_tScaledBrush; }
        }

        public float brushScaling
        {
            get { return m_fBrushScaling; }
            set { m_fBrushScaling = value; }
        }

        public Color drawingColor
        {
            get { return m_oDrawingColor; }
        }
        #endregion

        #region INTERNALS
        void Start()
        {
            BuildBrush();
        }
        #endregion

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Sets the brush's texture color to the given one.
        /// Alpha is not modified to preserve the brush shape.
        /// </summary>
        /// <param name="newValue">The new color for the brush</param>
        public void SetBrushColor(Color newValue)
        {
            m_oDrawingColor = newValue; //Keep consistency
            Color[] _aColorMatrix = m_tScaledBrush.GetPixels();

            Color _alphaKeeper = newValue;

            //Reassign the pixels color but keep the alpha value to preserve the shape
            for (int idx = 0; idx < _aColorMatrix.Length; ++idx)
            {
                _alphaKeeper.a = _aColorMatrix[idx].a;
                _aColorMatrix[idx] = _alphaKeeper;
            }

            m_tScaledBrush.SetPixels(_aColorMatrix);
            m_tScaledBrush.Apply();
        }

        /// <summary>
        /// Builds the brush to color with by using the stored texture, scaling and color.
        /// Must be called to apply any changes on the brush.
        /// </summary>
        public void BuildBrush()
        {

            if (!m_tBrushShape) //if not given, use a default square
            {
                m_tBrushShape = Texture2D.whiteTexture;
            }

            //Generate the scaled texture
            m_tScaledBrush = new Texture2D(
                Mathf.FloorToInt(m_tBrushShape.width * m_fBrushScaling),
                Mathf.FloorToInt(m_tBrushShape.height * m_fBrushScaling),
                TextureFormat.ARGB32,
                false);

            m_tScaledBrush.SetPixels(TextureUtilities.ScaleTexture(m_tBrushShape, m_tScaledBrush.width, m_tScaledBrush.height));

            m_tScaledBrush.Apply();

            SetBrushColor(m_oDrawingColor);

        }
        #endregion
    }
}
