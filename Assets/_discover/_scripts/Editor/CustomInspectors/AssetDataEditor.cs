using Antura.Discover;
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(AssetData))]
public class AssetDataEditor : UnityEditor.Editor
{
    SerializedProperty _scriptProp;

    void OnEnable()
    {
        _scriptProp = serializedObject.FindProperty("m_Script");
    }

    // [MenuItem("Assets/Rebuild Static Preview", true)]
    // private static bool RebuildPreviewValidate() => Selection.activeObject is AssetData;

    // [MenuItem("Assets/Rebuild Static Preview")]
    // private static void RebuildPreview()
    // {
    //     var path = AssetDatabase.GetAssetPath(Selection.activeObject);
    //     AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    // }

    public override bool HasPreviewGUI() => true;

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        var data = (AssetData)target;
        if (data.Image != null)
            GUI.DrawTexture(r, data.Image.texture, ScaleMode.ScaleToFit, true);
    }

    public override void OnInspectorGUI()
    {
        if (serializedObject == null || target == null)
        {
            base.OnInspectorGUI();
            return;
        }

        serializedObject.Update();

        // Draw default inspector except m_Script
        DrawPropertiesExcluding(serializedObject, "m_Script");

        // Inline preview from Image
        var data = (AssetData)target;
        if (data != null && data.Image != null && data.Image.texture != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                float maxW = EditorGUIUtility.currentViewWidth - 40f;
                float size = Mathf.Min(256f, maxW);
                var tex = data.Image.texture;
                Rect r = GUILayoutUtility.GetRect(size, size, GUILayout.ExpandWidth(false));
                EditorGUI.DrawPreviewTexture(r, tex, null, ScaleMode.ScaleToFit);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Utilities", EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope("box"))
        {
            if (GUILayout.Button("Set Id (filename)", GUILayout.MaxWidth(200)))
            {
                foreach (var t in targets)
                {
                    if (t is AssetData a)
                    {
                        string path = AssetDatabase.GetAssetPath(a);
                        if (string.IsNullOrEmpty(path))
                        {
                            Debug.LogWarning("Cannot set Id from filename: asset path not found.", a);
                            continue;
                        }
                        string fileName = Path.GetFileNameWithoutExtension(path);
                        if (string.IsNullOrWhiteSpace(fileName))
                        {
                            Debug.LogWarning("Cannot set Id from filename: filename is empty.", a);
                            continue;
                        }
                        Undo.RecordObject(a, "Set Id from filename");
                        string sanitized = IdentifiedData.SanitizeId(fileName);
                        a.Editor_SetId(sanitized);
                        EditorUtility.SetDirty(a);
                    }
                }
            }

            if (GUILayout.Button("Rename linked image like this file", GUILayout.MaxWidth(280)))
            {
                foreach (var t in targets)
                {
                    if (t is AssetData a)
                    {
                        if (a.Image == null)
                        {
                            Debug.LogWarning("No linked Image to rename.", a);
                            continue;
                        }
                        string assetPath = AssetDatabase.GetAssetPath(a);
                        if (string.IsNullOrEmpty(assetPath))
                        {
                            Debug.LogWarning("Cannot rename image: asset path not found.", a);
                            continue;
                        }
                        string baseName = Path.GetFileNameWithoutExtension(assetPath);
                        string imgPath = AssetDatabase.GetAssetPath(a.Image);
                        if (string.IsNullOrEmpty(imgPath))
                        {
                            Debug.LogWarning("Cannot rename image: image path not found.", a);
                            continue;
                        }
                        string error = AssetDatabase.RenameAsset(imgPath, baseName);
                        if (!string.IsNullOrEmpty(error))
                        {
                            Debug.LogError($"Rename failed for {imgPath} -> {baseName}: {error}", a);
                        }
                    }
                }
                AssetDatabase.SaveAssets();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    // public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    // {
    //     AssetData data = (AssetData)target;

    //     if (data == null || data.Image == null)
    //         return null;

    //     Texture sourceTex = null;

    //     if (data.Image != null)
    //         sourceTex = data.Image.texture;

    //     if (sourceTex == null)
    //         return null; // fall back to default icon


    //     // Create the preview texture Unity will cache & show in Project + Object Picker
    //     var preview = new Texture2D(width, height, TextureFormat.RGBA32, false);

    //     // Draw the source into a temporary RT, respecting Sprite rect if needed
    //     var rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
    //     var prev = RenderTexture.active;
    //     RenderTexture.active = rt;
    //     GL.Clear(true, true, new Color(0, 0, 0, 0));

    //     // Compute UVs if using a sprite atlas
    //     Rect uv = new Rect(0, 0, 1, 1);
    //     if (data.Image != null)
    //     {
    //         var sp = data.Image;
    //         uv = new Rect(
    //             sp.textureRect.x / sp.texture.width,
    //             sp.textureRect.y / sp.texture.height,
    //             sp.textureRect.width / sp.texture.width,
    //             sp.textureRect.height / sp.texture.height);
    //     }

    //     // Draw the texture (full-bleed) into the RT
    //     Graphics.DrawTexture(
    //         new Rect(0, 0, width, height),
    //         sourceTex,
    //         uv, 0, 0, 0, 0);

    //     // Read back into Texture2D
    //     preview.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    //     preview.Apply();

    //     // Cleanup
    //     RenderTexture.active = prev;
    //     RenderTexture.ReleaseTemporary(rt);

    //     return preview;
    // }
}

