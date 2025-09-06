#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Antura.Discover.Editor
{
    public class PublishWindow : EditorWindow
    {
        private Locale selectedLocale;
        private bool includeOnlyPublic = true;
        private Countries filterCountry = Countries.International;
        private bool filterByCountry = false;

        [MenuItem("Antura/Publish Website & Export", priority = 160)]
        public static void Open()
        {
            GetWindow<PublishWindow>(title: "Publish Website & Export").Show();
        }

        private void OnEnable()
        {
            try
            { selectedLocale = LocalizationSettings.SelectedLocale; }
            catch { }
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Publish website", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Locale", GUILayout.Width(80));
                EditorGUI.BeginChangeCheck();
                selectedLocale = LocalePopup(selectedLocale);
                if (EditorGUI.EndChangeCheck())
                    Repaint();
            }

            includeOnlyPublic = EditorGUILayout.ToggleLeft("Include only public quests", includeOnlyPublic);

            using (new EditorGUILayout.HorizontalScope())
            {
                filterByCountry = EditorGUILayout.ToggleLeft("Filter by Country", filterByCountry, GUILayout.Width(140));
                using (new GUIEnabledScope(filterByCountry))
                {
                    filterCountry = (Countries)EditorGUILayout.EnumPopup(filterCountry);
                }
            }

            EditorGUILayout.Space(8);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Export Quests to Folder"))
                {
                    ExportFilteredToFolder();
                }
                if (GUILayout.Button("Publish Website"))
                {
                    PublishAllToDocs();
                }
            }

            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField("Publish parts (only)", EditorStyles.miniBoldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Publish Quests Only"))
                {
                    PublishQuestsOnly();
                }
                if (GUILayout.Button("Publish Cards Only"))
                {
                    PublishCardsOnly();
                }
                if (GUILayout.Button("Publish Words Only"))
                {
                    PublishWordsOnly();
                }
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Publish Activities Only"))
                {
                    PublishActivitiesOnly();
                }
                if (GUILayout.Button("Publish Topics Only"))
                {
                    PublishTopicOnly();
                }
                if (GUILayout.Button("Publish Locations Only"))
                {
                    PublishLocationsOnly();
                }
                if (GUILayout.Button("Export Card Images"))
                {
                    ExportCardImages();
                }
            }

            EditorGUILayout.HelpBox("Select QuestData assets in Project, choose Locale, then Export. 'Publish All' writes markdown into docs.", MessageType.Info);
        }

        private static Locale LocalePopup(Locale current)
        {
            var locales = LocalizationSettings.AvailableLocales?.Locales;
            if (locales == null || locales.Count == 0)
            {
                EditorGUILayout.LabelField("No Locales configured");
                return current;
            }
            int idx = Mathf.Max(0, locales.IndexOf(current));
            var names = locales.Select(l => l?.Identifier.ToString() ?? l?.name ?? "").ToArray();
            idx = EditorGUILayout.Popup(idx, names);
            return locales[Mathf.Clamp(idx, 0, locales.Count - 1)];
        }

        private void ExportFilteredToFolder()
        {
            var guids = AssetDatabase.FindAssets("t:QuestData");
            var quests = new List<QuestData>();
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var q = AssetDatabase.LoadAssetAtPath<QuestData>(assetPath);
                if (q != null)
                    quests.Add(q);
            }
            string folder = EditorUtility.SaveFolderPanel("Export Quest Markdown Files", Application.dataPath, "QuestExports");
            if (string.IsNullOrEmpty(folder))
                return;

            PublishUtils.WithLocale(selectedLocale, () =>
            {
                foreach (var q in quests)
                {
                    if (includeOnlyPublic && !q.IsPublic)
                        continue;
                    if (filterByCountry && q.Country != filterCountry)
                        continue;
                    try
                    {
                        var md = QuestExportUtils.BuildQuestMarkdown(q);
                        var fileName = PublishUtils.GetQuestPublishFileNameForLocale(q, selectedLocale);
                        var path = Path.Combine(folder, fileName);
                        File.WriteAllText(path, md);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[Publish] Failed to export '{q?.Id}': {ex.Message}");
                    }
                }
            });

            EditorUtility.RevealInFinder(folder);
        }

        // Shared implementation to publish all quest markdowns and per-language quest indexes into the given folder.
        private static (int ok, int fail) PublishQuestMarkdowns(string folder, List<QuestData> quests)
        {
            var publishLangs = new[] { "en", "fr", "pl", "it" };
            int ok = 0, fail = 0;

            Countries[] order = new[] {
                Countries.International, Countries.France, Countries.Poland, Countries.Germany, Countries.Greece,
                Countries.Italy, Countries.Portugal, Countries.Spain, Countries.UnitedKingdom,
            };
            Func<Countries, string> countryLabel = c => c == Countries.UnitedKingdom ? "United Kingdom" : c.ToString();

            foreach (var lang in publishLangs)
            {
                var locale = FindLocaleByCode(lang);
                var grouped = new Dictionary<Countries, List<KeyValuePair<string, string>>>();

                PublishUtils.WithLocale(locale, () =>
                {
                    foreach (var q in quests)
                    {
                        if (!q.IsPublic)
                            continue;
                        try
                        {
                            string fileName = PublishUtils.GetQuestPublishFileNameForLocale(q, locale);
                            string outPath = Path.Combine(folder, fileName);

                            string scriptFileName = null;
                            if (q.IsScriptPublic)
                            {
                                scriptFileName = PublishUtils.GetQuestScriptPublishFileNameForLocale(q, locale);
                                string scriptPath = Path.Combine(folder, scriptFileName);
                                var scriptMd = QuestExportUtils.BuildQuestScriptMarkdown(q, includeLanguageMenu: true, locale: locale);
                                File.WriteAllText(scriptPath, scriptMd);
                            }

                            var md = QuestExportUtils.BuildQuestMarkdown(q, includeLanguageMenu: true, scriptPageFileName: scriptFileName, locale: locale);
                            File.WriteAllText(outPath, md);

                            var title = PublishUtils.GetHumanTitle(q)?.Replace("\r", " ").Replace("\n", " ");
                            var code = PublishUtils.GetQuestCode(q);
                            var linkText = string.IsNullOrEmpty(title) ? code : ($"{title} ({code})");
                            if (!grouped.TryGetValue(q.Country, out var list))
                            { list = new List<KeyValuePair<string, string>>(); grouped[q.Country] = list; }
                            list.Add(new KeyValuePair<string, string>(linkText, fileName));
                            ok++;
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"[Publish] Failed publishing '{q?.Id}' ({lang}): {ex.Message}");
                            fail++;
                        }
                    }
                });

                try
                {
                    var indexSb = new System.Text.StringBuilder();
                    indexSb.AppendLine("---");
                    indexSb.AppendLine("title: Quests");
                    indexSb.AppendLine("hide:");
                    indexSb.AppendLine("---");
                    indexSb.AppendLine();
                    indexSb.AppendLine("# Quests");
                    var langs = new (string code, string label)[] { ("en", "english"), ("fr", "french"), ("pl", "polish"), ("it", "italian") };
                    var parts = new List<string>(langs.Length);
                    foreach (var l in langs)
                    {
                        bool isCurrent = lang.StartsWith(l.code, StringComparison.OrdinalIgnoreCase);
                        string file = l.code == "en" ? "index.md" : $"index.{l.code}.md";
                        parts.Add(isCurrent ? l.label : $"[{l.label}](./{file})");
                    }
                    indexSb.AppendLine($"Language: {string.Join(" - ", parts)}");
                    indexSb.AppendLine();

                    foreach (var c in order)
                    {
                        if (!grouped.TryGetValue(c, out var list) || list == null || list.Count == 0)
                            continue;
                        indexSb.AppendLine($"## {countryLabel(c)}");
                        indexSb.AppendLine();
                        foreach (var kv in list.OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
                            indexSb.AppendLine($"- [{kv.Key}](./{kv.Value})");
                        indexSb.AppendLine();
                    }

                    string indexName = lang.StartsWith("en") ? "index.md" : $"index.{lang}.md";
                    string indexPath = Path.Combine(folder, indexName);
                    File.WriteAllText(indexPath, indexSb.ToString());
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[Publish] Failed to write quest index for {lang}: {ex.Message}");
                }
            }

            return (ok, fail);
        }

        public static void PublishAllToDocs()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string discoverRoot = Path.Combine(projectRoot, "docs", "discover");
            string folder = Path.Combine(discoverRoot, "quest");
            Directory.CreateDirectory(discoverRoot);
            Directory.CreateDirectory(folder);

            try
            {
                foreach (var file in Directory.GetFiles(folder, "*.md", SearchOption.TopDirectoryOnly))
                {
                    // Discover folder is fully generated; safe to clear all .md files
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Publish] Could not clean folder: {ex.Message}");
            }

            var guids = AssetDatabase.FindAssets("t:QuestData");
            var quests = new List<QuestData>();
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var q = AssetDatabase.LoadAssetAtPath<QuestData>(assetPath);
                if (q != null && q.Status != Status.Standby && q.Status != Status.Draft)
                    quests.Add(q);
            }

            var result = PublishQuestMarkdowns(folder, quests);
            Debug.Log($"[Publish] Published {result.ok} quest files to {folder}. Failures: {result.fail}");

            try
            {
                var en = FindLocaleByCode("en");
                string activitiesDir = Path.Combine(discoverRoot, "activities");
                string cardsDir = Path.Combine(discoverRoot, "cards");
                string wordsDir = Path.Combine(discoverRoot, "words");
                string topicsDir = Path.Combine(discoverRoot, "topics");
                string locationsDir = Path.Combine(discoverRoot, "locations");
                Directory.CreateDirectory(activitiesDir);
                Directory.CreateDirectory(cardsDir);
                Directory.CreateDirectory(wordsDir);
                Directory.CreateDirectory(topicsDir);
                Directory.CreateDirectory(locationsDir);

                // Activities/Words/Topics remain English-only for now
                PublishUtils.WithLocale(en, () =>
                {
                    var activitiesMd = ActivityExportUtils.BuildActivitiesIndexMarkdown(en);
                    File.WriteAllText(Path.Combine(activitiesDir, "index.md"), activitiesMd);
                    var wordsMd = WordExportUtils.BuildWordsIndexMarkdown(en);
                    File.WriteAllText(Path.Combine(wordsDir, "index.md"), wordsMd);
                    var topicsMd = TopicExportUtils.BuildTopicIndexMarkdown(en);
                    File.WriteAllText(Path.Combine(topicsDir, "index.md"), topicsMd);
                    var locationsMd = LocationsExportUtils.BuildLocationsIndexMarkdown(en);
                    File.WriteAllText(Path.Combine(locationsDir, "index.md"), locationsMd);
                });

                // Multilingual Cards indexes (en, fr, pl, it)
                var cardsLangs = new[] { "en", "fr", "pl", "it" };
                foreach (var lang in cardsLangs)
                {
                    var locale = FindLocaleByCode(lang);
                    PublishUtils.WithLocale(locale, () =>
                    {
                        var cardsMd = CardExportUtils.BuildCardsIndexMarkdown(locale);
                        string file = lang.StartsWith("en") ? "index.md" : $"index.{lang}.md";
                        File.WriteAllText(Path.Combine(cardsDir, file), cardsMd);
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Publish] Failed generating Activities/Cards/Words indexes: {ex.Message}");
            }

            EditorUtility.RevealInFinder(folder);
        }

        public static void PublishQuestsOnly()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string discoverRoot = Path.Combine(projectRoot, "docs", "discover");
            string folder = Path.Combine(discoverRoot, "quest");
            Directory.CreateDirectory(discoverRoot);
            Directory.CreateDirectory(folder);

            try
            {
                foreach (var file in Directory.GetFiles(folder, "*.md", SearchOption.TopDirectoryOnly))
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Publish] Could not clean quest folder: {ex.Message}");
            }

            var guids = AssetDatabase.FindAssets("t:QuestData");
            var quests = new List<QuestData>();
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var q = AssetDatabase.LoadAssetAtPath<QuestData>(assetPath);
                if (q != null && q.Status != Status.Standby && q.Status != Status.Draft)
                    quests.Add(q);
            }

            var res = PublishQuestMarkdowns(folder, quests);
            Debug.Log($"[Publish] Published {res.ok} quest files to {folder}. Failures: {res.fail}");
            EditorUtility.RevealInFinder(folder);
        }

        public static void PublishCardsOnly()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string discoverRoot = Path.Combine(projectRoot, "docs", "discover");
            string cardsDir = Path.Combine(discoverRoot, "cards");
            Directory.CreateDirectory(cardsDir);
            try
            {
                var cardLangs = new[] { "en", "fr", "pl", "it" };
                foreach (var lang in cardLangs)
                {
                    var locale = FindLocaleByCode(lang);
                    PublishUtils.WithLocale(locale, () =>
                    {
                        var cardsMd = CardExportUtils.BuildCardsIndexMarkdown(locale);
                        string file = lang.StartsWith("en") ? "index.md" : $"index.{lang}.md";
                        File.WriteAllText(Path.Combine(cardsDir, file), cardsMd);
                    });
                }
                Debug.Log($"[Publish] Published Cards index to {cardsDir}");
                EditorUtility.RevealInFinder(cardsDir);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Publish] Failed generating Cards index: {ex.Message}");
            }
        }

        public static void PublishWordsOnly()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string discoverRoot = Path.Combine(projectRoot, "docs", "discover");
            string wordsDir = Path.Combine(discoverRoot, "words");
            Directory.CreateDirectory(wordsDir);
            try
            {
                var en = FindLocaleByCode("en");
                PublishUtils.WithLocale(en, () =>
                {
                    var wordsMd = WordExportUtils.BuildWordsIndexMarkdown(en);
                    File.WriteAllText(Path.Combine(wordsDir, "index.md"), wordsMd);
                });
                Debug.Log($"[Publish] Published Words index to {wordsDir}");
                EditorUtility.RevealInFinder(wordsDir);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Publish] Failed generating Words index: {ex.Message}");
            }
        }

        public static void PublishActivitiesOnly()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string discoverRoot = Path.Combine(projectRoot, "docs", "discover");
            string activitiesDir = Path.Combine(discoverRoot, "activities");
            Directory.CreateDirectory(activitiesDir);
            try
            {
                var en = FindLocaleByCode("en");
                PublishUtils.WithLocale(en, () =>
                {
                    var activitiesMd = ActivityExportUtils.BuildActivitiesIndexMarkdown(en);
                    File.WriteAllText(Path.Combine(activitiesDir, "index.md"), activitiesMd);
                });
                Debug.Log($"[Publish] Published Activities index to {activitiesDir}");
                EditorUtility.RevealInFinder(activitiesDir);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Publish] Failed generating Activities index: {ex.Message}");
            }
        }

        public static void PublishTopicOnly()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string discoverRoot = Path.Combine(projectRoot, "docs", "discover");
            string topicsDir = Path.Combine(discoverRoot, "topics");
            Directory.CreateDirectory(topicsDir);
            try
            {
                var en = FindLocaleByCode("en");
                PublishUtils.WithLocale(en, () =>
                {
                    var topicMd = TopicExportUtils.BuildTopicIndexMarkdown(en);
                    File.WriteAllText(Path.Combine(topicsDir, "index.md"), topicMd);
                });
                Debug.Log($"[Publish] Published Topic index to {topicsDir}");
                EditorUtility.RevealInFinder(topicsDir);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Publish] Failed generating Topic index: {ex.Message}");
            }
        }

        public static void PublishLocationsOnly()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string discoverRoot = Path.Combine(projectRoot, "docs", "discover");
            string locationsDir = Path.Combine(discoverRoot, "locations");
            Directory.CreateDirectory(locationsDir);
            try
            {
                var en = FindLocaleByCode("en");
                PublishUtils.WithLocale(en, () =>
                {
                    var topicMd = LocationsExportUtils.BuildLocationsIndexMarkdown(en);
                    File.WriteAllText(Path.Combine(locationsDir, "index.md"), topicMd);
                });
                Debug.Log($"[Publish] Published Topic index to {locationsDir}");
                EditorUtility.RevealInFinder(locationsDir);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Publish] Failed generating Topic index: {ex.Message}");
            }
        }

        // Exports all CardData main image assets to /docs/assets/img/discover/cards/{cardId}.jpg
        // - Max width 640 (preserve aspect)
        // - JPG quality 85%
        // - Skips if no image or already up-to-date (same file size & newer timestamp)
        public static void ExportCardImages()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string outDir = Path.Combine(projectRoot, "docs", "assets", "img", "discover", "cards");
            Directory.CreateDirectory(outDir);

            var sw = System.Diagnostics.Stopwatch.StartNew();
            int exported = 0, skipped = 0, missing = 0;

            try
            {
                string[] guids = AssetDatabase.FindAssets("t:CardData");
                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    var card = AssetDatabase.LoadAssetAtPath<CardData>(path);
                    if (card == null)
                        continue;
                    if (card.ImageAsset == null || card.ImageAsset.Image == null)
                    {
                        missing++;
                        continue;
                    }
                    var sprite = card.ImageAsset.Image;
                    string fileName = card.Id + ".jpg";
                    string dstPath = Path.Combine(outDir, fileName);

                    Texture2D srcTex = sprite.texture;
                    // Compute target width/height
                    int srcWidth = sprite.rect.width > 0 ? (int)sprite.rect.width : srcTex.width;
                    int srcHeight = sprite.rect.height > 0 ? (int)sprite.rect.height : srcTex.height;
                    int targetWidth = Mathf.Min(640, srcWidth);
                    int targetHeight = Mathf.RoundToInt(srcHeight * (targetWidth / (float)srcWidth));

                    // Simple cache check by size (width/height) stored in filename meta? We'll just overwrite if missing or src is newer.
                    bool needsExport = true;
                    if (File.Exists(dstPath))
                    {
                        DateTime fileTime = File.GetLastWriteTimeUtc(dstPath);
                        // If source texture asset is older than exported file and sizes match requested, skip
                        string assetFullPath = Path.GetFullPath(path);
                        DateTime assetTime = File.GetLastWriteTimeUtc(assetFullPath);
                        if (assetTime <= fileTime)
                        {
                            // Could also check resolution marker by reading file length but that's overkill: skip.
                            needsExport = false;
                        }
                    }
                    if (!needsExport)
                    {
                        skipped++;
                        continue;
                    }

                    // Extract pixels (cropping sprite if needed)
                    Texture2D readable = GetReadableTexture(srcTex, sprite);
                    if (readable.width != targetWidth)
                    {
                        Texture2D resized = new Texture2D(targetWidth, targetHeight, TextureFormat.RGB24, false, false);
                        ResizeBilinear(readable, resized);
                        readable = resized;
                    }

                    byte[] jpg = readable.EncodeToJPG(85);
                    File.WriteAllBytes(dstPath, jpg);
                    exported++;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Publish] Card images export failed: {ex.Message}\n{ex.StackTrace}");
            }

            sw.Stop();
            Debug.Log($"[Publish] Card images export complete. Exported: {exported}, Skipped: {skipped}, Missing: {missing}. Time: {sw.ElapsedMilliseconds} ms -> {outDir}");
            EditorUtility.RevealInFinder(outDir);
        }

        // Returns a readable Texture2D representing ONLY the sprite area.
        private static Texture2D GetReadableTexture(Texture2D source, Sprite sprite)
        {
            // If sprite covers entire texture and is readable, return directly (clone to ensure readable)
            Rect r = sprite.rect;
            Texture2D tmp = new Texture2D((int)r.width, (int)r.height, TextureFormat.RGBA32, false);
            try
            {
                RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                Graphics.Blit(source, rt);
                RenderTexture prev = RenderTexture.active;
                RenderTexture.active = rt;
                Texture2D full = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
                full.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
                full.Apply();
                RenderTexture.active = prev;
                RenderTexture.ReleaseTemporary(rt);

                // Crop sprite area
                Color[] pixels = full.GetPixels((int)r.x, (int)r.y, (int)r.width, (int)r.height);
                tmp.SetPixels(pixels);
                tmp.Apply();
                UnityEngine.Object.DestroyImmediate(full);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Publish] Failed extracting sprite texture: {ex.Message}");
            }
            return tmp;
        }

        // Bilinear resize from src -> dst
        private static void ResizeBilinear(Texture2D src, Texture2D dst)
        {
            int w = dst.width;
            int h = dst.height;
            float invW = 1f / (w - 1);
            float invH = 1f / (h - 1);
            Color[] srcPixels = src.GetPixels();
            int sw = src.width;
            int sh = src.height;
            Color[] dstPixels = new Color[w * h];
            for (int y = 0; y < h; y++)
            {
                float v = y * invH;
                float sy = v * (sh - 1);
                int y0 = (int)sy;
                int y1 = Mathf.Min(y0 + 1, sh - 1);
                float fy = sy - y0;
                for (int x = 0; x < w; x++)
                {
                    float u = x * invW;
                    float sx = u * (sw - 1);
                    int x0 = (int)sx;
                    int x1 = Mathf.Min(x0 + 1, sw - 1);
                    float fx = sx - x0;

                    Color c00 = srcPixels[y0 * sw + x0];
                    Color c10 = srcPixels[y0 * sw + x1];
                    Color c01 = srcPixels[y1 * sw + x0];
                    Color c11 = srcPixels[y1 * sw + x1];
                    Color cx0 = Color.Lerp(c00, c10, fx);
                    Color cx1 = Color.Lerp(c01, c11, fx);
                    dstPixels[y * w + x] = Color.Lerp(cx0, cx1, fy);
                }
            }
            dst.SetPixels(dstPixels);
            dst.Apply();
        }

        private static Locale FindLocaleByCode(string code)
        {
            try
            {
                var locales = LocalizationSettings.AvailableLocales?.Locales;
                if (locales == null || locales.Count == 0)
                    return null;
                var exact = locales.FirstOrDefault(l => string.Equals(l?.Identifier.Code, code, StringComparison.OrdinalIgnoreCase));
                if (exact != null)
                    return exact;
                return locales.FirstOrDefault(l => l?.Identifier.Code != null && l.Identifier.Code.StartsWith(code, StringComparison.OrdinalIgnoreCase));
            }
            catch { return null; }
        }

        private readonly struct GUIEnabledScope : IDisposable
        {
            private readonly bool prev;
            public GUIEnabledScope(bool enabled)
            {
                prev = GUI.enabled;
                GUI.enabled = enabled;
            }
            public void Dispose() { GUI.enabled = prev; }
        }
    }
}
#endif
