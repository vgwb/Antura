// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

namespace Kore.Coroutines
{
    /// <summary>
    /// You need to implement this interface for custom yield instructions.
    /// As well you have to implement IYieldable in order to access the
    /// ICoroutineEngine to register the ICustomYieldable.
    /// (See Wait.For implementation)
    /// </summary>
    public interface ICustomYield
    {
        /// <summary>
        /// This method is called at each update so that you can perform checks like
        /// seeing if a player pressed a particular key / some event happened /
        /// and eventually update a countdown timer.
        /// You should check the "method" parameter when updating.
        /// </summary>
        /// <param name="method">Let's you know on which method the Coroutine is running. </param>
        void Update( Method method);

        /// <summary>
        /// CoroutineEngine will periodically check if the CustomYield "hasDone", if it has done
        /// then the CustomYield is terminated and the Coroutine will resume again.
        /// </summary>
        /// <returns> true when we should stop waiting, false otherwise.</returns>
        bool HasDone();
    }
}
