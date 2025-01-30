using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Homer;
using Antura.Core;
using Antura.Audio;
using Antura.Utilities;
using Antura.UI;
using Antura.Language;
using System.Runtime.Remoting.Messaging;

namespace Antura.Minigames.DiscoverCountry
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class QuestManager : SingletonMonoBehaviour<QuestManager>
    {
        public GameObject Player;

        public Quests Quests;
        public QuestData CurrentQuest;
        public BonesCounter bonesCounter;
        public BonesCounter coinsCounter;

        public Transform PlayerSpawnPoint;
        public string LanguageCode = "";
        private GameObject currentNPC;
        public int total_coins = 0;
        public int total_bones = 0;
        public int total_items = 0;
        private readonly List<QuestNode> tmpQuestNodes = new List<QuestNode>(); // Used to get all QuestNodes with old system, and return a single one

        [Header("DEBUG")]
        public bool DebugQuest = false;
        public string DebugLanguage = "";

        void Start()
        {
            // INSTATIATE LEVEL PREFAB
            // Vector3 spawnPosition = new Vector3(0, 1, 0);
            // GameObject levelInstance = Instantiate(CurrentQuest.GameLevel, spawnPosition, Quaternion.identity);
            // levelInstance.transform.SetParent(null);

            total_coins = 0;
            if (coinsCounter == null)
            {
                coinsCounter = GameObject.Find("CoinsCounter").GetComponent<BonesCounter>();
            }
            if (bonesCounter == null)
            {
                bonesCounter = GameObject.Find("BonesCounter").GetComponent<BonesCounter>();
            }

            if (PlayerSpawnPoint != null)
            {
                Player.transform.SetPositionAndRotation(PlayerSpawnPoint.position, PlayerSpawnPoint.rotation);
            }

            if (DebugQuest && DebugLanguage != "")
            {
                LanguageCode = DebugLanguage;
            }
            else
            {
                LanguageCode = "FR";
            }

            HomerAnturaManager.I.Setup(LanguageCode);
            HomerAnturaManager.I.InitNode(CurrentQuest.QuestId);
        }

        public QuestNode GetQuestNode(string permalink, string command)
        {
            // TODO
            return HomerAnturaManager.I.GetContentByCommand(CurrentQuest.QuestId, command, true);
        }


        public QuestNode GetNodeByCommand(string command)
        {
            return HomerAnturaManager.I.GetContentByCommand(CurrentQuest.QuestId, command, true);
        }

        public QuestNode GetNodeByPermalink(string permalink)
        {
            return HomerAnturaManager.I.GetNodeFromPermalink(permalink, CurrentQuest.QuestId, "");
        }

        public QuestNode GetNextNode(int choiceIndex = 0)
        {
            return HomerAnturaManager.I.NextNode(choiceIndex);
        }

        public void OnInteract(EdAgent agent)
        {
            // Debug.Log("ANTURA INTERACTS WITH LL " + agent.ActorId);
        }

        public void OnNodeStart(QuestNode node)
        {
            if (node.Action != null)
                ActionManager.I.ResolveAction(node.Action);
        }

        public void OnNodeEnd(QuestNode node)
        {
            if (node.NextTarget != null)
                ActionManager.I.CameraShowTarget(node.NextTarget);
            if (node.ActionPost != null)
                ActionManager.I.ResolveAction(node.ActionPost);
        }

        public void OnCollectItem(GameObject go)
        {
            total_items++;
            HomerVars.TOTAL_ITEMS = total_items;
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

    }
}
