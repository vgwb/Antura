using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Homer;
using Antura.Core;
using Antura.Audio;
using Antura.Homer;
using Antura.Minigames.DiscoverCountry.Interaction;
using Antura.Utilities;
using Antura.UI;
using Antura.Language;

namespace Antura.Minigames.DiscoverCountry
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class QuestManager : SingletonMonoBehaviour<QuestManager>
    {
        public Quests Quests;
        public QuestData CurrentQuest;
        public BonesCounter bonesCounter;
        public BonesCounter coinsCounter;

        public bool DebugEnglish = false;

        public string LanguageCode = "";
        private GameObject currentNPC;
        public int total_coins = 0;
        public int total_bones = 0;
        public int total_items = 0;
        private readonly List<QuestNode> tmpQuestNodes = new List<QuestNode>(); // Used to get all QuestNodes with old system, and return a single one

        void Start()
        {
            HomerAnturaManager.I.Setup();
            total_coins = 0;
            if (DebugEnglish)
            {
                LanguageCode = "EN";
            }
            else
            {
                LanguageCode = "FR";
            }

            var answers = new List<QuestNode>();

            HomerAnturaManager.I.GetContent(
                            CurrentQuest.QuestId,
                            "INIT",
                            answers,
                            true,
                            LanguageCode
                            );

            // foreach (QuestNode questNode in answers)
            // {
            //     DebugNodeInfo(questNode);
            // }
        }

        /// <summary>
        /// Returns the correct quest node for the given actorID
        /// </summary>
        public QuestNode GetQuestNode(EdAgent agent)
        {
            // TODO > At a certain point Homer shouldn't need to fill a list anymore and just return the first valid node?
            string command = "TALK_" + agent.ActorId.ToString();
            if (agent.SubCommand != "")
            {
                command += "_" + agent.SubCommand;
            }
            tmpQuestNodes.Clear();
            HomerAnturaManager.I.GetContent(CurrentQuest.QuestId, command, tmpQuestNodes, true, LanguageCode);
            return tmpQuestNodes.Count == 0 ? null : tmpQuestNodes[0];
        }

        public void OnInteract(EdAgent agent)
        {
            //            Debug.Log("ANTURA INTERACTS WITH LL " + ActorId);
            string command = "TALK_" + agent.ActorId.ToString();
            if (agent.SubCommand != "")
            {
                command += "_" + agent.SubCommand;
            }

            var answers = new List<QuestNode>();
            HomerAnturaManager.I.GetContent(
                            CurrentQuest.QuestId,
                            command,
                            answers,
                            restart: true,
                            LanguageCode
                            );

            // foreach (QuestNode questNode in answers)
            // {
            //     DebugNodeInfo(questNode);
            // }
        }

        public void OnInfoPoint(InfoPoint infoPoint, string nodeId)
        {
            var questNode = HomerAnturaManager.I.GetQuestNodeByPermalink(CurrentQuest.QuestId, nodeId);
            InteractionManager.I.StartInfoPointDialogue(infoPoint, questNode);
            AudioManager.I.PlayDiscoverDialogue(
                questNode.LocId,
                Language.LanguageCode.french
            );

            // DebugNodeInfo(questNode);
        }
        public void OnCollectItem(GameObject go)
        {
            total_items++;
            HomerVars.TOTAL_ITEMS_1 = total_items;
            Destroy(go);
        }

        public void OnCollectBone(GameObject go)
        {
            total_bones++;
            bonesCounter.IncreaseByOne();
            AppManager.I.Player.AddBones(1);
            Destroy(go);
        }

        public void UpateCoinsCounter()
        {
            coinsCounter.SetValue(HomerVars.TOTAL_COINS);
        }
        public void OnCollectCoin(GameObject go)
        {
            total_coins++;
            HomerVars.TOTAL_COINS = total_coins;
            coinsCounter.IncreaseByOne();
            Debug.Log("ANTURA COLLECTS coin nr " + HomerVars.TOTAL_COINS);
            Destroy(go);
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
            nodeInfo += "\nLocId: " + questNode.LocId;
            nodeInfo += "\nAction: " + questNode.Action;
            nodeInfo += "\nMood: " + questNode.Mood;
            nodeInfo += "\nNextTarget: " + questNode.NextTarget;
            Debug.Log("QuestNode INFO: " + nodeInfo);
        }

    }
}
