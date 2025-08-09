using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "ActivityDefinition", menuName = "Antura/Discover/Activity Definition")]
    public class ActivityDefinition : ScriptableObject
    {
        public LocalizedString Name;

        [Header("Media")]
        public Sprite Image;

        [Header("Credits")]
        public List<AuthorDefinition> CreditsDesign;
        public List<AuthorDefinition> CreditsDevelopment;
    }
}
