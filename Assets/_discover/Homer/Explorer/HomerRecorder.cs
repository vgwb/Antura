using System.Collections.Generic;
using UnityEngine;

namespace Homer
{
    public static class HomerRecorder
    {
        public static Dictionary<float, List<HomerNodeRecorded>> RecordedFlows;

        public static void Setup()
        {
            RecordedFlows = new Dictionary<float, List<HomerNodeRecorded>>();
        }

        public static List<HomerNodeRecorded> SetupARecording()
        {
            List<HomerNodeRecorded> newRecording = new List<HomerNodeRecorded>();
            RecordedFlows.Add(Time.time, newRecording);
            return newRecording;
        }

    }

    public class HomerNodeRecorded
    {
        public string Type;
        public string NodeId;
        public List<string> Contents;
        public int ChosenContent;
        public HomerNode Node;

        public HomerNodeRecorded(HomerNode node)
        {
            NodeId = node._id;
            Type = node.GetNodeType().ToString();
            Contents = new List<string>();
            ChosenContent = 0;
            Node = node;
        }
    }
}