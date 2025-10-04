using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Antura.Discover.Player
{
    public class Teleporter : MonoBehaviour
    {
        public Teleporter TeleportTo;

        public UnityAction<CharacterMotorController> OnCharacterTeleport;

        public bool isBeingTeleportedTo { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (!isBeingTeleportedTo)
            {
                CharacterMotorController cc = other.GetComponent<CharacterMotorController>();
                if (cc)
                {
                    cc.Motor.SetPositionAndRotation(TeleportTo.transform.position, TeleportTo.transform.rotation);

                    if (OnCharacterTeleport != null)
                    {
                        OnCharacterTeleport(cc);
                    }
                    TeleportTo.isBeingTeleportedTo = true;
                }
            }

            isBeingTeleportedTo = false;
        }
    }
}
