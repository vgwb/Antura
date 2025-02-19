using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Antura.Minigames.DiscoverCountry
{
    // Attached to UIManager
    public class MapIconsManager : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] AbstractMapIcon playerIco, navigatorIco, defaultInteractableIconPrefab;

        #endregion

        bool mapIconsActivated;
        readonly List<AbstractMapIcon> interactableIcons = new();
        readonly Dictionary<AbstractMapIcon, Interactable> interactableByIcon = new();

        #region Unity

        void Awake()
        {
            playerIco.gameObject.SetActive(true);
            navigatorIco.gameObject.SetActive(true);
            defaultInteractableIconPrefab.gameObject.SetActive(false);
        }

        void Start()
        {
            Refresh();
            
            DiscoverNotifier.Game.OnMapCameraActivated.Subscribe(OnMapCameraActivated);
        }

        void OnDestroy()
        {
            DiscoverNotifier.Game.OnMapCameraActivated.Unsubscribe(OnMapCameraActivated);
        }

        void Update()
        {
            if (!mapIconsActivated)
                return;

            if (playerIco.IsEnabled)
                playerIco.UpdatePosition();

            if (navigatorIco.IsEnabled)
                navigatorIco.UpdatePosition();

            foreach (AbstractMapIcon icon in interactableIcons)
            {
                if (!interactableByIcon.TryGetValue(icon, out Interactable interactable))
                    continue;
                if (interactable.IsInteractable && icon.IsEnabled)
                    icon.UpdatePosition();
            }
        }

        #endregion

        #region Public Methods

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        public void Refresh()
        {
            Clear();
            
            // Find all Interactables and mark the ones that should appear on the map
            Interactable[] allInteractables = Object.FindObjectsByType<Interactable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (Interactable interactable in allInteractables)
            {
                if (!interactable.ShowOnMap) continue;
                AbstractMapIcon icon = Instantiate(defaultInteractableIconPrefab, defaultInteractableIconPrefab.transform.parent);
                icon.gameObject.SetActive(true);
                icon.AssignFollowTarget(interactable.transform);
                interactableIcons.Add(icon);
                interactableByIcon.Add(icon, interactable);
            }
        }

        #endregion

        #region Methods

        void Clear()
        {
            foreach (AbstractMapIcon icon in interactableIcons) Destroy(icon.gameObject);
            interactableIcons.Clear();
            interactableByIcon.Clear();
        }

        #endregion

        #region Callbacks

        void OnMapCameraActivated(bool activated)
        {
            if (activated)
            {
                mapIconsActivated = true;
                playerIco.Show();
                navigatorIco.Show();
                foreach (AbstractMapIcon icon in interactableIcons)
                {
                    if (interactableByIcon.TryGetValue(icon, out Interactable interactable))
                    {
                        if (interactable.IsInteractable && icon.IsEnabled)
                            icon.Show();
                    }
                }
            }
            else
            {
                mapIconsActivated = false;
                playerIco.Hide();
                navigatorIco.Hide();
                foreach (AbstractMapIcon icon in interactableIcons) icon.Hide();
            }
        }

        #endregion
    }
}
