using System.Reflection;
using Antura.Core;
using Demigiant.DemiTools.DeUnityExtended;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace Antura.UI
{
    public class ButtonExtended : DeUIButton
    {
        public enum ButtonType
        {
            Normal,
            Toggle,
            OpenURL
        }
        
        #region Serialized

        [SerializeField] ButtonType type = ButtonType.Normal;
        [SerializeField] string url;

        #endregion

        #region Unity

        protected override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying) return;

            if (type == ButtonType.OpenURL)
            {
                this.onClick.AddListener(OpenURL);
            }
        }

        #endregion

        #region Methods

        void OpenURL()
        {
            if (url.StartsWith("AppConfig."))
            {
                // Retrieve url from AppConfig const
                string value = url.Substring(url.IndexOf('.') + 1);
                FieldInfo fullUrl = typeof(AppConfig).GetField(value, BindingFlags.Static | BindingFlags.Public);
                if (fullUrl == null) Debug.LogWarning($"Couldn't find URL constant at AppConfig.{value}");
                else Application.OpenURL((string)fullUrl.GetValue(null));
            }
            else
            {
                Application.OpenURL(url);
            }
        }

        #endregion
    }
}