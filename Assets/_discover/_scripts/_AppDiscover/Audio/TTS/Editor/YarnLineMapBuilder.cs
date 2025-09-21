#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Antura.Discover;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover.Audio.Editor
{
    /// <summary>Parses Yarn headers to map lineId → node title and actor.</summary>
    public static class YarnLineMapBuilder
    {
        public sealed class YarnLineMeta
        {
            public readonly Dictionary<string, string> Titles = new();
            public readonly Dictionary<string, VoiceActors?> Actors = new();
        }

        public static Dictionary<string, string> Build(QuestData quest)
        {
            // Back-compat: return only titles
            var meta = BuildMeta(quest);
            return meta.Titles;
        }

        public static YarnLineMeta BuildMeta(QuestData quest)
        {
            var meta = new YarnLineMeta();
            try
            {
                if (quest == null || quest.YarnScript == null)
                    return meta;
                string path = AssetDatabase.GetAssetPath(quest.YarnScript);
                if (string.IsNullOrEmpty(path))
                    return meta;
                var text = File.ReadAllText(path);
                string currentTitle = string.Empty;
                VoiceActors? currentActor = null;
                var rxTitle = new Regex("^\\s*title\\s*:\\s*(.+)$", RegexOptions.IgnoreCase);
                var rxActor = new Regex("^\\s*actor\\s*:\\s*(.+)$", RegexOptions.IgnoreCase);
                var rxLine = new Regex(@"#line:([A-Za-z0-9_-]+)");
                foreach (var rawLine in text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None))
                {
                    var mTitle = rxTitle.Match(rawLine);
                    if (mTitle.Success)
                    {
                        currentTitle = mTitle.Groups[1].Value.Trim();
                        continue;
                    }
                    var mActor = rxActor.Match(rawLine);
                    if (mActor.Success)
                    {
                        var actorStr = mActor.Groups[1].Value.Trim();
                        if (Enum.TryParse<VoiceActors>(actorStr, ignoreCase: true, out var parsed))
                            currentActor = parsed;
                        else
                            currentActor = null;
                        continue;
                    }
                    var mLine = rxLine.Match(rawLine);
                    if (mLine.Success)
                    {
                        var id = mLine.Groups[1].Value.Trim();
                        if (!meta.Titles.ContainsKey(id))
                            meta.Titles.Add(id, currentTitle);
                        if (!meta.Actors.ContainsKey(id))
                            meta.Actors.Add(id, currentActor);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[QVM] Yarn parse failed: {ex.Message}");
            }
            return meta;
        }
    }
}
#endif
