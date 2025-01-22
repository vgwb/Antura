using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Homer
{

    public class HomerLoaderWithRecorder : HomerLoaderPrinter
    {

        List<HomerNodeRecorded> currentRecording;
        
        public static HomerLoaderWithRecorder I;

        void Awake()
        {
            I = this;
        }

        void Start()
        {
            //loads the project definition and content
            homerProject = HomerJsonParser.LoadHomerProject();
            HomerProjectRunning.SetUp(homerProject);

            HomerRecorder.Setup();
            
            Project.gameObject.SetActive(false);
            Flows.gameObject.SetActive(false);

            ProjectName.text = homerProject._name;
            PrintFlows();
            SelectProject();
            PrintLocaleChooser();

        }

        public override void InitializeFlow(string id)
        {
            foreach (var flow in homerProject._flows)
            {
                if (flow._id == id)
                {
                    runningFlow = HomerFlowRunning.Instantiate(flow);

                    runningFlow.SetUp(homerProject);

                    NodeContent.gameObject.SetActive(false);

                    ProjectTitle.text = runningFlow.Project._name;
                    CoverTitle.text = runningFlow.Flow._name;
                    Cover.gameObject.SetActive(true);

                    currentRecording = HomerRecorder.SetupARecording();

                    break;
                }
            }
        }

        public void DrawNode()
        {
            if (runningFlow.SelectedNode == null)
            {
                //GetContent("THE END");
                NextButton.gameObject.SetActive(false);
                Debug.Log("THE END");
            }
            else
            {
                SelectFlow(runningFlow._selectedFlowId);

                //Debug.Log($"Selected node: {runningFlow.SelectedNode.Node._id} " +
                //          $"type {runningFlow.SelectedNode.Node._type}");

                var homerNodeRecorded = new HomerNodeRecorded(runningFlow.SelectedNode.Node);
                currentRecording.Add(homerNodeRecorded);
                
                string nodeType = runningFlow.SelectedNode.GetNodeType();
                if (nodeType == NodeType.choice || nodeType == NodeType.text)
                {
                    HomerActor actor = runningFlow.SelectedNode.GetActor();
                    ActorName.text = actor._name;
                }
                HomerFlow flow = runningFlow.GetSelectedFlow();
                FlowName.text = "" + flow._name;

                if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
                {
                    PrintChoicesContent();
                }
                else if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.TEXT &&
                         runningFlow.SelectedNode.Node._elements.Length > 0)
                {
                    PrintTextContent();
                }
                else
                {
                    Next();
                }
            }

            PrintNodeData();

        }
        
        public void Next(string elementId = null)
        {
            HomerNode nextNode = runningFlow.NextNode(elementId);
            
            if (nextNode == null)
            {
                InitializeFlow(runningFlow._selectedFlowId);
                Debug.Log("---------------- THE END ----------------");
            }
            else
                DrawNode();
        }
        
        public void Preview()
        {
            Cover.gameObject.SetActive(false);
            NodeContent.gameObject.SetActive(true);

            ActorName.text = "";
            NextButton.gameObject.SetActive(false);

            Next();
        }

        public override void Choose(HomerElement choice)
        {
            var chosenContent = runningFlow.SelectedNode.GetAvailableChoiceElements().IndexOf(choice);
            currentRecording.Last().ChosenContent = chosenContent;
            
            Next(choice._id);
        }
    }
}
