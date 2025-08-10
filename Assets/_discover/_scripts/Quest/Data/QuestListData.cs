using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(menuName = "Antura/Discover/Quests List")]
    public class QuestListData : ScriptableObject
    {
        public QuestData[] QuestList;
    }
}
