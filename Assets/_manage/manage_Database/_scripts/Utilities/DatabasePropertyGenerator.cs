#if UNITY_EDITOR
using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Antura.Database.Management
{
    /// <summary>
    /// Utility class that converts all fields of a class to properties for use with SQLite
    /// </summary>
    public static class DatabasePropertyGenerator
    {
        //[MenuItem("Assets/Generate Database Properties")]
        public static void GenerateDatabaseProperties()
        {
            var selectedGUIDs = Selection.assetGUIDs;
            foreach (var selectedGUID in selectedGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(selectedGUID);
                var assetName = assetPath.Split('/').Last().Split('.').First();

                Assembly asm = typeof(MiniGameData).Assembly;
                Type type = asm.GetType(typeof(MiniGameData).Namespace + "." + assetName);

                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
                FieldInfo[] fields = type.GetFields(flags);
                foreach (FieldInfo fieldInfo in fields)
                {
                    Debug.Log("Class: " + type.Name + ", Field: " + fieldInfo.Name);
                    ReplaceFieldWithProperty(assetPath, fieldInfo);
                }
            }
        }

        private static void ReplaceFieldWithProperty(string classFilePath, FieldInfo fieldInfo)
        {
            string fileText = File.ReadAllText(classFilePath);

            string fieldName = fieldInfo.Name;
            Type fieldType = fieldInfo.FieldType;

            var fieldTypeName = fieldType.Name;
            if (fieldType == typeof(String))
                fieldTypeName = "string";
            if (fieldType == typeof(String[]))
                fieldTypeName = "string[]";
            if (fieldType == typeof(Boolean))
                fieldTypeName = "bool";
            if (fieldType == typeof(Int32))
                fieldTypeName = "int";

            string inString = "public " + fieldTypeName + " " + fieldName + ";";
            string outString = "public " + fieldTypeName + " " + fieldName + "{ get { return _" + fieldName + "; } set { _" + fieldName + " = value; }}\n"
                    + "[SerializeField] private " + fieldTypeName + " _" + fieldName + ";";

            fileText = fileText.Replace(inString, outString);

            Debug.Log("Replacing:" + inString);
            Debug.Log("To: " + outString);
            //Debug.Log("Regenerated Class " + fileText);

            File.WriteAllText(classFilePath, fileText);
        }
    }

}
#endif
