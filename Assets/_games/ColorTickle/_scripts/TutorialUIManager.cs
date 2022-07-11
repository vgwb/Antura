using System.Linq;
using Antura.Core;
using Antura.Language;
using Antura.Tutorial;
using UnityEngine;

namespace Antura.Minigames.ColorTickle
{
    public class TutorialUIManager : MonoBehaviour
    {
        // One for each AlphabetCode
        public Transform[] line1FingerPivots;
        public Transform[] line2FingerPivots;

        private Transform[] m_Finger1Positions;
        private Transform[] m_Finger2Positions;

        [SerializeField]
        private float m_MaxDelay = 2.0f; // Time waiting before Start the Tutorial Animation

        [HideInInspector]
        public bool StartTutorial = false;

        float m_Delay;

        // Use this for initialization
        void Start()
        {
            int alphabetFamily = (int)LanguageSwitcher.I.GetLangConfig(LanguageUse.Learning).AlphabetFamily;

            m_Finger1Positions = line1FingerPivots[alphabetFamily].GetComponentsInChildren<Transform>().Where(tr => tr != line1FingerPivots[alphabetFamily]).ToArray();
            m_Finger2Positions = line2FingerPivots[alphabetFamily].GetComponentsInChildren<Transform>().Where(tr => tr != line2FingerPivots[alphabetFamily]).ToArray();
            m_Delay = m_MaxDelay;
        }

        private bool toggleLine = false;
        void Update()
        {
            if (StartTutorial)
            {
                m_Delay += Time.deltaTime;
                if (m_Delay >= m_MaxDelay)
                {
                    m_Delay = 0;

                    toggleLine = !toggleLine;

                    Vector3[] path = null;
                    if (toggleLine)
                    {
                        path = new Vector3[m_Finger1Positions.Length];
                        for (int i = 0; i < m_Finger1Positions.Length; i++)
                        {
                            path[i] = m_Finger1Positions[i].position;
                        }
                    }
                    else
                    {
                        path = new Vector3[m_Finger2Positions.Length];
                        for (int i = 0; i < m_Finger2Positions.Length; i++)
                        {
                            path[i] = m_Finger2Positions[i].position;
                        }
                    }
                    TutorialUI.DrawLine(path, TutorialUI.DrawLineMode.Finger);
                }
            }

        }

    }

}
