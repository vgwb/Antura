using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// Simple trigger volume that recruits a PartyMember into the nearest PartyManager (by Leader).
    /// - Place this on a trigger collider near your Living Letter spawn.
    /// - OnPlayer enter (or any PartyMember marked as Player), we add a configured target to the party.
    /// - You can also configure it to recruit the object that enters instead (AutoRecruitSelf).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PartyJoinZone : MonoBehaviour
    {
        [Tooltip("If set, this member will be added to the party when the Player enters.")]
        public PartyMember MemberToRecruit;

        [Tooltip("If true, we recruit the PartyMember on THIS GameObject when Player enters.")]
        public bool AutoRecruitSelf = false;

        private void Reset()
        {
            var col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var enteringMember = other.GetComponentInParent<PartyMember>();
            if (enteringMember == null)
                return;
            if (enteringMember.MemberKind != PartyMember.Kind.Player)
                return;

            var manager = PartyManager.I;
            if (manager == null)
            {
                Debug.LogWarning("[PartyJoinZone] No PartyManager found in the scene. Place PartyManager on a managers object.");
                return;
            }

            var recruit = AutoRecruitSelf ? GetComponent<PartyMember>() : MemberToRecruit;
            if (recruit == null)
            {
                Debug.LogWarning("[PartyJoinZone] No PartyMember to recruit assigned.");
                return;
            }

            manager.AddMember(recruit);
        }
    }
}
