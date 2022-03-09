using Antura.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.DeInspektor.Attributes;

namespace Antura.GoogleSheets
{
    [CreateAssetMenu(menuName = "Antura/Google Sheet Ref", order = 1)]
    public class GoogleSheetRef : ScriptableObject
    {
        public string DocID;
        public string SheetTitle;
        public string FileName;

#if UNITY_EDITOR
        [DeMethodButton("Open Sheet in Web Browser")]
        public void OpenSheetInBrowser()
        {
            Application.OpenURL("https://docs.google.com/spreadsheets/d/" + DocID + "/edit");
        }
#endif
    }

}
