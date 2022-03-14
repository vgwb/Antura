using Antura.Database;
using Antura.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Antura.Core;

namespace Antura.Debugging
{
    public class DebugMiniGameButton : MonoBehaviour, IPointerClickHandler
    {
        public TextRender Title;
        private DebugPanel manager;
        private MiniGameInfo minigameInfo;
        private bool played;

        public float difficulty;

        public void Init(DebugPanel _manager, MiniGameInfo _MiniGameInfo, bool _played, float _difficulty = 0.0f)
        {
            manager = _manager;
            minigameInfo = _MiniGameInfo;
            played = _played;
            difficulty = _difficulty;

            Title.text = _MiniGameInfo.data.Code.ToString().Replace(_MiniGameInfo.data.Main + "_", "");
            if (_difficulty > 0)
            {
                Title.text = "D: " + _difficulty;
            }

            ColorButton();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ColorButton();
            manager.LaunchMiniGame(minigameInfo.data.Code, difficulty);
        }

        void ColorButton()
        {
            // Debug.Log(Title.text + " " + played);
            var colors = GetComponent<Button>().colors;
            if (played)
            {
                colors.normalColor = Color.gray;
            }
            else
            {
                colors.normalColor = Color.white;
            }
            GetComponent<Button>().colors = colors;
        }
    }
}
