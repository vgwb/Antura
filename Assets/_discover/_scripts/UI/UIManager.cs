using UnityEngine;

namespace Antura.Minigames.DiscoverCountry.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager I { get; private set; }

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
        }

        void OnDestroy()
        {
            if (I == this) I = null;
        }

        #endregion
    }
}