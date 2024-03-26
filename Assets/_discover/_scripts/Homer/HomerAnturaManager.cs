using System.Collections;
using System.Collections.Generic;
using Homer;
using UnityEngine;

namespace Antura.Homer
{
    public class HomerAnturaManager : MonoBehaviour
    {
        //runtime
        private bool firstFlowSetup;
        HomerProject currentHomerProject;
        HomerFlowSlugs.FlowSlug currentFlowSlug;
        HomerFlowRunning runningFlow;

        public static HomerAnturaManager I;

        void Awake()
        {
            if (I == null)
            {
                I = this;
                firstFlowSetup = true;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("HomerAnturaManager DUPLICATED!");
                Destroy(gameObject);
            }
        }

        void Start()
        {
        }

        public class HomerAnturaContent
        {
            public HomerNode.NodeType Type;

            //Text node
            public string Content;

            //Choice node
            public string ChoiceHeader;
            public List<HomerElement> Choices;
        }

        public void Setup()
        {
            currentHomerProject = HomerJsonParser.LoadHomerProject();
            HomerProjectRunning.SetUp(currentHomerProject);
        }

        public HomerAnturaContent GetContent(HomerFlowSlugs.FlowSlug flowSlug, string command, bool restart,
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

            HomerAnturaContent hac = new HomerAnturaContent();

            // CHOICE
            if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                List<HomerElement> choices = runningFlow.SelectedNode.GetAvailableChoiceElements();

                HomerElement header = runningFlow.SelectedNode.Node._header;
                string headerText = runningFlow.SelectedNode.GetParsedText(header);

                hac.Type = HomerNode.NodeType.CHOICE;
                hac.ChoiceHeader = headerText;
                hac.Choices = choices;
            }

            // TEXT
            else if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.TEXT &&
                     runningFlow.SelectedNode.Node._elements.Length > 0)
            {
                HomerElement element = runningFlow.SelectedNode.GetTextElement();
                string text = runningFlow.SelectedNode.GetParsedText(element);

                hac.Type = HomerNode.NodeType.TEXT;
                hac.Content = text;

            }

            // RECUR
            else
            {
                return GetContent(currentFlowSlug, command, false, language);
            }

            // RESULT ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            return hac;
        }
    }
}
