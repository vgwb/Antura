using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                var item = m.ToString();
                //remove punctuation
                string pattern = @"[^\w\s_$]";
                // Replace matched characters with an empty string (or any other character you prefer)
                item = Regex.Replace(item, pattern, "");
                globalVariables.Add(item);
            }
            
            Regex regexActors = new Regex(Regexes.ACTOR_PROPERTIES);
            foreach (Match m in regexActors.Matches(text))
            {
                Console.WriteLine("M " + m);
                var item = m.ToString();
                globalVariables.Add(item);
            }
            
            globalVariables = globalVariables.OrderByDescending(x => x.Length)
                .ToList();
            
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
                List<string> vs = FindGlobalVariables(contentText);
                if (vs.Count > 0)
                {
                    foreach (string v in vs)
                    {
                        var replace = v.Replace("$", "");
                        var hasAt = v.IndexOf("@") > -1;
                        if (hasAt)
                        {
                            replace = replace.Replace("@","").Replace(".","_");
                        }
                        contentText = contentText.Replace(v, "HomerVars." + 
                                                             replace);
                        
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
            var hasAt = expr.IndexOf("@") > -1;
            if (hasAt)
            {
                expr = expr.Replace("@", "");
                expr = expr.Replace(".", "_");
            }

            if (expr.IndexOf('=') < 0)
                return;

            string lhs = expr.Split('=')[0].Trim();
            string rhs = expr.Split('=')[1].Trim();

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