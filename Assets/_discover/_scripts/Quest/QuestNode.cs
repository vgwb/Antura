using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Antura.Discover
{

    public enum NodeType
    {
        TEXT = 1,
        CHOICE = 2,
        QUIZ = 3,
        PANEL = 4
    }

    public struct NodeChoice
    {
        public DialogueOption YarnOption;
        public YarnTaskCompletionSource<DialogueOption?>? OnOptionSelected;
        public System.Threading.CancellationToken completionToken;

        public int Index;
        public string Content;
        public string ContentNative;
        public string AudioId;
        public string Image;

        public bool Highlight;

        public Sprite GetImage()
        {
            if (Image != null)
            {
                //return Resources.Load<Sprite>("Discover_Photos/" + QuestManager.I.CurrentQuest.assetsFolder + "/" + Image + "_" + Index);
                //return Resources.Load<Sprite>("Discover/TUTORIAL/quiz_" + Index);
                return null;
            }
            return null;
        }
    }

    public class QuestNode
    {
        public NodeType Type;

        public string AudioId;
        public string Permalink;

        public string HomerNodeId;

        // Text node
        public string Content;
        public string ContentNative;

        public List<NodeChoice> Choices;
        public string Image;
        public string ImageTitle;

        public string Color;

        // if this node is an objective
        public string Task;

        // if a special method needs to be triggered in the scene
        public string Action;

        // if a special method needs to be triggered in the scene when the dialogue is closed
        public string ActionPost;

        // used to set QUIZ type
        public string BalloonType;

        // well.. the mood of the avatar talking to the player
        public string Mood;

        // if we want to show the text in the native language first
        public bool Native;

        // if a pin / direction should be higlighted in scene / minimap
        public string NextTarget;

        // shoudl we auto open the image?
        public bool ImageAutoOpen => Image != null && Color != "";

        public bool IsDialogueNode()
        {
            return Type == NodeType.TEXT;
        }

        public bool IsQuizNode()
        {
            return Type == NodeType.QUIZ;
        }

        public bool IsPanel()
        {
            return Type == NodeType.PANEL;
        }

        public Sprite GetImage()
        {
            if (Image != null)
            {
                return Resources.Load<Sprite>("Discover/" + QuestManager.I.CurrentQuest.assetsFolder + "/_img/" + Image);
            }
            return null;
        }

        public void Print()
        {
            string nodeInfo = "Type: " + Type + "\n";
            nodeInfo += "Content: " + Content + "\n";
            nodeInfo += "Content Native: " + ContentNative + "\n";

            if (Choices != null && Choices.Count > 0)
            {
                foreach (var choice in Choices)
                {
                    nodeInfo += "--- Choice ---" + "\n";
                    nodeInfo += "Content: " + choice.Content + "\n";
                    nodeInfo += "Content Native: " + choice.ContentNative + "\n";
                }
            }

            nodeInfo += "Permalink: " + Permalink + "\n";
            nodeInfo += "HomerId: " + HomerNodeId + "\n";
            nodeInfo += "AudioId: " + AudioId + "\n";
            nodeInfo += "Action: " + Action + "\n";
            nodeInfo += "Action Post: " + ActionPost + "\n";
            nodeInfo += "Task: " + Task + "\n";
            nodeInfo += "BalloonType: " + BalloonType + "\n";
            nodeInfo += "Mood: " + Mood + "\n";
            nodeInfo += "NextTarget: " + NextTarget + "\n";

            Debug.Log("QuestNode INFO\n" + nodeInfo);
        }

    }
}
