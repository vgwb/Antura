using System.Collections;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;
using Homer;
using UnityEngine.Events;

namespace Antura.Minigames.DiscoverCountry
{
    public enum InteractionIcon
    {
        None = 0,
        Look = 1,
        Talk = 2,
        Use = 4
    }

    public enum ReActionType
    {
        None = 0,
        NodeAction = 1,
        UnityAction = 3
    }

    public class Interactable : MonoBehaviour
    {
        [Header("Interaction")]
        public bool IsInteractable;
        public InteractionIcon Icon;

        public Transform IconTransform;
        public bool FocusCameraOnInteract;

        public ReActionType Type;

        [Header("Quest")]
        public string NodePermalink;
        public string NodeCommand;
        
        [Header("UnityAction")]
        [SerializeField] UnityEvent unityAction;

        void Start()
        {
            // IconTransform = gameObject.transform.Find("icon");
            if (IconTransform == null)
            {
                IconTransform = transform;
            }
        }

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void LaunchUnityAction()
        {
            if (unityAction != null) unityAction.Invoke();
        }
        
    }
}
