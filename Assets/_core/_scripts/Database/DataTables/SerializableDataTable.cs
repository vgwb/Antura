using System.Collections.Generic;

namespace Antura.Database
{
    /// <summary>
    /// Concrete implementation of IDataTable.
    /// Can be serialized.
    /// </summary>
    [System.Serializable]
    public class SerializableDataTable<K> : IDataTable where K : IData
    {
        [UnityEngine.SerializeField]
        private List<K> innerList = new List<K>();

        public void AddRange(IEnumerable<K> range)
        {
            this.innerList.AddRange(range);
        }

        public void Add(K data)
        {
            innerList.Add(data);
        }

        public void Clear()
        {
            innerList.Clear();
        }

        public List<IData> GetList()
        {
            return new List<IData>(this.GetValues());
        }

        public IEnumerable<IData> GetValues()
        {
            foreach (var value in innerList)
            {
                yield return value;
            }
        }

        public IEnumerable<K> GetValuesTyped()
        {
            foreach (var value in innerList)
            {
                yield return value;
            }
        }

        public IData GetValue(string id)
        {
            return innerList.Find(x => x.GetId() == id);
        }

        public int GetDataCount()
        {
            return innerList.Count;
        }
    }
}
