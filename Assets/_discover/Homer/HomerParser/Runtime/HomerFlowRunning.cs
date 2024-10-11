using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static Homer.HomerFlowRunningHelper;
using Random = UnityEngine.Random;

namespace Homer
{
    public class HomerFlowRunning
    {
        public bool Started;

        public HomerProject Project;
        public HomerFlow Flow;

        public string _selectedFlowId;
        public string _selectedNodeId;


        List<HomerNodeRunning> allNodes;

        public HomerNodeRunning SelectedNode;

        bool isJumping;

        public static HomerFlowRunning Instantiate(HomerFlow flow)
        {
            HomerFlowRunning hfr = new HomerFlowRunning() { Flow = flow };

            hfr._selectedFlowId = flow._id;

            hfr.Started = false;
            return hfr;
        }


        public void SetUp(HomerProject project)
        {
            Project = project;
            Started = true;

            InitializeVariations();

            foreach (var homerNode in Flow._nodes)
            {
                if (homerNode._type == "Start")
                {
                    SelectedNode = HomerNodeRunning.Instantiate(homerNode, this);
                    break;
                }
            }
        }

        public void InitializeVariations()
        {
            HomerProjectRunning._variations = new List<HomerVariation>();

            foreach (HomerFlow flow in Project._flows)
            {
                foreach (HomerNode node in flow._nodes)
                {
                    foreach (HomerElement element in node._elements)
                    {
                        if (element._localizedContents.Length > 0)
                        {
                            Regex variationsRegExp = new Regex(Regexes.VARIATIONS);
                            string content = HomerNodeRunningHelper.GetContentText(Project, element);

                            int idx = 0;
                            foreach (Match variationBlock in variationsRegExp.Matches(content))
                            {
                                string variation = variationBlock.ToString();

                                string v = variation.Replace("[[", "").Replace("]]", "").Trim();
                                v = v.Replace(" | ", "|");
                                string type = v.Split(' ')[0].Trim();
                                string[] variations = v.Replace(type, "").Split('|');

                                HomerVariation variationObj = new HomerVariation();
                                variationObj._index = idx;
                                variationObj._type = type;
                                variationObj._values = variations;
                                variationObj._elementId = element._id;
                                variationObj._visited = new string[0];

                                //{ _index: idx, _type: type, _values:
                                //variations, _elementId: element._id, _visited:[]}
                                HomerProjectRunning._variations.Add(variationObj);
                            }
                        }
                    }
                }
            }
        }

        public HomerFlow GetSelectedFlow()
        {
            HomerFlow f = null;

            foreach (HomerFlow flow in Project._flows)
            {
                if (flow._id == _selectedFlowId)
                    f = flow;
            }

            return f;
        }

        public HomerNode GetNode(string nodeId = null)
        {
            nodeId = nodeId == null ? _selectedNodeId : nodeId;

            HomerNode n = null;
            HomerFlow f = GetSelectedFlow();

            foreach (HomerNode node in f._nodes)
            {
                if (node._id == nodeId)
                    n = node;
            }

            return n;
        }


        public HomerElement GetNodeElement(string nodeId, string elementId)
        {
            HomerNode node = GetNode(nodeId);

            foreach (HomerElement element in node._elements)
            {
                if (element._id == elementId)
                    return element;
            }

            return null;
        }

