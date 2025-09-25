#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover.Audio.Editor
{
    internal static class VoiceoverManifestUtil
    {
        private const string LangBundlesRoot = "Assets/_lang_bundles/_quests";
        private const string ManifestFileName = "_index.json";

        [Serializable]
        private class ManifestLine
        {
            public string key;
            public string nodeTitle;
            public string sourceText;
            public string sourceEnText;
            public string audioFile;
            public string voiceProfile;
            public string actorId;
            public int? durationMs;
            public string updatedAt; // ISO 8601 UTC timestamp
            public string textHash;
        }

        [Serializable]
        private class Manifest
        {
            public string version = "1";
            public string questId;
            public string lang;
            public List<ManifestLine> lines = new();
        }

        public static string GetManifestPath(string questId, string lang)
        {
            var folder = Path.Combine(LangBundlesRoot, SanitizeFolderName(questId), lang).Replace('\\', '/');
            return Path.Combine(folder, ManifestFileName).Replace('\\', '/');
        }

        public static bool Upsert(string questId, Locale locale, string key, string audioFileName, string textHash, int? durationMs, string voiceProfileId, string actorId, string nodeTitle = null, string sourceText = null, string sourceEnText = null)
        {
            if (string.IsNullOrEmpty(questId) || locale == null || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(audioFileName))
            { Debug.LogError("[QVM] Manifest upsert: invalid inputs"); return false; }

            var lang = locale.Identifier.Code;
            var manifestPath = GetManifestPath(questId, lang);
            EnsureFolder(Path.GetDirectoryName(manifestPath));

            Manifest m = null;
            if (File.Exists(manifestPath))
            {
                try
                {
                    m = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(manifestPath));
                }
                catch (Exception ex)
                { Debug.LogError($"[QVM] Failed to read manifest: {manifestPath} — {ex.Message}"); return false; }
                if (m == null)
                    m = new Manifest { questId = questId, lang = lang };
                // Do not allow changing questId or lang once exists
                if (!string.Equals(m.questId, questId, StringComparison.Ordinal) || !string.Equals(m.lang, lang, StringComparison.Ordinal))
                { Debug.LogError($"[QVM] Manifest quest/lang mismatch at {manifestPath} (found {m.questId}/{m.lang}, got {questId}/{lang})"); return false; }
            }
            else
            {
                m = new Manifest { questId = questId, lang = lang };
            }

            var line = m.lines.FirstOrDefault(l => string.Equals(l.key, key, StringComparison.Ordinal));
            if (line == null)
            {
                line = new ManifestLine { key = key };
                m.lines.Add(line);
            }

            line.audioFile = audioFileName;
            // If textHash not supplied but we have sourceText, compute it like Editor callers do.
            if (string.IsNullOrEmpty(textHash) && sourceText != null)
            {
                var normalized = NormalizeText(sourceText);
                textHash = ComputeTextHash(normalized, voiceProfileId, actorId, lang);
            }
            if (!string.IsNullOrEmpty(textHash))
                line.textHash = textHash;
            if (durationMs.HasValue)
                line.durationMs = durationMs;
            if (!string.IsNullOrEmpty(voiceProfileId))
                line.voiceProfile = voiceProfileId;
            if (!string.IsNullOrEmpty(actorId))
                line.actorId = actorId;
            if (!string.IsNullOrEmpty(nodeTitle))
                line.nodeTitle = nodeTitle;
            if (sourceText != null)
                line.sourceText = sourceText;
            if (sourceEnText != null)
                line.sourceEnText = sourceEnText;

            // Update timestamp: prefer audio file last write UTC; fallback to existing; else set to now (UTC)
            try
            {
                var manifestDir = Path.GetDirectoryName(manifestPath);
                var audioPath = string.IsNullOrEmpty(manifestDir) ? audioFileName : Path.Combine(manifestDir, audioFileName);
                if (!string.IsNullOrEmpty(audioPath) && File.Exists(audioPath))
                {
                    var t = File.GetLastWriteTimeUtc(audioPath);
                    line.updatedAt = FormatUtcTimestamp(t);
                }
                else if (string.IsNullOrEmpty(line.updatedAt))
                {
                    line.updatedAt = FormatUtcTimestamp(DateTime.UtcNow);
                }
            }
            catch
            {
                if (string.IsNullOrEmpty(line.updatedAt))
                {
                    line.updatedAt = FormatUtcTimestamp(DateTime.UtcNow);
                }
            }

            m.lines = m.lines.OrderBy(l => l.key, StringComparer.Ordinal).ToList();

            var json = JsonConvert.SerializeObject(m, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            try
            {
                var tmp = manifestPath + ".tmp";
                File.WriteAllText(tmp, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                if (File.Exists(manifestPath))
                {
                    try
                    {
                        File.Replace(tmp, manifestPath, manifestPath + ".bak");
                        // Clean up backup once replace succeeds
                        try
                        {
                            var bak = manifestPath + ".bak";
                            if (File.Exists(bak))
                            { File.Delete(bak); }
                        }
                        catch { /* ignore */ }
                    }
                    catch
                    {
                        File.Copy(tmp, manifestPath, overwrite: true);
                        File.Delete(tmp);
                        // Also try to remove any old backup if present
                        try
                        {
                            var bak = manifestPath + ".bak";
                            if (File.Exists(bak))
                            { File.Delete(bak); }
                        }
                        catch { /* ignore */ }
                    }
                }
                else
                {
                    File.Move(tmp, manifestPath);
                }
                AssetDatabase.ImportAsset(manifestPath, ImportAssetOptions.ForceSynchronousImport);
                return true;
            }
            catch (Exception ex)
            { Debug.LogError($"[QVM] Failed to write manifest: {manifestPath} — {ex.Message}"); return false; }
        }

        public static string ComputeTextHash(string normalizedText, string voiceProfileId, string actorId, string lang)
        {
            var s = (normalizedText ?? string.Empty) + "|" + (voiceProfileId ?? string.Empty) + "|" + (actorId ?? string.Empty) + "|" + (lang ?? string.Empty);
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(s));
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public static string NormalizeText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            var s = text.Replace("\r\n", "\n").Replace("\r", "\n");
            s = s.Trim();
            s = s.Normalize(NormalizationForm.FormC);
            s = s.ToLowerInvariant();
            // collapse multiple spaces
            var sb = new StringBuilder(s.Length);
            bool wasSpace = false;
            foreach (var ch in s)
            {
                bool isSpace = ch == ' ' || ch == '\t' || ch == '\n';
                if (isSpace)
                {
                    if (!wasSpace)
                        sb.Append(' ');
                }
                else
                    sb.Append(ch);
                wasSpace = isSpace;
            }
            return sb.ToString();
        }

        private static void EnsureFolder(string folder)
        {
            if (string.IsNullOrEmpty(folder))
                return;
            folder = folder.Replace('\\', '/');
            if (AssetDatabase.IsValidFolder(folder))
                return;
            var parts = folder.Split('/');
            var cur = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                var next = cur + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(cur, parts[i]);
                }
                cur = next;
            }
        }

        private static string SanitizeFolderName(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "quest";
            foreach (var c in Path.GetInvalidFileNameChars())
                s = s.Replace(c, '_');
            return s;
        }

        private static string FormatUtcTimestamp(DateTime utc)
        {
            // ISO 8601 without fractional seconds, Z for UTC
            return utc.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
        }
    }
}
#endif
