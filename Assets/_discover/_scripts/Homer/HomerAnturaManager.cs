using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Homer;
using UnityEngine;

namespace Antura.Homer
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

        public void Setup()
        {
            if (currentHomerProject == null)
            {
                currentHomerProject = HomerJsonParser.LoadHomerProject();
                HomerProjectRunning.SetUp(currentHomerProject);
            }
        }

        public QuestNode GetQuestNodeById(HomerFlowSlugs.FlowSlug flowSlug, string permalink,
            string language = "EN")
        {
            currentLanguage = language;
            if (currentFlowSlug != flowSlug)
            {
                SetupCurrentFlow(flowSlug);
            }

            foreach (HomerNode homerNode in runningFlow.Flow._nodes)
            {
                if (homerNode._permalink == permalink)
                {
                    QuestNode questNode = getQuestNode(homerNode);
                    //we just take the first line
                    questNode.Content = GetLocalizedContentFromElements(homerNode._elements[0]._localizedContents, language);
                    return questNode;
                }
            }

            return null;
        }

        public void GetContentFromPermalink(string permalink, HomerFlowSlugs.FlowSlug flowSlug, string command,
            List<QuestNode> answers, string language = "EN")
        {
            currentLanguage = language;
            MoveToPermalinkNode(permalink, flowSlug);

            GetContent(flowSlug, command, answers, false, language);
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

        public void GetContentFromChoice(int choiceIndex, string permalink, HomerFlowSlugs.FlowSlug flowSlug,
            string command, List<QuestNode> answers, string language = "EN")
        {
            MoveToPermalinkNode(permalink, flowSlug);
            GetContentFromChoice(choiceIndex, flowSlug, command, answers, language);
        }

        public void GetContentFromChoice(int choiceIndex, HomerFlowSlugs.FlowSlug flowSlug,
            string command, List<QuestNode> answers, string language = "EN")
        {
            currentLanguage = language;
            if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                List<HomerElement> choices = runningFlow.SelectedNode.GetAvailableChoiceElements();
                var chosenChoice = choices[choiceIndex];
                //Debug.Log($"Auto chose {chosen+1}: {runningFlow.SelectedNode.GetParsedText(chosenChoice)}\n");
                runningFlow.NextNode(chosenChoice._id);
                GetContent(flowSlug, command, answers, false, language);
            }
            else
            {
                throw new Exception("Current node is not a choice!");
            }
        }

        public void GetContent(HomerFlowSlugs.FlowSlug flowSlug, string command, List<QuestNode> answers,
            bool restart, string language = "EN")
        {
            // SETUP ::::::::::::::::::::::
            currentLanguage = language;
            HomerVars.CMD = command;

            SetupCurrentFlow(flowSlug);

            if (restart)
            {
                runningFlow.Restart();
            }

            // SEARCH CONTENT ::::::::::::::

            HomerNode homerNode = runningFlow.NextNode();

            //END
            if (homerNode == null)
            {
                return;
            }

            QuestNode questNode = getQuestNode(homerNode);

            // CHOICE
            if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                List<HomerElement> choices = runningFlow.SelectedNode.GetAvailableChoiceElements();

                HomerElement header = runningFlow.SelectedNode.Node._header;
                string headerText = GetLocalizedContentFromElements(runningFlow.SelectedNode.Node._header._localizedContents, language);

                questNode.LocId = homerNode._header._id;
                questNode.Content = headerText;
                questNode.Choices = choices;

                answers.Add(questNode);
            }

            // TEXT
            else if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.TEXT &&
                     runningFlow.SelectedNode.Node._elements.Length > 0)
            {
                HomerElement element = runningFlow.SelectedNode.GetTextElement();
                string text = GetLocalizedContentFromElements(runningFlow.SelectedNode.ChosenElement._localizedContents, language);
                questNode.Content = text;

                answers.Add(questNode);
            }

            // RECUR
            else
            {
                GetContent(currentFlowSlug, command, answers, false, language);
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
            {
                return null;
            }

            return NextNodeRecur();
        }

        private QuestNode NextNodeRecur()
        {
            if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                QuestNode questNode = getQuestNode(runningFlow.SelectedNode.Node);
                HomerElement header = runningFlow.SelectedNode.Node._header;
                string headerText = runningFlow.SelectedNode.GetParsedText(header);
                questNode.Content = headerText;
                questNode.Choices = runningFlow.SelectedNode.GetAvailableChoiceElements();

                return questNode;
            }
            else if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.TEXT)
            {
                QuestNode questNode = getQuestNode(runningFlow.SelectedNode.Node);
                HomerElement element = runningFlow.SelectedNode.GetTextElement();
                string text = GetLocalizedContentFromElements(runningFlow.SelectedNode.ChosenElement._localizedContents, currentLanguage);

                questNode.Content = text;
                return questNode;
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

        private QuestNode getQuestNode(HomerNode homerNode)
        {
            var node = new QuestNode();
            node.Id = homerNode._permalink;
            node.Type = homerNode.GetNodeType();
            node.Image = GetImage(homerNode._image);
            node.Action = GetMetadata("ACTION", homerNode._metadata);
            node.ActionPost = GetMetadata("ACTION_POST", homerNode._metadata);
            node.Mood = GetMetadata("MOOD", homerNode._metadata);
            node.Native = GetMetadata("NATIVE", homerNode._metadata) == "native";
            node.NextTarget = GetMetadata("NEXTTARGET", homerNode._metadata);
            node.Native = GetMetadata("NATIVE", homerNode._metadata) == "native";
            node.NextTarget = GetMetadata("NEXTTARGET", homerNode._metadata);
            node.IsDialogueNode = homerNode.GetNodeType() == HomerNode.NodeType.TEXT || homerNode.GetNodeType() == HomerNode.NodeType.START;
            node.IsChoiceNode = homerNode.GetNodeType() == HomerNode.NodeType.CHOICE;

            if (homerNode._elements.Count() > 0)
            {
                node.LocId = homerNode._elements[0]._id;
            }
            else
            {
                node.LocId = homerNode._header._id;
            }

            //node.Content = GetLocalizedContentFromElements(homerNode._elements[0]._localizedContents, currentLanguage);

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
    }
}
