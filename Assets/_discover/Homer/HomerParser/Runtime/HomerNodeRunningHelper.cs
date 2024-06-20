using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using HomerNCalc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Homer
{
    public static class HomerNodeRunningHelper
    {
        public static HomerLocalizedContent GetContent(HomerElement element, string locale)
        {
            HomerLocalizedContent content = null;

            foreach (HomerLocalizedContent localizedContent in element._localizedContents)
            {
                if (localizedContent._localeCode == locale)
                    content = localizedContent;
            }

            return content;
        }

        public static List<string> FindGlobalVariables(string text)
        {
            List<string> globalVariables = new List<string>();
            Regex regex = new Regex(Regexes.GLOBAL_VARIABLES);

            foreach (Match m in regex.Matches(text))
            {
                Console.WriteLine("M " + m);
                globalVariables.Add(m.ToString());
            }

            return globalVariables;
        }

        public static List<string> FindLocalVariables(string text)
        {
            List<string> localVariables = new List<string>();
            Regex regex = new Regex(Regexes.LOCAL_VARIABLES);

            foreach (Match m in regex.Matches(text))
            {
                Console.WriteLine("M " + m);
                localVariables.Add(m.ToString());
            }

            return localVariables;
        }

        public static string GetContentText(HomerProject project, HomerElement element,
            bool resolveVariables = false, string localeCode = null)
        {
            if (localeCode == null)
                localeCode = project._locale;

            HomerLocalizedContent content = HomerNodeRunningHelper.GetContent(element, localeCode);

            // If the text content is empty and not in the main locale, try to get it from the main locale.
            if (content == null || string.IsNullOrEmpty(content._text))
            {
                content = GetContent(element, project._mainLocale._code);
            }

            if (content == null)
                return "NO CONTENT";

            string contentText = content._text;

            if (resolveVariables)
            {
                List<string> vs = HomerNodeRunningHelper.FindGlobalVariables(contentText);
                if (vs.Count > 0)
                {
                    foreach (string v in vs)
                    {
                        contentText = contentText.Replace(v, "HomerVars." + v.Replace("$", ""));
                    }
                }
            }

            return contentText;
        }

        public static List<HomerVariation> GetElementVariations(string elementId)
        {
            List<HomerVariation> variations = new List<HomerVariation>();

            foreach (HomerVariation variation in HomerProjectRunning._variations)
            {
                if (variation._elementId == elementId)
                    variations.Add(variation);
            }

            return variations;
        }

        public static string SanitizeVariables(string str, bool removeDoubleApex = true)
        {
            str = str.Replace("'", "’");
            str = str.Replace("<br>", "");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("\\", "");
            if (removeDoubleApex)
                str = str.Replace("\"", "'");
            return str;
        }

        public static void NCalcExtensionFunctions(string name, FunctionArgs functionArgs)
        {
            if (name == "RND")
            {
                functionArgs.Result = Random.Range((int)functionArgs.Parameters[0].Evaluate(),
                    (int)functionArgs.Parameters[1].Evaluate() + 1);
            }
        }

        public static void Assign(string expr)
        {
            expr = SanitizeVariables(expr);
            expr = expr.Replace("$", "");
            expr = expr.Replace("%", "___");
            if (expr.IndexOf('=') < 0)
                return;

            string lhs = expr.Split('=')[0].Trim();
            string rhs = expr.Split('=')[1].Trim();

            /*if (lhs.Length > 2)
            {
                var internalPart = lhs.Substring(1, lhs.Length - 2);
                internalPart = internalPart.Replace("'", "’");
                lhs = "'" + internalPart + "'";
            }
            if (rhs.Length > 2)
            {
                var internalPart = rhs.Substring(1, rhs.Length - 2);
                internalPart = internalPart.Replace("'", "’");
                rhs = "'" + internalPart + "'";
            }*/

            Expression e = new Expression(rhs);
            e.EvaluateFunction += NCalcExtensionFunctions;

            Type type = typeof(HomerVars);
            foreach (var p in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var v = p.GetValue(null);


                // static classes cannot be instanced, so use null
                //Debug.Log(p.Name + "  " + v + "  " + p.FieldType);
                e.Parameters[p.Name] = v;
            }

            foreach (var p in HomerProjectRunning.I.LocalVariables)
            {
                e.Parameters[p.Name] = p.GetValue();
            }

            var value = e.Evaluate();

            if (lhs.StartsWith("___"))
            {
                HomerProjectRunning.I.GetLocalVariable(lhs).SetValue(value);
            }
            else
            {
                var field = type.GetField(lhs);
                if (field != null)
                {
                    var o = value;
                    if (o is double)
                    {
                        try
                        {
                            field.SetValue(null, (float)(o));
                        }
                        catch (Exception)
                        {
                            field.SetValue(null, Convert.ToInt32(o));
                        }
                    }
                    else
                    {
                        field.SetValue(null, o);
                    }
                }
            }
        }
    }
}