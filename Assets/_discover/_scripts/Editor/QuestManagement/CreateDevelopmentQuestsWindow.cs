#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Yarn.Unity;

namespace Antura.Discover.EditorTools
{
    public sealed class CreateDevelopmentQuestsWindow : EditorWindow
    {
        private const string DevelopmentFolder = "Assets/_discover/_quests/__development";
        private int _startNumber = 103;
        private int _count = 1;
        private string _templateFolder;
        private readonly List<string> _templateFolders = new List<string>();
        private int _templateIndex;

        [MenuItem("Antura/Quest/Create Development Quests...", priority = 24)]
        public static void ShowWindow()
        {
            var window = GetWindow<CreateDevelopmentQuestsWindow>(true, "Create Development Quests", true);
            window.minSize = new Vector2(460f, 190f);
            window.RefreshTemplateFolder();
            window.ShowUtility();
        }

        private void OnEnable()
        {
            RefreshTemplateFolder();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Clone a chosen development quest template into new DEV folders.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.Space(8f);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Template", GUILayout.Width(60f));
                using (new EditorGUI.DisabledScope(_templateFolders.Count == 0))
                {
                    _templateIndex = EditorGUILayout.Popup(_templateIndex, _templateFolders.Select(GetTemplateLabel).ToArray());
                }
                if (GUILayout.Button("Refresh", GUILayout.Width(70f)))
                {
                    RefreshTemplateFolder();
                }
            }

            _startNumber = EditorGUILayout.IntField("Start number", _startNumber);
            _count = EditorGUILayout.IntField("How many quests", _count);

            EditorGUILayout.Space(10f);

            using (new EditorGUI.DisabledScope(!CanCreate()))
            {
                if (GUILayout.Button("Create quests", GUILayout.Height(30f)))
                {
                    CreateQuests();
                }
            }
        }

        private bool CanCreate()
        {
            return !string.IsNullOrEmpty(_templateFolder) && _startNumber > 0 && _count > 0;
        }

        private void RefreshTemplateFolder()
        {
            _templateFolders.Clear();

            if (AssetDatabase.IsValidFolder(DevelopmentFolder))
            {
                var root = GetFullPath(DevelopmentFolder);
                var candidates = new List<(int number, string path)>();
                foreach (var folder in Directory.GetDirectories(root, "DEV_*", SearchOption.TopDirectoryOnly))
                {
                    var folderName = Path.GetFileName(folder);
                    if (!TryParseDevNumber(folderName, out var number))
                        continue;

                    var assetPath = ToAssetPath(folder);
                    var questDataPath = Path.Combine(assetPath, $"{folderName} title - Quest Data.asset").Replace("\\", "/");
                    if (AssetDatabase.LoadAssetAtPath<QuestData>(questDataPath) != null)
                        candidates.Add((number, assetPath));
                }

                foreach (var candidate in candidates.OrderBy(c => c.number))
                    _templateFolders.Add(candidate.path);
            }

            if (_templateFolders.Count == 0)
            {
                _templateFolder = null;
                _templateIndex = 0;
                return;
            }

            if (_templateIndex < 0 || _templateIndex >= _templateFolders.Count)
                _templateIndex = _templateFolders.Count - 1;

            _templateFolder = _templateFolders[_templateIndex];
        }

        private void CreateQuests()
        {
            RefreshTemplateFolder();
            if (string.IsNullOrEmpty(_templateFolder))
            {
                EditorUtility.DisplayDialog("Create Development Quests", "No template quest folder was found in Assets/_discover/_quests/__development.", "OK");
                return;
            }

            if (_startNumber <= 0 || _count <= 0)
            {
                EditorUtility.DisplayDialog("Create Development Quests", "Start number and quest count must be greater than zero.", "OK");
                return;
            }

            var targetNumbers = Enumerable.Range(_startNumber, _count).ToList();
            var targetFolders = targetNumbers.Select(n => Path.Combine(DevelopmentFolder, $"DEV_{n}").Replace("\\", "/")).ToList();

            var conflicts = targetFolders.Where(AssetDatabase.IsValidFolder).ToList();
            if (conflicts.Count > 0)
            {
                EditorUtility.DisplayDialog(
                    "Create Development Quests",
                    "One or more target folders already exist:\n\n" + string.Join("\n", conflicts),
                    "OK");
                return;
            }

            AssetDatabase.StartAssetEditing();
            try
            {
                for (int i = 0; i < targetNumbers.Count; i++)
                {
                    var number = targetNumbers[i];
                    var destFolder = targetFolders[i];
                    var progress = (float)i / Math.Max(1, targetNumbers.Count);
                    EditorUtility.DisplayProgressBar("Create Development Quests", $"Creating DEV_{number}", progress);
                    CloneQuestFolder(_templateFolder, destFolder, number);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CreateDevelopmentQuests] Failed: {ex.Message}\n{ex}");
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                EditorUtility.ClearProgressBar();
            }

            EditorUtility.DisplayDialog("Create Development Quests", $"Created {targetNumbers.Count} quest folder(s).", "OK");
        }

