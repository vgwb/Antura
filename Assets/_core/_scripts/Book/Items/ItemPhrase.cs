using Antura.Database;
using Antura.UI;
using Antura.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Book
{
    /// <summary>
    /// Displays a Phrase item in the Dictionary page of the Player Book.
    /// </summary>
    public class ItemPhrase : MonoBehaviour, IPointerClickHandler
    {
        PhraseInfo info;
        public TextRender Title;
        public TextRender SubTitle;
        public Image LockIcon;

        PhrasesPage manager;

        public void Init(PhrasesPage _manager, PhraseInfo _info)
        {
            info = _info;
            manager = _manager;

            if (info.unlocked || AppManager.I.Player.IsDemoUser)
            {
                LockIcon.enabled = false;
            }
            else
            {
                LockIcon.enabled = true;
            }

            Title.text = info.data.Text;
            SubTitle.text = AppManager.I.ContentEdition.LearnMethod.ShowHelpText ? info.data.Id : "";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            manager.DetailPhrase(info);
        }
    }
}
