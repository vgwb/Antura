using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

namespace Antura.Database.Management
{
    /// <summary>
    /// Allows the parsing of a set of data contained in a JSON string and converts it 
    /// Provides support for custom validation and automatic generation of enumerators.
    /// </summary>
    /// <typeparam name="D">Data type to parse for each row of the JSON content</typeparam>
    /// <typeparam name="Dtable">Type for a table of the type to parse</typeparam>
    public abstract class DataParser<D, Dtable> where D : IData where Dtable : SerializableDataTable<D>
    {
        public void Parse(string json, DatabaseObject db, Dtable table)
        {
            table.Clear();  // we re-generate the whole table

            var rootDict = Json.Deserialize(json) as Dictionary<string, object>;
            foreach (var rootPair in rootDict) {
                var list = rootPair.Value as List<object>;
                foreach (var row in list) {
                    var dict = row as Dictionary<string, object>;
                    var data = CreateData(dict, db);
                    if (data == null) {
                        continue;
                    }

                    var value = table.GetValue(data.GetId());
                    if (value != null) {
                        if (!CanHaveSameKeyMultipleTimes) {
                            LogValidation(data, "found multiple ID.");
                        }
                        continue;
                    }

                    table.Add(data);
                }

            }

            FinalValidation(table, db);
        }

        protected virtual bool CanHaveSameKeyMultipleTimes
        {
            get {
                return false;
            }
        }

        protected abstract D CreateData(Dictionary<string, object> dict, DatabaseObject db);
        protected virtual void FinalValidation(Dtable table, DatabaseObject db) { }

        protected T ParseEnum<T>(D data, object enum_object)
        {
            string enum_string = ToString(enum_object);
            if (enum_string == "") enum_string = "None";
            T parsed_enum = default(T);
            try {
                parsed_enum = (T)System.Enum.Parse(typeof(T), enum_string);
            } catch {
                LogValidation(data, "field valued '" + enum_string + "', not available as an enum value for type " + typeof(T).ToString() + ".");
            }
            return parsed_enum;
        }

        protected string ParseID<OtherD, OtherDTable>(D data, string id_string, OtherDTable table) where OtherDTable : SerializableDataTable<OtherD> where OtherD : IData
        {
            id_string = id_string.Trim(); // remove spaces
            if (id_string == "") return ""; // skip empties

            var value = table.GetValue(id_string);
            if (value == null) {
                LogValidation(data, "could not find a reference inside " + typeof(OtherDTable).Name + " for ID " + id_string);
            }
            return id_string;
        }

        protected string[] ParseIDArray<OtherD, OtherDTable>(D data, string array_string, OtherDTable table) where OtherDTable : SerializableDataTable<OtherD> where OtherD : IData
        {
            if (table == null) {
                LogValidation(data, "Table of type " + typeof(OtherDTable).Name + " was null!");
            }

            var array = array_string.Split(',');
            if (array_string == "") return new string[0];  // skip if empty (could happen if the string was empty)    
            for (int i = 0; i < array.Length; i++) {
                array[i] = array[i].Trim(); // remove spaces
                var value = table.GetValue(array[i]);
                if (value == null) {
                    LogValidation(data, "could not find a reference inside " + typeof(OtherDTable).Name + " for ID " + array[i]);
                }
            }
            return array;
        }

        protected void LogValidation(D data, string msg)
        {
            Debug.LogWarning(data.GetType().ToString() + " (ID " + data.GetId() + "): " + msg);
        }

        #region Conversions
        protected string ToString(object _input)
        {
            return ((string)_input).Trim();
        }

        protected int ToInt(object _input)
        {
            // Force empty to 0
            if ((string)_input == "")
                return 0;

            int target_int = 0;
            if (!int.TryParse((string)_input, out target_int)) {
                Debug.LogError("Object " + (string)_input + " should be an int.");
            }
            return target_int;
        }

        protected float ToFloat(object _input)
        {
            // Force empty to 0
            if ((string)_input == "") {
                return 0f;
            }

            float target_float = 0f;
            if (!float.TryParse((string)_input, out target_float)) {
                Debug.LogError("Object " + (string)_input + " should be a float.");
            }
            return target_float;
        }
        #endregion

        #region Enums

        public void RegenerateEnums(string json)
        {
            var list = Json.Deserialize(json) as List<object>;
            var rowdicts_list = new List<Dictionary<string, object>>();
            foreach (var row in list) {
                var dict = row as Dictionary<string, object>;
                rowdicts_list.Add(dict);
            }
            RegenerateEnums(rowdicts_list);
        }

        protected abstract void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list);

        protected void ExtractEnum(List<Dictionary<string, object>> rowdicts_list, string key, bool addNoneValue = false, string customEnumName = null)
        {
#if UNITY_EDITOR
            EnumGenerator.ExtractEnum(typeof(D).Name, key, rowdicts_list, addNoneValue, customEnumName);
#endif
        }

        #endregion
    }
}