        private static void CloneQuestFolder(string sourceFolder, string destinationFolder, int questNumber)
        {
            var sourceUpper = Path.GetFileName(sourceFolder);
            var sourceLower = sourceUpper.ToLowerInvariant();
            var destinationUpper = $"DEV_{questNumber}";
            var destinationLower = $"dev_{questNumber}";
            var createdFolders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            CreateFolderHierarchy(destinationFolder, createdFolders);

            var sourceRootFull = GetFullPath(sourceFolder);
            var sourceFiles = Directory.GetFiles(sourceRootFull, "*", SearchOption.AllDirectories)
                .Where(ShouldCopyFile)
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var sourceDirectories = Directory.GetDirectories(sourceRootFull, "*", SearchOption.AllDirectories)
                .OrderBy(path => path.Length)
                .ToList();

            foreach (var directory in sourceDirectories)
            {
                var relative = GetRelativePath(sourceRootFull, directory);
                if (IsLocalizationPath(relative))
                    continue;
                var renamedRelative = ReplaceQuestTokens(relative, sourceUpper, sourceLower, destinationUpper, destinationLower);
                CreateFolderHierarchy(Path.Combine(destinationFolder, renamedRelative).Replace("\\", "/"), createdFolders);
            }

            var copiedPaths = new List<(string source, string destination)>();
            foreach (var sourceFile in sourceFiles)
            {
                var relative = GetRelativePath(sourceRootFull, sourceFile);
                if (IsLocalizationPath(relative))
                    continue;
                var renamedRelative = ReplaceQuestTokens(relative, sourceUpper, sourceLower, destinationUpper, destinationLower);
                var destinationPath = Path.Combine(destinationFolder, renamedRelative).Replace("\\", "/");

                if (!AssetDatabase.CopyAsset(ToAssetPath(sourceFile), destinationPath))
                    throw new IOException($"Failed to copy '{sourceFile}' to '{destinationPath}'.");

                copiedPaths.Add((ToAssetPath(sourceFile), destinationPath));
            }

            AssetDatabase.Refresh();

            var guidMap = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var pair in copiedPaths)
            {
                var sourceGuid = AssetDatabase.AssetPathToGUID(pair.source);
                var destinationGuid = AssetDatabase.AssetPathToGUID(pair.destination);
                if (!string.IsNullOrEmpty(sourceGuid) && !string.IsNullOrEmpty(destinationGuid) && !guidMap.ContainsKey(sourceGuid))
                    guidMap.Add(sourceGuid, destinationGuid);
            }

            foreach (var pair in copiedPaths)
            {
                RewriteCopiedTextAsset(pair.destination, sourceUpper, sourceLower, destinationUpper, destinationLower, questNumber, guidMap);
            }

