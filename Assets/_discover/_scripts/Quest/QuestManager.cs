using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Homer;
using Antura.Core;
using Antura.Audio;
using Antura.Homer;
using Antura.Utilities;
using Antura.UI;

namespace Antura.Minigames.DiscoverCountry
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class QuestManager : SingletonMonoBehaviour<QuestManager>
    {
        public Quests Quests;
        public QuestData CurrentQuest;
        public BonesCounter bonesCounter;
        public BonesCounter coinsCounter;

        private GameObject currentNPC;
        private int total_coins = 0;
        private int total_bones = 0;
        private readonly List<QuestNode> tmpQuestNodes = new List<QuestNode>(); // Used to get all QuestNodes with old system, and return a single one

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

            // AudioManager.I.PlayDiscoverDialogue(
            //     "NODEL-a8afc7ca-5911-431d-b0c9-1713862706608",
            //     Language.LanguageCode.arabic
            // );

        }

        /// <summary>
        /// Returns the correct quest node for the given actorID
        /// </summary>
        public QuestNode GetQuestNode(HomerActors.Actors actorId)
        {
            // TODO > At a certain point Homer shouldn't need to fill a list anymore and just return the first valid node?
            string command = $"TALK_{actorId.ToString()}";
            tmpQuestNodes.Clear();
            HomerAnturaManager.I.GetContent(CurrentQuest.QuestId, command, tmpQuestNodes, true);
            return tmpQuestNodes.Count == 0 ? null : tmpQuestNodes[0];
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

        public void OnCollectBone(GameObject go)
        {
            total_bones++;
            bonesCounter.IncreaseByOne();
            AppManager.I.Player.AddBones(1);
            Destroy(go);
        }

        public void OnCollectCoin(GameObject go)
        {
            total_coins++;
            HomerVars.TOTAL_COINS = total_coins;
            coinsCounter.IncreaseByOne();
            // Debug.Log("ANTURA COLLECTS coin nr " + total_coins);
            Destroy(go);
        }

        public void OnInfoPoint(string nodeId)
        {
            var questNode = HomerAnturaManager.I.GetQuestNodeByPermalink(CurrentQuest.QuestId, nodeId);
            DebugNodeInfo(questNode);
        }

        private void DebugNodeInfo(QuestNode questNode)
        {
            string nodeInfo = "\nType: " + questNode.Type;
            if (questNode.Type == HomerNode.NodeType.CHOICE)
            {
                nodeInfo += "\nContent: " + questNode.Content;
                foreach (var choice in questNode.Choices)
                {
                    nodeInfo += "\nChoice: " + choice._localizedContents[0]._text;
                }
            }
            else
            {
                nodeInfo += "\nContent: " + questNode.Content;
            }

            nodeInfo += "\nId: " + questNode.Id;
            nodeInfo += "\nAction: " + questNode.Action;
            nodeInfo += "\nMood: " + questNode.Mood;
            nodeInfo += "\nAudio: " + questNode.Audio;
            nodeInfo += "\nNextTarget: " + questNode.NextTarget;
            Debug.Log("QuestNode INFO: " + nodeInfo);
        }

    }
}
