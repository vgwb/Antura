using System.Collections;
using System.Collections.Generic;
using Homer;
using UnityEngine;

namespace Antura.Homer
{
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

        public QuestNode GetContent(HomerFlowSlugs.FlowSlug flowSlug, string command, bool restart,
            string language = "EN")
        {
            // SETUP :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            HomerVars.CMD = command;

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

            if (restart)
                runningFlow.Restart();

            // SEARCH CONTENT ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            HomerNode homerNode = runningFlow.NextNode();

            //END
            if (homerNode == null)
            {
                return null;
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
                questNode.ChoiceHeader = headerText;
                questNode.Choices = choices;
            }

            // TEXT
            else if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.TEXT &&
                     runningFlow.SelectedNode.Node._elements.Length > 0)
            {
                HomerElement element = runningFlow.SelectedNode.GetTextElement();
                string text = runningFlow.SelectedNode.GetParsedText(element);

                questNode.Type = HomerNode.NodeType.TEXT;
                questNode.Content = text;
            }

            // RECUR
            else
            {
                return GetContent(currentFlowSlug, command, false, language);
            }

            // RESULT ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            return questNode;
        }
    }
}
