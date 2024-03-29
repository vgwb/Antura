using System.Collections;
using System.Collections.Generic;
using Homer;
using UnityEngine;

namespace Antura.Homer
{
    public class QuestNode
    {
        public HomerNode.NodeType Type;
        // permalink
        public string Id;
        // persed by custom methods. do not parse these manually
        public string[] Metadata;

        // Text node
        public string Content;

        // Choice node
        public string ChoiceHeader;
        public List<HomerElement> Choices;

        // audio id to be player as voiceover (language to be added)
        public string GetAudio()
        {
            return GetMetadata("AUDIO");
        }

        // if a special methid needs to be triggered in the scene
        public string GetAction()
        {
            return GetMetadata("ACTION");
        }

        // well.. the mood
        public string GetMood()
        {
            return GetMetadata("MOOD");
        }

        // if a pin / direction should be higlighted in scene / minimap
        public string GetNextTarget()
        {
            return GetMetadata("NEXTTARGET");
        }

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
    }
}
