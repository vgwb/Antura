using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homer;

namespace Antura.Discover
{
    [CreateAssetMenu(menuName = "Antura/Quests")]
    public class Quests : ScriptableObject
    {
        public QuestData[] AvailableQuests;
    }
}
