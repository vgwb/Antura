using System.Collections;
using System.Collections.Generic;
using Antura.Minigames.DiscoverCountry;
using Homer;
using UnityEngine;
// ReSharper disable All

namespace Antura.Homer
{
    public struct QuestElement
    {
        public string Content;
        public string ContentNative;
        public string AudioId;
    }

    public class QuestNode
    {
        public HomerNode.NodeType Type;
        public string LocId;
        // permalink
        public string Id;

        public string HomerNodeId;

        // Text node
        public string Content;

        // Choices (for choice node)
        public List<HomerElement> Choices;

        // image to be loaded from resources
        public string Image;

        // if a special method needs to be triggered in the scene
        public string Action;

        // if a special method needs to be triggered in the scene when the dialogue is closed
        public string ActionPost;

        // well.. the mood
        public string Mood;

        // if we want to show the text in the native language first
        public bool Native;

        // if a pin / direction should be higlighted in scene / minimap
        public string NextTarget;

        public bool IsDialogueNode;
        public bool IsChoiceNode;

        //  This method assumes you have called SetupForNavigation(flowSlug) as flow setup.
        public QuestNode NextNode(int choiceIndex = 0)
        {
            return HomerAnturaManager.I.NextNode(choiceIndex);
        }

        public Sprite GetImage()
        {
            if (Image != null)
            {
                return Resources.Load<Sprite>("Discover_Photos/" + QuestManager.I.CurrentQuest.assetsFolder + "/" + Image);
            }
            return null;
        }

    }
}
