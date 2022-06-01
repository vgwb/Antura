using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Antura.Tools
{
    // DEPRECATED (used only to generate the first addressables)
    public class ShapeDataFixer : MonoBehaviour
    {

        //[MenuItem("Antura/Tools/Fix ShapeData Addressables")]
        public static void FixShapeData()
        {
            AppManager.I.AppSettingsManager.SetLearningContentID(LearningContentID.LearnToRead_Dari);
            var db = new DatabaseManager(AppManager.I.ContentEdition);

            Debug.Log("Fixing addressable for shape data.");
            var group = AddressableAssetSettingsDefaultObject.Settings.groups.FirstOrDefault(x => x.name.ToLower().Contains("common"));
            var guids = AssetDatabase.FindAssets("", new[] { "Assets/_core/Fonts/" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (!path.Contains("ShapeData/shapedata"))
                    continue;

                var entry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, group);
                var splits = path.Split('.')[0].Split('/');
                var fontName = splits[splits.Length - 3].Split(' ')[1];
                var assetName = splits[splits.Length - 1];
                var letterId = assetName.Replace("shapedata_", "");
                char[] hexChars = { 'A', 'B', 'C', 'D', 'E', 'F' };
                bool allDigits = letterId.ToCharArray().All(ch => char.IsDigit(ch) || hexChars.Contains(ch));
                if (!allDigits)
                {
                    var letterData = db.GetLetterDataById(letterId);
                    if (letterData == null)
                    {
                        Debug.LogWarning("Could not find letter data " + letterId);
                    }
                    else
                    {
                        var unicode = letterData.GetCompleteUnicodes();
                        assetName = $"shapedata_{unicode}";
                        Debug.Log($"Renamed {letterId} to: {assetName}");
                        AssetDatabase.RenameAsset(path, assetName);
                    }
                }
                entry.address = fontName + "/" + assetName;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("FINISHED Fixing addressable for shape data");
        }
    }
}
