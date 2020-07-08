using System;
using Antura.Core;
using Antura.Database;
using Antura.Keeper;
using UnityEngine;

namespace Antura.Map
{
    public class IntroDialogues : MonoBehaviour
    {
        public bool dialoguePlayed;

        private void OnTriggerEnter(Collider other)
        {
            var isAtMaxJourneyPosition = AppManager.I.Player.IsAtMaxJourneyPosition();
            var pin = GetComponent<Pin>();

            if (other.gameObject.CompareTag("Player")
                && isAtMaxJourneyPosition
                && !Equals(AppManager.I.Player.MaxJourneyPosition, AppManager.I.JourneyHelper.GetInitialJourneyPosition())
                && !dialoguePlayed
                && AppManager.I.Player.MaxJourneyPosition.PlaySession == 1
                && Equals(AppManager.I.Player.MaxJourneyPosition, pin.journeyPosition))
            {

                var maxJP = AppManager.I.Player.MaxJourneyPosition;
                LocalizationDataId locID;

                var lbData = AppManager.I.DB.GetLearningBlockDataById(maxJP.LearningBlockID);
                if (!string.IsNullOrEmpty(lbData.AudioFile))
                {
                    if (Enum.TryParse($"Map_Intro_LB_{lbData.AudioFile}", out locID))
                    {
                        KeeperManager.I.PlayDialogue(locID, true, true);
                    }
                }
                dialoguePlayed = true;
            }
        }
    }
}