        public HomerConnection GetAvailableConnections(string ElementId = null)
        {
            HomerNode node = GetNode();

            HomerConnection availableConnection = null;

            List<HomerElement> availableElements = new List<HomerElement>();

            foreach (HomerElement element in node._elements)
            {
                if (!element._visited)
                    availableElements.Add(element);
            }

            HomerElement possibleElement;

            //if (node._connections.Length == 0 && node._type != NodeType.jumpToNode)
            //    return null;


            switch (node._type)
            {
                case NodeType.start:
                case NodeType.text:
                case NodeType.note:
                case NodeType.layout:
                    if (node._connections != null && node._connections.Length > 0)
                        availableConnection = node._connections[0];
                    break;

                case NodeType.choice:
                    availableConnection = GetConnectionsByElementId(node, ElementId);
                    break;

                case NodeType.subFlow:
                    if (node._connections != null && node._connections.Length > 0)
                    {
                        if (!HomerProjectRunning.ActiveSubFlows.Contains(node._id))
                        {
                            foreach (var homerConnection in node._connections)
                            {
                                if (homerConnection._type == NodeType.subFlow)
                                {
                                    availableConnection = homerConnection;
                                    break;
                                }
                            }

                            if (availableConnection != null)
                                HomerProjectRunning.ActiveSubFlows.Add(node._id);
                        }
                        else
                        {
                            HomerProjectRunning.ActiveSubFlows.Remove(HomerProjectRunning.ActiveSubFlows.Last());

                            foreach (var homerConnection in node._connections)
                            {
                                if (homerConnection._type != NodeType.subFlow)
                                {
                                    availableConnection = homerConnection;
                                    break;
                                }
                            }
                        }
                    }

                    break;


                case NodeType.condition:

                    foreach (HomerElement element in node._elements)
                    {
                        string condition =
                            HomerNodeRunningHelper.GetContentText(HomerProjectRunning.I.Project, element);
                        
                        //strip {}
                        condition = condition.TrimStart('{').TrimEnd('}');
                        
                        bool result = HomerNodeRunning.Evaluate(condition);
                        if (result && availableConnection == null)
                        {
                            availableConnection = GetConnectionsByElementId(node, element._id);
                        }
                    }

                    if (availableConnection == null)
                        availableConnection = GetFailedConnection(node);

                    break;

                case NodeType.variables:

                    foreach (HomerElement element in node._elements)
                    {
                        SelectedNode.ParsedText(element);
                    }

                    if (node._connections != null && node._connections.Length > 0)
                        availableConnection = node._connections[0];
                    break;

                case NodeType.random:
                    if (node._connections != null && node._connections.Length > 0)
                    {
                        var rnd = Random.Range(0, node._connections.Length);
                        availableConnection = node._connections[rnd];
                    }

                    break;

                case NodeType.sequence:

                    switch (node._cycleType)
                    {
                        case CycleType.list:

                            possibleElement = null;

                            if (availableElements.Count > 0)
                                possibleElement = availableElements[0];

                            if (possibleElement != null)
                            {
                                possibleElement._visited = true;
                                availableConnection = GetConnectionsByElementId(node, possibleElement._id);
                            }
                            else
                            {
                                var failedConnection = GetFailedConnection(node);
                                if (failedConnection != null)
                                    availableConnection = failedConnection;
                                else
                                    availableConnection = GetConnectionsByElementId(node,
                                        node._elements[node._elements.Length - 1]._id);
                            }

                            break;

                        case CycleType.loop:

                            List<HomerElement> possibleElements = GetPossibleElements(node);

                            possibleElement = null;

                            if (availableElements.Count > 0)
                                possibleElement = availableElements[0];

                            if (possibleElement != null)
                            {
                                possibleElement._visited = true;
                                availableConnection = GetConnectionsByElementId(node, possibleElement._id);
                            }
                            else
                            {
                                foreach (HomerElement element in node._elements)
                                {
                                    element._visited = false;
                                    availableConnection = GetConnectionsByElementId(node, node._elements[0]._id);
                                }
                            }

                            break;

                        case CycleType.random:
                            var rnd = Random.Range(0, node._elements.Length);
                            possibleElement = node._elements[rnd];
                            availableConnection = GetConnectionsByElementId(node, possibleElement._id);
                            break;

                        case CycleType.smartRandom:

                            if (availableElements.Count == 0)
                            {
                                HomerConnection failedConnection = GetFailedConnection(node);
                                if (failedConnection != null)
                                {
                                    availableConnection = failedConnection;
                                    break;
                                }
                                else
                                {
                                    foreach (HomerElement element in node._elements)
                                    {
                                        element._visited = false;
                                    }

                                    availableElements = new List<HomerElement>(node._elements);
                                }
                            }

                            var sRnd = Random.Range(0, availableElements.Count);
                            possibleElement = availableElements[sRnd];
                            possibleElement._visited = true;
                            availableConnection = GetConnectionsByElementId(node, possibleElement._id);
                            break;
                    }

                    break;

                case NodeType.jumpToNode:
                    _selectedFlowId = node._jumpTo.flowId;
                    _selectedNodeId = node._jumpTo.nodeId;
                    availableConnection = null;
                    break;
            }

            return availableConnection;
        }

