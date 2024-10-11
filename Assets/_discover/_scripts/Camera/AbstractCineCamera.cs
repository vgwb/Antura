using DG.DeInspektor.Attributes;
using Unity.Cinemachine;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public abstract class AbstractCineCamera : MonoBehaviour
    {
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] protected CinemachineCamera cineMain;

        #endregion

        public bool Active { get; private set; }
        public CinemachineCamera CineMain { get { return cineMain; } }

        #region Public Methods

        public virtual void Activate(bool activate)
        {
            Active = activate;
            cineMain.gameObject.SetActive(activate);
        }

        #endregion
    }
}
