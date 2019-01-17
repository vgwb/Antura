#if UNITY_EDITOR

using Antura.Helpers;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Replacement;

// @note: this script was used to convert TextMeshPro from the OLD to the NEW version

/*
public class TMP_Replacer : MonoBehaviour
{
    public enum SceneChoice
    {
        CurrentScene,
        AllScenes
    }

    private static TMP_Replacer_Referencer referencer;
    private static bool testModifications = false;

    [MenuItem("Tools/Replace Text Mesh Pro")]
    public static void ReplaceTextMeshPro(MenuCommand command)
    {
        SceneChoice sceneChoice = SceneChoice.CurrentScene;

        // Find the referencer prefab with the data we want to swap
        referencer = Resources.Load<TMP_Replacer_Referencer>("TMP_Replacer_Referencer");

        switch (sceneChoice)
        {
            case SceneChoice.AllScenes:
                var editorSceneInfos = EditorBuildSettings.scenes;
                foreach (var editorSceneInfo in editorSceneInfos)
                {
                    if (!editorSceneInfo.enabled) continue;

                    ReplaceInScene(editorSceneInfo.path);
                }
                break;
            case SceneChoice.CurrentScene:
                ReplaceInScene(EditorSceneManager.GetActiveScene().path);
                break;
        }

    }

    static void ReplaceInScene(string path)
    {
        Debug.Log("Checking scene: " + path);
        var scene = EditorSceneManager.OpenScene(path);
        if (!scene.isLoaded) return;
        if (!scene.IsValid()) return;

        PerformFullReplacement<TextMeshPro_OLD, TextMeshPro>(referencer);
        PerformFullReplacement<TextMeshProUGUI_OLD, TextMeshProUGUI>(referencer);

        if (!testModifications)
        {
            if (!EditorSceneManager.MarkSceneDirty(scene)) Debug.Log("Not set as dirty!");
            if (!EditorSceneManager.SaveScene(scene)) Debug.Log("Not saved!");

            // Reopen and close the scene to fix references
            EditorSceneManager.OpenScene(path);
        }
    }

    static void PerformFullReplacement<TFrom, TTo>(TMP_Replacer_Referencer referencer)    
            where TFrom : Component, TTo
            where TTo : Component
    {
    
        // Get all references to From
        var referencesDict = ReplacementUtility.CollectObjectsReferencingComponent<TFrom>();
        foreach (var pair in referencesDict)
        {
            if (pair.Value.Count > 0)
            {
                Debug.Log("Found " + pair.Value.Count + " references of " + ReplacementUtility.ToS(pair.Key), pair.Key);
                Debug.Log(pair.Value.ToDebugString());
            }
        }
        
        // Get all references of old assets from From
        var dependencyDict = ReplacementUtility.CollectObjectsOfTypeTheComponentReferencesInFields<TFrom, TMP_FontAsset_OLD>();
        foreach (var pair in dependencyDict)
        {
            if (pair.Value.Count > 0)
            {
                Debug.Log("Found " + pair.Value.Count + " dependencies for " + ReplacementUtility.ToS(pair.Key), pair.Key);
                Debug.Log(pair.Value.ToDebugString());
            }
        }

        // Replace all references with To
        var replacementDict = ReplacementUtility.ReplaceAllComponentsOfType<TFrom, TTo>();
        foreach (var pair in replacementDict)
            Debug.Log("Replaced component " + ReplacementUtility.ToS(pair.Key) + " with " + ReplacementUtility.ToS(pair.Value));

        // Replace all references in To for Assets that From referred to that should now be different (could not copy as field as they are different now)
        ReplacementUtility.PlaceAllAssetReferencesMirroring<TFrom,TTo, TMP_FontAsset_OLD, TMP_FontAsset, FontReplacementPair>(replacementDict, referencer.fonts, dependencyDict);

        // Replace all references for Components that referred to From and now should refer to To
        ReplacementUtility.ReplaceAllComponentReferences(replacementDict, referencesDict);
    }
}
*/
#endif