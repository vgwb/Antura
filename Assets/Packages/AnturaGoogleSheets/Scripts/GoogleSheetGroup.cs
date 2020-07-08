using Antura.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.GoogleSheets
{
    [CreateAssetMenu(menuName = "Antura/Google SheetsGroup", order = 1)]
    public class GoogleSheetGroup : ScriptableObject
    {
        public AppEditions Edition;
        public GoogleSheetRef[] Sheets;

    }

}