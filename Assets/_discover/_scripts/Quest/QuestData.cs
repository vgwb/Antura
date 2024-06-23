using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homer;

namespace Antura.Minigames.DiscoverCountry
{
    public enum QuestCategory
    {
        Place,
        Character,
        NaturalSite,
        Food,
        Art,
        CultureElement
    }

    [CreateAssetMenu(menuName = "Antura/Quest Data")]
    public class QuestData : ScriptableObject
    {
        public bool Active;
        public string Title;
        public HomerFlowSlugs.FlowSlug QuestId;

        public Texture Thumbnail;
        public string Description;

        public QuestCategory[] Categories;

        [Header("Level Prefabs")]
        public GameObject Town;
        public GameObject GameLevel;

    }
}
