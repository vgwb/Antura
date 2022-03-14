using System.Collections.Generic;
using System.Linq;

namespace Antura.Minigames.MissingLetter
{
    public class Pool<T>
    {
        #region INTERFACE
        public Pool(int size, bool allowResizeUp = false)
        {
            m_iSize = size;
            m_aoPool = new T[m_iSize];

            for (int index = 0; index < m_iSize; ++index)
            {
                m_aiIndexOfFree.Add(index);
            }

            m_bAllowResizeUp = allowResizeUp;
        }


        //null if not free
        //element else
        public T GetElement()
        {

            T result = default(T);

            if (m_aiIndexOfFree.Count == 0 && m_bAllowResizeUp)
            {
                ResizeUp();
            }

            if (m_aiIndexOfFree.Count > 0)
            {
                int index = m_aiIndexOfFree.ElementAt(0);
                m_aiIndexOfFree.RemoveAt(0);

                result = m_aoPool[index];

                m_aiIndexOfBusy.Add(index);
            }

            return result;

        }

        public void FreeElement(T element)
        {
            for (int index = 0; index < m_aoPool.Length; ++index)
            {
                if (m_aoPool[index].Equals(element))
                {
                    m_aiIndexOfBusy.Remove(index);
                    m_aiIndexOfFree.Add(index);
                    break;
                }
            }
        }
        #endregion

        #region PRIVATE
        protected virtual void ResizeUp()
        {
            T[] temp_Bullets = new T[m_aoPool.Length * 2];
            for (int index = 0; index < m_aoPool.Length; ++index)
            {
                temp_Bullets[index] = m_aoPool[index];
            }
            for (int index = m_aoPool.Length; index < temp_Bullets.Length; ++index)
            {
                m_aiIndexOfFree.Add(index);
            }
            m_aoPool = temp_Bullets;
        }
        #endregion

        #region VARS
        protected int m_iSize;
        protected T[] m_aoPool;
        protected List<int> m_aiIndexOfFree = new List<int>();
        protected List<int> m_aiIndexOfBusy = new List<int>();
        protected bool m_bAllowResizeUp;
        #endregion
    }
}
