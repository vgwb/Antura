using System.Collections.Generic;
using UnityEngine;

namespace Antura.AnturaSpace
{
    [System.Serializable]
    public class ShopSlotState
    {
        public ShopDecorationSlotType slotType;
        public int slotIndex;
        public string decorationID;

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public bool MatchesSlot(ShopDecorationSlot slot)
        {
            return slotType == slot.slotType && slotIndex == slot.slotIndex;
        }

        public override string ToString()
        {
            return slotType + "-" + slotIndex + " with decoration " + decorationID;
        }
    }

    [System.Serializable]
    public class ShopState
    {
        public ShopSlotState[] occupiedSlots;

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public static ShopState CreateFromJson(string jsonData)
        {
            var shopState = JsonUtility.FromJson<ShopState>(jsonData);
            if (shopState == null)
                shopState = new ShopState();
            return shopState;
        }

        public override string ToString()
        {
            string s = "";
            if (occupiedSlots != null)
            {
                foreach (var slotState in occupiedSlots)
                {
                    s += "- slot " + slotState + "\n";
                }
            }
            return s;
        }

    }
}
