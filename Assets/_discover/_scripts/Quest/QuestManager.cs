using System.Collections;
using System.Collections.Generic;
using Antura.Utilities;
using UnityEngine;
using Homer;
using Antura.Homer;

namespace Antura.Minigames.DiscoverCountry
{
    public class QuestManager : SingletonMonoBehaviour<QuestManager>
    {
        public QuestData Quest;

        private GameObject currentNPC;

        void Start()
        {
            HomerAnturaManager.I.Setup();

            var questNode = HomerAnturaManager.I.GetContent(
                            Quest.QuestId,
                            "INIT",
                            true
                            );

            Debug.Log("HOMER: " + questNode.Content);
        }


        public void OnInteract(HomerActors.Actors ActorId)
        {
            Debug.Log("ANTURA INTERACTS WITH LL " + ActorId);
            string talk_action = "TALK_" + ActorId.ToString();

            var questNode = HomerAnturaManager.I.GetContent(
                            Quest.QuestId,
                            talk_action,
                            true
                            );

            Debug.Log("HOMER: " + questNode.Content);
        }

    }
}
