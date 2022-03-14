#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Antura.EditorUtilities
{
    public class ScriptableObjectHelper : ScriptableObject
    {
        [MenuItem("Assets/Create/Empty asset")]
        public static void Create()
        {
            var asset = CreateInstance<ScriptableObject>();
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(path + "/Empty asset.asset"));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

#endif
