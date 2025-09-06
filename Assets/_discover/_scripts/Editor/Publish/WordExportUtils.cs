#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

namespace Antura.Discover.Editor
{
    public static class WordExportUtils
    {
        // Cached dynamic TMP font generated from raw OTF if needed
        private static TMP_FontAsset s_DrawingDynamicFont;
        public static string BuildWordsIndexMarkdown(Locale locale)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.AppendLine("title: Words");
            sb.AppendLine("hide:");
            sb.AppendLine("  - navigation");
            sb.AppendLine("---\n");
            sb.AppendLine("# Words\n");

            var guids = AssetDatabase.FindAssets("t:WordData");
            var words = new List<WordData>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var w = AssetDatabase.LoadAssetAtPath<WordData>(path);
                if (w != null)
                    words.Add(w);
            }

            // Only include active words
            words = words.Where(w => w != null && w.Active).ToList();

            if (words.Count == 0)
            {
                sb.AppendLine("(No words found)");
                return sb.ToString();
            }

            // Resolve target locales
            var enLoc = FindLocaleByCode("en");
            var frLoc = FindLocaleByCode("fr");
            var plLoc = FindLocaleByCode("pl");
            var roLoc = FindLocaleByCode("ro");
            var ukLoc = FindLocaleByCode("uk");
            var itLoc = FindLocaleByCode("it");

            // Group by category and render a table per category
            var groups = words
                .GroupBy(w => w.Category)
                .OrderBy(g => g.Key.ToString(), StringComparer.OrdinalIgnoreCase);

