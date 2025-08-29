using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public abstract class ActableAbstract : MonoBehaviour
    {
        [Tooltip("A unique code to call this action from Dialogue")]
        public string Id;

        public void Trigger()
        {
            OnTrigger();
        }

        public abstract void OnTrigger();

    }
}
