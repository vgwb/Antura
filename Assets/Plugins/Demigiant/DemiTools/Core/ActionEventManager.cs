// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2023/11/03

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Demigiant.DemiTools
{
    public static class ActionEventManager
    {
        public enum LogMode
        {
            None,
            All,
            Subscribe,
            Unsubscribe,
            SubscribeAndUnsubscribe,
            Dispatch
        }
        
        /// <summary>
        /// If different from None logs the given event types,
        /// otherwise only ActionEvents created with the logMe parameter set to TRUE will be logged
        /// </summary>
        public static LogMode logMode {
            get { return _foo_logMode; }
            set {
                _foo_logMode = value;
                logSubscribe = value is LogMode.All or LogMode.Subscribe or LogMode.SubscribeAndUnsubscribe;
                logUnsubscribe = value is LogMode.All or LogMode.Unsubscribe or LogMode.SubscribeAndUnsubscribe;
                logDispatch = value is LogMode.All or LogMode.Dispatch;
            }
        }
        /// <summary>
        /// Total subscriptions to all ActionEvents currently active
        /// </summary>
        public static int totSubscriptions { get; private set; }
        
        internal static bool logSubscribe { get; private set; }
        internal static bool logUnsubscribe { get; private set; }
        internal static bool logDispatch { get; private set; }

        static readonly StringBuilder _Strb = new();
        static readonly Dictionary<ActionEventBase, int> _ActionEventToTotSubscriptions = new();
        static LogMode _foo_logMode;

        #region Public Methods

        public static void LogAllSubscriptions()
        {
            _Strb.Length = 0;
            _Strb.Append("Tot subscriptions to all ActionEvents: ").Append(totSubscriptions);
            foreach (KeyValuePair<ActionEventBase,int> d in _ActionEventToTotSubscriptions) {
                _Strb.Append("\n   ").Append(d.Key.name).Append(": ").Append(d.Value);
            }
            Debug.Log(_Strb.ToString());
            _Strb.Length = 0;
        }

        #endregion

        #region Internal Methods

        internal static void AddSubscriptionTo(ActionEventBase actionEvent)
        {
            if (_ActionEventToTotSubscriptions.ContainsKey(actionEvent)) _ActionEventToTotSubscriptions[actionEvent] += 1;
            else _ActionEventToTotSubscriptions.Add(actionEvent, 1);
            int tot = _ActionEventToTotSubscriptions[actionEvent];
            if (tot != actionEvent.totSubscribed) Debug.LogError($"ActionEventManager.AddSubscriptionTo({actionEvent.name}) : subscriptions are not equal between event and manager");
            totSubscriptions++;
        }

        internal static void RemoveSubscriptionFrom(ActionEventBase actionEvent)
        {
            if (_ActionEventToTotSubscriptions.ContainsKey(actionEvent)) _ActionEventToTotSubscriptions[actionEvent] -= 1;
            else Debug.LogError($"ActionEventManager.RemoveSubscriptionFrom({actionEvent.name}) : no subscriptions present");
            int tot = _ActionEventToTotSubscriptions[actionEvent];
            if (tot < 0) Debug.LogError($"ActionEventManager.RemoveSubscriptionFrom({actionEvent.name}) : subscriptions are below zero ({tot})");
            else if (tot != actionEvent.totSubscribed) Debug.LogError($"ActionEventManager.RemoveSubscriptionFrom({actionEvent.name}) : subscriptions are not equal between event and manager");
            else if (tot == 0) _ActionEventToTotSubscriptions.Remove(actionEvent);
            totSubscriptions--;
        }

        internal static void RemoveAllSubscriptionsTo(ActionEventBase actionEvent)
        {
            if (_ActionEventToTotSubscriptions.ContainsKey(actionEvent)) _ActionEventToTotSubscriptions.Remove(actionEvent);
            else Debug.LogError($"ActionEventManager.RemoveAllSubscriptionsTo({actionEvent.name}) : no subscriptions present");
            totSubscriptions -= actionEvent.totSubscribed;
        }

        #endregion
    }
}