using Antura.UI;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    /// <summary>
    /// Displays a Toast message on the screen.
    /// </summary>
    public class ToastDisplay : MonoBehaviour
    {
        #region Serialized


        #endregion

        public bool IsOpen { get; private set; }
        bool initialized;

        #region Unity + INIT

        void Init()
        {
            if (initialized)
                return;

            initialized = true;

        }

        void Awake()
        {
            Init();
            if (!IsOpen)
                this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {

        }

        #endregion

        #region Public Methods
        public void Show(string Message)
        {
            Debug.Log("ToastDisplay.Show: " + Message);
        }

        #endregion
    }
}
