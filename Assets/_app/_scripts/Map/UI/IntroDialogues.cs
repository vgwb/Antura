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

            if (other.gameObject.CompareTag("Player") && isAtMaxJourneyPosition
                && stageNumber > 1
                && AppManager.I.Player.MaxJourneyPosition.LearningBlock == 1
                && AppManager.I.Player.MaxJourneyPosition.PlaySession == 1
                && !dialoguePlayed)
            {
                var data = new LocalizationDataId[7];
                data[2] = LocalizationDataId.Map_Intro_Map2;
                data[3] = LocalizationDataId.Map_Intro_Map3;
                data[4] = LocalizationDataId.Map_Intro_Map4;
                data[5] = LocalizationDataId.Map_Intro_Map5;
                data[6] = LocalizationDataId.Map_Intro_Map6;
                KeeperManager.I.PlayDialog(data[stageNumber], true, true);
                dialoguePlayed = true;
            }
        }
    }
}