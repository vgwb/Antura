using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class TextChoiceBox : AbstractChoiceBox
    {
        #region Serialized

        [Header("References - Specific")]
        [DeEmptyAlert]
        [SerializeField] TMP_Text tf;

        #endregion
    }
}