// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

using Kore.Utils;
using System.Collections;

namespace Kore.Coroutines
{
    public static class State 
    {
        private static StateChangeYieldable StateChanger = new StateChangeYieldable();

        /// <summary>
        /// Suspend (permanently stop) execution of current coroutine and start a new
        /// one, this is equivalent to exiting a state and entering another state.
        /// </summary>
        /// <param name="state"> Next coroutine to be runned</param>
        /// <returns> A Yieldable object (you can "yield return" it).</returns>
        public static IYieldable Change( IEnumerator state)
        {
            State.StateChanger.NextState = state;
            return State.StateChanger;
        }

        /// <summary>
        /// Suspend (permanently stop) execution of current coroutine and start a new
        /// one, this is equivalent to exiting a state and entering another state.
        /// </summary>
        /// <param name="state"> Next coroutine to be runned</param>
        /// <param name="callback"> Callback (Executed immediatly).</param>
        /// <returns> A Yieldable object (you can "yield return" it).</returns>
        public static IYieldable Change( IEnumerator state, KoreCallback callback)
        {
            callback();
            State.StateChanger.NextState = state;
            return State.StateChanger;
        }

        /// <summary>
        /// Create a state cache, usefull for optimizing State machines: be sure to read
        /// documentation about optimization tips for Coroutines and 0-garbage generation.
        /// </summary>
        public static StateCache Cache()
        {
            //statefull and persistent object handled by users, cannot get
            //it from a pool, but they can cache it if they want.
            return new StateCache();
        }
    }

    public class StateChangeYieldable : IYieldable
    {
        public IEnumerator NextState;

        public void OnYield( ICoroutineEngine engine)
        {
            engine.ReplaceCurrentWith( NextState);
        }
    }
}
