#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

// Localization
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;

// Editor Localization APIs
using UnityEditor.Localization;
using UnityEditor.Localization.Reporting;

// Your data
using Antura.Discover;


public class WordDataLocalizationImporter : EditorWindow
{
    private string collectionName = "Words";
    private SystemLanguage englishLanguage = SystemLanguage.English;

    [MenuItem("Tools/Antura/Import WordData to Localization")]
    private static void Open()
    {
        GetWindow<WordDataLocalizationImporter>("WordData → Localization");
    }

    private void OnGUI()
    {
        GUILayout.Label("Import WordData into a String Table Collection", EditorStyles.boldLabel);
        collectionName = EditorGUILayout.TextField("Collection Name", collectionName);
        englishLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("English Locale", englishLanguage);

        if (GUILayout.Button("Scan Project & Import"))
        {
            ImportAll();
        }
    }

    private void ImportAll()
    {
        try
        {
            // 1) Ensure Collection exists
            var collection = LocalizationEditorSettings.GetStringTableCollection(collectionName);


            // 2) Ensure English Locale exists
            var allLocales = LocalizationEditorSettings.GetLocales();
            var enLocale = allLocales.FirstOrDefault(l => l.Identifier.CultureInfo.TwoLetterISOLanguageName == "en");

            if (enLocale == null)
            {
                // Fallback: create from SystemLanguage selection
                enLocale = Locale.CreateLocale(englishLanguage);
                LocalizationEditorSettings.AddLocale(enLocale);
                Debug.Log($"Created Locale: {enLocale.name}");
            }

            var enTable = collection.StringTables.FirstOrDefault(t => t != null && t.LocaleIdentifier == enLocale.Identifier);


            // 4) Find all WordData assets
            var guids = AssetDatabase.FindAssets("t:WordData");
            int created = 0, updated = 0, linked = 0;

            var shared = collection.SharedData;

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var wd = AssetDatabase.LoadAssetAtPath<WordData>(path);
                if (wd == null)
                    continue;

                if (string.IsNullOrEmpty(wd.Id))
                {
                    Debug.LogWarning($"[WordData] Missing Id: {path}");
                    continue;
                }


                // 5) Ensure shared key exists
                var sharedEntry = shared.GetEntry(wd.Id);
                if (sharedEntry == null)
                {
                    sharedEntry = shared.AddKey(wd.Id);
                    created++;
                }

                // 6) Add/Update EN value
                var entry = enTable.GetEntry(sharedEntry.Id);
                if (entry == null)
                {
                    enTable.AddEntry(sharedEntry.Id, wd.TextEn ?? string.Empty);
                    created++;
                }
                else
                {
                    // Update only if different to avoid dirtying assets unnecessarily
                    if (entry.Value != (wd.TextEn ?? string.Empty))
                    {
                        entry.Value = wd.TextEn ?? string.Empty;
                        updated++;
                    }
                }

                // 7) Link the LocalizedString on the asset
                if (wd.TextLocalized == null)
                {
                    wd.TextLocalized = new LocalizedString();
                }

                bool needsLinkUpdate = false;

                if (wd.TextLocalized.TableReference.TableCollectionName != collectionName)
                {
                    wd.TextLocalized.TableReference = collectionName;
                    needsLinkUpdate = true;
                }

                // Accept either key name or GUID id; here we use the key string
                if (wd.TextLocalized.TableEntryReference.Key != wd.Id)
                {
                    wd.TextLocalized.TableEntryReference = wd.Id;
                    needsLinkUpdate = true;
                }

                if (needsLinkUpdate)
                {
                    linked++;
                    EditorUtility.SetDirty(wd);
                }
            }

            // 8) Save changes
            EditorUtility.SetDirty(shared);
            EditorUtility.SetDirty(enTable);
            AssetDatabase.SaveAssets();

            Debug.Log($"WordData → Localization done. Created: {created}, Updated: {updated}, Linked: {linked}. Collection: '{collectionName}', EN Locale: '{enLocale.Identifier}'");
            EditorUtility.DisplayDialog("Import Complete",
                $"Collection: {collectionName}\nEnglish: {enLocale.Identifier}\n\nCreated: {created}\nUpdated: {updated}\nLinked: {linked}",
                "OK");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Import failed: {ex.Message}\n{ex}");
            EditorUtility.DisplayDialog("Import Failed", ex.Message, "OK");
        }
    }
}

#endif

