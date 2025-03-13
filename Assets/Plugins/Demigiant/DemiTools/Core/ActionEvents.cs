// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2022/04/13

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demigiant.DemiTools
{
    public abstract class ActionEventBase
    {
        internal string name;
        internal int totSubscribed;
        internal bool logMe;
    }
    
    // ---------------------------------------------------------------------------------------------------------------------
    
    public abstract class ActionEventBase<T> : ActionEventBase
    {
        readonly HashSet<T> _subscribedActions = new HashSet<T>();
        
        protected ActionEventBase(string name, bool logMe = false) { this.name = name; this.logMe = logMe; }

        /// <summary>
        /// Returns TRUE if the Action was successfully added (meaning it wasn't already present)
        /// </summary>
        protected bool AddSubscribed(T action)
        {
            if (_subscribedActions.Contains(action))
            {
                Debug.LogWarning($"ActionEvent: Duplicate action subscription to {name}: ignoring it");
                return false;
            }
            _subscribedActions.Add(action);
            totSubscribed++;
            ActionEventManager.AddSubscriptionTo(this);
            if (logMe || ActionEventManager.logSubscribe) Debug.Log($"ActionEvent [+] Added 1 subscription to ■{name}■ (tot subscribed: {totSubscribed})");
            return true;
        }

        protected void RemoveSubscribed(T action)
        {
            if (!_subscribedActions.Contains(action)) return;
            _subscribedActions.Remove(action);
            totSubscribed--;
            ActionEventManager.RemoveSubscriptionFrom(this);
            if (logMe || ActionEventManager.logUnsubscribe) Debug.Log($"ActionEvent [-] Removed 1 subscription to ■{name}■ (tot subscribed: {totSubscribed})");
        }

        protected void RemoveAllSubscribed()
        {
            _subscribedActions.Clear();
            ActionEventManager.RemoveAllSubscriptionsTo(this);
            totSubscribed = 0;
            if (logMe || ActionEventManager.logUnsubscribe) Debug.Log($"ActionEvent [---] Removed all subscriptions to ■{name}■");
        }
    }

    // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
    
    public class ActionEvent : ActionEventBase<Action>
    {
        event Action Event;
        public ActionEvent(string name, bool logMe = false) : base(name, logMe) {}
        public void Subscribe(Action method) { if (AddSubscribed(method)) Event += method; }
        public void Unsubscribe(Action method) { Event -= method; RemoveSubscribed(method); }
        public void UnsubscribeAll() { Event = null; RemoveAllSubscribed(); }

        public void Dispatch()
        {
            if (logMe || ActionEventManager.logDispatch) Debug.Log($"ActionEvent [>] Dispatching ■{name}■ (tot subscribed: {totSubscribed})");
            Event?.Invoke();
        }
    }
    
    // ---------------------------------------------------------------------------------------------------------------------
    
    public class ActionEvent<T> : ActionEventBase<Action<T>>
    {
        event Action<T> Event;
        public ActionEvent(string name, bool logMe = false) : base(name, logMe) {}
        public void Subscribe(Action<T> method) { if (AddSubscribed(method)) Event += method; }
        public void Unsubscribe(Action<T> method) { Event -= method; RemoveSubscribed(method); }
        public void UnsubscribeAll() { Event = null; RemoveAllSubscribed(); }

        public void Dispatch(T t)
        {
            if (logMe || ActionEventManager.logDispatch) Debug.Log($"ActionEvent [>] Dispatching ■{name} ({t})■ (tot subscribed: {totSubscribed})");
            Event?.Invoke(t);
        }
    }
    
    // ---------------------------------------------------------------------------------------------------------------------
    
    public class ActionEvent<T0,T1> : ActionEventBase<Action<T0,T1>>
    {
        event Action<T0,T1> Event;
        public ActionEvent(string name, bool logMe = false) : base(name, logMe) { }
        public void Subscribe(Action<T0,T1> method) { if (AddSubscribed(method)) Event += method; }
        public void Unsubscribe(Action<T0,T1> method) { Event -= method; RemoveSubscribed(method); }
        public void UnsubscribeAll() { Event = null; RemoveAllSubscribed(); }

        public void Dispatch(T0 t0, T1 t1)
        {
            if (logMe || ActionEventManager.logDispatch) Debug.Log($"ActionEvent [>] Dispatching ■{name} ({t0}, {t1})■ (tot subscribed: {totSubscribed})");
            Event?.Invoke(t0, t1);
        }
    }
    
    // ---------------------------------------------------------------------------------------------------------------------
    
    public class ActionEvent<T0,T1,T2> : ActionEventBase<Action<T0,T1,T2>>
    {
        event Action<T0,T1,T2> Event;
        public ActionEvent(string name, bool logMe = false) : base(name, logMe) { }
        public void Subscribe(Action<T0,T1,T2> method) { if (AddSubscribed(method)) Event += method; }
        public void Unsubscribe(Action<T0,T1,T2> method) { Event -= method; RemoveSubscribed(method); }
        public void UnsubscribeAll() { Event = null; RemoveAllSubscribed(); }

        public void Dispatch(T0 t0, T1 t1, T2 t2)
        {
            if (logMe || ActionEventManager.logDispatch) Debug.Log($"ActionEvent [>] Dispatching ■{name} ({t0}, {t1}, {t2})■ (tot subscribed: {totSubscribed})");
            Event?.Invoke(t0, t1, t2);
        }
    }
    
    // ---------------------------------------------------------------------------------------------------------------------
    
    public class ActionEvent<T0,T1,T2,T3> : ActionEventBase<Action<T0,T1,T2,T3>>
    {
        event Action<T0,T1,T2,T3> Event;
        public ActionEvent(string name, bool logMe = false) : base(name, logMe) { }
        public void Subscribe(Action<T0,T1,T2,T3> method) { if (AddSubscribed(method)) Event += method; }
        public void Unsubscribe(Action<T0,T1,T2,T3> method) { Event -= method; RemoveSubscribed(method); }
        public void UnsubscribeAll() { Event = null; RemoveAllSubscribed(); }

        public void Dispatch(T0 t0, T1 t1, T2 t2, T3 t3)
        {
            if (logMe || ActionEventManager.logDispatch) Debug.Log($"ActionEvent [>] Dispatching ■{name} ({t0}, {t1}, {t2}, {t3})■ (tot subscribed: {totSubscribed})");
            Event?.Invoke(t0, t1, t2, t3);
        }
    }
}