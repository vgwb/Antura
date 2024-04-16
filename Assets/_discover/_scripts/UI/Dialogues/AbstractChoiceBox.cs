using System.Globalization;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public abstract class AbstractChoiceBox : MonoBehaviour
    {
        #region EVENTS

        public readonly ActionEvent<int> OnConfirm = new("ChoiceBox.OnConfirm");

        #endregion

        #region Serialized

        [SerializeField] Color selectedNumberColor = Color.green;
        [SerializeField] protected float selectedScale = 1.2f;
        [SerializeField] protected float selectedShift = 0; // Used by children
        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] RectTransform numbox;
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfNumber;
        [DeEmptyAlert]
        [SerializeField] RectTransform bg, box;

        #endregion
        

        #region Unity

        void Awake()
        {
            
        }

        #endregion

        #region Public Methods

        public void SetNumber(int number)
        {
            tfNumber.text = number.ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}