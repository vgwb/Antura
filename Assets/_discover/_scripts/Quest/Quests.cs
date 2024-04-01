using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homer;

namespace Antura.Minigames.DiscoverCountry
{
    [CreateAssetMenu(menuName = "Antura/Quests")]
    public class Quests : ScriptableObject
    {
        public QuestData[] AvailableQuests;
    }
}
