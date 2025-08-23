using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace Antura.Discover
{
    public static class YarnDiscoverCommands
    {

        // ASSET

        [YarnCommand("asset")]
        public static void CommandAsset(string assetCode)
        {
            //Debug.Log($"ActionManager: ResolveNodeCommandAsset: {assetCode}");
            if (string.IsNullOrEmpty(assetCode))
                return;
            var db = DatabaseProvider.Instance;
            //var assetImage = db.Get<ItemData>("assetCode");
            if (db.TryGet<AssetData>(assetCode, out var assetImage))
            {
                UIManager.I.dialogues.ShowPostcard(assetImage.Image);
            }
        }

        [YarnCommand("asset_hide")]
        public static void CommandAssetHide()
        {
            UIManager.I.dialogues.HidePostcard();
        }

        // TASK

        [YarnCommand("task_start")]
        public static void CommandTaskStart(string taskCode)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandTaskStart: {taskCode}");
            if (string.IsNullOrEmpty(taskCode))
                return;

            //var task = QuestManager.I.GetTaskByCode(taskCode);
            //if (task == null)
            //{
            //    Debug.LogError($"ActionManager: Task not found for command {taskCode}");
            //    return;
            //}

            //QuestManager.I.TaskStart(task);
            ActionManager.I.ResolveQuestAction(taskCode);
        }

        [YarnCommand("task_end")]
        public static void CommandTaskEnd(string taskCode)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandTaskEnd: {taskCode}");
            if (string.IsNullOrEmpty(taskCode))
                return;

            //QuestManager.I.TaskEnd(task);
            ActionManager.I.ResolveQuestAction(taskCode);
        }

        // QUEST

        [YarnCommand("quest_end")]
        public static void CommandEndquest(int finalStars = 0)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandEndquest: {finalStars}");

            QuestEnd questResult = new QuestEnd();
            questResult.questId = QuestManager.I.CurrentQuest.Id;
            questResult.stars = finalStars;
            DiscoverAppManager.I.RecordQuestEnd(questResult);
        }

        // INVENTORY

        [YarnCommand("inventory")]
        public static void CommandInventory(string itemCode, string action = "add")
        {
            Debug.Log($"ActionManager: ResolveNodeCommandInventory: {itemCode} {action}");
            if (string.IsNullOrEmpty(itemCode))
                return;

            if (action == "add")
            {
                QuestManager.I.inventory.CollectItem(itemCode);
            }
            else if (action == "remove")
            {
                QuestManager.I.inventory.RemoveItem(itemCode);
            }
            else
            {
                Debug.LogError($"ActionManager: Unknown inventory action: {action}");
            }
        }

        // CARD

        [YarnCommand("card")]
        public static void CommandCard(string cardId)
        {
            // Debug.Log($"ActionManager: ResolveNodeCommandCard: {cardId}");
            DatabaseProvider.TryGet<CardData>(cardId, out var c);
            CardData card = c;
            DiscoverAppManager.I.RecordCardInteraction(card, true);
        }

        // ACTION

        [YarnCommand("action")]
        public static void CommandAction(string actionCode)
        {
            if (string.IsNullOrEmpty(actionCode))
                return;
            ActionManager.I.ResolveQuestAction(actionCode);
        }

        // ACTIVITY

        [YarnCommand("activity")]
        public static void ResolveNodeCommandActivity(string activityCode, string difficulty = null)
        {
            Debug.Log($"ActionManager: ResolveNodeCommandActivity: {activityCode} {difficulty}");
            if (string.IsNullOrEmpty(activityCode))
                return;

            //var activity = QuestManager.I.GetActivityByCode(command);
            // if (activity == null)
            //{
            //    Debug.LogError($"ActionManager: Activity not found for command {command}");
            //    return;
            //}

            //QuestManager.I.ActivityStart(activity);
            ActionManager.I.ResolveQuestAction(activityCode);
        }

    }
}
