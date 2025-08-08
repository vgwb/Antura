using System;
using System.Collections.Generic;
using UnityEngine;
namespace Antura.Discover.Activities
{
    public enum NoteName { C, Cs, D, Ds, E, F, Fs, G, Gs, A, As, B }
    public enum NoteDuration { Eighth = 1, Quarter = 2, Half = 4, Whole = 8 }

    [Serializable]
    public struct NoteToken
    {
        public bool IsRest;
        public NoteName Note;
        public int Octave;
    }

    [Serializable]
    public struct MelodyEvent
    {
        public NoteToken Token;
        public NoteDuration Duration;
    }

    public static class NoteUtils
    {
        public static int SemitoneOf(NoteName name)
        {
            switch (name)
            {
                case NoteName.C:
                    return 0;
                case NoteName.Cs:
                    return 1;
                case NoteName.D:
                    return 2;
                case NoteName.Ds:
                    return 3;
                case NoteName.E:
                    return 4;
                case NoteName.F:
                    return 5;
                case NoteName.Fs:
                    return 6;
                case NoteName.G:
                    return 7;
                case NoteName.Gs:
                    return 8;
                case NoteName.A:
                    return 9;
                case NoteName.As:
                    return 10;
                case NoteName.B:
                    return 11;
            }
            return 0;
        }

        public static string DisplayName(NoteName n)
        {
            switch (n)
            {
                case NoteName.Cs:
                    return "C#";
                case NoteName.Ds:
                    return "D#";
                case NoteName.Fs:
                    return "F#";
                case NoteName.Gs:
                    return "G#";
                case NoteName.As:
                    return "A#";
                default:
                    return n.ToString();
            }
        }
    }
}
