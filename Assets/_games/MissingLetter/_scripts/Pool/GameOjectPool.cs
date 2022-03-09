using UnityEngine;
using UnityEngine.Assertions;

namespace Antura.Minigames.MissingLetter
{
    public class GameObjectPool : Pool<GameObject>
    {
        #region INTERFACE
        public GameObjectPool(GameObject prefabType, int size, bool allowResizeUp) : base(size, allowResizeUp)
        {
            Assert.IsNotNull(prefabType, "GameObjectPool: Set this variable before use it!");

            if (prefabType != null)
            {
                m_oPrefabType = prefabType;

                for (int index = 0; index < m_iSize; ++index)
                {
                    m_aoPool[index] = GameObject.Instantiate(m_oPrefabType);
                    m_aoPool[index].SetActive(false);
                }
            }
        }

        //null if not free
        //element else
        new public GameObject GetElement()
        {
            GameObject result = base.GetElement();
            if (result != null)
            {
                result.SetActive(true);
            }
            return result;

        }

        new public void FreeElement(GameObject element)
        {
            element.SetActive(false);
            base.FreeElement(element);
        }

        public void FreeAll()
        {
            foreach (int index in m_aiIndexOfBusy)
            {
                m_aoPool[index].SetActive(false);
                m_aiIndexOfFree.Add(index);
            }
            m_aiIndexOfBusy.Clear();
        }
        #endregion

        #region PRIVATE
        protected override void ResizeUp()
        {
            Debug.LogWarning("Resizing Pool!");
            GameObject[] temp_Elements = new GameObject[m_aoPool.Length * 2];
            for (int index = 0; index < m_aoPool.Length; ++index)
            {
                temp_Elements[index] = m_aoPool[index];
            }
            for (int index = m_aoPool.Length; index < temp_Elements.Length; ++index)
            {
                temp_Elements[index] = GameObject.Instantiate(m_oPrefabType);
                temp_Elements[index].SetActive(false);

                m_aiIndexOfFree.Add(index);
            }
            m_aoPool = temp_Elements;
        }
        #endregion

        #region VARS

        private GameObject m_oPrefabType;

        #endregion
    }
}
