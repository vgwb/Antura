using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry.Interaction
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
