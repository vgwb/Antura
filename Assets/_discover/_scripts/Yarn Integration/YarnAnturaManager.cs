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
                runner = UnityEngine.Object.FindFirstObjectByType<DialogueRunner>(FindObjectsInactive.Include);

            }
            if (!presenter)
            {
                presenter = UnityEngine.Object.FindFirstObjectByType<DiscoverDialoguePresenter>(FindObjectsInactive.Include);
            }
        }

        private void Start()
        {
            if (runner != null)
            {
                runner.onNodeStart.AddListener(nodeName => { OnNodeStarted?.Invoke(nodeName); presenter?.SetCurrentNodeName(nodeName); });
                runner.onDialogueComplete.AddListener(() => OnDialogueComplete?.Invoke());

                //Hook command(s) into Yarn

                runner.AddCommandHandler<string>("camera_focus", (arg) =>
                {
                    var focus = gameObject.FindCameraFocus(arg);
                    CameraManager.I.FocusOn(focus.LookAt, focus.Origin);
                });
                runner.AddCommandHandler("camera_reset", () =>
                {
                    CameraManager.I.ResetFocus();
                });
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

        internal void EmitQuestNode(QuestNode node)
        {
            OnQuestNode?.Invoke(node);
        }

        internal void EmitOptionsNode(QuestNode node)
        {
            OnQuestOptions?.Invoke(node);
        }

    }
}
