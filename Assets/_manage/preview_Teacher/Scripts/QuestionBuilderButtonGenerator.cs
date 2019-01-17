using Antura.Helpers;
using Antura.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Teacher.Test
{
    /// <summary>
    /// Helper class that generates a button for each QuestionBuilder in the Teacher Management scene.
    /// </summary>
    public class QuestionBuilderButtonGenerator : MonoBehaviour
    {
        public GameObject buttonPrefab;
        public TeacherTester tester;

        void Start()
        {
            foreach (var enumValue in GenericHelper.SortEnums<QuestionBuilderType>())
            {
                if (enumValue == QuestionBuilderType.Empty) continue;
                if (enumValue == QuestionBuilderType.MAX) continue;

                QuestionBuilderType type = enumValue;
                var btnGO = Instantiate(buttonPrefab);
                btnGO.transform.SetParent(this.transform);
                btnGO.GetComponentInChildren<Text>().text = (enumValue.ToString()).Replace("_", "\n");
                btnGO.GetComponent<Button>().onClick.AddListener(() => { tester.DoTestQuestionBuilder(type);});
                tester.qbButtonsDict[enumValue] = btnGO.GetComponent<Button>();
            }
            Destroy(buttonPrefab);
        }

    }

}