            foreach (var group in groups)
            {
                var category = group.Key;
                var list = group
                    .OrderBy(x => string.IsNullOrEmpty(x.TextEn) ? x.Id : x.TextEn, StringComparer.OrdinalIgnoreCase)
                    .ToList();
                if (list.Count == 0)
                    continue;

                sb.AppendLine($"## {category}");
                sb.AppendLine();
                sb.AppendLine("| icon | en | fr | pl | ro | uk | it |");
                sb.AppendLine("|---|---|---|---|---|---|---|");

                foreach (var w in list)
                {
                    string fallback = !string.IsNullOrEmpty(w.TextEn) ? w.TextEn : (!string.IsNullOrEmpty(w.name) ? w.name : w.Id);

                    string en = LocalizeWord(w, enLoc, fallback);
                    string fr = LocalizeWord(w, frLoc, fallback);
                    string pl = LocalizeWord(w, plLoc, fallback);
                    string ro = LocalizeWord(w, roLoc, fallback);
                    string uk = LocalizeWord(w, ukLoc, fallback);
                    string it = LocalizeWord(w, itLoc, fallback);

                    string iconCell = GenerateWordIconIfNeeded(w);
                    sb.AppendLine($"| {iconCell} | {EscapeMd(en)} | {EscapeMd(fr)} | {EscapeMd(pl)} | {EscapeMd(ro)} | {EscapeMd(uk)} | {EscapeMd(it)} |");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string LocalizeWord(WordData w, Locale locale, string fallback)
        {
            string result = fallback;
            try
            {
                PublishUtils.WithLocale(locale, () =>
                {
                    if (w.TextLocalized != null)
                        result = PublishUtils.SafeLocalized(w.TextLocalized, fallback);
                    else
                        result = fallback;
                });
            }
            catch { result = fallback; }
            return result;
        }

        private static string EscapeMd(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            // Escape pipes to avoid breaking tables
            return s.Replace("|", "\\|");
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

        // Creates (if needed) a JPG icon for the word's DrawingUnicode using the Drawings font, returns markdown image or empty cell.
        private static string GenerateWordIconIfNeeded(WordData w)
        {
            if (w == null || string.IsNullOrEmpty(w.DrawingUnicode))
                return ""; // Empty cell

            // Try loading existing SDF first
            const string sdfResourcePath = "Fonts/Drawings/Antura Drawings - Common SDF"; // Without extension
            TMP_FontAsset font = Resources.Load<TMP_FontAsset>(sdfResourcePath);

            // If not found, attempt to load raw OTF and create a dynamic TMP font asset
            if (font == null)
            {
                const string rawFontPath = "Fonts/Drawings/AnturaWordDrawings-Regular"; // OTF font imported as Font
                Font rawFont = Resources.Load<Font>(rawFontPath);
                if (rawFont != null)
                {
                    try
                    {
                        if (s_DrawingDynamicFont == null)
                        {
                            s_DrawingDynamicFont = TMP_FontAsset.CreateFontAsset(rawFont);
                            // Configure fallback / settings for clarity if needed
                            s_DrawingDynamicFont.name = "AnturaWordDrawings-DynamicTMP";
                        }
                        font = s_DrawingDynamicFont;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"[Publish] Failed creating dynamic TMP font from OTF: {ex.Message}");
                    }
                }
            }
            if (font == null)
            {
                return ""; // Font missing, skip
            }

            try
            {
                string projectRoot = Directory.GetParent(Application.dataPath).FullName;
                string relDir = Path.Combine("docs", "assets", "img", "discover", "words");
                string outDir = Path.Combine(projectRoot, relDir);
                Directory.CreateDirectory(outDir);

                string fileName = w.Id + ".jpg";
                string outPath = Path.Combine(outDir, fileName);

                // Basic cache: if file exists and newer than script assembly timestamp, skip
                if (File.Exists(outPath))
                {
                    DateTime existing = File.GetLastWriteTimeUtc(outPath);
                    // If the WordData asset hasn't changed since export (best-effort using its meta file timestamp) skip.
                    string assetPath = AssetDatabase.GetAssetPath(w);
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        string fullAssetPath = Path.GetFullPath(assetPath);
                        if (File.Exists(fullAssetPath))
                        {
                            DateTime assetTime = File.GetLastWriteTimeUtc(fullAssetPath);
                            if (assetTime <= existing)
                            {
                                return $"![](../../assets/img/discover/words/{fileName})"; // Already exported
                            }
                        }
                    }
                }

                // Decide text to render from DrawingUnicode patterns (supports raw char, hex, multiple tokens)
                string renderText = ParseDrawingUnicodeSequence(w.DrawingUnicode);

                if (string.IsNullOrEmpty(renderText))
                {
                    // Fallback to sprite if available
                    if (w.Drawing != null)
                    {
                        return ExportSpriteFallback(w, outDir, fileName);
                    }
                    return "";
                }

                // Prefer inline font usage: create a span using the custom web font if single glyph / short text (<=3 codepoints)
                if (renderText.Length <= 3)
                {
                    // Use HTML inside markdown table cell
                    string htmlEscaped = System.Security.SecurityElement.Escape(renderText);
                    return $"<span class=\"word-icon\">{htmlEscaped}</span>";
                }

                // Render glyph to a Texture2D using a temporary TextMeshPro object and Camera
                const int size = 256; // capture higher res then downscale implicitly via JPG
                RenderTexture rt = RenderTexture.GetTemporary(size, size, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
                rt.Create();

                // Create hidden root
                var root = new GameObject("__WordGlyphRender");
                root.hideFlags = HideFlags.HideAndDontSave;
                var camGO = new GameObject("Cam");
                camGO.hideFlags = HideFlags.HideAndDontSave;
                camGO.transform.SetParent(root.transform, false);
                var cam = camGO.AddComponent<Camera>();
                cam.orthographic = true;
                cam.orthographicSize = 1f;
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = Color.white;
                cam.cullingMask = ~0;
                cam.targetTexture = rt;

                var textGO = new GameObject("TMP");
                textGO.hideFlags = HideFlags.HideAndDontSave;
                textGO.transform.SetParent(root.transform, false);
                var tmp = textGO.AddComponent<TextMeshPro>();
                tmp.font = font;
                tmp.textWrappingMode = TextWrappingModes.NoWrap;
                tmp.richText = false;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.fontSize = 200; // big, will scale down
                tmp.color = Color.black;
                // Attempt to add missing characters dynamically if needed
                TryEnsureCharacters(font, renderText);
                tmp.text = renderText;
                textGO.transform.localPosition = Vector3.zero;
                tmp.ForceMeshUpdate();

                // Scale & center to fit nicely in view (padding factor)
                Bounds glyphBounds = tmp.bounds; // world space after initial layout
                float maxSide = Mathf.Max(glyphBounds.size.x, glyphBounds.size.y);
                if (maxSide < 1e-4f)
                    maxSide = 1f; // avoid div by zero
                float targetSide = 1.6f; // we want glyph to occupy ~80% of 2*orthoSize
                float scale = targetSide / maxSide;
                textGO.transform.localScale = Vector3.one * scale;
                tmp.ForceMeshUpdate();
                // Recompute bounds after scale then center
                glyphBounds = tmp.bounds;
                Vector3 offset = glyphBounds.center;
                textGO.transform.position -= offset; // recenters glyph at origin

                cam.transform.position = new Vector3(0, 0, -10f);
                cam.transform.rotation = Quaternion.identity;

                // Render
                cam.Render();
                RenderTexture.active = rt;
                Texture2D tex = new Texture2D(size, size, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, size, size), 0, 0);
                tex.Apply();
                RenderTexture.active = null;

                // Optional: Could crop excess white; current scaling already maximizes usage.
                // Check if texture is effectively blank (mostly white)
                if (IsMostlyWhite(tex))
                {
                    if (w.Drawing != null)
                    {
                        UnityEngine.Object.DestroyImmediate(tex);
                        RenderTexture.ReleaseTemporary(rt);
                        UnityEngine.Object.DestroyImmediate(root);
                        return ExportSpriteFallback(w, outDir, fileName);
                    }
                }

                byte[] jpg = tex.EncodeToJPG(85);
                File.WriteAllBytes(outPath, jpg);

                // Cleanup
                RenderTexture.ReleaseTemporary(rt);
                UnityEngine.Object.DestroyImmediate(tex);
                UnityEngine.Object.DestroyImmediate(root);

                return $"![](../../assets/img/discover/words/{fileName})"; // relative from words index.md
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Publish] Failed to generate glyph icon for word {w.Id}: {ex.Message}");
                return "";
            }
        }

