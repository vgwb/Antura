using Antura.Helpers;
using Antura.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Teacher.Test
{
    /// <summary>
    /// Helper class that generates a button for each minigame in the Teacher Management scene.
    /// </summary>
    public class MinigameButtonGenerator : MonoBehaviour
    {
        public GameObject buttonPrefab;
        public TeacherTester tester;

        public bool doAssessments = false;

        void Start()
        {
            foreach (var enumValue in GenericHelper.SortEnums<MiniGameCode>())
            {
                // Skip unused values
                if (enumValue == MiniGameCode.Invalid) continue;
                if (enumValue == MiniGameCode.Assessment_VowelOrConsonant) continue;

                // First only assessments
                bool isAssessment = enumValue.ToString().Contains("Assessment");
                if (!doAssessments && isAssessment) continue;
                if (doAssessments && !isAssessment) continue;

                CreateButtonForCode(enumValue);
            }

            Destroy(buttonPrefab);
        }

        private void CreateButtonForCode(MiniGameCode enumValue)
        {
            MiniGameCode code = enumValue;
            var btnGO = Instantiate(buttonPrefab);
            btnGO.transform.SetParent(this.transform);
            btnGO.GetComponentInChildren<Text>().text = (enumValue.ToString()).Replace("_", "\n");
            btnGO.GetComponent<Button>().onClick.AddListener(() => { tester.DoTestMinigame(code); });
            tester.minigamesButtonsDict[enumValue] = btnGO.GetComponent<Button>();
        }
    }

}
