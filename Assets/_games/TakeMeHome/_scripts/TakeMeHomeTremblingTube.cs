using System.Collections;
using System.Collections.Generic;
using Antura.Minigames.Tobogan;
using UnityEngine;

namespace Antura.Minigames.TakeMeHome
{
    public class TakeMeHomeTremblingTube : MonoBehaviour
    {
        public bool Trembling;
        public bool playSound = false;
        public float soundBasePitch = 1;
        public float soundBaseVolume = 1;
        public float soundTurnOnSpeed = 3;
        public float soundTurnOffSpeed = 3;

        float tremblingAmount;
        Vector3 tremblingOffset;
        Vector3 initialPosition;

        public float tremblingSpeedX = 1;
        public float tremblingSpeedY = 1;
        public float tremblingSpeedZ = 1;
        public float tremblingAmountX = 0.1f;
        public float tremblingAmountY = 0.1f;
        public float tremblingAmountZ = 0.1f;

        TubeAudio audioLooper;
        void Start()
        {
            initialPosition = transform.localPosition;

            if (playSound)
            {
                var source = TakeMeHomeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.PipeBlowIn);
                source.Loop = true;
                source.Volume = 0;
                audioLooper = new TubeAudio(source);
                audioLooper.basePitch = soundBasePitch;
                audioLooper.baseVolume = soundBaseVolume;
                audioLooper.turnOnSpeed = soundTurnOnSpeed;
                audioLooper.turnOffSpeed = soundTurnOffSpeed;
            }
        }

        void Update()
        {
            // Trembling
            Vector3 noise = new Vector3(
                tremblingAmountX * Mathf.Cos(Mathf.Repeat(Time.realtimeSinceStartup * tremblingSpeedX, 2 * Mathf.PI)),
                tremblingAmountY * Mathf.Cos(Mathf.Repeat(Time.realtimeSinceStartup * tremblingSpeedY, 2 * Mathf.PI)),
                tremblingAmountZ * Mathf.Cos(Mathf.Repeat(Time.realtimeSinceStartup * tremblingSpeedZ, 2 * Mathf.PI)));

            tremblingOffset = Vector3.Lerp(tremblingOffset, noise, 40.0f * Time.deltaTime);

            if (audioLooper != null)
            {
                audioLooper.enable = Trembling;
                audioLooper.Update(Time.deltaTime);
            }

            if (Trembling)
            {
                tremblingAmount = Mathf.Lerp(tremblingAmount, 1.0f, 20.0f * Time.deltaTime);
            }
            else
            {
                tremblingAmount = Mathf.Lerp(tremblingAmount, 0.0f, 5.0f * Time.deltaTime);
            }

            transform.localPosition = Vector3.Lerp(initialPosition, initialPosition + tremblingOffset, tremblingAmount);
        }

        void OnDisable()
        {
            if (audioLooper != null)
                audioLooper.Stop();
        }
    }
}
