using UnityEngine;

namespace Antura.Extensions
{
    public static class Vector3Extension
    {
        public static bool DistanceIsLessThan(this Vector3 me, Vector3 other, float distance)
        {
            return SquaredDistance(me, other) < distance * distance;
        }

        public static float SquaredDistance(this Vector3 me, Vector3 other)
        {
            float dx = me.x - other.x;
            float dy = me.y - other.y;
            float dz = me.z - other.z;
            return dx * dx + dy * dy + dz * dz;
        }
    }
}
