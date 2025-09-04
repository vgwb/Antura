using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// Formation contract: given N members, return local-space offsets for their target slots.
    /// - Index 0 is for the leader and should return Vector3.zero by convention.
    /// - Implementations define geometry (Line, V, Circle, etc).
    /// </summary>
    public interface IFormation
    {
        string Name { get; }
        // Fill 'buffer' with exactly 'memberCount' local-space offsets. Reuse the provided list to avoid allocations.
        void FillLocalOffsets(List<Vector3> buffer, int memberCount, float spacing);
    }

    /// <summary>Simple line: leader at 0, others behind along -Z.</summary>
    [System.Serializable]
    public class LineFormation : IFormation
    {
        public string Name => "line";

        public void FillLocalOffsets(List<Vector3> buffer, int count, float spacing)
        {
            if (buffer == null)
                return;
            buffer.Clear();
            for (int i = 0; i < count; i++)
                buffer.Add(new Vector3(0f, 0f, -spacing * i));
        }
    }

    /// <summary>V formation: alternating left/right slots stepping outward and backward.</summary>
    [System.Serializable]
    public class VFormation : IFormation
    {
        public string Name => "v";

        public void FillLocalOffsets(List<Vector3> buffer, int count, float spacing)
        {
            if (buffer == null)
                return;
            buffer.Clear();
            if (count <= 0)
                return;
            buffer.Add(Vector3.zero); // leader
            int left = 1, right = 1;
            for (int i = 1; i < count; i++)
            {
                bool placeLeft = (i % 2 == 1);
                int rank = placeLeft ? left++ : right++;
                float side = placeLeft ? -1f : 1f;
                buffer.Add(new Vector3(side * spacing * rank, 0f, -spacing * rank));
            }
        }
    }

    /// <summary>Circle around the leader (who sits at the center).</summary>
    [System.Serializable]
    public class CircleFormation : IFormation
    {
        public string Name => "circle";

        public void FillLocalOffsets(List<Vector3> buffer, int count, float spacing)
        {
            if (buffer == null)
                return;
            buffer.Clear();
            if (count <= 0)
                return;
            buffer.Add(Vector3.zero); // leader center
            if (count == 1)
                return;
            int ringCount = count - 1;
            float radius = Mathf.Max(spacing, spacing * (ringCount * 0.35f));
            for (int i = 0; i < ringCount; i++)
            {
                float t = (i / (float)ringCount) * Mathf.PI * 2f;
                buffer.Add(new Vector3(Mathf.Cos(t) * radius, 0f, Mathf.Sin(t) * radius));
            }
        }
    }
}
