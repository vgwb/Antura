// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

namespace Kore.Utils
{
    public interface IPoolable
    {
        /// <summary>
        /// CONTRACT: each time you obtain a object from the pool Reset
        /// has been called already exactly once. It is undefined if that happens
        /// when object is returned to the pool or just before it is given
        /// back to the user. Objects implementing Reset, should use Reset to
        /// restore a initial state, and most important they should set to "null"
        /// all references to other objects in order to help Garbage Collection.
        /// </summary>
        void Reset();
    }
}