        // Attempts to parse a DrawingUnicode string which may contain patterns like "U+E123", "0xE123", "&#xE123;", raw hex, or direct characters.
        private static string ParseDrawingUnicodeSequence(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;
            var tokens = raw.Trim().Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            var chars = new List<char>();
            foreach (var t in tokens)
            {
                if (TryParseCodePoint(t, out int cp))
                {
                    if (cp <= 0xFFFF)
                        chars.Add((char)cp);
                    else
                    {
                        // surrogate pair
                        cp -= 0x10000;
                        chars.Add((char)((cp >> 10) + 0xD800));
                        chars.Add((char)((cp & 0x3FF) + 0xDC00));
                    }
                }
                else if (t.Length == 1)
                {
                    chars.Add(t[0]);
                }
                // else ignore invalid token
            }
            if (chars.Count == 0)
                return null;
            return new string(chars.ToArray());
        }

        private static bool TryParseCodePoint(string token, out int codepoint)
        {
            codepoint = 0;
            string s = token.Trim().ToUpperInvariant();
            if (s.StartsWith("U+"))
                s = s.Substring(2);
            else if (s.StartsWith("0X"))
                s = s.Substring(2);
            else if (s.StartsWith("&#X"))
            {
                int end = s.IndexOf(';');
                if (end > 3)
                    s = s.Substring(3, end - 3);
                else
                    s = s.Substring(3);
            }
            // Now s should be hex digits if it's a codepoint
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                bool isHex = (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F');
                if (!isHex)
                    return false;
            }
            if (int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out int cpVal))
            {
                codepoint = cpVal;
                return true;
            }
            return false;
        }

        private static void TryEnsureCharacters(TMP_FontAsset font, string text)
        {
            if (font == null || string.IsNullOrEmpty(text))
                return;
            // Collect unique codepoints
            var missingChars = new List<uint>();
            foreach (char c in text)
            {
                int cp = c;
                if (!font.HasCharacter(c))
                    missingChars.Add((uint)cp);
            }
            if (missingChars.Count == 0)
                return;
            var arr = missingChars.Select(u => (char)u).ToArray();
            var str = new string(arr);
            font.TryAddCharacters(str);
        }

        private static bool IsMostlyWhite(Texture2D tex)
        {
            try
            {
                var pixels = tex.GetPixels32();
                int nonWhite = 0;
                int total = pixels.Length;
                for (int i = 0; i < total; i += 8) // sample every 8th pixel for speed
                {
                    var p = pixels[i];
                    if (!(p.r > 245 && p.g > 245 && p.b > 245))
                        nonWhite++;
                    if (nonWhite > 10)
                        return false; // enough detail
                }
                return true;
            }
            catch { return false; }
        }

        private static string ExportSpriteFallback(WordData w, string outDir, string fileName)
        {
            if (w.Drawing == null)
                return "";
            string outPath = Path.Combine(outDir, fileName);
            try
            {
                var sprite = w.Drawing;
                var texSrc = sprite.texture;
                Rect r = sprite.rect;
                Texture2D tmp = new Texture2D((int)r.width, (int)r.height, TextureFormat.RGBA32, false);
                Color[] px = texSrc.GetPixels((int)r.x, (int)r.y, (int)r.width, (int)r.height);
                tmp.SetPixels(px);
                tmp.Apply();
                // Square pad on white
                int size = Mathf.Max(tmp.width, tmp.height);
                Texture2D canvas = new Texture2D(size, size, TextureFormat.RGB24, false);
                var white = Enumerable.Repeat(Color.white, size * size).ToArray();
                canvas.SetPixels(white);
                int ox = (size - tmp.width) / 2;
                int oy = (size - tmp.height) / 2;
                for (int y = 0; y < tmp.height; y++)
                {
                    for (int x = 0; x < tmp.width; x++)
                    {
                        Color c = tmp.GetPixel(x, y);
                        if (c.a < 0.01f)
                            c = Color.white; // treat transparent as white
                        canvas.SetPixel(ox + x, oy + y, c);
                    }
                }
                canvas.Apply();
                File.WriteAllBytes(outPath, canvas.EncodeToJPG(85));
                UnityEngine.Object.DestroyImmediate(tmp);
                UnityEngine.Object.DestroyImmediate(canvas);
                return $"![](../assets/img/discover/words/{fileName})";
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Publish] Fallback sprite export failed for {w.Id}: {ex.Message}");
                return "";
            }
        }
    }
}
#endif
