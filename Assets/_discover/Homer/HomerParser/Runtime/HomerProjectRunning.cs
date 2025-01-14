using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Homer
{
    public class HomerProjectRunning
    {
        public HomerProject Project;

        public List<HomerLocalVariable> LocalVariables;

        
        public static List<HomerVariation> _variations;

        public static List<string> ActiveSubFlows;


        public static HomerProjectRunning I;

        public static void SetUp(HomerProject project)
        {
            I = new HomerProjectRunning() { Project = project };

            I.Project._locale = I.Project._mainLocale._code;

            I.InitializeLocalvars();
            I.InitializeActiveSubFlows();
        }

        public void InitializeLocalvars()
        {
            LocalVariables = new List<HomerLocalVariable>();
        }

       
        public void InitializeActiveSubFlows()
        {
            ActiveSubFlows = new List<string>();
        }

        public void AddLocalVariable(string name)
        {
            if (!name.StartsWith("___"))
                name = name.Replace("%", "___");

            foreach (HomerLocalVariable var in LocalVariables)
            {
                if (var.Name == name)
                    return;
            }

            HomerLocalVariable h = new HomerLocalVariable() { Name = name };
            LocalVariables.Add(h);
        }

        public HomerLocalVariable GetLocalVariable(string name)
        {
            if (!name.StartsWith("___"))
            {
                if (name.StartsWith("%"))
                    name = name.Replace("%", "___");
                else
                    name = "___" + name;
            }

            foreach (HomerLocalVariable var in LocalVariables)
            {
                if (var.Name == name)
                    return var;
            }

            throw new System.Exception("Variable does not exist " + name);
        }

       
        public string GetLabel(string key, string localeCode = null)
        {
            if (localeCode == null)
                localeCode = Project._locale;

            HomerLabel label = null;

            foreach (HomerLabel l in Project._labels)
            {
                if (l != null && l._key == key)
                    label = l;
            }

            if (label == null)
            {
                Debug.Log($"No label for {key}");
                return key;
            }

            HomerLocalizedContent content = null;
            foreach (HomerLocalizedContent localizedContent in label._localizedContents)
            {
                if (localizedContent._localeCode == localeCode)
                    content = localizedContent;
            }

            var contentText = content._text;

            List<string> vs = HomerNodeRunningHelper.FindGlobalVariables(contentText);
            if (vs.Count > 0)
            {
                foreach (string v in vs)
                {
                    Type type = typeof(HomerVars);
                    var vCleaned = v.Replace("$", "");
                    var field = type.GetField(vCleaned);
                    if (field != null && field.GetValue(null)!=null)
                        contentText = contentText.Replace(v, field.GetValue(null).ToString());
                    else
                        Debug.Log($"GlobalVariables: missing field: {v}");
                }
            }

            return contentText;
        }
    }
}