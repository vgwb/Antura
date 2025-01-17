﻿using System;
using System.Reflection;
using System.Text;
using Antura.Homer;
using Demigiant.DemiTools;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public static class DiscoverNotifier
    {
        public static class Game
        {
            /// <summary>Dispatched when the a dialogue starts, before its UI even appears</summary>
            public static readonly ActionEvent OnStartDialogue = new("DialoguesUI.OnStartDialogue");
            /// <summary>Dispatched when the dialogue UI closes</summary>
            public static readonly ActionEvent OnCloseDialogue = new("DialoguesUI.OnCloseDialogue");
            /// <summary>Dispatched when a main dialogue balloon opens</summary>
            public static readonly ActionEvent<QuestNode> OnShowDialogueBalloon = new("DialoguesUI.OnShowDialogueBalloon");
            /// <summary>Dispatched when a main dialogue balloon closes</summary>
            public static readonly ActionEvent<QuestNode> OnCloseDialogueBalloon = new("DialoguesUI.OnCloseDialogueBalloon");
            /// <summary>When an Agent's collider OnTriggerEnter is entered by the player</summary>
            public static readonly ActionEvent<EdAgent> OnAgentTriggerEnteredByPlayer = new("DiscoverNotifier.Game.OnAgentTriggerEnteredByPlayer");
            /// <summary>When an Agent's collider OnTriggerEnter is exited by the player</summary>
            public static readonly ActionEvent<EdAgent> OnAgentTriggerExitedByPlayer = new("DiscoverNotifier.Game.OnAgentTriggerExitedByPlayer");
            /// <summary>When an infoPoint's collider OnTriggerEnter is entered by the player</summary>
            public static readonly ActionEvent<InfoPoint, string, string> OnInfoPointTriggerEnteredByPlayer = new("DiscoverNotifier.Game.OnInfoPointTriggerEnteredByPlayer");
            /// <summary>When an infoPoint's collider OnTriggerEnter is exited by the player</summary>
            public static readonly ActionEvent<InfoPoint> OnInfoPointTriggerExitedByPlayer = new("DiscoverNotifier.Game.OnInfoPointTriggerExitedByPlayer");
            /// <summary>When the action button is pressed</summary>
            public static readonly ActionEvent OnActClicked = new("DiscoverNotifier.Game.OnActClicked");
            /// <summary>When the map button is toggled on or off</summary>
            public static readonly ActionEvent<bool> OnMapButtonToggled = new("DiscoverNotifier.Game.OnMapButtonToggled");
            /// <summary>When the map camera is activated or deactivated</summary>
            public static readonly ActionEvent<bool> OnMapCameraActivated = new("DiscoverNotifier.Game.OnMapCameraActivated");
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
                    if (!field.FieldType.IsSubclassOf(actionEventBaseType))
                        continue;
                    MethodInfo mInfo = field.FieldType.GetMethod("UnsubscribeAll", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (mInfo == null)
                        continue;
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
                    if (!field.FieldType.IsSubclassOf(actionEventBaseType))
                        continue;
                    ActionEventBase actionEvent = (ActionEventBase)field.GetValue(null);
                    int subscribed = (int)pTotSubscribed.GetValue(actionEvent);
                    if (subscribed <= 0)
                        continue;
                    strb.Append('\n').Append(pName.GetValue(actionEvent)).Append(" - tot subscribed: ").Append(subscribed);
                }
            }
            strb.Append("\n---------------------------------");
            Debug.Log(strb.ToString());
            strb.Length = 0;
        }
    }
}
