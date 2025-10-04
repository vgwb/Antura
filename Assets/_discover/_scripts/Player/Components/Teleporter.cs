using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Antura.Discover.Player
{
    public class Teleporter : MonoBehaviour
    {
        [Header("Prefab configuration")]
        [Tooltip("The teleporter to teleport to")]
        public Teleporter TeleportTo;
        public bool Activated = true;

        [Tooltip("Set to 0 to disable recharge")]
        public int TimeToRecharge = 5;

        [Header("Internal References")]
        public GameObject EnergyWall;
        public UnityAction<CharacterMotorController> OnCharacterTeleport;

        public bool isBeingTeleportedTo { get; set; }

        private Coroutine rechargeRoutine;

        void Start()
        {
            Activate(Activated);
        }

        public void Activate(bool status)
        {
            Activated = status;
            EnergyWall.SetActive(status);
            isBeingTeleportedTo = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Activated)
                return;

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

                    if (TeleportTo != null && TeleportTo.Activated)
                    {
                        TeleportTo.BeginRecharge();
                    }
                }
            }

            isBeingTeleportedTo = false;
        }

        public void BeginRecharge()
        {
            if (TimeToRecharge <= 0)
            {
                return;
            }

            if (rechargeRoutine != null)
            {
                StopCoroutine(rechargeRoutine);
            }

            rechargeRoutine = StartCoroutine(RechargeCoroutine());
        }

        private IEnumerator RechargeCoroutine()
        {
            Activate(false);
            yield return new WaitForSeconds(TimeToRecharge);
            Activate(true);
            rechargeRoutine = null;
        }
    }
}
