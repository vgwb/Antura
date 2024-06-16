using System.Collections;
using System.Collections.Generic;
using Homer;
using UnityEngine;
// ReSharper disable All

namespace Antura.Homer
{
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

        // audio id to be player as voiceover (language to be added)
        public string Audio => GetMetadata("AUDIO");

        // if a special methid needs to be triggered in the scene
        public string Action => GetMetadata("ACTION");

        // well.. the mood
        public string Mood => GetMetadata("MOOD");

        // if a pin / direction should be higlighted in scene / minimap
        public string NextTarget => GetMetadata("NEXTTARGET");

        private string GetMetadata(string kind)
        {
            foreach (var metaId in Metadata)
            {
                var metadata = HomerAnturaManager.I.GetMetadataByValueId(metaId);
                //                Debug.Log("metadata._uid= " + metadata._uid);
                if (metadata._uid == kind)
                {
                    return HomerAnturaManager.I.GetMetadataValueById(metaId)._value;
                }
            }
            return null;
        }

        //This method assumes you have called SetupForNavigation(flowSlug) as flow setup.
        public QuestNode NextNode(int choiceIndex = 0)
        {
            return HomerAnturaManager.I.NextNode(choiceIndex);
        }

    }
}
