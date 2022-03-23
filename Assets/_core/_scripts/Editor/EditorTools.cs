using UnityEngine;
using UnityEditor;

public class EditorTools : ScriptableObject
{
    [MenuItem("Antura/Reveal Data Path")]
    static void RevelDataPath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
}
