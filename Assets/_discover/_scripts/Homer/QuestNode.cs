using System.Collections;
using System.Collections.Generic;
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
        // persed by custom methods. do not parse these manually
        public string[] Metadata;

        // Text node
        public string Content;

        // Choices (for choice node)
        public List<HomerElement> Choices;

        // image to be loaded from resources
        public string Image => GetMetadata("IMAGE");

        // if a special method needs to be triggered in the scene
        public string Action => GetMetadata("ACTION");

        // if a special method needs to be triggered in the scene when the dialogue is closed
        public string ActionPost => GetMetadata("ACTION_POST");

        // well.. the mood
        public string Mood => GetMetadata("MOOD");

        // if we want to show the text in the native language first
        public bool Native => GetMetadata("NATIVE") == "native";

        // if a pin / direction should be higlighted in scene / minimap
        public string NextTarget => GetMetadata("NEXTTARGET");

        public bool IsDialogueNode => Type == HomerNode.NodeType.TEXT || Type == HomerNode.NodeType.START;
        public bool IsChoiceNode => Type == HomerNode.NodeType.CHOICE;

        private string GetMetadata(string kind)
        {
            foreach (var metaId in Metadata)
            {
                var metadata = HomerAnturaManager.I.GetMetadataByValueId(metaId);
                // Debug.Log("metadata._uid= " + metadata._uid);
                if (metadata._uid == kind)
                {
                    return HomerAnturaManager.I.GetMetadataValueById(metaId)._value;
                }
            }
            return null;
        }

        //  This method assumes you have called SetupForNavigation(flowSlug) as flow setup.
        public QuestNode NextNode(int choiceIndex = 0)
        {
            return HomerAnturaManager.I.NextNode(choiceIndex);
        }

        public Sprite GetImage()
        {
            if (Image != null)
            {
                return Resources.Load<Sprite>("DiscoverImages/" + Image);
            }
            return null;
        }

    }
}
