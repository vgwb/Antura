using Antura.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Map
{
    public class MapStageIndicatorIcon : MonoBehaviour
    {
        public StageMapsManager stageMapsManager;

        public Color SelectedColor = Color.white;
        public Image ColorizedImage;
        public TextRender text;

        bool initialized;
        Color defColor;

        private int _assignedStage;
        public int AssignedStage
        {
            get { return _assignedStage; }
            set
            {
                _assignedStage = value;
                text.SetTextUnfiltered(_assignedStage.ToString());
            }
        }

        public void Select(bool doSelect)
        {
            if (!initialized)
            {
                initialized = true;
                defColor = ColorizedImage.color;
            }

            ColorizedImage.color = doSelect ? SelectedColor : defColor;
        }

        public void OnClick()
        {
            stageMapsManager.MoveToStageMap(AssignedStage, animateCamera: true);
        }
    }
}