using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homer;

namespace Antura.Minigames.DiscoverCountry
{

    [CreateAssetMenu(menuName = "Antura/Quest Data")]
    public class QuestData : ScriptableObject
    {
        public bool Active;
        public string Title;
        public HomerFlowSlugs.FlowSlug QuestId;

        public Texture Thumbnail;
        public string Description;

        [Header("Scene")]
        public string scene;

        [Header("Level Prefabs")]
        public GameObject Town;
        public GameObject GameLevel;

    }
}
