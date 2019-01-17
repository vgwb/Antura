// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

using System.Collections;

namespace Kore.Coroutines
{
    public interface ICoroutineEngine
    {
        /// <summary>
        /// Suspend (permanently stop) execution of current coroutine and
        /// schedule for execution of another coroutine
        /// </summary>
        /// <param name="nextState"> Next Coroutine</param>
        void ReplaceCurrentWith( IEnumerator nextState);

        /// <summary>
        /// Register a CustomYield, it will be updated as long as it has
        /// to wait.
        /// </summary>
        /// <param name="customYield"> The Custom Yield we are waiting for</param>
        void RegisterCustomYield( ICustomYield customYield);

        /// <summary>
        /// Pause execution of current coroutine and start executing another
        /// coroutine. When done resume execution of the Paused coroutine.
        /// </summary>
        /// <param name="nested"> Next coroutine to be runned</param>
        void PushOverCurrent( IEnumerator nested);
    }
}
