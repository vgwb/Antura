using Antura.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Book
{
    public class BookManager : MonoBehaviour
    {
        public static BookManager I;
        GameObject BookInstance;

        const string RESOURCES_BOOK = "Prefabs/Book/Book";

        void Awake()
        {
            I = this;
        }

        public void OpenBook(BookArea area)
        {
            // TODO maybe first check if Book is already isntatiated!
            BookInstance = Instantiate(Resources.Load(RESOURCES_BOOK, typeof(GameObject))) as GameObject;
            AppManager.I.ModalWindowActivated = true;
            Book.I.OpenArea(area);
        }

        public void CloseBook()
        {
            BookInstance.SetActive(false);
            Destroy(BookInstance);
            AppManager.I.ModalWindowActivated = false;
        }

    }
}