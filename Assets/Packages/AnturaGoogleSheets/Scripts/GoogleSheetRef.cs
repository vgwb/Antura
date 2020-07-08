using Antura.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.GoogleSheets
{
    [CreateAssetMenu(menuName = "Antura/Google Sheet Ref", order = 1)]
    public class GoogleSheetRef : ScriptableObject
    {
        public string DocID;
        public string SheetTitle;
        public string FileName;
    }

}
