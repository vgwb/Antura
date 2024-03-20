using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homer;

namespace Antura.Minigames.DiscoverCountry
{
    [CreateAssetMenu(menuName = "Antura/Quest Data")]
    public class QuestData : ScriptableObject
    {
        public string Title;
        public HomerFlowSlugs.FlowSlug QuestId;

    }
}
