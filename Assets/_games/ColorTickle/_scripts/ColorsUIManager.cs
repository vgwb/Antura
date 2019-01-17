using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.ColorTickle
{
    public class ColorsUIManager : MonoBehaviour
    {

        #region PUBLIC MEMBERS
        [SerializeField]
		private Button m_SampleButton;
		//[SerializeField]
		//private int m_NumberOfButtons = 4;
        [SerializeField]
        private float m_OutlineSize = 1.2f;
        [SerializeField]
        private Color m_OutlineColor = new Color(0, 0, 0, 255);

        [Header("Max Colors = Number of Buttons * Rounds")]
        public Color[] m_Colors;
        public event System.Action<Color> SetBrushColor;

        #endregion

        #region PRIVATE MEMBERS
        
        Button[] m_Buttons;
        Button m_OutlineButton;
        RectTransform m_OutlineTransform;
        int m_PreviousColor;
        int m_ColorNumber = 0;
        int m_NumberOfButtons = 4;
        int selectedButton = -1;
        #endregion

        #region GETTER/SETTERS

        public Color defaultColor
        {
            get { return m_Buttons[0].image.color; }
        }
        #endregion

        // Use this for initialization
        void Awake()
		{
            m_Buttons = new Button[m_NumberOfButtons];	        
            
            BuildButtons();
            BuildOutlineButton();
            SelectButton(0);
        }

        void Update()
        {
            if (selectedButton >= 0 && selectedButton < m_NumberOfButtons)
            {
                m_OutlineButton.transform.position = m_Buttons[selectedButton].transform.position;
                if (m_OutlineTransform == null)
                    m_OutlineTransform = m_OutlineButton.GetComponent<RectTransform>();

                m_OutlineTransform.sizeDelta = m_Buttons[selectedButton].GetComponent<RectTransform>().sizeDelta * m_OutlineSize;
            }
        }

        void BuildOutlineButton()
        {
            m_OutlineButton = Instantiate(m_SampleButton);

            m_OutlineButton.transform.SetParent(transform.parent);
            m_OutlineButton.transform.SetAsFirstSibling();
            Color newcolor = m_OutlineColor;
            m_OutlineButton.image.color = newcolor;
            m_OutlineButton.GetComponent<RectTransform>().localScale = Vector3.one;
        }

        void BuildButtons()
        {
            for (int i = 0; i < m_NumberOfButtons; ++i)
            {
                m_Buttons[i] = Instantiate(m_SampleButton);
                m_Buttons[i].transform.SetParent(gameObject.transform);
               
                m_Colors[i].a = 255.0f;
                m_Buttons[i].image.color = m_Colors[i];

                int buttonNumber = i;
                m_Buttons[i].onClick.AddListener(delegate {
                    ColorTickleConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Blip);
                    SelectButton(buttonNumber);
                });
                m_Buttons[i].GetComponent<RectTransform>().localScale = Vector3.one;
            }

            m_ColorNumber = m_NumberOfButtons - 1;
        }

        void SelectButton(int buttonNumber)
        {
            selectedButton = buttonNumber;

            if (SetBrushColor != null)
            {
                SetBrushColor(m_Buttons[buttonNumber].image.color);
            }
        }

        public void ChangeButtonsColor()
        {
            for (int i = 0; i < m_NumberOfButtons; ++i)
            {
                m_ColorNumber++;
                if (m_ColorNumber >= m_Colors.Length)
                {
                    m_ColorNumber = 0;
                }
                m_Colors[m_ColorNumber].a = 255.0f;
                m_Buttons[i].image.color = m_Colors[m_ColorNumber];
            }
            SelectButton(0);
        }
    }
}