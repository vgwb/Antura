using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover.Activities
{
    public class PianoKeyboard : MonoBehaviour
    {
        public int startOctave = 4;

        [Header("Build by semitones")]
        public NoteName startNote = NoteName.C;
        [Tooltip("How many semitones to build starting from startNote/startOctave (e.g., 25 => C3..C5)")]
        public int semitoneCount = 25;

        public Vector2 keySize = new Vector2(60, 200);
        public Vector2 blackKeySize = new Vector2(40, 120);
        public float blackKeyOffsetY = 40f;
        public float spacing = 2f;

        public GameObject whiteKeyPrefab;
        public GameObject blackKeyPrefab;

        public AudioClip[] baseSemitoneClips = new AudioClip[12];

        [Header("Visuals")]
        [Tooltip("Color the 7 white keys (C D E F G A B) with rainbow colors")]
        public bool useRainbowColors = true;

        [Tooltip("Colors for white keys: C, D, E, F, G, A, B")]
        public Color[] WhiteKeyColors = new Color[7]
        {
            new Color(1.00f, 0.20f, 0.20f),  // C - Red
            new Color(1.00f, 0.60f, 0.00f),  // D - Orange
            new Color(1.00f, 0.90f, 0.10f),  // E - Yellow
            new Color(0.10f, 0.75f, 0.30f),  // F - Green
            new Color(0.00f, 0.70f, 0.90f),  // G - Cyan
            new Color(0.20f, 0.35f, 0.95f),  // A - Blue
            new Color(0.65f, 0.45f, 0.25f)   // B - Warm Brown
            // new Color(1.00f, 0.55f, 0.55f),  // C - Pastel Red
            // new Color(1.00f, 0.75f, 0.50f),  // D - Pastel Orange
            // new Color(1.00f, 0.98f, 0.60f),  // E - Pastel Yellow
            // new Color(0.60f, 0.90f, 0.70f),  // F - Pastel Green
            // new Color(0.60f, 0.90f, 1.00f),  // G - Pastel Cyan
            // new Color(0.65f, 0.75f, 1.00f),  // A - Pastel Blue
            // new Color(0.80f, 0.70f, 0.55f)   // B - Pastel Brown
        };

        private readonly NoteName[] whiteOrder = new NoteName[] {
            NoteName.C, NoteName.D, NoteName.E, NoteName.F, NoteName.G, NoteName.A, NoteName.B
        };

        private readonly HashSet<NoteName> blackSet = new HashSet<NoteName> {
            NoteName.Cs, NoteName.Ds, NoteName.Fs, NoteName.Gs, NoteName.As
        };

        private static readonly NoteName[] SemitoneOrder = new[]
        {
            NoteName.C, NoteName.Cs, NoteName.D, NoteName.Ds, NoteName.E, NoteName.F,
            NoteName.Fs, NoteName.G, NoteName.Gs, NoteName.A, NoteName.As, NoteName.B
        };

        private Dictionary<(NoteName, int), PianoKey> keys = new Dictionary<(NoteName, int), PianoKey>();

        public PianoKey GetKey(NoteName n, int octave)
        {
            keys.TryGetValue((n, octave), out var k);
            return k;
        }

        public IEnumerable<PianoKey> AllKeys() => keys.Values;

        public void Build()
        {
            // Clear
            for (int i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);
            keys.Clear();

            // Build semitone sequence
            var seq = new List<(NoteName note, int octave)>(Mathf.Max(1, semitoneCount));
            NoteName n = startNote;
            int oct = startOctave;
            for (int i = 0; i < Mathf.Max(1, semitoneCount); i++)
            {
                seq.Add((n, oct));
                StepSemitone(ref n, ref oct);
            }

            // First pass: place whites and record their X positions
            var whiteX = new Dictionary<int, float>(); // index in seq -> x position
            float xCursor = 0f;
            for (int i = 0; i < seq.Count; i++)
            {
                if (!blackSet.Contains(seq[i].note))
                {
                    whiteX[i] = xCursor;
                    xCursor += keySize.x + spacing;
                }
            }

            // Create white keys
            for (int i = 0; i < seq.Count; i++)
            {
                var (note, octave) = seq[i];
                if (blackSet.Contains(note))
                    continue;

                var go = Instantiate(whiteKeyPrefab, transform);
                var rt = go.GetComponent<RectTransform>();
                rt.sizeDelta = keySize;
                rt.anchorMin = rt.anchorMax = new Vector2(0, 0.5f);
                rt.pivot = new Vector2(0, 0.5f);
                rt.anchoredPosition = new Vector2(whiteX[i], 0);

                var key = go.GetComponent<PianoKey>();
                key.noteName = note;
                key.octave = octave;
                key.keyboard = this; // centralize audio in keyboard

                // Always assign rainbow colors to white keys
                if (useRainbowColors && key.background != null)
                {
                    int wi = WhiteIndex(note);
                    if (wi >= 0 && wi < WhiteKeyColors.Length)
                    {
                        var baseCol = WhiteKeyColors[wi];
                        key.normalColor = baseCol;
                        key.highlightColor = Lighten(baseCol, 0.35f); // lighter for highlight
                        key.background.color = key.normalColor;
                    }
                }

                keys[(note, octave)] = key;
            }

            // Create black keys
            for (int i = 0; i < seq.Count; i++)
            {
                var (note, octave) = seq[i];
                if (!blackSet.Contains(note))
                    continue;

                bool hasPrevWhite = TryFindNearestWhiteX(whiteX, i, -1, out float prevWhiteX);
                bool hasNextWhite = TryFindNearestWhiteX(whiteX, i, +1, out float nextWhiteX);

                float xPos;
                if (hasPrevWhite)
                    xPos = prevWhiteX + keySize.x * 0.75f;
                else if (hasNextWhite)
                    xPos = nextWhiteX - keySize.x * 0.25f;
                else
                    xPos = 0f;

                var go = Instantiate(blackKeyPrefab, transform);
                var rt = go.GetComponent<RectTransform>();
                rt.sizeDelta = blackKeySize;
                rt.anchorMin = rt.anchorMax = new Vector2(0, 0.5f);
                rt.pivot = new Vector2(0, 0.5f);
                rt.anchoredPosition = new Vector2(xPos, blackKeyOffsetY);

                var key = go.GetComponent<PianoKey>();
                key.noteName = note;
                key.octave = octave;
                key.keyboard = this; // centralize audio in keyboard

                // Optional: ensure black keys have sensible visuals
                if (key.background != null)
                {
                    key.normalColor = Color.black;
                    key.highlightColor = Lighten(Color.black, 0.6f);
                    key.background.color = key.normalColor;
                }

                keys[(note, octave)] = key;
            }
        }

        private int WhiteIndex(NoteName n)
        {
            switch (n)
            {
                case NoteName.C:
                    return 0;
                case NoteName.D:
                    return 1;
                case NoteName.E:
                    return 2;
                case NoteName.F:
                    return 3;
                case NoteName.G:
                    return 4;
                case NoteName.A:
                    return 5;
                case NoteName.B:
                    return 6;
                default:
                    return -1;
            }
        }

        private Color Lighten(Color c, float t)
        {
            // t in [0,1], 0=no change, 1=white
            return Color.Lerp(c, Color.white, Mathf.Clamp01(t));
        }

        private static int SemitoneIndex(NoteName n) => Array.IndexOf(SemitoneOrder, n);

        private static void StepSemitone(ref NoteName n, ref int octave)
        {
            int idx = SemitoneIndex(n);
            int next = (idx + 1) % 12;
            // If we wrap from B to C, increment octave
            if (idx == 11)
                octave += 1;
            n = SemitoneOrder[next];
        }

        private bool TryFindNearestWhiteX(Dictionary<int, float> whiteX, int fromIndex, int dir, out float x)
        {
            // dir: -1 search backward, +1 forward
            int i = fromIndex + dir;
            while (i >= 0 && i < int.MaxValue) // bounded by later check
            {
                if (whiteX.TryGetValue(i, out x))
                    return true;
                i += dir;
                if (i < 0 || i > 10000)
                    break; // safety
            }
            x = 0f;
            return false;
        }
    }
}
