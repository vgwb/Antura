using System;
using System.Linq;
using Antura.AnturaSpace;
using Antura.Core;
using UnityEngine;

namespace Antura.Dog
{
    [Serializable]
    public class CustomizationShopState
    {
        [Serializable]
        public class CustomizationShopItemState
        {
            public string SharedID;
            public bool Bought;
        }

        public CustomizationShopItemState[] CustomizationShopStates = new CustomizationShopItemState[0];

        public void ConfirmPurchase(string sharedID)
        {
            var list = CustomizationShopStates.ToList();
            list.RemoveAll(x => x.SharedID == sharedID);
            list.Add(new CustomizationShopItemState{Bought = true, SharedID = sharedID});
            CustomizationShopStates = list.ToArray();
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public static CustomizationShopState CreateFromJson(string jsonData)
        {
            var state = JsonUtility.FromJson<CustomizationShopState>(jsonData);
            if (state == null)
                state = new CustomizationShopState();
            return state;
        }
    }
}
