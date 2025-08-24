using System;
using System.Collections.Generic;
using UnityEngine;
using Antura.Discover.Activities;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "ActivityListData", menuName = "Antura/Discover/Activity List")]
    public class ActivityListData : ScriptableObject
    {
        public List<ActivityData> Activities;
    }
}
