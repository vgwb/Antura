using Antura.Minigames.DiscoverCountry.Interaction;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class UIManager : MonoBehaviour
    {
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] Canvas canvas;
        [DeEmptyAlert]
        [SerializeField] GameObject joysticksUI;
        [DeEmptyAlert]
        [SerializeField] UIVirtualButton btAct;

        #endregion
        
        public static UIManager I { get; private set; }
        public DialoguesUI dialogues { get; private set; }

        #region Unity

        void Awake()
        {
            if (I != null)
            {
                Debug.LogError("UIManager already exists, deleting duplicate");
                Destroy(this);
                return;
            }

            I = this;
            dialogues = this.GetComponentInChildren<DialoguesUI>(true);
            canvas.gameObject.SetActive(true);
            dialogues.gameObject.SetActive(true);
            btAct.gameObject.SetActive(false);
        }

        void Start()
        {
            DiscoverNotifier.Game.OnAgentTriggerEnter.Subscribe(OnAgentTriggerEnter);
            DiscoverNotifier.Game.OnAgentTriggerExit.Subscribe(OnAgentTriggerExit);
            DiscoverNotifier.Game.OnStartDialogue.Subscribe(OnStartDialogue);
            DiscoverNotifier.Game.OnCloseDialogue.Subscribe(OnCloseDialogue);
        }

        void OnDestroy()
        {
            if (I == this) I = null;
            DiscoverNotifier.Game.OnAgentTriggerEnter.Unsubscribe(OnAgentTriggerEnter);
            DiscoverNotifier.Game.OnAgentTriggerExit.Unsubscribe(OnAgentTriggerExit);
            DiscoverNotifier.Game.OnStartDialogue.Unsubscribe(OnStartDialogue);
            DiscoverNotifier.Game.OnCloseDialogue.Unsubscribe(OnCloseDialogue);
        }

        #endregion

        #region Callbacks
        
        void OnAgentTriggerEnter(EdAgent agent)
        {
            btAct.gameObject.SetActive(true);
        }

        void OnAgentTriggerExit(EdAgent agent)
        {
            btAct.gameObject.SetActive(false);
        }
        
        void OnStartDialogue()
        {
            joysticksUI.gameObject.SetActive(false);
        }
        
        void OnCloseDialogue()
        {
            joysticksUI.gameObject.SetActive(true);
        }

        #endregion
    }
}