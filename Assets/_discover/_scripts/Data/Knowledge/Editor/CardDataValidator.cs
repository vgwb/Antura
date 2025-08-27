using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace Antura.Discover.Editor
{
    public static class CardDataValidator
    {
        [MenuItem("Antura/Discover/Knowledge/Validate All CardData")]
        public static void ValidateAllCards()
        {
            string[] guids = AssetDatabase.FindAssets("t:CardData");
            int issues = 0;

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                CardData card = AssetDatabase.LoadAssetAtPath<CardData>(assetPath);

                if (card != null)
                {
                    if (ValidateCard(card, assetPath))
                        issues++;
                }
            }

            if (issues == 0)
                Debug.Log("✅ All cards validated successfully!");
            else
                Debug.LogWarning($"⚠️ Found {issues} cards with issues");
        }

        private static bool ValidateCard(CardData card, string assetPath)
        {
            bool hasIssues = false;

            if (string.IsNullOrEmpty(card.Id))
            {
                Debug.LogWarning($"Card missing ID: {assetPath}");
                hasIssues = true;
            }

            if (card.Title == null || card.Title.IsEmpty)
            {
                Debug.LogWarning($"Card missing title: {card.name}");
                hasIssues = true;
            }

            if (card.Topics == null || card.Topics.Count == 0)
            {
                Debug.LogWarning($"Card missing topics: {card.name}");
                hasIssues = true;
            }

            if (card.ImageAsset == null)
            {
                Debug.LogWarning($"Card missing image: {card.name}");
                hasIssues = true;
            }

            return hasIssues;
        }
    }
}
