using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.UI
{
    public class ClassroomInfoPanel : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] CreditsUI credits;

        #endregion
        
        #region Unity

        void OnEnable()
        {
            credits.Show(true, true);
        }

        void Start()
        {
            credits.Show(true, true);
        }
        
        void OnDisable()
        {
            credits.Show(false);
        }

        #endregion
    }
}