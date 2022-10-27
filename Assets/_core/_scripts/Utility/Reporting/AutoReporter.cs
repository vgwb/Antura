using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura
{

    public class AutoReporter : MonoBehaviour
    {
        public UserReportingScript UserReportingScript;

        public void Awake()
        {
            Application.logMessageReceivedThreaded += HandleLog;
        }

        void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= HandleLog;
        }

        private void HandleLog(string condition, string stacktrace, LogType type)
        {
            //if (Application.isEditor) return; // Avoid handling logs here, it slows down too much
            if (type == LogType.Exception)
            {
                UserReportingScript.CreateUserReport();
            }
        }

    }

}
