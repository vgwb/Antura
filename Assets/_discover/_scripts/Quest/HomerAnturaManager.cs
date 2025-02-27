using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Homer;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class HomerAnturaManager : MonoBehaviour
    {
        public static HomerAnturaManager I;
        private bool firstFlowSetup;
        private HomerProject currentHomerProject;
        private HomerFlowSlugs.FlowSlug currentFlowSlug;
        private HomerFlowRunning runningFlow;
        private string currentLanguage;
        private string nativeLanguage;

        void Awake()
        {
            if (I == null)
            {
                I = this;
                firstFlowSetup = true;
            }
            else
            {
                Debug.Log("HomerAnturaManager DUPLICATED!");
                Destroy(gameObject);
            }
        }

        public void Setup(string language = "EN", string native = "EN")
        {
            currentLanguage = language;
            nativeLanguage = native;
            if (currentHomerProject == null)
            {
                currentHomerProject = HomerJsonParser.LoadHomerProject();
                HomerProjectRunning.SetUp(currentHomerProject);
                currentHomerProject._locale = language;
            }
        }

        public void InitNode(HomerFlowSlugs.FlowSlug flowSlug)
        {
            GetContent(flowSlug, "", "INIT", true);
        }

        private string GetLocalizedContentFromElements(HomerLocalizedContent[] elements, string language)
        {
            string content = "";
            foreach (HomerLocalizedContent localizedContent in elements)
            {
                if (localizedContent._localeCode == language)
                {
                    content = localizedContent._text;
                    // Debug.Log($"GetQuestNodeByPermalink {permalink} : result: {questNode.Content}");
                    break;
                }
            }
            return content;
        }

        private void MoveToPermalinkNode(string permalink, HomerFlowSlugs.FlowSlug flowSlug)
        {
            HomerNode startNode = null;

            foreach (HomerNode homerNode in runningFlow.Flow._nodes)
            {
                if (homerNode._permalink == permalink)
                {
                    //Debug.Log("NODE FOUND " + permalink);
                    startNode = homerNode;
                    break;
                }
            }

            if (startNode != null)
            {
                SetupCurrentFlow(flowSlug);
                runningFlow.SelectedNode = HomerNodeRunning.Instantiate(startNode, runningFlow);
            }
        }

        public QuestNode GetNodeFromPermalink(string permalink, HomerFlowSlugs.FlowSlug flowSlug, string command)
        {
            //MoveToPermalinkNode(permalink, flowSlug);
            var node = GetContent(flowSlug, permalink, "", false);
            runningFlow._selectedNodeId = node.HomerNodeId;
            return node;
        }

        public QuestNode GetContentByCommand(HomerFlowSlugs.FlowSlug flowSlug, string command, bool restart)
        {
            return GetContent(flowSlug, "", command, restart);
        }

        private QuestNode GetContent(HomerFlowSlugs.FlowSlug flowSlug, string permalink, string command, bool restart)
        {
            HomerVars.CMD = command;
            HomerNode homerNode;

            SetupCurrentFlow(flowSlug);
            if (restart)
                runningFlow.Restart();

            // SEARCH CONTENT ::::::::::::::
            if (permalink != "")
            {
                homerNode = null;
                foreach (HomerNode hNode in runningFlow.Flow._nodes)
                {
                    if (hNode._permalink == permalink)
                    {
                        //Debug.Log("NODE FOUND " + permalink);
                        homerNode = hNode;
                        break;
                    }
                }

                if (homerNode != null)
                {
                    SetupCurrentFlow(flowSlug);
                    runningFlow.SelectedNode = HomerNodeRunning.Instantiate(homerNode, runningFlow);
                }
                //homerNode = runningFlow.GetNode();
                // Debug.Log("searching and found for " + homerNode._permalink);
            }
            else
            {
                homerNode = runningFlow.NextNode();
            }
            //END
            if (homerNode == null)
            {
                return null;
            }

            // if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CONDITION)
            // {
            //     homerNode = runningFlow.NextNode();
            // }

            if ((runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            || (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.TEXT))
            {
                return getQuestNode(runningFlow.SelectedNode);
            }
            // RECURRING
            else
            {
                Debug.LogWarning("RECURRING");
                return GetContent(currentFlowSlug, "", command, false);
            }
        }

        public void SetupForNavigation(HomerFlowSlugs.FlowSlug flowSlug)
        {
            SetupCurrentFlow(flowSlug);
        }

        public QuestNode NextNode(int choiceIndex = 0)
        {
            HomerNode next = null;

            if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                List<HomerElement> choices = runningFlow.SelectedNode.GetAvailableChoiceElements();
                var chosenChoice = choices[choiceIndex];
                next = runningFlow.NextNode(chosenChoice._id);
            }
            else
            {
                next = runningFlow.NextNode();
            }

            if (next == null)
                return null;

            return NextNodeRecur();
        }

        private QuestNode NextNodeRecur()
        {
            if ((runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            || (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.TEXT))
            {
                return getQuestNode(runningFlow.SelectedNode);
            }
            else
            {
                HomerNode homerNode = runningFlow.NextNode();
                if (homerNode == null)
                    return null;
                return NextNodeRecur();
            }
        }

        private void SetupCurrentFlow(HomerFlowSlugs.FlowSlug flowSlug)
        {
            if (currentHomerProject == null)
                Setup();

            if (firstFlowSetup || currentFlowSlug != flowSlug)
            {
                firstFlowSetup = false;

                foreach (var flow in currentHomerProject._flows)
                {
                    if (flow._slug == flowSlug.ToString())
                    {
                        currentFlowSlug = flowSlug;
                        runningFlow = HomerFlowRunning.Instantiate(flow);
                        runningFlow.SetUp(currentHomerProject);
                        break;
                    }
                }
            }
        }

        private QuestNode getQuestNode(HomerNodeRunning runner)
        {
            var node = new QuestNode();
            node.Permalink = runner.Node._permalink;
            node.HomerNodeId = runner.Node._id;
            node.Color = runner.Node._color;

            node.Image = GetImage(runner.Node._image);
            node.ImageTitle = ""; // TODO
            node.Action = GetMetadata("ACTION", runner.Node._metadata);
            node.ActionPost = GetMetadata("ACTION_POST", runner.Node._metadata);
            node.BalloonType = GetMetadata("BALLOON_TYPE", runner.Node._metadata);
            node.Mood = GetMetadata("MOOD", runner.Node._metadata);
            node.Native = GetMetadata("NATIVE", runner.Node._metadata) == "native";
            node.NextTarget = GetMetadata("NEXTTARGET", runner.Node._metadata);

            switch (runner.Node.GetNodeType())
            {
                case HomerNode.NodeType.CHOICE:
                    node.Type = NodeType.CHOICE;
                    break;
                case HomerNode.NodeType.TEXT:
                case HomerNode.NodeType.START:
                    node.Type = NodeType.TEXT;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (node.BalloonType == "quiz")
            {
                node.Type = NodeType.QUIZ;
            }

            var currentLocale = currentHomerProject._locale;

            if (runner.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                node.AudioId = runner.Node._header._id;

                HomerElement header = runner.Node._header;
                node.Content = runner.GetParsedText(header);

                currentHomerProject._locale = nativeLanguage;
                node.ContentNative = runner.GetParsedText(header);
                currentHomerProject._locale = currentLocale;

                node.Choices = new List<NodeChoice>();
                var HomerElements = runner.GetAvailableChoiceElements();
                int elementsIndex = 0;
                int elementsCount = HomerElements.Count();
                foreach (var element in HomerElements)
                {
                    var choice = new NodeChoice();
                    choice.Content = runner.GetParsedText(element);

                    currentHomerProject._locale = nativeLanguage;
                    choice.ContentNative = runner.GetParsedText(element);
                    currentHomerProject._locale = currentLocale;

                    //choice.Image = GetImage(element._image);
                    choice.AudioId = element._id;
                    choice.Index = HomerElements.IndexOf(element);

                    // if we are in the last element and the node color is blue then we higlight this!
                    if ((elementsIndex == elementsCount - 1) && (node.Color == "Blue"))
                    {
                        choice.Highlight = true;
                    }

                    node.Choices.Add(choice);
                    elementsIndex++;
                }
            }

            if (runner.Node.GetNodeType() == HomerNode.NodeType.TEXT)
            {
                node.AudioId = runner.Node._elements[0]._id;

                HomerElement element = runner.GetTextElement();
                node.Content = runner.ParsedText(element);

                currentHomerProject._locale = nativeLanguage;
                node.ContentNative = runner.ParsedText(element);
                currentHomerProject._locale = currentLocale;
            }

            return node;
        }

        private string GetMetadata(string kind, string[] Metadata)
        {
            foreach (var metaId in Metadata)
            {
                var metadata = GetMetadataByValueId(metaId);
                // Debug.Log("metadata._uid= " + metadata._uid);
                if (metadata._uid == kind)
                {
                    return GetMetadataValueById(metaId)._value;
                }
            }
            return null;
        }

        private string GetImage(string imageId)
        {
            if (imageId == "" || imageId == null)
            {
                return "";
            }
            if (runningFlow.Project._assets.TryGetValue(imageId, out HomerAsset foundAsset))
            {
                return foundAsset.name;
            }
            else
            {
                return "NOT FOUND";
            }
        }

        private HomerMetadata GetMetadataByValueId(string metadataValueId)
        {
            return runningFlow.Project.GetMetadataByValueId(metadataValueId);
        }

        private HomerMetadataValue GetMetadataValueById(string metadataValueId)
        {
            return runningFlow.Project.GetMetadataValueById(metadataValueId);
        }

        public void GetContentFromChoice(int choiceIndex, string permalink, HomerFlowSlugs.FlowSlug flowSlug,
            string command, List<QuestNode> answers, string language = "EN")
        {
            MoveToPermalinkNode(permalink, flowSlug);
            GetContentFromChoice(choiceIndex, flowSlug, command, language);
        }

        private void GetContentFromChoice(int choiceIndex, HomerFlowSlugs.FlowSlug flowSlug,
            string command, string language = "EN")
        {
            currentLanguage = language;
            if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                List<HomerElement> choices = runningFlow.SelectedNode.GetAvailableChoiceElements();
                var chosenChoice = choices[choiceIndex];
                //Debug.Log($"Auto chose {chosen+1}: {runningFlow.SelectedNode.GetParsedText(chosenChoice)}\n");
                runningFlow.NextNode(chosenChoice._id);
                GetContent(flowSlug, "", command, false);
            }
            else
            {
                throw new Exception("Current node is not a choice!");
            }
        }

        /// <summary>
        /// Retrieves a localized label based on the provided key and locale code.
        /// </summary>
        /// <param name="key">The key identifying the label to retrieve.</param>
        /// <param name="localeCode">The locale code specifying the language of the label.</param>
        /// <returns>The localized label corresponding to the given key and locale code.</returns>
        public string GetLabel(string key, string localeCode)
        {
            return HomerProjectRunning.I.GetLabel(key, localeCode);
        }

    }
}
