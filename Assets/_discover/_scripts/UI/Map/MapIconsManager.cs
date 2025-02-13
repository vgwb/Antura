using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    // Attached to UIManager
    public class MapIconsManager : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] AbstractMapIcon playerMapIco, anturaMapIco, defaultInteractableIconPrefab;

        #endregion

        bool mapIconsActivated;
        readonly List<AbstractMapIcon> interactableIcons = new();
        
        #region Unity

        void Awake()
        {
            playerMapIco.gameObject.SetActive(true);
            anturaMapIco.gameObject.SetActive(true);
            defaultInteractableIconPrefab.gameObject.SetActive(false);
        }

        void Start()
        {
            // Find all Interactables and mark the ones that should appear on the map
            Interactable[] allInteractables = Object.FindObjectsByType<Interactable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            DiscoverNotifier.Game.OnMapCameraActivated.Subscribe(OnMapCameraActivated);
        }

        void OnDestroy()
        {
            DiscoverNotifier.Game.OnMapCameraActivated.Unsubscribe(OnMapCameraActivated);
        }

        void Update()
        {
            if (!mapIconsActivated) return;

            if (playerMapIco.IsEnabled) playerMapIco.UpdatePosition();
            if (anturaMapIco.IsEnabled) anturaMapIco.UpdatePosition();
            foreach (AbstractMapIcon icon in interactableIcons)
            {
                if (icon.IsEnabled) icon.UpdatePosition();
            }
        }

        #endregion

        #region Callbacks

        void OnMapCameraActivated(bool activated)
        {
            if (activated)
            {
                mapIconsActivated = true;
                playerMapIco.Show();
                anturaMapIco.Show();
            }
            else
            {
                mapIconsActivated = false;
                playerMapIco.Hide();
                anturaMapIco.Hide();
            }
        }

        #endregion
    }
}