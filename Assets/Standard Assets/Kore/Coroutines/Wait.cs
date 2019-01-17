// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

using Kore.Utils;

namespace Kore.Coroutines
{
    public static class Wait
    {
        private static MiniPool< WaitForYieldable> WaitPool = new MiniPool< WaitForYieldable>( 16);

        /// <summary>
        /// Yield return Wait.For(seconds) to actually pause execution of your coroutine
        /// for a given amount of time.
        /// </summary>
        /// <param name="seconds"> time in seconds </param>
        /// <returns> A Yieldable object (you can "yield return" it).</returns>
        public static IYieldable For( float seconds)
        {
            if (seconds <= 0)
                return null;

            var item = WaitPool.Acquire();
            item.waitPool = WaitPool;
            item.TimeToWait = seconds;
            return item;
        }
    }

    /// <summary>
    /// You can use this class as template to create Yieldables with custom 
    /// time management easily.
    /// </summary>
    internal class WaitForYieldable : IYieldable, ICustomYield, IPoolable
    {
        public float TimeToWait;
        public MiniPool< WaitForYieldable> waitPool;

        public bool HasDone()
        {
            if( TimeToWait <= 0)
            {
                waitPool.Release( this);
                return true;
            }

            return false;
        }

        public void OnYield( ICoroutineEngine engine)
        {
            engine.RegisterCustomYield( this);
        }

        public void Reset()
        {
            waitPool = null; //comply with IPoolable Contract, not strictly
                             //needed in this case, but code may change later.
        }

        public void Update( Method method)
        {
            TimeToWait -= 
                method == Method.FixedUpdate?
                UnityEngine.Time.fixedDeltaTime:
                UnityEngine.Time.deltaTime;
        }
    }
}

