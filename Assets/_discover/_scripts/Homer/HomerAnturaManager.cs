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

        //runtime
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
            currentLanguage = language;
            SetupCurrentFlow(flowSlug);

            foreach (HomerNode homerNode in runningFlow.Flow._nodes)
            {
                if (homerNode._permalink == permalink)
                {
                    QuestNode questNode = new QuestNode();
                    questNode.Id = homerNode._permalink;
                    if (homerNode._elements.Count() > 0)
                    {
                        questNode.LocId = homerNode._elements[0]._id;
                    }
                    else
                    {
                        questNode.LocId = homerNode._header._id;
                    }
                    questNode.Metadata = homerNode._metadata;

                    //we just take the first line
                    questNode.Content = GetLocalizedContentFromElements(homerNode._elements[0]._localizedContents, language);
                    return questNode;
                }
            }

            return null;
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

        public void GetContentFromPermalink(string permalink, HomerFlowSlugs.FlowSlug flowSlug, string command,
            List<QuestNode> answers, string language = "EN")
        {
            currentLanguage = language;
            MoveToPermalinkNode(permalink, flowSlug);

            GetContent(flowSlug, command, answers, false, language);
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
            // SETUP :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            currentLanguage = language;
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
            questNode.LocId = homerNode._header._id;
            questNode.Metadata = homerNode._metadata;

            // CHOICE
            if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                List<HomerElement> choices = runningFlow.SelectedNode.GetAvailableChoiceElements();

                HomerElement header = runningFlow.SelectedNode.Node._header;
                //string headerText = runningFlow.SelectedNode.GetParsedText(header);
                string headerText = GetLocalizedContentFromElements(runningFlow.SelectedNode.Node._header._localizedContents, language);

                questNode.Type = HomerNode.NodeType.CHOICE;
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
                //string text = runningFlow.SelectedNode.GetParsedText(element);
                string text = GetLocalizedContentFromElements(runningFlow.SelectedNode.ChosenElement._localizedContents, language);

                questNode.Type = HomerNode.NodeType.TEXT;
                if (homerNode._elements.Count() > 0)
                {
                    questNode.LocId = homerNode._elements[0]._id;
                }
                else
                {
                    questNode.LocId = homerNode._header._id;
                }

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

        QuestNode NextNodeRecur()
        {
            if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                QuestNode questNode = new QuestNode();
                questNode.Id = runningFlow.SelectedNode.Node._permalink;
                if (runningFlow.SelectedNode.Node._elements.Count() > 0)
                {
                    questNode.LocId = runningFlow.SelectedNode.Node._elements[0]._id;
                }
                else
                {
                    questNode.LocId = runningFlow.SelectedNode.Node._header._id;
                }
                questNode.Metadata = runningFlow.SelectedNode.Node._metadata;
                questNode.Type = HomerNode.NodeType.CHOICE;
                HomerElement header = runningFlow.SelectedNode.Node._header;
                string headerText = runningFlow.SelectedNode.GetParsedText(header);
                questNode.Content = headerText;
                questNode.Choices = runningFlow.SelectedNode.GetAvailableChoiceElements();

                return questNode;
            }
            else if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.TEXT)
            {
                QuestNode questNode = new QuestNode();
                questNode.Id = runningFlow.SelectedNode.Node._permalink;
                if (runningFlow.SelectedNode.Node._elements.Count() > 0)
                {
                    questNode.LocId = runningFlow.SelectedNode.Node._elements[0]._id;
                }
                else
                {
                    questNode.LocId = runningFlow.SelectedNode.Node._header._id;
                }
                questNode.Metadata = runningFlow.SelectedNode.Node._metadata;
                questNode.Type = HomerNode.NodeType.TEXT;
                HomerElement element = runningFlow.SelectedNode.GetTextElement();
                //string text = runningFlow.SelectedNode.GetParsedText(element);
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

        void SetupCurrentFlow(HomerFlowSlugs.FlowSlug flowSlug)
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
    }
}
