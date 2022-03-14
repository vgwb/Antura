#if UNITY_EDITOR

using UnityEngine;
using System.Collections.Generic;
using Antura.Helpers;

namespace Antura.Database.Management
{
    /// <summary>
    /// Used for testing queries to retrieve player data.
    /// </summary>
    public class QueriesTester : MonoBehaviour
    {
        private DatabaseTester dbTester;
        private DatabaseManager dbManager;

        void Start()
        {
            dbTester = FindObjectOfType<DatabaseTester>();
            dbManager = dbTester.dbManager;
        }

        private List<T> DoSelect<T>(string orderBy = "", int limit = 0) where T : IData, new()
        {
            string query = "SELECT * FROM " + typeof(T).Name;
            if (orderBy != "")
                query += " ORDER BY " + orderBy;
            if (limit > 0)
                query += " LIMIT " + limit;
            return dbManager.Query<T>(query);
        }


        // 12. Mood indicator curve
        public void QueryLastMoods()
        {
            int nEntries = 20;
            List<LogMoodData> list = DoSelect<LogMoodData>("Timestamp", 20);

            string output = "Latest " + nEntries + " moods:\n";
            foreach (var data in list)
                output += GenericHelper.FromTimestamp(data.Timestamp) + ": " + data.ToString() + "\n";
            dbTester.PrintOutput(output);
        }

    }
}
#endif
