using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Antura.Utilities;
using UnityEngine;
using Homer;
using Antura.Homer;

namespace Antura.Minigames.DiscoverCountry
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class QuestManager : SingletonMonoBehaviour<QuestManager>
    {
        public QuestData Quest;

        private GameObject currentNPC;
        private int total_coins = 0;

        void Start()
        {
            HomerAnturaManager.I.Setup();
            total_coins = 0;

            List<QuestNode> answers = new List<QuestNode>();

            HomerAnturaManager.I.GetContent(
                            Quest.QuestId,
                            "INIT",
                            answers,
                            true
                            );

            Debug.Log("HOMER: " + answers.Count);
        }


        public void OnInteract(HomerActors.Actors ActorId)
        {
            Debug.Log("ANTURA INTERACTS WITH LL " + ActorId);
            string talk_action = "TALK_" + ActorId.ToString();

            List<QuestNode> answers = new List<QuestNode>();

            HomerAnturaManager.I.GetContent(
                            Quest.QuestId,
                            talk_action,
                            answers,
                            true
                            );

            foreach (QuestNode questNode in answers)
            {
                Debug.Log("QuestNode Content: " + questNode.Content);
                Debug.Log("QuestNode Id: " + questNode.Id);
                Debug.Log("QuestNode Action: " + questNode.GetAction());
                Debug.Log("QuestNode Mood: " + questNode.GetMood());
            }
        }

        public void OnCollectCoin(GameObject go)
        {
            total_coins++;
            HomerVars.TOTAL_COINS = total_coins;
            Debug.Log("ANTURA COLLECTS coin nr " + total_coins);
            Destroy(go);
        }

    }
}
