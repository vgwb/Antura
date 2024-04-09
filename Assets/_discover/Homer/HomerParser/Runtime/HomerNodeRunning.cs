using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using HomerNCalc;
using Random = UnityEngine.Random;

namespace Homer
{
    public class HomerNodeRunning
    {
        public HomerNode Node;
        public HomerFlowRunning FlowRunning;
        public HomerElement ChosenElement;

        public static HomerNodeRunning Instantiate(HomerNode node, HomerFlowRunning flowRunning)
        {
            HomerNodeRunning hnr = new HomerNodeRunning() { Node = node };
            hnr.FlowRunning = flowRunning;
            return hnr;
        }

        public HomerElement GetElement(string elementId)
        {
            foreach (var homerElement in Node._elements)
            {
                if (homerElement._id == elementId)
                    return homerElement;
            }

            throw new Exception($"No elementid {elementId} in node {Node._id}");
        }

        List<HomerElement> GetAvailableElements()
        {
            List<HomerElement> elements = new List<HomerElement>();
            foreach (HomerElement element in Node._elements)
            {
                if (!element._visited)
                    elements.Add(element);
            }

            return elements;
        }

        public HomerElement GetTextElement()
        {
            List<HomerElement> availableElements = GetAvailableElements();
            HomerElement element = null;

            if (Node._type != NodeType.text)
                return element;

            switch (Node._cycleType)
            {
                case CycleType.list:
                    element = availableElements.Count > 0
                        ? availableElements[0]
                        : Node._elements[Node._elements.Length - 1];
                    element._visited = true;
                    break;

                case CycleType.smartRandom:
                    availableElements = availableElements.Count > 0
                        ? availableElements
                        : new List<HomerElement>(Node._elements);
                    int sRnd = availableElements.Count > 1 ? Random.Range(0, availableElements.Count) : 0;
                    element = availableElements[sRnd];
                    element._visited = true;
                    break;

                case CycleType.random:
                    int rnd = Node._elements.Length > 1 ? Random.Range(0, Node._elements.Length) : 0;
                    element = Node._elements[rnd];
                    element._visited = true;
                    break;

                case CycleType.loop:
                    if (availableElements.Count == 0)
                    {
                        foreach (HomerElement el in Node._elements)
                        {
                            el._visited = false;
                        }

                        availableElements = GetAvailableElements();
                    }

                    element = availableElements[0];
                    element._visited = true;
                    break;
            }

            FlowRunning.SelectedNode.ChosenElement = element;
            return element;
        }

        public List<HomerElement> GetAvailableChoiceElements()
        {
            List<HomerElement> availableElements = GetAvailableElements();
            return availableElements;
        }

        public HomerConnection GetConnection(HomerNode node, HomerElement selectedElement = null)
        {
            foreach (var homerConnection in node._connections)
            {
                if (homerConnection._from == node._id &&
                    homerConnection._nodeElementId == selectedElement._id)
                    return homerConnection;
            }

            //case single connection on node
            foreach (var homerConnection in node._connections)
            {
                if (homerConnection._from == node._id &&
                    string.IsNullOrEmpty(homerConnection._nodeElementId))
                    return homerConnection;
            }

            return null;
        }

