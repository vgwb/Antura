using Antura.Core;
using Antura.UI;
using UnityEngine;

namespace Antura.Book
{
    public class TableRow : MonoBehaviour
    {
        public TextRender TxTitle;
        public TextRender TxTitleEn;
        public TextRender TxValue;
        public CompletionSlider slider;

        public void Init(string _titleEn, string _title, string _value)
        {
            if (TxTitle != null)
            { TxTitle.text = _title; }

            if (TxTitleEn != null)
            {
                TxTitleEn.gameObject.SetActive(AppManager.I.ContentEdition.LearnMethod.ShowHelpText);
                TxTitleEn.text = _titleEn;
            }

            TxValue.SetText(_value);
        }

        public void InitSlider(string _titleEn, string _title, float _value, float _valueMax)
        {
            if (TxTitle != null)
            { TxTitle.text = _title; }

            if (TxTitleEn != null)
            {
                TxTitleEn.gameObject.SetActive(AppManager.I.ContentEdition.LearnMethod.ShowHelpText);
                TxTitleEn.text = _titleEn;
            }
            slider.SetValue(_value, _valueMax);
        }
    }
}
