using System.Collections;
using System.Collections.Generic;
using Homer;
using UnityEngine;

namespace Homer
{
    public static class HomerFlowRunningHelper
    {
        public static List<HomerNode> GetNodesByType(string type, HomerFlow flow)
        {
            List<HomerNode> ns = new List<HomerNode>();
            foreach (HomerNode node in flow._nodes)
            {
                if (node._type == type)
                    ns.Add(node);
            }
            return ns;
        }

        public static HomerConnection GetConnectionsByElementId(HomerNode node, string nodeElementId)
        {
            HomerConnection c = null;

            foreach (HomerConnection connection in node._connections)
            {
                if (connection._nodeElementId == nodeElementId)
                    c = connection;
            }
            return c;
        }

        public static HomerConnection GetFailedConnection(HomerNode node)
        {
            HomerConnection c = null;

            foreach (HomerConnection connection in node._connections)
            {
                if (connection._type == NodeType.failCondition)
                    c = connection;
            }
            return c;
        }

        public static List<HomerElement> GetPossibleElements(HomerNode node)
        {
            List<HomerElement> elements = new List<HomerElement>();
            foreach (HomerElement element in node._elements)
            {
                if (!element._visited)
                    elements.Add(element);
            }

            return elements;
        }
    }
}
