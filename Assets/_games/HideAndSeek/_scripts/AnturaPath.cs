using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.HideAndSeek
{
    public class AnturaPath : MonoBehaviour
    {
        public List<Vector3> GetPath()
        {
            List<Vector3> path = new List<Vector3>();

            for (int i = 0, count = transform.childCount; i < count; ++i)
            {
                path.Add(transform.GetChild(i).position);
            }
            return path;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            for (int i = 0, count = transform.childCount; i < count; ++i)
            {
                var child = transform.GetChild(i);
                Gizmos.color = i == 0 ? Color.red : Color.white;
                Gizmos.DrawSphere(child.position, 0.8f);

                if (i > 0)
                {
                    Gizmos.DrawLine(transform.GetChild(i - 1).position, child.position);
                }
            }

        }
#endif
    }
}
