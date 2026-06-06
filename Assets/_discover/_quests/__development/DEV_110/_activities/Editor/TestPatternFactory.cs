using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover.DEV110.EditorTools
{
    /// <summary>
    /// One-click generator for a set of demo <see cref="ProjectilePattern"/> assets that exercise every
    /// shape and movement. Re-running it overwrites the demo assets in place, so it doubles as a
    /// "reset my test patterns" button. Output: <c>AnturaUndertale/TestPatterns/*.asset</c>.
    /// </summary>
    public static class TestPatternFactory
    {
        private const string FolderName = "TestPatterns";

        // Distinct bullet colours so the patterns are easy to tell apart in the designer.
        private static readonly Color White = Color.white;
        private static readonly Color Cyan = new Color(0.40f, 0.90f, 1.00f);
        private static readonly Color Yellow = new Color(1.00f, 0.90f, 0.30f);
        private static readonly Color Sky = new Color(0.60f, 0.80f, 1.00f);
        private static readonly Color Magenta = new Color(1.00f, 0.40f, 0.90f);
        private static readonly Color Orange = new Color(1.00f, 0.60f, 0.20f);
        private static readonly Color Green = new Color(0.50f, 1.00f, 0.55f);
        private static readonly Color Red = new Color(1.00f, 0.40f, 0.40f);

        [MenuItem("JimmyHelpers/Antura/Create 10 Test Patterns")]
        public static void CreateAll()
        {
            string outDir = ResolveOutputFolder();
            if (outDir == null)
                return;

            Save("01 - Aimed Volley", outDir,
                E("Aimed", EmitterShape.Aimed, count: 1, bursts: 6, interval: 0.45f, speed: 240, size: 16, color: White, start: 0.2f));

            Save("02 - Spread Fan", outDir,
                E("Fan", EmitterShape.Fan, count: 9, bursts: 4, interval: 0.6f, speed: 200, size: 14, color: Cyan,
                  spread: 90, baseAngle: 0));

            Save("03 - Ring Burst", outDir,
                E("Ring", EmitterShape.Ring, count: 16, bursts: 3, interval: 0.8f, speed: 180, size: 14, color: Yellow));

            Save("04 - Rain", outDir,
                E("Rain", EmitterShape.Stream, count: 4, bursts: 14, interval: 0.28f, speed: 260, size: 12, color: Sky,
                  edge: StreamEdge.Top));

            Save("05 - Spiral", outDir,
                E("Spiral", EmitterShape.Spiral, count: 2, bursts: 30, interval: 0.08f, speed: 170, size: 12, color: Magenta,
                  spin: 17f));

            Save("06 - Zig-Zag Stream", outDir,
                E("ZigZag", EmitterShape.Stream, count: 3, bursts: 12, interval: 0.32f, speed: 200, size: 14, color: Orange,
                  edge: StreamEdge.Top, move: BulletMovement.ZigZag, amp: 36, freq: 2.5f));

            Save("07 - Wave Curtain", outDir,
                E("Wave", EmitterShape.Stream, count: 6, bursts: 8, interval: 0.4f, speed: 160, size: 14, color: Green,
                  edge: StreamEdge.Top, move: BulletMovement.Wave, amp: 28, freq: 2f));

            Save("08 - Cross Streams", outDir,
                E("Left", EmitterShape.Stream, count: 3, bursts: 10, interval: 0.35f, speed: 200, size: 12, color: Cyan, edge: StreamEdge.Left),
                E("Right", EmitterShape.Stream, count: 3, bursts: 10, interval: 0.35f, speed: 200, size: 12, color: Magenta, edge: StreamEdge.Right, start: 0.17f),
                E("Top", EmitterShape.Stream, count: 2, bursts: 10, interval: 0.5f, speed: 220, size: 12, color: White, edge: StreamEdge.Top));

            Save("09 - Aimed + Rain", outDir,
                E("Aimed", EmitterShape.Aimed, count: 3, bursts: 5, interval: 0.6f, speed: 240, size: 14, color: Red, spread: 20),
                E("Rain", EmitterShape.Stream, count: 3, bursts: 10, interval: 0.35f, speed: 180, size: 12, color: Sky, edge: StreamEdge.Top, start: 0.3f));

            Save("10 - Bullet Hell Finale", outDir,
                E("Spiral", EmitterShape.Spiral, count: 3, bursts: 40, interval: 0.07f, speed: 150, size: 10, color: Magenta, spin: 13f),
                E("Ring", EmitterShape.Ring, count: 18, bursts: 4, interval: 1.2f, speed: 200, size: 12, color: Yellow, start: 1.0f),
                E("ZigRain", EmitterShape.Stream, count: 4, bursts: 16, interval: 0.3f, speed: 240, size: 10, color: Orange,
                  edge: StreamEdge.Top, move: BulletMovement.ZigZag, amp: 24, freq: 3f, start: 0.5f),
                E("Aimed", EmitterShape.Aimed, count: 1, bursts: 8, interval: 0.5f, speed: 280, size: 16, color: White, start: 0.8f));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            var folder = AssetDatabase.LoadAssetAtPath<Object>(outDir);
            if (folder != null) EditorGUIUtility.PingObject(folder);
            Debug.Log($"Created 10 test projectile patterns in {outDir}");
        }

        /// <summary>Build one emitter; only the fields a shape/movement uses need to be passed.</summary>
        private static Emitter E(string name, EmitterShape shape, int count, int bursts, float interval,
                                 float speed, float size, Color color, float start = 0f, float spread = 60f,
                                 float baseAngle = 0f, StreamEdge edge = StreamEdge.Top, float spin = 0f,
                                 BulletMovement move = BulletMovement.Straight, float amp = 24f, float freq = 3f,
                                 int damage = 1)
        {
            return new Emitter
            {
                Name = name,
                StartTime = start,
                Bursts = bursts,
                BurstInterval = interval,
                Shape = shape,
                Count = count,
                SpreadAngle = spread,
                BaseAngle = baseAngle,
                Edge = edge,
                SpinPerBurst = spin,
                Movement = move,
                WaveAmplitude = amp,
                WaveFrequency = freq,
                Sprite = null,
                Color = color,
                Size = size,
                Speed = speed,
                Damage = damage
            };
        }

        private static void Save(string assetName, string outDir, params Emitter[] emitters)
        {
            var p = ScriptableObject.CreateInstance<ProjectilePattern>();
            p.ReferenceBoxSize = new Vector2(480f, 270f);
            p.Emitters = new List<Emitter>(emitters);

            string path = $"{outDir}/{assetName}.asset";
            AssetDatabase.DeleteAsset(path); // overwrite cleanly if it already exists
            AssetDatabase.CreateAsset(p, path);
        }

        /// <summary>Find/create the TestPatterns folder next to the AnturaUndertale scripts.</summary>
        private static string ResolveOutputFolder()
        {
            string self = AssetDatabase.FindAssets($"t:Script {nameof(TestPatternFactory)}")
                                       .Select(AssetDatabase.GUIDToAssetPath)
                                       .FirstOrDefault();
            if (string.IsNullOrEmpty(self))
            {
                Debug.LogError("TestPatternFactory: could not locate its own script path.");
                return null;
            }

            // self = .../AnturaUndertale/Editor/TestPatternFactory.cs  ->  baseDir = .../AnturaUndertale
            string editorDir = Path.GetDirectoryName(self).Replace('\\', '/');
            string baseDir = Path.GetDirectoryName(editorDir).Replace('\\', '/');
            string outDir = $"{baseDir}/{FolderName}";

            if (!AssetDatabase.IsValidFolder(outDir))
                AssetDatabase.CreateFolder(baseDir, FolderName);
            return outDir;
        }
    }
}