        public string ParsedText(HomerElement element, bool forced = false)
        {
            if (element == null)
                return "";

            string content = HomerNodeRunningHelper.GetContentText(HomerProjectRunning.I.Project, element);

            string output = content;

            // Find VARIATIONS

            Regex variationsRegExp = new Regex(Regexes.VARIATIONS);

            int idx = 0;
            foreach (Match variation in variationsRegExp.Matches(content))
            {
                string v = variation.ToString().Replace("[[", "").Replace("]]", "").Trim();

                Debug.Log("variation  " + variation);
                v = v.Replace(" | ", "|");

                string type = v.Split(' ')[0].Trim();
                string[] variations = v.Replace(type, "").Split('|');


                List<HomerVariation> elementVariations = HomerNodeRunningHelper.GetElementVariations(element._id);

                HomerVariation persistedVariation = elementVariations[idx];
                string variationContent = "";

                switch (persistedVariation._type)
                {
                    case VariationType.list:
                    {
                        variationContent = persistedVariation._values[0];
                        var clean = new List<string>(persistedVariation._values);
                        clean.Remove(variationContent);
                        persistedVariation._values = clean.ToArray();
                        if (variationContent == null)
                            variationContent = variations[variations.Length - 1];
                    }
                        break;

                    case VariationType.loop:
                    {
                        variationContent = persistedVariation._values[0];
                        var clean = new List<string>(persistedVariation._values);
                        clean.Remove(variationContent);
                        persistedVariation._values = clean.ToArray();

                        if (persistedVariation._values.Length == 0)
                            persistedVariation._values = variations;
                    }
                        break;
                    case VariationType.srnd:
                    {
                        var rndV = Random.Range(0, persistedVariation._values.Length);
                        variationContent = persistedVariation._values[rndV];
                        var clean = new List<string>(persistedVariation._values);
                        clean.Remove(variationContent);
                        persistedVariation._values = clean.ToArray();
                        if (persistedVariation._values.Length == 0)
                            persistedVariation._values = variations;
                    }
                        break;
                }

                output = output.Replace(variation.Value, "<link=variation>" + variationContent + "</link>");
                ++idx;
            }


            // Find CONDITIONAL_INSIDE_TEXT

            Regex conditionalInsideTextRegExp = new Regex(Regexes.CONDITIONALS);

            foreach (Match conditionalInsideTextBlock in conditionalInsideTextRegExp.Matches(content))
            {
                var condition = conditionalInsideTextBlock.Value.Split('?')[0].Replace("[IF ", "").Trim();
                Regex resultRegEx = new Regex(Regexes.CONDITIONALS_INTERNAL);
                string resultsString = conditionalInsideTextBlock.Value.Split('?')[1].Replace("]", "");
                var possibleResults = resultRegEx.Matches(resultsString);

                string strTWT = condition;

                var print = " --ERROR-- ";

                strTWT = HomerNodeRunningHelper.SanitizeVariables(strTWT);
                var result = Evaluate(strTWT);

                if (result)
                    print = possibleResults[0].Value;
                else
                    print = possibleResults[1].Value;

                print = print.Replace("\"", "");

                output = output.Replace("" + conditionalInsideTextBlock, print);
            }

            List<string> variableBlocks = new List<string>();

            // Find Variables
            var variableRegExp = new Regex(Regexes.VARIABLES);
            var variableBlocksMatches = variableRegExp.Matches(content);

            foreach (var m in variableBlocksMatches)
            {
                variableBlocks.Add(m.ToString());
            }

            // Variables can also be set without curly brackets in node type
            // in that case get all the content
            if (variableBlocks.Count == 0 && (Node._type == NodeType.condition || Node._type == NodeType.variables))
                variableBlocks.Add(content);

            foreach (var block in variableBlocks)
            {
                var tmpString = block;
                var globalVariables = HomerNodeRunningHelper.FindGlobalVariables(block);
                var localVariables = HomerNodeRunningHelper.FindLocalVariables(block);

                foreach (string localVar in localVariables)
                {
                    HomerProjectRunning.I.AddLocalVariable(localVar);
                }

                if (globalVariables.Count > 0 || localVariables.Count > 0)
                {
                    tmpString = block.Replace("{", "").Replace("}", "");

                    //Evaluate the variable.
                    if (element._type != NodeType.choice || forced)
                        HomerNodeRunningHelper.Assign(tmpString);

                    if (globalVariables.Count > 0 && block.Replace("{", "").Replace("}", "").Trim() ==
                        globalVariables[0])
                    {
                        Type type = typeof(HomerVars);
                        var field = type.GetField(globalVariables[0].Replace("$", ""));
                        output = output.Replace(block, field.GetValue(null).ToString());
                    }
                    else if (localVariables.Count > 0 && block.Replace("{", "").Replace("}", "").Trim() ==
                             localVariables[0])
                    {
                        HomerLocalVariable v = HomerProjectRunning.I.GetLocalVariable(localVariables[0]);
                        if (v.GetValue() != null)
                            output = output.Replace(block, v.GetValue().ToString());
                        else
                        {
                            output = output.Replace(block, v.GetValue().ToString());
                        }
                    }
                    else
                    {
                        output = output.Replace(block, "");
                    }
                }
            }

            // Find TODOs
            var todoRegExp = new Regex(Regexes.TODO);
            var todoBlocks = todoRegExp.Matches(content);

            if (todoBlocks.Count > 0)
            {
                foreach (Match todoBlock in todoBlocks)
                {
                    output = output.Replace(todoBlock.Value, "");
                }
            }

            // Find JUSTONCE
            var justOnceRegExp = new Regex(Regexes.ONCE);
            var justOnceBlocks = justOnceRegExp.Matches(content);

            if (justOnceBlocks.Count > 0)
            {
                foreach (Match justonce in justOnceBlocks)
                {
                    element._justOnce = true;
                    output = output.Replace(justonce.Value, "");
                }
            }

            // Find IFNOMORE
            var ifNoMoreRegExp = new Regex(Regexes.SQUARE_BRACKETS);
            var ifNoMoreBlocks = ifNoMoreRegExp.Matches(content);

            if (ifNoMoreBlocks.Count > 0)
            {
                foreach (Match ifNoMore in ifNoMoreBlocks)
                {
                    element._ifNoMore = true;
                    output = output.Replace(ifNoMore.Value, "");
                }
            }

            //Trim from BR and \n
            output = output.Replace(Regexes.EXTENDED_TRIM, "");

            return output;
        }

        public string GetParsedText(HomerElement element = null, bool forced=false)
        {
            if (element == null)
                element = GetTextElement();

            string parsedText = ParsedText(element, forced);
            Regex.Replace(parsedText, Regexes.EXTENDED_REPLACE, "");

            return parsedText;
        }

        public static bool Evaluate(string expr)
        {
            expr = HomerNodeRunningHelper.SanitizeVariables(expr);

            expr = expr.Replace("$", "");
            expr = expr.Replace("%", "___");

            Expression e = new Expression(expr);
            e.EvaluateFunction += HomerNodeRunningHelper.NCalcExtensionFunctions;

            Type type = typeof(HomerVars);
            foreach (var p in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var v = p.GetValue(null);
                e.Parameters[p.Name] = v;
            }

            foreach (var p in HomerProjectRunning.I.LocalVariables)
            {
                e.Parameters[p.Name] = p.GetValue();
            }

            return (bool)e.Evaluate();
        }

        public string GetNodeType()
        {
            return Node._type;
        }

        public HomerActor GetActor()
        {
            foreach (HomerActor a in FlowRunning.Project._actors)
            {
                if (a._id == Node._actorId)
                    return a;
            }

            throw new Exception("No Actor with UID " + Node._actorId);
        }
    }
}