        public HomerNode NextNode(string elementId = null)
        {
            if (!Started)
                throw new Exception("Must call start before next");

            if (_selectedNodeId == "THE END")
                return null;

            // If the current node ID is not set, set it to the ID of the start node
            if (_selectedNodeId == null)
                _selectedNodeId = GetNodesByType(NodeType.start, Flow)[0]._id;

            HomerNode currentNode = GetNode(_selectedNodeId);

            HomerElement nodeElement = GetNodeElement(_selectedNodeId, elementId);

            HomerConnection connection = GetAvailableConnections(elementId);

            if (connection == null)
                connection = GetFailedConnection(currentNode);

            // Handle logic based on the type of the previous node
            if (currentNode._type == NodeType.choice && !isJumping)
            {
                // Logic for handling choices and just-once elements
                if (nodeElement != null && nodeElement._justOnce)
                {
                    nodeElement._visited = true;
                }

                SelectedNode.GetParsedText(nodeElement, true);
            }
            else if (currentNode._type == NodeType.jumpToNode)
            {
                if (!NodeExists(currentNode._jumpTo.nodeId, currentNode._jumpTo.flowId))
                {
                    // if (actualNode._jumpTo.flowId === "false" || actualNode._jumpTo.flowId === "Select a Flow") {
                    Debug.Log("This 'jump to Node' is not set");
                    return currentNode;
                }

                // Set the jumping flag
                isJumping = true;
            }
            else
            {
                if (nodeElement != null)
                    nodeElement._visited = true;
            }

            if (connection == null || connection._to == null  && currentNode._type != NodeType.jumpToNode)
            {
                if (HomerProjectRunning.ActiveSubFlows.Count > 0)
                {
                    var subFlow_node_id =
                        HomerProjectRunning.ActiveSubFlows[HomerProjectRunning.ActiveSubFlows.Count - 1];
                    _selectedNodeId = subFlow_node_id;
                    NextNode();
                    return GetNode(_selectedNodeId);
                }
                else if (currentNode._type != NodeType.jumpToNode)
                {
                    _selectedNodeId = "THE END";

                    return null;
                }
            }
            else if (!isJumping)
            {
                _selectedNodeId = connection._to;
            }

            /**
             * Next Node
             */
            HomerNode targetNode = GetNode(_selectedNodeId);
            targetNode._previousNodeId = currentNode._id;

            /**
             * if node type choice and no elements are available
             * check if the node has a failed connection and get it for next node.
             */
            List<HomerElement> possibleElements = GetPossibleElements(targetNode);

            if (targetNode._type == NodeType.choice && possibleElements.Count == 0)
            {
                connection = GetFailedConnection(targetNode);

                if (connection != null)
                {
                    _selectedNodeId = connection._to;
                    targetNode = GetNode(_selectedNodeId);
                    targetNode._previousNodeId = currentNode._id;
                    return NextNode();
                }
            }

            isJumping = false;

            SelectedNode = HomerNodeRunning.Instantiate(GetNode(), this);

            if (
                targetNode._type == NodeType.start ||
                targetNode._type == NodeType.note ||
                targetNode._type == NodeType.sequence ||
                targetNode._type == NodeType.random ||
                targetNode._type == NodeType.variables ||
                targetNode._type == NodeType.layout ||
                targetNode._type == NodeType.subFlow ||
                targetNode._type == NodeType.jumpToNode ||
                targetNode._type == NodeType.condition)
            {
                return NextNode();
            }
            else
            {
                return targetNode;
            }
        }

        public void Restart()
        {
            _selectedNodeId = null;
        }
        
        bool NodeExists(string jumpToNodeId, string jumpToFlowId)
        {
            HomerFlow flow = null;
            foreach (var aflow in Project._flows)
            {
                if (aflow._id == jumpToFlowId)
                     flow = aflow;
            }

            if (flow == null)
                flow = Flow;

            foreach (var flowNode in flow._nodes)
            {
                if (flowNode._id == jumpToNodeId)
                    return true;
            }

            return false;
        }
    }
}