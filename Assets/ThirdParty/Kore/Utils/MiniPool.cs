// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

using System.Collections.Generic;

namespace Kore.Utils
{
    /// <summary>
    /// General purpose object pool, used to avoid generating too much
    /// garbage by caching object and resetting them when they are
    /// created.
    /// </summary>
    /// <typeparam name="T">Poolable (Reset method) class with new operator</typeparam>
    public class MiniPool<T> where T: IPoolable, new()
    {
        // Stack is slightly faster than a Queue: I guess because Pushing/Popping
        // from a stack access the same memory location, thus has slightly more
        // cache locality than a Queue (which access 2 different 
        // memory locations and eventually jump back when reach end of array).
        Stack<T> pool;

        /// <summary>
        /// Initialize the pool by preallocating a given amount of objects.
        /// </summary>
        /// <param name="initialCapacity"> Initial size</param>
        public MiniPool( int initialCapacity = 2)
        {
            pool = new Stack< T>( initialCapacity);
        }

        /// <summary>
        /// Get a Resetted object from the pool, you have to return it back
        /// </summary>
        public T Acquire()
        {
            T obj;

            if( pool.Count == 0)
                (obj = new T()).Reset();

            else
                obj = pool.Pop();

            return obj;
        }

        /// <summary>
        /// Release an item and reset it
        /// </summary>
        /// <param name="item"> reset is called immediatly to help GC, but don't rely on that behaviour</param>
        public void Release( T item)
        {
            item.Reset();
            pool.Push( item);
        }
    }
}
