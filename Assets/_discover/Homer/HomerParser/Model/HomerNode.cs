using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homer
{
    public class HomerNode
    {
        public string _id;
        public Int64 _date;
        public string _flowId;
        public string _type;
        public string _actorId;
        public string[] _metadata;
        public string _cycleType;

        public string _permalink;

        //public HomerMetadataValue[] _metadata;

        public string _previousNodeId;

        public HomerElement[] _elements;
        public HomerElement _header;
        //"_x": 320,
        //"_y": 160,
        public HomerJumpTo _jumpTo;
        public HomerConnection[] _connections;

        public enum NodeType
        {
            START,
            TEXT,
            NOTE,
            CHOICE,
            VARIABLES,
            CONDITION,
            FAILCONDITION,
            RANDOM,
            SEQUENCE,
            JUMPTONODE,
            LABEL
        }

        public NodeType GetNodeType()
        {
            return (NodeType)Enum.Parse(typeof(NodeType), _type.ToUpper());
        }


        /*public bool GetMetaValue(string type)
        {
            foreach (var homerMetadata in _metadata)
            {
                if (homerMetadata._name==type)
                    return homerMetadata._values
            }
        }*/
    }
}
