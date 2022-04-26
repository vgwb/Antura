// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

namespace Kore.Coroutines
{
    /// <summary>
    /// Interface to allow users implement custom yield instructions
    /// </summary>
    public interface IYieldable
    {
        /// <summary>
        /// Called when the coroutine engine "unpacks" the yield instruction
        /// </summary>
        /// <param name="engine">You usually use this to register a ICustomYield</param>
        void OnYield( ICoroutineEngine engine);
    }
}
