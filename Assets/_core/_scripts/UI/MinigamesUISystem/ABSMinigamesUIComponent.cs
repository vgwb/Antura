using UnityEngine;

namespace Antura.UI
{
    public class ABSMinigamesUIComponent : MonoBehaviour
    {
        public bool IsSetup { get; protected set; }

        public RectTransform RectTransform
        {
            get
            {
                if (rt == null)
                { rt = this.GetComponent<RectTransform>(); }
                return rt;
            }
        }

        RectTransform rt;

        #region Methods

        // Returns TRUE if the setup was correctly called, otherwise returns FALSE and dispatches a log warning
        protected bool Validate(string _caller)
        {
            if (IsSetup)
                return true;
            Debug.LogWarning(_caller + " ► you didn't Setup this (call the Setup method first)");
            return false;
        }

        #endregion
    }
}
