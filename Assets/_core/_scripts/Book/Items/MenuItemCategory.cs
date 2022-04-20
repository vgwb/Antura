using Antura.Core;
using Antura.Language;
using Antura.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Book
{
    /// <summary>
    /// Displays a category button in the PlayerBook. Used to select a page of the book.
    /// </summary>
    public class MenuItemCategory : MonoBehaviour, IPointerClickHandler
    {
        GenericCategoryData categoryData;
        public TextRender Title;
        public TextRender Code;
        public TextRender SubTitle;
        IBookPanel myManager;

        UIButton uIButton;

        public void Init(IBookPanel _manager, GenericCategoryData _data, bool _selected)
        {
            uIButton = GetComponent<UIButton>();

            categoryData = _data;
            myManager = _manager;

            Title.text = categoryData.TitleLearning;
            Title.SetTextAlign(LanguageSwitcher.I.IsLearningLanguageRTL());
            SubTitle.text = AppManager.I.ContentEdition.LearnMethod.ShowHelpText ? categoryData.TitleHelp : "";

            if (categoryData.Stage > 0)
            {
                Code.text = categoryData.Stage.ToString();
            }
            else
            {
                Code.text = "";
            }

            hightlight(_selected);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            myManager.SelectSubCategory(categoryData);
        }

        public void Select(string code)
        {
            hightlight(code == categoryData.Id);
        }

        void hightlight(bool _status)
        {
            uIButton.Toggle(_status);
        }
    }
}
