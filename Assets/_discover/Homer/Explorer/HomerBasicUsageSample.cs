using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Homer
{
/**
 * This class presents a very simple way to get the JSON data parsed and available
 * for navigating via "next" and choices.
 * This class is not used in the sample project.
 */
    public class HomerBasicUsageSample : MonoBehaviour
    {
        void Start()
        {
            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // PROJECT SET UP AND INFO
            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            HomerProject homerProject = HomerJsonParser.LoadHomerProject();
            HomerProjectRunning.SetUp(homerProject);

            Debug.Log($"Project Title {homerProject._name}\n");

            // Print all global vars. Eventually also change them, set them up...
            Type type = typeof(HomerVars);

            foreach (var p in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var defaultValue = p.GetValue(null); // static classes cannot be instanced, so use null...
                Debug.Log($"Global var: {p.Name} ({p.FieldType}): {defaultValue}");
            }

            // Print all actors
            StringBuilder sb = new StringBuilder("Actors:");
            
            //todo restore
            /*foreach (HomerActors.Actors actor in Enum.GetValues(typeof(HomerActors.Actors)))
            {
                sb.Append($" {actor},");
            }*/
            Debug.Log(sb.ToString().TrimEnd(','));

            //More methods for metadata, labels in <see cref="HomerLoaderPrinter"/>

            // Eventually set non default locale
            //homerProject._locale = "DE";

            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // FOCUSED FLOW SET UP
            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            // Just pick the first flow
            var runningFlow = HomerFlowRunning.Instantiate(homerProject._flows[0]);
            runningFlow.SetUp(homerProject);
            Debug.Log($"\nFlow Title {runningFlow.Flow._name}\n");
            
            // Now navigates the whole flow by always choosing the first choice, up to a 100 steps
            // (in case of looping flow).
            DoNext(runningFlow);
        }

        int watchDog = 0;
        void DoNext(HomerFlowRunning runningFlow)
        {
            if ((runningFlow.SelectedNode == null && watchDog > 0) || watchDog > 99)
            {
                Debug.Log($"\nEND AT STEP {watchDog}");
                return;
            }
            watchDog++;
            
            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // CHOICES
            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
 
            if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.CHOICE)
            {
                List<HomerElement> choices = runningFlow.SelectedNode.GetAvailableChoiceElements();

                HomerElement header = runningFlow.SelectedNode.Node._header;
                string headerText = runningFlow.SelectedNode.GetParsedText(header);
                if (!string.IsNullOrEmpty(headerText))
                    Debug.Log($"Choice title {headerText}");

                int c = 1;
                foreach (HomerElement choice in choices)
                {
                    Debug.Log($"Choice {c++}: {runningFlow.SelectedNode.GetParsedText(choice)}");
                }

                var chosen = Random.Range(0, choices.Count);
                var chosenChoice = choices[chosen];
                Debug.Log($"Auto chose {chosen+1}: {runningFlow.SelectedNode.GetParsedText(chosenChoice)}\n");
                runningFlow.NextNode(chosenChoice._id);
            }

            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // TEXT
            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            
            else if (runningFlow.SelectedNode.Node.GetNodeType() == HomerNode.NodeType.TEXT &&
                     runningFlow.SelectedNode.Node._elements.Length > 0)
            {
                HomerElement element = runningFlow.SelectedNode.GetTextElement();
                string text = runningFlow.SelectedNode.GetParsedText(element);
                Debug.Log($"\nText: {text}\n");

                runningFlow.NextNode();
            }

            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // OTHERS
            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            
            else
            {
                //Debug.Log($"Logic/structural node {runningFlow.SelectedNode.GetNodeType()}\n");
                runningFlow.NextNode();
            }

            // here you could check the value of local variables, see <see cref="HomerLoaderPrinter"/>

            DoNext(runningFlow);
        }
    }
}
