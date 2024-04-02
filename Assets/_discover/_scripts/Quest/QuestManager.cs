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
        public Quests Quests;
        public QuestData CurrentQuest;

        private GameObject currentNPC;
        private int total_coins = 0;

        void Start()
        {
            HomerAnturaManager.I.Setup();
            total_coins = 0;

            var answers = new List<QuestNode>();

            HomerAnturaManager.I.GetContent(
                            CurrentQuest.QuestId,
                            "INIT",
                            answers,
                            true
                            );

            foreach (QuestNode questNode in answers)
            {
                DebugNodeInfo(questNode);
            }
        }


        public void OnInteract(HomerActors.Actors ActorId)
        {
            Debug.Log("ANTURA INTERACTS WITH LL " + ActorId);
            string talk_action = "TALK_" + ActorId.ToString();

            var answers = new List<QuestNode>();
            HomerAnturaManager.I.GetContent(
                            CurrentQuest.QuestId,
                            talk_action,
                            answers,
                            true
                            );

            foreach (QuestNode questNode in answers)
            {
                DebugNodeInfo(questNode);
            }
        }

        public void OnCollectCoin(GameObject go)
        {
            total_coins++;
            HomerVars.TOTAL_COINS = total_coins;
            Debug.Log("ANTURA COLLECTS coin nr " + total_coins);
            Destroy(go);
        }

        public void OnInfoPoint(string nodeId)
        {
            var questNode = HomerAnturaManager.I.GetQuestNodeByPermalink(CurrentQuest.QuestId, nodeId);
            DebugNodeInfo(questNode);
        }

        private void DebugNodeInfo(QuestNode questNode)
        {
            string nodeInfo = "";
            nodeInfo += "\nContent: " + questNode.Content;
            nodeInfo += "\nId: " + questNode.Id;
            nodeInfo += "\nAction: " + questNode.Action;
            nodeInfo += "\nMood: " + questNode.Mood;
            nodeInfo += "\nAudio: " + questNode.Audio;
            nodeInfo += "\nNextTarget: " + questNode.NextTarget;
            Debug.Log("QuestNode INFO: " + nodeInfo);
        }

    }
}
