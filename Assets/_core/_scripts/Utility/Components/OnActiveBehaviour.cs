using UnityEngine;
using System;

namespace Antura.Utilities
{
    public class OnActiveBehaviour : MonoBehaviour
    {
        void OnEnable()
        {
            if (OnEnableAction != null)
            {
                OnEnableAction();
            }
        }

        void OnDisable()
        {
            if (OnDisableAction != null)
            {
                OnDisableAction();
            }
        }

        [HideInInspector]
        public Action OnEnableAction;

        public Action OnDisableAction;
    }
}
