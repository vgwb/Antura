// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

using Kore.Utils;
using System.Collections;

namespace Kore.Coroutines
{
    public class StateCache
    {
        CachedStateChangeYieldable change;
        bool executed;

        internal StateCache()
        {
            change = new CachedStateChangeYieldable();
        }

        /// <summary>
        /// Prefer yielding this function instead of "null" to have a code that
        /// is more clear about its intent
        /// </summary>
        /// <returns> A Yieldable object (you can "yield return" it).</returns>
        public IYieldable EnterState()
        {
            return null;
        }

        /// <summary>
        /// Prefer yielding this function instead of "null" to have a code that
        /// is more clear about its intent, also the callback will be executed
        /// only once when the state is entered
        /// </summary>
        /// <param name="onEnterState"> Callback (Executed immediatly).</param>
        /// <returns> A Yieldable object (you can "yield return" it).</returns>
        public IYieldable EnterState(KoreCallback onEnterState)
        {
            if (executed == false)
            {
                onEnterState();
                executed = true;
            }

            return null;
        }

        /// <summary>
        /// Suspend (permanently stop) execution of current coroutine and start a new
        /// one, this is equivalent to exiting a state and entering another state.
        /// It also allow "EnterState" to trigger again its callback once it will be executed.
        /// </summary>
        /// <param name="nextState"> Next coroutine to be runned</param>
        /// <returns> A Yieldable object (you can "yield return" it).</returns>
        public IYieldable Change(IEnumerator nextState)
        {
            executed = false;
            change.NextState = nextState;
            return change;
        }

        /// <summary>
        /// Suspend (permanently stop) execution of current coroutine and start a new
        /// one, this is equivalent to exiting a state and entering another state.
        /// It also allow "EnterState" to trigger again its callback once it will be executed.
        /// This method also have callback
        /// </summary>
        /// <param name="nextState"> Next coroutine to be runned</param>
        /// <param name="onExitState"> Callback (Executed immediatly).</param>
        /// <returns> A Yieldable object (you can "yield return" it).</returns>
        public IYieldable Change( IEnumerator nextState, KoreCallback onExitState)
        {
            executed = false;
            onExitState();
            change.NextState = nextState;
            return change;
        }

        /// <summary>
        /// A simple notification mechanism that can be polled like a input
        /// to detect state changes.
        /// </summary>
        public StateEvent Event()
        {
            return new StateEvent();
        }
    }

    internal class CachedStateChangeYieldable : IYieldable
    {
        public IEnumerator NextState;

        public void OnYield( ICoroutineEngine engine)
        {
            engine.ReplaceCurrentWith( NextState);
        }
    }

    public class StateEvent
    {
        private bool triggered = false;

        /// <summary>
        /// Trigger the event
        /// </summary>
        public void Trigger()
        {
            triggered = true;
        }

        /// <summary>
        /// Check if event is triggered and eat the event.
        /// </summary>
        public static bool operator true( StateEvent t)
        {
            bool val = t.triggered;
            t.triggered = false;
            return val;
        }

        /// <summary>
        /// Check if event is triggered and eat the event.
        /// </summary>
        public static bool operator false( StateEvent t)
        {
            bool val = !t.triggered;
            t.triggered = false;
            return val;
        }
    }
}
