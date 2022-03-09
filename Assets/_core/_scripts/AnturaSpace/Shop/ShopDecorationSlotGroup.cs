using System;
using DG.DeExtensions;
using DG.DeInspektor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Antura.AnturaSpace
{
    public class ShopDecorationSlotGroup : MonoBehaviour
    {
        public ShopDecorationSlotType slotType;

        // @note: these are set and serialized by EditorSetup's calls in ShopDecorationManager
        [HideInInspector]
        public ShopDecorationSlot[] slots;

#if UNITY_EDITOR
        public void UpdateSlotIndexes()
        {
            slots = GetComponentsInChildren<ShopDecorationSlot>();
            int sequentialIndex = 0;
            foreach (var slot in slots)
            {
                slot.slotType = slotType;
                slot.slotIndex = sequentialIndex++;
                EditorUtility.SetDirty(slot);
                //Debug.LogError("SET SLOT INDEX: " + slot.slotIndex);
            }
        }
#endif

    }
}
