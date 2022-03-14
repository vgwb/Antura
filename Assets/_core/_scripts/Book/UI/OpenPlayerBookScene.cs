using Antura.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Book
{
    /// <summary>
    /// Button that allows access to the PlayerBook.
    /// </summary>
    public class OpenPlayerBookScene : MonoBehaviour, IPointerClickHandler
    {
        public BookArea bookArea;

        public void OnPointerClick(PointerEventData eventData)
        {
            BookManager.I.OpenBook(bookArea);
            //AppManager.I.NavigationManager.GoToPlayerBook(bookArea);
        }
    }
}
