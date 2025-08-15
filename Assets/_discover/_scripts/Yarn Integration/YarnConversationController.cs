using UnityEngine;
using Yarn.Unity;

namespace Antura.Discover
{
    public class YarnConversationController : MonoBehaviour
    {
        [SerializeField] private DialogueRunner runner;

        public void DebugMetadata(string nodeName)
        {
            if (runner == null || string.IsNullOrEmpty(nodeName))
                return;

            var metadata = runner.Dialogue.GetHeaderValue(nodeName, "tags");


            Debug.Log($"TAGS for {nodeName}: {metadata}");
        }

        public void StartDialogue(string nodeName)
        {
            if (runner == null || string.IsNullOrEmpty(nodeName))
                return;

            // If a dialogue is running, optionally request next/stop, or ignore.
            if (!runner.IsDialogueRunning)
                runner.StartDialogue(nodeName); // starts that node immediately
            else
                runner.RequestNextLine(); // or runner.Stop();
        }
    }
}
