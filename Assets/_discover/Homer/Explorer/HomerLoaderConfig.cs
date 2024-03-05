using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Homer
{
    public abstract class HomerLoaderConfig : MonoBehaviour
    {
        public Button FlowButtonprefab;
        public Button LocaleButtonprefab;

        public Transform FlowListRoot;

        //CONTENT
        public Transform Project;
        public Transform Flows;

        //FLOWS
        public Transform Cover;
        public TextMeshProUGUI ProjectTitle;
        public TextMeshProUGUI CoverTitle;
        public Transform NodeContent;

        public Transform ChoicePrefab;
        public TextMeshProUGUI ActorName;
        public TextMeshProUGUI NodeContentText;
        public TextMeshProUGUI FlowName;

        public Button PreviewButton;
        public Button NextButton;

        public TextMeshProUGUI ProjectName;
        public TextMeshProUGUI ProjectContentTitle;
        public TextMeshProUGUI ProjectContentText;
        public Transform Choices;

        public Transform AvailableLocale;

        public TextMeshProUGUI PropertiesText;

        //RUNTIME
        public HomerProject homerProject;
        protected HomerFlowRunning runningFlow;


    }
}
