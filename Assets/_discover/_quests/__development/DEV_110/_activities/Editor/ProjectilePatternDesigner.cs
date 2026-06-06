using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Antura.Discover.DEV110.EditorTools
{
    /// <summary>
    /// Visual authoring tool for <see cref="ProjectilePattern"/>s: edit emitters on the left and watch an
    /// animated preview on the right — Play / Pause / scrub / loop — without entering Play Mode. The
    /// preview runs the exact same maths (<see cref="ProjectilePatternMath"/>) the fight uses, so what you
    /// see is what you get.
    /// </summary>
    public class ProjectilePatternDesigner : EditorWindow
    {
        private ProjectilePattern _pattern;
        private SerializedObject _so;
        private int _selected;
        private Vector2 _listScroll;
        private Vector2 _fieldScroll;

        // Preview clock
        private bool _playing = true;
        private bool _loop = true;
        private float _previewTime;
        private float _speed = 1f;
        private double _lastTick;

        // Scratch buffers (reused each repaint to avoid garbage)
        private readonly List<Spawn> _burst = new List<Spawn>();

        private static readonly Color BoxBg = new Color(0.10f, 0.10f, 0.12f);
        private static readonly Color BoxBorder = new Color(0.55f, 0.55f, 0.6f);
        private static readonly Color SoulColor = new Color(1f, 0.3f, 0.3f);

        [MenuItem("JimmyHelpers/Antura/Projectile Pattern Designer")]
        public static void Open() => GetWindow<ProjectilePatternDesigner>("Pattern Designer");

        public static void Open(ProjectilePattern pattern)
        {
            var w = GetWindow<ProjectilePatternDesigner>("Pattern Designer");
            w.Select(pattern);
        }

        private void OnEnable()
        {
            minSize = new Vector2(720, 420);
            _lastTick = EditorApplication.timeSinceStartup;
            EditorApplication.update += OnTick;
            if (_pattern != null) _so = new SerializedObject(_pattern);
        }

        private void OnDisable() => EditorApplication.update -= OnTick;

        private void OnTick()
        {
            double now = EditorApplication.timeSinceStartup;
            float dt = Mathf.Min(0.1f, (float)(now - _lastTick));
            _lastTick = now;

            if (!_playing || _pattern == null) return;

            float total = Mathf.Max(0.01f, _pattern.EstimateDuration());
            _previewTime += dt * _speed;
            if (_previewTime > total)
            {
                if (_loop) _previewTime %= total;
                else { _previewTime = total; _playing = false; }
            }
            Repaint();
        }

        private void Select(ProjectilePattern p)
        {
            _pattern = p;
            _so = p != null ? new SerializedObject(p) : null;
            _selected = 0;
            _previewTime = 0f;
        }

        // ── GUI ────────────────────────────────────────────────────────────────

        private void OnGUI()
        {
            DrawHeader();

            if (_pattern == null)
            {
                EditorGUILayout.Space(8);
                EditorGUILayout.HelpBox(
                    "Select or create a Projectile Pattern to begin.\n" +
                    "Create → AnturaUndertale → Projectile Pattern, or press \"New\" above.",
                    MessageType.Info);
                return;
            }

            _so.Update();
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandHeight(true)))
            {
                DrawLeftPanel();
                DrawPreviewPanel();
            }
            _so.ApplyModifiedProperties();
        }

        private void DrawHeader()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                EditorGUILayout.LabelField("Pattern", EditorStyles.boldLabel, GUILayout.Width(54));
                var picked = (ProjectilePattern)EditorGUILayout.ObjectField(
                    _pattern, typeof(ProjectilePattern), false, GUILayout.Width(220));
                if (picked != _pattern) Select(picked);

                if (GUILayout.Button("New", EditorStyles.toolbarButton, GUILayout.Width(50)))
                    CreateNew();

                GUILayout.FlexibleSpace();
                if (_pattern != null)
                    EditorGUILayout.LabelField($"~{_pattern.EstimateDuration():0.0}s loop", EditorStyles.miniLabel, GUILayout.Width(90));
            }
        }

        // ── Left: emitter list + fields ──────────────────────────────────────────

        private void DrawLeftPanel()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(300), GUILayout.ExpandHeight(true)))
            {
                var emitters = _so.FindProperty("Emitters");

                EditorGUILayout.PropertyField(_so.FindProperty("ReferenceBoxSize"), new GUIContent("Preview Box"));
                EditorGUILayout.Space(4);
                EditorGUILayout.LabelField("Emitters", EditorStyles.boldLabel);

                using (var sv = new EditorGUILayout.ScrollViewScope(_listScroll, GUILayout.Height(140)))
                {
                    _listScroll = sv.scrollPosition;
                    for (int i = 0; i < emitters.arraySize; i++)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            var el = emitters.GetArrayElementAtIndex(i);
                            string name = el.FindPropertyRelative("Name").stringValue;
                            var shape = (EmitterShape)el.FindPropertyRelative("Shape").enumValueIndex;
                            bool sel = i == _selected;
                            var style = sel ? EditorStyles.boldLabel : EditorStyles.label;
                            if (GUILayout.Button($"{(sel ? "▸ " : "   ")}{name}  ({shape})", style))
                                _selected = i;
                            if (GUILayout.Button("✕", GUILayout.Width(22)))
                            {
                                RemoveEmitter(i);
                                GUIUtility.ExitGUI();
                            }
                        }
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("+ Add")) AddEmitter();
                    using (new EditorGUI.DisabledScope(emitters.arraySize == 0))
                        if (GUILayout.Button("Duplicate")) DuplicateEmitter(_selected);
                }

                EditorGUILayout.Space(6);

                if (emitters.arraySize == 0)
                {
                    EditorGUILayout.HelpBox("Add an emitter to start building the pattern.", MessageType.Info);
                    return;
                }

                _selected = Mathf.Clamp(_selected, 0, emitters.arraySize - 1);
                using (var sv = new EditorGUILayout.ScrollViewScope(_fieldScroll, GUILayout.ExpandHeight(true)))
                {
                    _fieldScroll = sv.scrollPosition;
                    DrawEmitterFields(emitters.GetArrayElementAtIndex(_selected));
                }
            }
        }

        private void DrawEmitterFields(SerializedProperty e)
        {
            EditorGUILayout.PropertyField(e.FindPropertyRelative("Name"));

            EditorGUILayout.LabelField("Timing", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(e.FindPropertyRelative("StartTime"));
            EditorGUILayout.PropertyField(e.FindPropertyRelative("Bursts"));
            EditorGUILayout.PropertyField(e.FindPropertyRelative("BurstInterval"));

            EditorGUILayout.Space(2);
            EditorGUILayout.LabelField("Emission", EditorStyles.miniBoldLabel);
            var shapeProp = e.FindPropertyRelative("Shape");
            EditorGUILayout.PropertyField(shapeProp);
            var shape = (EmitterShape)shapeProp.enumValueIndex;
            EditorGUILayout.PropertyField(e.FindPropertyRelative("Count"));
            if (shape == EmitterShape.Fan || shape == EmitterShape.Aimed)
                EditorGUILayout.PropertyField(e.FindPropertyRelative("SpreadAngle"));
            if (shape == EmitterShape.Fan || shape == EmitterShape.Ring || shape == EmitterShape.Spiral)
                EditorGUILayout.PropertyField(e.FindPropertyRelative("BaseAngle"));
            if (shape == EmitterShape.Stream)
                EditorGUILayout.PropertyField(e.FindPropertyRelative("Edge"));
            if (shape == EmitterShape.Spiral || shape == EmitterShape.Ring || shape == EmitterShape.Fan)
                EditorGUILayout.PropertyField(e.FindPropertyRelative("SpinPerBurst"));

            EditorGUILayout.Space(2);
            EditorGUILayout.LabelField("Movement", EditorStyles.miniBoldLabel);
            var moveProp = e.FindPropertyRelative("Movement");
            EditorGUILayout.PropertyField(moveProp);
            if ((BulletMovement)moveProp.enumValueIndex != BulletMovement.Straight)
            {
                EditorGUILayout.PropertyField(e.FindPropertyRelative("WaveAmplitude"));
                EditorGUILayout.PropertyField(e.FindPropertyRelative("WaveFrequency"));
            }

            EditorGUILayout.Space(2);
            EditorGUILayout.LabelField("Appearance", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(e.FindPropertyRelative("Sprite"));
            if (e.FindPropertyRelative("Sprite").objectReferenceValue == null)
                EditorGUILayout.LabelField(" ", "empty = white square", EditorStyles.miniLabel);
            EditorGUILayout.PropertyField(e.FindPropertyRelative("Color"));
            EditorGUILayout.PropertyField(e.FindPropertyRelative("Size"));
            EditorGUILayout.PropertyField(e.FindPropertyRelative("Speed"));
            EditorGUILayout.PropertyField(e.FindPropertyRelative("Damage"));
        }

        // ── Right: animated preview ──────────────────────────────────────────────

        private void DrawPreviewPanel()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                var canvas = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                int live = 0;
                if (Event.current.type == EventType.Repaint)
                    live = SimulateAndDraw(canvas);
                else
                    live = CountLive();

                DrawTransport(live);
            }
        }

        private void DrawTransport(int live)
        {
            float total = Mathf.Max(0.01f, _pattern.EstimateDuration());
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                if (GUILayout.Button(_playing ? "⏸ Pause" : "▶ Play", EditorStyles.toolbarButton, GUILayout.Width(70)))
                    _playing = !_playing;
                if (GUILayout.Button("⟲ Restart", EditorStyles.toolbarButton, GUILayout.Width(70)))
                    { _previewTime = 0f; _playing = true; }
                _loop = GUILayout.Toggle(_loop, "Loop", EditorStyles.toolbarButton, GUILayout.Width(48));

                GUILayout.Space(8);
                GUILayout.Label("Speed", EditorStyles.miniLabel, GUILayout.Width(40));
                _speed = GUILayout.HorizontalSlider(_speed, 0.25f, 2f, GUILayout.Width(80));
                GUILayout.Label($"{_speed:0.00}x", EditorStyles.miniLabel, GUILayout.Width(40));

                GUILayout.FlexibleSpace();
                GUILayout.Label($"t {_previewTime:0.0} / {total:0.0}s   live:{live}", EditorStyles.miniLabel, GUILayout.Width(160));
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                float t = EditorGUILayout.Slider(_previewTime, 0f, total);
                if (!Mathf.Approximately(t, _previewTime)) { _previewTime = t; }
            }
        }

        /// <summary>Draw the box, the dummy soul and every live bullet at the current preview time.</summary>
        private int SimulateAndDraw(Rect canvas)
        {
            Vector2 boxSize = _pattern.ReferenceBoxSize;
            if (boxSize.x < 1f || boxSize.y < 1f) boxSize = new Vector2(480, 270);

            // Fit the box into the canvas (with padding) preserving aspect.
            const float pad = 16f;
            Rect area = new Rect(canvas.x + pad, canvas.y + pad, canvas.width - 2 * pad, canvas.height - 2 * pad);
            float scale = Mathf.Min(area.width / boxSize.x, area.height / boxSize.y);
            Vector2 drawnHalf = boxSize * 0.5f * scale;
            Vector2 center = area.center;
            Rect boxRect = new Rect(center.x - drawnHalf.x, center.y - drawnHalf.y, drawnHalf.x * 2, drawnHalf.y * 2);

            EditorGUI.DrawRect(canvas, new Color(0.06f, 0.06f, 0.07f));
            EditorGUI.DrawRect(boxRect, BoxBg);
            DrawBorder(boxRect, BoxBorder);

            Vector2 half = boxSize * 0.5f;
            Vector2 soulPos = Vector2.zero; // dummy soul at centre (Aimed shots target this)

            // soul marker
            float ss = Mathf.Max(6f, 12f * scale);
            EditorGUI.DrawRect(new Rect(center.x - ss * 0.5f, center.y - ss * 0.5f, ss, ss), SoulColor);

            float t = _previewTime;
            int live = 0;
            Vector2 cullHalf = half + Vector2.one * 24f;

            for (int ei = 0; ei < _pattern.Emitters.Count; ei++)
            {
                var e = _pattern.Emitters[ei];
                if (e == null) continue;
                float lifetime = boxSize.magnitude / Mathf.Max(1f, e.Speed) + 1.5f;
                int bursts = Mathf.Max(1, e.Bursts);
                float drawSize = Mathf.Max(2f, e.Size * scale);

                for (int bi = 0; bi < bursts; bi++)
                {
                    float burstTime = Mathf.Max(0f, e.StartTime) + bi * Mathf.Max(0f, e.BurstInterval);
                    float age = t - burstTime;
                    if (age < 0f || age > lifetime) continue;

                    ProjectilePatternMath.GetBurst(e, ei, bi, half, soulPos, _burst);
                    foreach (var s in _burst)
                    {
                        Vector2 p = ProjectilePatternMath.PositionAt(s, e.Speed, e.Movement, e.WaveAmplitude, e.WaveFrequency, age);
                        if (Mathf.Abs(p.x) > cullHalf.x || Mathf.Abs(p.y) > cullHalf.y) continue;

                        // anchored (+y up, centre origin) -> GUI (+y down)
                        Vector2 g = new Vector2(center.x + p.x * scale, center.y - p.y * scale);
                        Rect r = new Rect(g.x - drawSize * 0.5f, g.y - drawSize * 0.5f, drawSize, drawSize);
                        DrawBullet(r, e);
                        live++;
                    }
                }
            }
            return live;
        }

        private int CountLive()
        {
            int live = 0;
            Vector2 half = _pattern.ReferenceBoxSize * 0.5f;
            Vector2 cullHalf = half + Vector2.one * 24f;
            float t = _previewTime;
            for (int ei = 0; ei < _pattern.Emitters.Count; ei++)
            {
                var e = _pattern.Emitters[ei];
                if (e == null) continue;
                float lifetime = _pattern.ReferenceBoxSize.magnitude / Mathf.Max(1f, e.Speed) + 1.5f;
                for (int bi = 0; bi < Mathf.Max(1, e.Bursts); bi++)
                {
                    float age = t - (Mathf.Max(0f, e.StartTime) + bi * Mathf.Max(0f, e.BurstInterval));
                    if (age < 0f || age > lifetime) continue;
                    ProjectilePatternMath.GetBurst(e, ei, bi, half, Vector2.zero, _burst);
                    foreach (var s in _burst)
                    {
                        Vector2 p = ProjectilePatternMath.PositionAt(s, e.Speed, e.Movement, e.WaveAmplitude, e.WaveFrequency, age);
                        if (Mathf.Abs(p.x) <= cullHalf.x && Mathf.Abs(p.y) <= cullHalf.y) live++;
                    }
                }
            }
            return live;
        }

        private static void DrawBullet(Rect r, Emitter e)
        {
            if (e.Sprite != null && e.Sprite.texture != null)
            {
                var tex = e.Sprite.texture;
                Rect tr = e.Sprite.textureRect;
                Rect uv = new Rect(tr.x / tex.width, tr.y / tex.height, tr.width / tex.width, tr.height / tex.height);
                Color prev = GUI.color;
                GUI.color = e.Color;
                GUI.DrawTextureWithTexCoords(r, tex, uv, true);
                GUI.color = prev;
            }
            else
            {
                EditorGUI.DrawRect(r, e.Color);
            }
        }

        private static void DrawBorder(Rect r, Color c)
        {
            EditorGUI.DrawRect(new Rect(r.x, r.y, r.width, 1), c);
            EditorGUI.DrawRect(new Rect(r.x, r.yMax - 1, r.width, 1), c);
            EditorGUI.DrawRect(new Rect(r.x, r.y, 1, r.height), c);
            EditorGUI.DrawRect(new Rect(r.xMax - 1, r.y, 1, r.height), c);
        }

        // ── Structural edits ─────────────────────────────────────────────────────

        private void AddEmitter()
        {
            Undo.RecordObject(_pattern, "Add Emitter");
            _pattern.Emitters.Add(new Emitter { Name = $"Emitter {_pattern.Emitters.Count + 1}" });
            _selected = _pattern.Emitters.Count - 1;
            EditorUtility.SetDirty(_pattern);
            _so.Update();
        }

        private void DuplicateEmitter(int i)
        {
            if (i < 0 || i >= _pattern.Emitters.Count) return;
            Undo.RecordObject(_pattern, "Duplicate Emitter");
            var src = _pattern.Emitters[i];
            var copy = JsonUtility.FromJson<Emitter>(JsonUtility.ToJson(src));
            copy.Sprite = src.Sprite; // JsonUtility drops object refs; restore it
            copy.Name = src.Name + " copy";
            _pattern.Emitters.Insert(i + 1, copy);
            _selected = i + 1;
            EditorUtility.SetDirty(_pattern);
            _so.Update();
        }

        private void RemoveEmitter(int i)
        {
            if (i < 0 || i >= _pattern.Emitters.Count) return;
            Undo.RecordObject(_pattern, "Remove Emitter");
            _pattern.Emitters.RemoveAt(i);
            _selected = Mathf.Clamp(_selected, 0, Mathf.Max(0, _pattern.Emitters.Count - 1));
            EditorUtility.SetDirty(_pattern);
            _so.Update();
        }

        private void CreateNew()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "New Projectile Pattern", "NewProjectilePattern", "asset", "Where should the pattern be saved?");
            if (string.IsNullOrEmpty(path)) return;

            var p = CreateInstance<ProjectilePattern>();
            p.Emitters.Add(new Emitter { Name = "Emitter 1" });
            AssetDatabase.CreateAsset(p, path);
            AssetDatabase.SaveAssets();
            Select(p);
        }
    }
}
