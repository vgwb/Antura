using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public abstract class ActionAbstract : MonoBehaviour
    {
        public void Trigger()
        {
            OnTrigger();
        }

        public abstract void OnTrigger();

    }
}
