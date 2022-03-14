using Antura.Core;
using UnityEngine;

namespace Antura.Book
{
    public class InfoTable : MonoBehaviour
    {
        public GameObject RowPrefab;
        public GameObject RowSliderPrefab;

        GameObject rowGO;

        public void Reset()
        {
            emptyListContainers();
        }

        public void AddRow(Database.LocalizationDataId _locaData, string _value)
        {
            var loc = LocalizationManager.GetLocalizationData(_locaData);
            rowGO = Instantiate(RowPrefab);
            rowGO.transform.SetParent(transform, false);
            rowGO.GetComponent<TableRow>().Init(loc.NativeText, loc.LearningText, _value);
        }

        public void AddSliderRow(Database.LocalizationDataId _locaData, float _value, float _valueMax)
        {
            var loc = LocalizationManager.GetLocalizationData(_locaData);
            rowGO = Instantiate(RowSliderPrefab);
            rowGO.transform.SetParent(transform, false);
            rowGO.GetComponent<TableRow>().InitSlider(loc.NativeText, loc.LearningText, _value, _valueMax);
        }

        void emptyListContainers()
        {
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }
        }
    }
}
