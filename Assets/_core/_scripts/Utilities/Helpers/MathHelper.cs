using System.Collections.Generic;
using UnityEngine;

namespace Antura.Helpers
{
    /// <summary>
    /// Static helper class for math functions.
    /// </summary>
    public static class MathHelper
    {
        public static float AngleCounterClockwise(Vector2 a, Vector2 b)
        {
            float dot = Vector2.Dot(a.normalized, b.normalized);
            dot = Mathf.Clamp(dot, -1.0f, 1.0f);

            if (Cross(a, b) >= 0)
            {
                return Mathf.Acos(dot);
            }
            return Mathf.PI * 2 - Mathf.Acos(dot);
        }

        static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        public static float GetAverage(List<float> floatsList)
        {
            if (floatsList.Count < 1)
            {
                return 0f;
            }

            var average = 0f;

            foreach (var item in floatsList)
            {
                average += item;
            }

            return (average / floatsList.Count);
        }
    }
}
