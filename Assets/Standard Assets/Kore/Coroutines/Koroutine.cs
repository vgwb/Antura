// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

using System.Collections;

namespace Kore.Coroutines
{
    /// <summary>
    /// Static class to access special coroutines without having to call each time
    /// "Instance" over CoroutineCore.
    /// </summary>
    public static class Koroutine
    {
        private static CoroutineNestedYieldable NestedYield = new CoroutineNestedYieldable();

        /// <summary>
        /// The preferred method for launching a coroutine is to call
        /// this method: Coroutine.Run( YourEnumerator() );
        /// </summary>
        /// <param name="enumerator"> Your Enumerator</param>
        /// <param name="method"> Select when to update the coroutine.</param>
        public static void Run( IEnumerator enumerator, Method method = Method.Update)
        {
            CoroutineCore.Instance.Run( enumerator, method);
        }

        /// <summary>
        /// Start another coroutine and pause the current one. Can be called only
        /// from within an already called coroutine.
        /// </summary>
        /// <param name="enumerator"> Your Enumerator</param>
        /// <returns> A Yieldable object (you can "yield return" it).</returns>
        public static IYieldable Nested( IEnumerator enumerator)
        {
            Koroutine.NestedYield.Nested = enumerator;
            return Koroutine.NestedYield;
        }
    }

    internal class CoroutineNestedYieldable: IYieldable
    {
        public IEnumerator Nested;

        public void OnYield( ICoroutineEngine engine)
        {
            engine.PushOverCurrent( Nested);
        }
    }
}
