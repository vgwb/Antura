using System;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class Navigator : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] GameObject indicatorL, indicatorR, indicatorT, indicatorB;

        #endregion

        #region Unity

        void Awake()
        {
            HideAll();
        }

        #endregion

        #region Public Methods

        public void SetIndicators(OutOfBoundsHor outHor = OutOfBoundsHor.None, OutOfBoundsVert outVert = OutOfBoundsVert.None)
        {
            HideAll();
            if (outHor != OutOfBoundsHor.None)
            {
                if (outHor == OutOfBoundsHor.Left) indicatorL.SetActive(true);
                else indicatorR.SetActive(true);
            }
            else if (outVert != OutOfBoundsVert.None)
            {
                if (outVert == OutOfBoundsVert.Top) indicatorT.SetActive(true);
                else indicatorB.SetActive(true);
            }
        }

        #endregion

        #region Methods

        void HideAll()
        {
            indicatorL.SetActive(false);
            indicatorR.SetActive(false);
            indicatorT.SetActive(false);
            indicatorB.SetActive(false);
        }

        #endregion
    }
}