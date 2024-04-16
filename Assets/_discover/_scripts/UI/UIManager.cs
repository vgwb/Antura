using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class UIManager : MonoBehaviour
    {
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] Canvas canvas;

        #endregion
        
        public static UIManager I { get; private set; }
        public DialoguesUI dialogues { get; private set; }

        #region Unity

        void Awake()
        {
            if (I != null)
            {
                Debug.LogError("UIManager already exists, deleting duplicate");
                Destroy(this);
                return;
            }

            I = this;
            dialogues = this.GetComponentInChildren<DialoguesUI>(true);
            canvas.gameObject.SetActive(true);
            dialogues.gameObject.SetActive(true);
        }

        void OnDestroy()
        {
            if (I == this) I = null;
        }

        #endregion
    }
}