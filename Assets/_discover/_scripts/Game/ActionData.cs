using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public enum ActionType
    {
        UnityAction = 1,
        Area = 2,
    }

    [System.Serializable]
    public class ActionData
    {
        public ActionType Type;
        public string ActionCode;
        public GameObject mainObject;
        public GameObject Area;
        public GameObject Beam;
        public GameObject Target;
    }
}
