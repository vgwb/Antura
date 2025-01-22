using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Homer
{

    public abstract class HomerLoaderPrinter : HomerLoaderConfig
    {
        public void PrintFlows()
        {
            int at = 0;
            foreach (var flow in homerProject._flows)
            {
                var button = Instantiate(FlowButtonprefab, FlowListRoot).GetComponent<Button>();
                button.GetComponentInChildren<TextMeshProUGUI>().text = flow._name;
                button.name = flow._id;

                button.onClick.AddListener(() => {
                    ProjectName.transform.parent.Find("selected").gameObject.SetActive(false);
                    SelectFlow(flow._id, button);
                });

                at++;

                //if (at > 9)
                //    break;
            }
        }

        public void PrintLocale()
        {
            ProjectContentTitle.text = "Locale";
            ProjectContentText.text = $"Actual Locale = {homerProject._locale}<br>";

            List<string> availableLocale = new List<string>();

            /**
             * Get all available locale
             * */
            foreach (HomerLocale locale in homerProject._availableLocale)
            {
                availableLocale.Add(locale._code);
            }

            ProjectContentText.text += $"Available Locale = {string.Join(", ", availableLocale)}<br>";
            ProjectContentText.text += $"Main Locale = {homerProject._mainLocale._code}";

        }

        public void PrintActors()
        {
            ProjectContentTitle.text = "Actors";
            ProjectContentText.text = $"";

            List<string> actors = new List<string>();

            /**
             * Get all actors
             * */
            foreach (HomerActor actor in homerProject._actors)
            {
                var p = $"";
                foreach (var actorProperty in actor._properties)
                {
                    p += $"{actorProperty.Key} = {actorProperty.Value} ";
                }
                
                actors.Add($"{actor._name}\nproperties: {p}\n");
            }

            ProjectContentText.text += $"{string.Join("<br>", actors)}<br>";
        }

        public void PrintVariables()
        {
            ProjectContentTitle.text = "Global & Local Variables";
            ProjectContentText.text = $"Global Variables:<br><br>";

            Type type = typeof(HomerVars); // MyClass is static class with static properties

            foreach (var p in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var v = p.GetValue(null); // static classes cannot be instanced, so use null...
                ProjectContentText.text += $"{p.Name} ({p.FieldType}): {v}<br>";
            }

            ProjectContentText.text += $"<br><br>Local Variables:<br><br>";

            foreach (var p in HomerProjectRunning.I.LocalVariables)
            {
                var v = p.GetValue();
                Debug.Log(p.Name + "  " + v);
                ProjectContentText.text += $"{p.Name.Replace("___", "")}: {v}<br>";
            }
        }

        public void PrintMeta()
        {
            ProjectContentTitle.text = "Metadata";
            ProjectContentText.text = $"";

            var color = "#214199";

            foreach (HomerMetadata metadata in homerProject._metadata)
            {
                ProjectContentText.text += $"<br><color={color}>{metadata._name}<color=#ffffff><br>";

                foreach (HomerMetadataValue metadataValue in metadata._values)
                {
                    ProjectContentText.text += $"{metadataValue._value}<br>";
                }
            }
        }

        public void PrintLabels()
        {
            ProjectContentTitle.text = "Labels";
            ProjectContentText.text = $"";
            
            foreach (HomerLabel label in homerProject._labels)
            {
                string content = HomerProjectRunning.I.GetLabel(label._key);
                ProjectContentText.text += $"{label._key} = {content}<br>";
            }
        }

        public void PrintFlowData()
        {
            PropertiesText.text = $"";
            PropertiesText.text += $"<br>Flow ID:<br>{runningFlow.Flow._id}";
            PropertiesText.text += $"<br><br>N° Nodes: {runningFlow.Flow._nodes.Length}";
        }

        public void PrintNodeData()
        {
            PropertiesText.text = $"";
            PropertiesText.text += $"<br>Node ID:<br>{runningFlow.SelectedNode.Node._id}";
            PropertiesText.text += $"<br><br>Node Type: {runningFlow.SelectedNode.Node._type}<br>";
            PropertiesText.text += $"<br>Global Variables:<br><br>";

            Type type = typeof(HomerVars);
            foreach (var p in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var v = p.GetValue(null); // static classes cannot be instanced, so use null...
                PropertiesText.text += $"{p.Name} = {v}<br>"; //{p.FieldType}
            }

            PropertiesText.text += $"<br><br>Local Variables:<br><br>";
            foreach (var p in HomerProjectRunning.I.LocalVariables)
            {
                var v = p.GetValue();
                Debug.Log(p.Name + "  " + v);
                PropertiesText.text += $"{p.Name.Replace("___", "")}: {v}<br>";
            }

        }

        protected void PrintChoicesContent()
        {
            List<HomerElement> choices = runningFlow.SelectedNode.GetAvailableChoiceElements();

            Transform headerTextPlaceHolder = Choices.Find("Header/Text");
            Transform ButtonsBox = Choices.Find("Buttons");
            foreach (Transform child in ButtonsBox)
            {
                Destroy(child.gameObject);
            }

            headerTextPlaceHolder.GetComponent<TextMeshProUGUI>().text = "";

            HomerElement header = runningFlow.SelectedNode.Node._header;
            string headerText = runningFlow.SelectedNode.GetParsedText(header);

            if (headerText != null)
                headerTextPlaceHolder.GetComponent<TextMeshProUGUI>().text = headerText;

            int idx = 0;
            foreach (HomerElement choice in choices)
            {

                string text = runningFlow.SelectedNode.GetParsedText(choice);
                Transform choiceElement = Instantiate(ChoicePrefab, ButtonsBox.transform);
                choiceElement.GetComponentInChildren<TMP_Text>().text = text;
                choiceElement.GetComponent<Button>().onClick.AddListener(() => {
                    Choose(choice);
                });
                idx++;
            }

            Choices.gameObject.SetActive(true);
            NodeContentText.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(false);

        }

        protected void PrintTextContent()
        {
            HomerElement element = runningFlow.SelectedNode.GetTextElement();
            string text = runningFlow.SelectedNode.GetParsedText(element);

            NodeContentText.text = text;
            NodeContentText.gameObject.SetActive(true);

            Choices.gameObject.SetActive(false);
            NodeContentText.gameObject.SetActive(true);
            NextButton.gameObject.SetActive(true);

        }

        /**
        * Locale Console
        * */

        public void PrintLocaleChooser()
        {
            Project.gameObject.SetActive(true);
            Flows.gameObject.SetActive(false);
            UnselectFlows();

            foreach (HomerLocale locale in homerProject._availableLocale)
            {

                var button = Instantiate(LocaleButtonprefab, AvailableLocale).GetComponent<Button>();
                button.GetComponentInChildren<TextMeshProUGUI>().text = locale._code;
                button.name = locale._code;

                button.onClick.AddListener(() => {

                    foreach (Transform child in AvailableLocale)
                    {
                        child.Find("selected").gameObject.SetActive(false);
                    }

                    button.transform.Find("selected").gameObject.SetActive(true);

                    homerProject._locale = locale._code;
                });
            }

            AvailableLocale.Find(homerProject._locale).Find("selected").gameObject.SetActive(true);

        }


        /**
         * Project Console
         * */

        public void SelectProject()
        {
            Project.gameObject.SetActive(true);
            Flows.gameObject.SetActive(false);
            ProjectName.transform.parent.Find("selected").gameObject.SetActive(true);

            UnselectFlows();
            Project.Find("ProjectProperties/Locale").GetComponent<Button>().Select();
            PrintLocale();
        }


        public void UnselectFlows()
        {
            foreach (Transform child in FlowListRoot.transform)
            {
                child.Find("selected").gameObject.SetActive(false);
            }
        }

        public void SelectFlow(string flowId, Button element = null)
        {
            UnselectFlows();

            if (element == null)
            {
                string elementname = "" + flowId;
                Transform btn = FlowListRoot.Find(elementname);
                if (btn != null)
                    element = btn.GetComponent<Button>();

            }
            else
            {
                InitializeFlow(flowId);
            }

            element.transform.Find("selected").gameObject.SetActive(true);
            ProjectName.transform.parent.Find("selected").gameObject.SetActive(false);

            Project.gameObject.SetActive(false);
            Flows.gameObject.SetActive(true);

            PrintFlowData();

        }
        
        public abstract void InitializeFlow(string id);
        public abstract void Choose(HomerElement choice);
    }
}
