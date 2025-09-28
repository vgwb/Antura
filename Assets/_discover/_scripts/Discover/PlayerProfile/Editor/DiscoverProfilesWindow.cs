#if UNITY_EDITOR
using System.Data.Common;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover.Editor
{
    /// <summary>
    /// Tiny editor window to browse/create/delete Discover profiles stored on disk.
    /// - Lists profiles from index.json (id, uuid, displayName).
    /// - Create new profile via legacy UUID + optional display name.
    /// - Load, Set Current, Delete, Reveal in Finder/Explorer.
    /// </summary>
    public class DiscoverProfilesWindow : EditorWindow
    {
        private DiscoverProfileManager store;
        private Vector2 scroll;
        private string legacyUuidInput = "";
        private string displayNameInput = "";

        [MenuItem("Antura/Discover/Player Profiles", priority = 150)]
        public static void Open()
        {
            var win = GetWindow<DiscoverProfilesWindow>("Discover Profiles");
            win.minSize = new Vector2(520, 380);
            win.Show();
        }

        private void OnEnable()
        {
            store = new DiscoverProfileManager(); // default folder under persistentDataPath
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Discover Profiles", EditorStyles.boldLabel);

            // Current pointer
            using (new EditorGUILayout.VerticalScope("box"))
            {
                var currentId = PlayerPrefs.GetString(DiscoverProfileManager.CurrentIdPrefsKey, "<none>");
                EditorGUILayout.LabelField("Current Profile Id:", currentId);
                if (GUILayout.Button("Open Profiles Folder"))
                {
                    EditorUtility.RevealInFinder(Path.Combine(Application.persistentDataPath, "discover_profiles"));
                }
            }

            // Create / Link section
            EditorGUILayout.Space();
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Create / Link by Legacy UUID", EditorStyles.boldLabel);
                legacyUuidInput = EditorGUILayout.TextField("Legacy UUID", legacyUuidInput);
                displayNameInput = EditorGUILayout.TextField("Display Name (optional)", displayNameInput);

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Create / Load"))
                    {
                        if (!string.IsNullOrEmpty(legacyUuidInput))
                        {
                            var platform = IsMobileEditor() ? "mobile" : "desktop";
                            var profile = store.LoadOrCreateByLegacyUuid(legacyUuidInput, null, platform, Application.version);
                            if (!string.IsNullOrEmpty(displayNameInput))
                            {
                                profile.profile.displayName = displayNameInput;
                                store.Save(profile);
                            }
                            store.SetCurrent(profile.profile.id);
                            Repaint();
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Missing UUID", "Please enter a legacy UUID.", "OK");
                        }
                    }

                    if (GUILayout.Button("Clear Inputs"))
                    {
                        legacyUuidInput = "";
                        displayNameInput = "";
                    }
                }
            }

            // List
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("All Profiles", EditorStyles.boldLabel);
            var list = store.ListProfiles();

            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var h in list)
            {
                using (new EditorGUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField($"ID: {h.id}");
                    EditorGUILayout.LabelField($"UUID: {h.uuid}");
                    EditorGUILayout.LabelField($"Name: {h.displayName}");

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Load JSON"))
                        {
                            var p = store.LoadById(h.id);
                            if (p != null)
                            {
                                var json = JsonConvert.SerializeObject(p, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                                var temp = Path.Combine(Application.dataPath, $"../Temp_{h.id}.json");
                                File.WriteAllText(temp, json);
                                EditorUtility.RevealInFinder(temp);
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("Load Failed", $"Profile file for {h.id} not found or invalid.", "OK");
                            }
                        }

                        if (GUILayout.Button("Set Current"))
                        {
                            store.SetCurrent(h.id);
                            Repaint();
                        }

                        if (GUILayout.Button("Reveal File"))
                        {
                            var path = Path.Combine(Application.persistentDataPath, "discover_profiles", $"{h.id}.json");
                            EditorUtility.RevealInFinder(path);
                        }

                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("Delete"))
                        {
                            if (EditorUtility.DisplayDialog("Delete Profile",
                                    $"Delete {h.id} ({h.displayName})? This cannot be undone.", "Delete", "Cancel"))
                            {
                                if (!store.Delete(h.id))
                                    EditorUtility.DisplayDialog("Delete Failed", "Profile not found in index.", "OK");
                                Repaint();
                            }
                        }
                        GUI.backgroundColor = Color.white;
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            // Footer
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Persistent Data Path:\n{Application.persistentDataPath}", EditorStyles.miniLabel);
        }

        private bool IsMobileEditor()
        {
            // Simple hint for platform tagging when creating via editor window
#if UNITY_IOS || UNITY_ANDROID
            return true;
#else
            return false;
#endif
        }
    }
}
#endif
