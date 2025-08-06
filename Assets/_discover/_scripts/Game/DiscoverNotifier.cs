using System;
using System.Reflection;
using System.Text;
using Demigiant.DemiTools;
using UnityEngine;

namespace Antura.Discover
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
            /// <summary>When an Interactable collider OnTriggerEnter is entered by the player</summary>
            public static readonly ActionEvent<Interactable> OnInteractableEnteredByPlayer = new("DiscoverNotifier.Game.OnInteractableEnteredByPlayer");
            /// <summary>When an Interactable collider OnTriggerEnter is exited by the player</summary>
            public static readonly ActionEvent<Interactable> OnInteractableExitedByPlayer = new("DiscoverNotifier.Game.OnInteractableExitedByPlayer");
            /// <summary>When the action button is pressed</summary>
            public static readonly ActionEvent OnActClicked = new("DiscoverNotifier.Game.OnActClicked");
            /// <summary>When the map button is toggled</summary>
            public static readonly ActionEvent OnMapButtonToggled = new("DiscoverNotifier.Game.OnMapButtonToggled");
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
