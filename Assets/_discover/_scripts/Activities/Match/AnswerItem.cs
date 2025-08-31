using UnityEngine;
using Antura.Discover;

namespace Antura.Discover.Activities
{
    [DisallowMultipleComponent]
    public class AnswerItem : MonoBehaviour
    {
        public Transform AttachedTo;

        [Header("Data")]
        public CardData Data;

        public bool IsAttached => AttachedTo != null;
    }
}
