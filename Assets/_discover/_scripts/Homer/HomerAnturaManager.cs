using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Homer;
using UnityEngine;

namespace Antura.Homer
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class HomerAnturaManager : MonoBehaviour
    {
        public static HomerAnturaManager I;

        //runtime
        private bool firstFlowSetup;
        private HomerProject currentHomerProject;
        private HomerFlowSlugs.FlowSlug currentFlowSlug;
        private HomerFlowRunning runningFlow;

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
            currentHomerProject = HomerJsonParser.LoadHomerProject();
            HomerProjectRunning.SetUp(currentHomerProject);
        }

        public HomerMetadata GetMetadataByValueId(string metadataValueId)
        {
            return runningFlow.Project.GetMetadataByValueId(metadataValueId);
        }

        public HomerMetadataValue GetMetadataValueById(string metadataValueId)
        {
            return runningFlow.Project.GetMetadataValueById(metadataValueId);
        }

        public QuestNode GetQuestNodeByPermalink(HomerFlowSlugs.FlowSlug flowSlug, string permalink,
             string language = "EN")
        {
            SetupCurrentFlow(flowSlug);

            foreach (HomerNode homerNode in runningFlow.Flow._nodes)
            {
                if (homerNode._permalink == permalink)
                {
                    QuestNode questNode = new QuestNode();
                    questNode.Id = homerNode._permalink;
                    questNode.Metadata = homerNode._metadata;

                    //we just take the first line
                    HomerLocalizedContent[] homerLocalizedContents = homerNode._elements[0]._localizedContents;

                    foreach (HomerLocalizedContent localizedContent in homerLocalizedContents)
                    {
                        if (localizedContent._localeCode == language)
                        {
                            questNode.Content = localizedContent._text;
                            // Debug.Log($"GetQuestNodeByPermalink {permalink} : result: {questNode.Content}");
                            break;
                        }
                    }
                    return questNode;
                }
            }
            return null;
        }

        public void GetContentFromPermalink(string permalink, HomerFlowSlugs.FlowSlug flowSlug, string command,
            List<QuestNode> answers, string language = "EN")
        {
            MoveToPermalinkNode(permalink, flowSlug);

            GetContent(flowSlug, command, answers, false, language);
        }

        void MoveToPermalinkNode(string permalink, HomerFlowSlugs.FlowSlug flowSlug)
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
            if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                List<HomerElement> choices = runningFlow.SelectedNode.GetAvailableChoiceElements();
                var chosenChoice = choices[choiceIndex];
                //Debug.Log($"Auto chose {chosen+1}: {runningFlow.SelectedNode.GetParsedText(chosenChoice)}\n");
                runningFlow.NextNode(chosenChoice._id);
                GetContent(flowSlug, command, answers, false, language);
            }
            else
                throw new Exception("Current node is not a choice!");
        }


        public void GetContent(HomerFlowSlugs.FlowSlug flowSlug, string command, List<QuestNode> answers,
            bool restart, string language = "EN")
        {
            // SETUP :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            HomerVars.CMD = command;

            SetupCurrentFlow(flowSlug);

            if (restart)
                runningFlow.Restart();

            // SEARCH CONTENT ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            HomerNode homerNode = runningFlow.NextNode();

            //END
            if (homerNode == null)
            {
                return;
            }

            QuestNode questNode = new QuestNode();
            questNode.Id = homerNode._permalink;
            questNode.Metadata = homerNode._metadata;

            // CHOICE
            if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                List<HomerElement> choices = runningFlow.SelectedNode.GetAvailableChoiceElements();

                HomerElement header = runningFlow.SelectedNode.Node._header;
                string headerText = runningFlow.SelectedNode.GetParsedText(header);

                questNode.Type = HomerNode.NodeType.CHOICE;
                questNode.Content = headerText;
                questNode.Choices = choices;

                answers.Add(questNode);
            }

            // TEXT
            else if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.TEXT &&
                     runningFlow.SelectedNode.Node._elements.Length > 0)
            {
                HomerElement element = runningFlow.SelectedNode.GetTextElement();
                string text = runningFlow.SelectedNode.GetParsedText(element);

                questNode.Type = HomerNode.NodeType.TEXT;
                questNode.Content = text;

                answers.Add(questNode);
            }

            // RECUR
            else
            {
                GetContent(currentFlowSlug, command, answers, false, language);
            }

        }

        void SetupCurrentFlow(HomerFlowSlugs.FlowSlug flowSlug)
        {
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
    }
}
