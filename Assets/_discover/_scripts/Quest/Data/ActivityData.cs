using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "ActivityData", menuName = "Antura/Discover/Activity")]
    public class ActivityData : ScriptableObject
    {
        public LocalizedString Name;

        [Header("Media")]
        public Sprite Image;

        [Header("Credits")]
        public List<AuthorData> CreditsDesign;
        public List<AuthorData> CreditsDevelopment;
    }
}
