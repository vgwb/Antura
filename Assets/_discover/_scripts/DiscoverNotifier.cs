﻿using System;
using System.Reflection;
using System.Text;
using Demigiant.DemiTools;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public static class DiscoverNotifier
    {
        public static class Game
        {
            /// <summary>When a Living Letter's collider OnTriggerEnter is entered</summary>
            public static readonly ActionEvent<EdLivingLetter> OnLivingLetterTriggerEnter = new("DiscoverNotifier.Game.OnLivingLetterTriggerEnter");
            /// <summary>When a Living Letter's collider OnTriggerEnter is exited</summary>
            public static readonly ActionEvent<EdLivingLetter> OnLivingLetterTriggerExit = new("DiscoverNotifier.Game.OnLivingLetterTriggerExit");
        }
        
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ UTILITY METHODS █████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        static readonly StringBuilder strb = new();
        
        /// <summary>Unsubscribes all events (using Reflection)</summary>
        public static void UnsubscribeAll()
        {
            Type actionEventBaseType = typeof(ActionEventBase);
            Type[] nestedTypes = typeof(DiscoverNotifier).GetNestedTypes(BindingFlags.Public | BindingFlags.Static);
            foreach (Type type in nestedTypes)
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (FieldInfo field in fields)
                {
                    if (!field.FieldType.IsSubclassOf(actionEventBaseType)) continue;
                    MethodInfo mInfo = field.FieldType.GetMethod("UnsubscribeAll", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (mInfo == null) continue;
                    mInfo.Invoke(field.GetValue(null), null);
                }
            }
        }

        /// <summary>Logs a list of all events that have active subscriptions (using Reflection)</summary>
        public static void LogAllSubscribed()
        {
            strb.Append("All Subscribed ------------------");
            Type actionEventBaseType = typeof(ActionEventBase);
            PropertyInfo pName = actionEventBaseType.GetProperty("name", BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo pTotSubscribed = actionEventBaseType.GetProperty("totSubscribed", BindingFlags.Public | BindingFlags.Instance);
            Type[] nestedTypes = typeof(DiscoverNotifier).GetNestedTypes(BindingFlags.Public | BindingFlags.Static);
            foreach (Type type in nestedTypes)
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (FieldInfo field in fields)
                {
                    if (!field.FieldType.IsSubclassOf(actionEventBaseType)) continue;
                    ActionEventBase actionEvent = (ActionEventBase)field.GetValue(null);
                    int subscribed = (int)pTotSubscribed.GetValue(actionEvent);
                    if (subscribed <= 0) continue;
                    strb.Append('\n').Append(pName.GetValue(actionEvent)).Append(" - tot subscribed: ").Append(subscribed);
                }
            }
            strb.Append("\n---------------------------------");
            Debug.Log(strb.ToString());
            strb.Length = 0;
        }
    }
}