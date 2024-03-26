using System.Collections;
using System.Collections.Generic;
using Antura.Homer;
using DG.Tweening;
using Homer;
using UnityEngine;

// ReSharper disable All

namespace Antura.Homer
{
    public class HomerAnturaTester : MonoBehaviour
    {
        void Start()
        {
            HomerAnturaManager.I.Setup();

            ContentTest(true, "TALK_TEACHER");
            DOVirtual.DelayedCall(1, () => ContentTest(true, "TALK_TEACHER"));

            DOVirtual.DelayedCall(2, () => ContentTest(true, "TALK_MAJOR"));
            DOVirtual.DelayedCall(3, () => ContentTest(true, "TALK_MAJOR"));
        }

        static int watchDog = 0;

        static void ContentTest(bool restart, string command)
        {
            watchDog++;

            if (watchDog > 100)
                return;

            var questNode = HomerAnturaManager.I.GetContent(
                HomerFlowSlugs.FlowSlug.FR_01_TOUR_EIFFEL,
                command,
                restart
                );

            if (questNode != null)
            {
                //full content printer in HomerBasicUsageSample

                if (questNode.Type == HomerNode.NodeType.TEXT)
                    Debug.Log($"\nCMD: {command} RESULT: {questNode.Content}");
                else if (questNode.Type == HomerNode.NodeType.CHOICE)
                    Debug.Log($"\nCMD: {command} RESULT: # of choices {questNode.Choices.Count}");

                ContentTest(false, command);
            }
        }
    }
}
