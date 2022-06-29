using UnityEngine;
using UnityEditor;

public class EditorTools : ScriptableObject
{
    [MenuItem("Antura/Utility/Reveal Data Path")]
    static void RevelDataPath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
}
