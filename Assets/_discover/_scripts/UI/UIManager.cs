using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class UIManager : MonoBehaviour
    {
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
            Canvas canvas = dialogues.GetComponentInParent<Canvas>(true);
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