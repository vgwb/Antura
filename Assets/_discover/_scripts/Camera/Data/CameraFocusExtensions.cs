using System;
using UnityEngine;

namespace Antura.Discover
{
    public static class CameraFocusExtensions
    {

        /// <summary>
        /// Finds a CameraFocusData component in the GameObject or its children by Id.
        /// </summary>
        public static CameraFocusData FindCameraFocus(this GameObject go, string id, bool includeInactive = true)
        {
            var items = go.GetComponentsInChildren<CameraFocusData>(includeInactive);
            foreach (var it in items)
            {
                if (string.Equals(it.Id, id, StringComparison.Ordinal))
                    return it;
            }
            return null;
        }
    }
}
