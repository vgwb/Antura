using UnityEngine;
using UnityEditor;
using System.Reflection;
using Homer;

public class HomerVarsWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private string searchQuery = ""; // Stores filter text

    [MenuItem("Tools/Homer/Homer Vars Viewer")]
    public static void ShowWindow()
    {
        HomerVarsWindow window = GetWindow<HomerVarsWindow>("Homer Vars Viewer");
        window.Show();
        EditorApplication.update += window.UpdateWindow; // Ensure auto-refresh
    }

    private void OnDestroy()
    {
        EditorApplication.update -= UpdateWindow; // Remove listener on close
    }

    private void UpdateWindow()
    {
        if (Application.isPlaying)
        {
            Repaint(); // Refresh every frame
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Homer Vars (Live)", EditorStyles.boldLabel);

        // 🔹 Search Bar
        GUILayout.BeginHorizontal();
        GUILayout.Label("Filter:", GUILayout.Width(50));
        searchQuery = GUILayout.TextField(searchQuery, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        GUILayout.Space(10); // Adds spacing for better UI

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        System.Type type = typeof(HomerVars);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (FieldInfo field in fields)
        {
            if (!string.IsNullOrEmpty(searchQuery) && !field.Name.ToLower().Contains(searchQuery.ToLower()))
                continue; // 🔹 Skip fields that don't match search query

            object value = field.GetValue(null);
            GUILayout.BeginHorizontal();
            GUILayout.Label(field.Name, GUILayout.Width(250));

            if (value is int intValue)
                GUILayout.Label(intValue.ToString(), GUILayout.Width(100));
            else if (value is bool boolValue)
                GUILayout.Label(boolValue ? "True" : "False", GUILayout.Width(100));
            else if (value is string strValue)
                GUILayout.Label(strValue, GUILayout.Width(150));
            else
                GUILayout.Label(value != null ? value.ToString() : "NULL", GUILayout.Width(150));

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}
