using System;
using Antura.Core;
using Antura.Database;
using Antura.Keeper;
using UnityEngine;

namespace Antura.Map
{
    public class IntroDialogues : MonoBehaviour
    {
        public int stageNumber;
        public bool dialoguePlayed;

        private void OnTriggerEnter(Collider other)
        {
            var isAtMaxJourneyPosition = AppManager.I.Player.IsAtMaxJourneyPosition();

            if (other.gameObject.CompareTag("Player")
                && isAtMaxJourneyPosition
                && !Equals(AppManager.I.Player.MaxJourneyPosition, AppManager.I.JourneyHelper.GetInitialJourneyPosition())
                && !dialoguePlayed
                && AppManager.I.Player.MaxJourneyPosition.PlaySession == 1)
            {
                var maxJP = AppManager.I.Player.MaxJourneyPosition;
                LocalizationDataId locID;
                if (Enum.TryParse("Map_Intro_LB_" + maxJP.Stage + "_" + maxJP.LearningBlock, out locID))
                {
                    KeeperManager.I.PlayDialogue(locID, true, true);
                }
                dialoguePlayed = true;
            }
        }
    }
}