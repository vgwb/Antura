using UnityEngine;
using System.Collections.Generic;

namespace Antura.Minigames
{
    /// <summary>
    /// Utility script that automatically activates a list of game objects when awoken.
    /// </summary>
    public class AutoActivate : MonoBehaviour
    {
        public List<GameObject> toAwake = new List<GameObject>();

        void Awake()
        {
            foreach (var g in toAwake)
            {
                if (g != null)
                {
                    g.SetActive(true);
                }
            }
        }
    }
}
