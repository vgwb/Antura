using System;
using System.Collections;
using System.Collections.Generic;

namespace Antura.GoogleSheets
{
#pragma warning disable 0649
    [Serializable]
    class GSheet_Spreadsheet
    {
        public string spreadsheetId;
        public GSheet_SpreadsheetProperties properties;
        public GSheet_Sheet[] sheets;
    }

    [Serializable]
    class GSheet_Sheet
    {
        public GSheet_SheetProperties properties;
        public GSheet_GridData[] data;
    }

    [Serializable]
    class GSheet_SpreadsheetProperties
    {
        public string title;
    }

    [Serializable]
    class GSheet_SheetProperties
    {
        public string title;
    }

    [Serializable]
    class GSheet_GridData
    {
        public int startRow;
        public int startColumn;
        public GSheet_RowData[] rowData;
    }

    [Serializable]
    class GSheet_RowData
    {
        public GSheet_CellData[] values;
    }

    [Serializable]
    class GSheet_CellData
    {
        public GSheet_ExtendedValue userEnteredValue;
        public GSheet_ExtendedValue effectiveValue;
        public string formattedValue;
    }

    [Serializable]
    class GSheet_ExtendedValue
    {
        public double numberValue;
        public string stringValue;
        public bool boolValue;
    }
#pragma warning restore 0649
}