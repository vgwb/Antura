using System;
using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class TextInputPopup : AbstractGlobalPopup
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] TMP_InputField textInput;
        [DeEmptyAlert]
        [SerializeField] Button btOk, btCancel;

        #endregion

        Action<string> onSubmit;

        #region Unity

        protected override void Awake()
        {
            base.Awake();
            
            btCancel.onClick.AddListener(Close);
            btOk.onClick.AddListener(OnSubmit);
            textInput.onSubmit.AddListener(x => OnSubmit());
        }

        #endregion

        #region Public Methods

        public void Open(string title, string existingText, Action<string> onSubmitted)
        {
            BaseClear();

            textInput.text = existingText;
            onSubmit = onSubmitted;
            this.gameObject.SetActive(true);

            BaseOpen(title);
        }

        #endregion

        #region Callbacks

        void OnSubmit()
        {
            Close();
            if (onSubmit != null) onSubmit(textInput.text);
        }

        #endregion
    }
}