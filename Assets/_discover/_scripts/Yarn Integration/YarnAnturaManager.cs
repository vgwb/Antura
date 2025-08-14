using System;
using UnityEngine;
using Yarn.Unity;

namespace Antura.Discover
{
    /// <summary>
    /// Facade/proxy over Yarn's DialogueRunner to keep a stable API for Quest/Node systems.
    /// Not a LineProvider. Works with DiscoverDialoguePresenter to emit QuestNode events.
    /// </summary>
    public class YarnAnturaManager : MonoBehaviour
    {
        public static YarnAnturaManager I { get; private set; }

        [Header("Yarn References")]
        [SerializeField] private DialogueRunner runner;
        [SerializeField] private DiscoverDialoguePresenter presenter;

        [Header("Locale")] public string CurrentLanguage = "EN";
        public string NativeLanguage = "EN";

        public event Action<string> OnNodeStarted;
        public event Action<QuestNode> OnQuestNode;         // for text lines
        public event Action<QuestNode> OnQuestOptions;      // for choices
        public event Action OnDialogueComplete;

        public DialogueRunner Runner => runner;

        private void Awake()
        {
            if (I != null && I != this)
            {
                Destroy(gameObject);
                return;
            }
            I = this;
        }

        private void OnValidate()
        {
            if (!runner)
            {
#if UNITY_2023_1_OR_NEWER
                runner = UnityEngine.Object.FindFirstObjectByType<DialogueRunner>(FindObjectsInactive.Include);
#else
                runner = UnityEngine.Object.FindObjectOfType<DialogueRunner>(true);
#endif
            }
            if (!presenter)
            {
#if UNITY_2023_1_OR_NEWER
                presenter = UnityEngine.Object.FindFirstObjectByType<DiscoverDialoguePresenter>(FindObjectsInactive.Include);
#else
                presenter = UnityEngine.Object.FindObjectOfType<DiscoverDialoguePresenter>(true);
#endif
            }
        }

        private void Start()
        {
            if (runner != null)
            {
                runner.onNodeStart.AddListener(nodeName => { OnNodeStarted?.Invoke(nodeName); presenter?.SetCurrentNodeName(nodeName); });
                runner.onDialogueComplete.AddListener(() => OnDialogueComplete?.Invoke());

                // Hook simple command(s) to ActionManager
                runner.AddCommandHandler<string>("action", (arg) => { ActionManager.I.ResolveQuestAction(arg); });
                runner.AddCommandHandler<string>("action_post", (arg) => { ActionManager.I.ResolveQuestAction(arg); });
            }

            if (presenter != null)
            {
                presenter.Manager = this;
            }
        }

        public void Setup(string language = "EN", string native = "EN")
        {
            CurrentLanguage = language;
            NativeLanguage = native;
        }

        public void InitNode(string nodeName)
        {
            StartDialogue(nodeName);
        }

        public void StartDialogue(string nodeName)
        {
            if (runner == null || string.IsNullOrEmpty(nodeName))
                return;
            if (!runner.IsDialogueRunning)
            {
                runner.StartDialogue(nodeName);
            }
            else
            {
                runner.RequestNextLine();
            }
        }

        public QuestNode GetNodeFromPermalink(string nodeName, string command)
        {
            StartDialogue(nodeName);
            return null;
        }

        internal void EmitQuestNode(QuestNode node)
        {
            OnQuestNode?.Invoke(node);
        }

        internal void EmitOptionsNode(QuestNode node)
        {
            OnQuestOptions?.Invoke(node);
        }

        public string GetLabel(string key, string localeCode)
        {
            return key;
        }
    }
}
