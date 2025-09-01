using System;

namespace Antura.Discover
{
    [Serializable]
    public struct SubjectCount
    {
        public Subject Subject;
        public int Count;
        public SubjectCount(Subject subject, int count) { Subject = subject; Count = count; }
        public override string ToString() => $"{Subject}: {Count}";
    }
}
