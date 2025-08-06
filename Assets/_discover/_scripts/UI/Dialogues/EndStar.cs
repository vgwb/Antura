using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Discover
{
    public class EndStar : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] GameObject icoOn;

        #endregion

        #region Public Methods

        public void TurnOn(bool on)
        {
            icoOn.SetActive(on);
        }

        #endregion
    }
}