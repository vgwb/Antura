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

        public string GetAction()
        {
            return GetMetadata("ACTION");
        }

        public string GetMood()
        {
            return GetMetadata("MOOD");
        }

        private string GetMetadata(string kind)
        {
            foreach (var metaId in Metadata)
            {
                var metadata = HomerAnturaManager.I.GetMetadataByValueId(metaId);
                //Debug.Log("metadata._uid= " + metadato._uid);
                if (metadata._uid == kind)
                {
                    return HomerAnturaManager.I.GetMetadataValueById(metaId)._value;
                }
            }
            return null;
        }
    }
}