            EnsureSceneUsesQuestPrefab(sourceFolder, destinationFolder, sourceUpper, destinationUpper, destinationLower);
            EnsureQuestDataReferences(destinationFolder, destinationUpper, destinationLower, questNumber);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void EnsureSceneUsesQuestPrefab(string sourceFolder, string destinationFolder, string sourceUpper, string destinationUpper, string destinationLower)
        {
            var sourcePrefabPath = Path.Combine(sourceFolder, $"{sourceUpper} title - Quest Prefab.prefab").Replace("\\", "/");
            var destinationPrefabPath = Path.Combine(destinationFolder, $"{destinationUpper} title - Quest Prefab.prefab").Replace("\\", "/");
            var scenePath = Path.Combine(destinationFolder, $"discover {destinationLower}.unity").Replace("\\", "/");

            var sourcePrefabGuid = AssetDatabase.AssetPathToGUID(sourcePrefabPath);
            var destinationPrefabGuid = AssetDatabase.AssetPathToGUID(destinationPrefabPath);
            if (string.IsNullOrEmpty(sourcePrefabGuid) || string.IsNullOrEmpty(destinationPrefabGuid))
                return;

            var fullScenePath = GetFullPath(scenePath);
            if (!File.Exists(fullScenePath))
                return;

            var text = File.ReadAllText(fullScenePath, Encoding.UTF8);
            var updated = text.Replace(sourcePrefabGuid, destinationPrefabGuid);
            if (!string.Equals(updated, text, StringComparison.Ordinal))
            {
                File.WriteAllText(fullScenePath, updated, Encoding.UTF8);
                AssetDatabase.ImportAsset(scenePath, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            }
        }

        private static void EnsureQuestDataReferences(string destinationFolder, string destinationUpper, string destinationLower, int questNumber)
        {
            var questDataPath = Path.Combine(destinationFolder, $"{destinationUpper} title - Quest Data.asset").Replace("\\", "/");
            var yarnProjectPath = Path.Combine(destinationFolder, $"{destinationUpper} title - Yarn Project.yarnproject").Replace("\\", "/");
            var yarnScriptPath = Path.Combine(destinationFolder, $"{destinationUpper} title - Yarn Script.yarn").Replace("\\", "/");
            var questPrefabPath = Path.Combine(destinationFolder, $"{destinationUpper} title - Quest Prefab.prefab").Replace("\\", "/");
            var designNotesPath = Path.Combine(destinationFolder, $"{destinationUpper} README design notes.md").Replace("\\", "/");

            var questData = AssetDatabase.LoadAssetAtPath<QuestData>(questDataPath);
            if (questData == null)
                return;

            var yarnProject = AssetDatabase.LoadAssetAtPath<YarnProject>(yarnProjectPath);
            var yarnScript = AssetDatabase.LoadAssetAtPath<TextAsset>(yarnScriptPath);
            var designNotes = AssetDatabase.LoadAssetAtPath<TextAsset>(designNotesPath);
            var questPrefabGuid = AssetDatabase.AssetPathToGUID(questPrefabPath);
            var questManagerInPrefab = AssetDatabase.LoadAllAssetsAtPath(questPrefabPath).OfType<QuestManager>().FirstOrDefault();

            bool changed = false;
            if (questData.YarnProject != yarnProject)
            {
                questData.YarnProject = yarnProject;
                changed = true;
            }

            if (questData.YarnScript != yarnScript)
            {
                questData.YarnScript = yarnScript;
                changed = true;
            }

            if (questData.AdditionalResources != designNotes)
            {
                questData.AdditionalResources = designNotes;
                changed = true;
            }

            var expectedTitle = $"Template {destinationUpper}";
            if (!string.Equals(questData.TitleEn, expectedTitle, StringComparison.Ordinal))
            {
                questData.TitleEn = expectedTitle;
                changed = true;
            }

            var expectedIdDisplay = questNumber.ToString();
            if (!string.Equals(questData.IdDisplay, expectedIdDisplay, StringComparison.Ordinal))
            {
                questData.IdDisplay = expectedIdDisplay;
                changed = true;
            }

            var expectedScene = $"discover {destinationLower}";
            if (!string.Equals(questData.scene, expectedScene, StringComparison.Ordinal))
            {
                questData.scene = expectedScene;
                changed = true;
            }

            var serializedQuestData = new SerializedObject(questData);
            var questPrefabProp = serializedQuestData.FindProperty("questPrefab");
            if (questPrefabProp != null)
            {
                var assetGuidProp = questPrefabProp.FindPropertyRelative("m_AssetGUID");
                if (assetGuidProp != null && !string.Equals(assetGuidProp.stringValue, questPrefabGuid, StringComparison.Ordinal))
                {
                    assetGuidProp.stringValue = questPrefabGuid;
                    changed = true;
                }
            }

            if (questManagerInPrefab != null)
            {
                var serializedQuestManager = new SerializedObject(questManagerInPrefab);
                var currentQuestProp = serializedQuestManager.FindProperty("CurrentQuest");
                if (currentQuestProp != null && currentQuestProp.objectReferenceValue != questData)
                {
                    currentQuestProp.objectReferenceValue = questData;
                    serializedQuestManager.ApplyModifiedPropertiesWithoutUndo();
                    EditorUtility.SetDirty(questManagerInPrefab);
                }
            }

            if (changed)
            {
                serializedQuestData.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(questData);
            }
        }

        private static void RewriteCopiedTextAsset(
            string assetPath,
            string sourceUpper,
            string sourceLower,
            string destinationUpper,
            string destinationLower,
            int questNumber,
            IReadOnlyDictionary<string, string> guidMap)
        {
            if (!IsRewritableTextAsset(assetPath))
                return;

            var fullPath = GetFullPath(assetPath);
            if (!File.Exists(fullPath))
                return;

            var text = File.ReadAllText(fullPath, Encoding.UTF8);
            var updated = text;
            var destinationGuid = AssetDatabase.AssetPathToGUID(assetPath);

            updated = updated.Replace(sourceUpper, destinationUpper);
            updated = updated.Replace(sourceLower, destinationLower);
            updated = updated.Replace($"IdDisplay: {ExtractDevNumber(sourceUpper)}", $"IdDisplay: {questNumber}");
            updated = updated.Replace($"name: {sourceUpper}", $"name: {destinationUpper}");

            if (Path.GetFileName(assetPath).EndsWith("Yarn Script.yarn", StringComparison.OrdinalIgnoreCase))
            {
                updated = SetYarnFirstLineToQuestId(updated, destinationLower);
            }

            if (Path.GetFileName(assetPath).IndexOf("Quest Data.asset", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                updated = System.Text.RegularExpressions.Regex.Replace(
                    updated,
                    @"QuestStringsTable:\s*\n(?:\s+.*\n)*?\s*QuestAssetsTable:",
                    "QuestStringsTable:\n    m_TableReference:\n      m_TableCollectionName: \n  QuestAssetsTable:",
                    System.Text.RegularExpressions.RegexOptions.Singleline);
                updated = System.Text.RegularExpressions.Regex.Replace(
                    updated,
                    @"QuestAssetsTable:\s*\n(?:\s+.*\n)*?\s*references:",
                    "QuestAssetsTable:\n    m_TableReference:\n      m_TableCollectionName: \n  references:",
                    System.Text.RegularExpressions.RegexOptions.Singleline);

                var questFolder = Path.GetDirectoryName(assetPath)?.Replace("\\", "/") ?? string.Empty;
                var yarnProjectPath = Path.Combine(questFolder, $"{destinationUpper} title - Yarn Project.yarnproject").Replace("\\", "/");
                var yarnScriptPath = Path.Combine(questFolder, $"{destinationUpper} title - Yarn Script.yarn").Replace("\\", "/");
                var questPrefabPath = Path.Combine(questFolder, $"{destinationUpper} title - Quest Prefab.prefab").Replace("\\", "/");

                var yarnProjectGuid = AssetDatabase.AssetPathToGUID(yarnProjectPath);
                var yarnScriptGuid = AssetDatabase.AssetPathToGUID(yarnScriptPath);
                var questPrefabGuid = AssetDatabase.AssetPathToGUID(questPrefabPath);

                if (!string.IsNullOrEmpty(yarnProjectGuid))
                {
                    updated = System.Text.RegularExpressions.Regex.Replace(
                        updated,
                        @"YarnProject:\s*\{fileID:\s*[-0-9]+,\s*guid:\s*[0-9a-fA-F]+,\s*\n\s*type:\s*3\}",
                        $"YarnProject: {{fileID: 7545569452688597077, guid: {yarnProjectGuid},\n    type: 3}}");
                }

                if (!string.IsNullOrEmpty(yarnScriptGuid))
                {
                    updated = System.Text.RegularExpressions.Regex.Replace(
                        updated,
                        @"YarnScript:\s*\{fileID:\s*[-0-9]+,\s*guid:\s*[0-9a-fA-F]+,\s*\n\s*type:\s*3\}",
                        $"YarnScript: {{fileID: -8253061345870894857, guid: {yarnScriptGuid},\n    type: 3}}");
                }

                if (!string.IsNullOrEmpty(questPrefabGuid))
                {
                    updated = System.Text.RegularExpressions.Regex.Replace(
                        updated,
                        @"questPrefab:\s*\n\s*m_AssetGUID:\s*[0-9a-fA-F]+",
                        $"questPrefab:\n    m_AssetGUID: {questPrefabGuid}");
                }

                updated = System.Text.RegularExpressions.Regex.Replace(
                    updated,
                    @"TitleEn:\s*.*",
                    $"TitleEn: Template {destinationUpper}");
            }

            foreach (var pair in guidMap)
            {
                updated = updated.Replace(pair.Key, pair.Value);
            }

            if (Path.GetFileName(assetPath).IndexOf("Shared Data", StringComparison.OrdinalIgnoreCase) >= 0 && !string.IsNullOrEmpty(destinationGuid))
            {
                updated = System.Text.RegularExpressions.Regex.Replace(
                    updated,
                    @"m_TableCollectionNameGuidString:\s*[0-9a-fA-F]+",
                    $"m_TableCollectionNameGuidString: {destinationGuid}");
            }

            if (!string.Equals(updated, text, StringComparison.Ordinal))
            {
                File.WriteAllText(fullPath, updated, Encoding.UTF8);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            }
        }

        private static string SetYarnFirstLineToQuestId(string scriptText, string questId)
        {
            if (string.IsNullOrEmpty(scriptText))
                return $"// {questId}\n";

            var normalized = scriptText.Replace("\r\n", "\n");
            var firstBreak = normalized.IndexOf('\n');
            var newHeader = $"// {questId}";

            if (firstBreak < 0)
                return newHeader + "\n";

            var firstLine = normalized.Substring(0, firstBreak);
            var rest = normalized.Substring(firstBreak + 1);

            if (firstLine.TrimStart().StartsWith("//", StringComparison.Ordinal))
                return newHeader + "\n" + rest;

            return newHeader + "\n" + normalized;
        }

        private static bool IsRewritableTextAsset(string assetPath)
        {
            var extension = Path.GetExtension(assetPath);
            if (string.IsNullOrEmpty(extension))
                return false;

            switch (extension.ToLowerInvariant())
            {
                case ".asset":
                case ".prefab":
                case ".unity":
                case ".yarn":
                case ".yarnproject":
                case ".md":
                case ".json":
                case ".txt":
                    return true;
                default:
                    return false;
            }
        }

        private static string GetTemplateLabel(string path)
        {
            return string.IsNullOrEmpty(path) ? "(none)" : Path.GetFileName(path);
        }

        private static bool ShouldCopyFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var fileName = Path.GetFileName(path);
            if (string.IsNullOrEmpty(fileName))
                return false;

            if (fileName.StartsWith(".", StringComparison.Ordinal))
                return false;

            return !fileName.EndsWith(".meta", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsLocalizationPath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return false;

            var normalized = relativePath.Replace("\\", "/");
            return normalized.StartsWith("_localizations/", StringComparison.OrdinalIgnoreCase)
                || normalized.IndexOf("/_localizations/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string ReplaceQuestTokens(string input, string sourceUpper, string sourceLower, string destinationUpper, string destinationLower)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input
                .Replace(sourceUpper, destinationUpper)
                .Replace(sourceLower, destinationLower);
        }

        private static string GetRelativePath(string rootPath, string fullPath)
        {
            var root = Path.GetFullPath(rootPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            var path = Path.GetFullPath(fullPath);
            return path.StartsWith(root, StringComparison.OrdinalIgnoreCase) ? path.Substring(root.Length) : Path.GetFileName(path);
        }

        private static void CreateFolderHierarchy(string assetFolderPath, HashSet<string> createdFolders = null)
        {
            createdFolders ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (AssetDatabase.IsValidFolder(assetFolderPath))
            {
                createdFolders.Add(assetFolderPath);
                return;
            }

            var normalized = assetFolderPath.Replace("\\", "/");
            var parts = normalized.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return;

            var current = parts[0];
            createdFolders.Add(current);
            for (int i = 1; i < parts.Length; i++)
            {
                var next = parts[i];
                var candidate = Path.Combine(current, next).Replace("\\", "/");
                if (!AssetDatabase.IsValidFolder(candidate) && !createdFolders.Contains(candidate))
                    AssetDatabase.CreateFolder(current, next);
                createdFolders.Add(candidate);
                current = candidate;
            }
        }

        private static bool TryParseDevNumber(string folderName, out int number)
        {
            number = 0;
            if (string.IsNullOrEmpty(folderName))
                return false;

            var match = System.Text.RegularExpressions.Regex.Match(folderName, @"^DEV_(\d+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return match.Success && int.TryParse(match.Groups[1].Value, out number);
        }

        private static int ExtractDevNumber(string folderName)
        {
            return TryParseDevNumber(folderName, out var number) ? number : 0;
        }

        private static string GetFullPath(string assetPath)
        {
            var projectRoot = Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;
            return Path.GetFullPath(Path.Combine(projectRoot, assetPath));
        }

        private static string ToAssetPath(string fullPath)
        {
            var projectRoot = Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;
            var full = Path.GetFullPath(fullPath);
            var root = Path.GetFullPath(projectRoot).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            return full.StartsWith(root, StringComparison.OrdinalIgnoreCase)
                ? full.Substring(root.Length).Replace("\\", "/")
                : fullPath.Replace("\\", "/");
        }
    }
}
#endif
