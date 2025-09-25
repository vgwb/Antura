using System;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    [RequireComponent(typeof(Image))]
    public class ActIcon : MonoBehaviour
    {
        #region Serialized

        [SerializeField] Preset[] presets;

        #endregion

        Image img;

        #region Unity

        void Awake()
        {
            img = this.GetComponent<Image>();
            
            if (InteractionManager.I != null) Refresh(InteractionManager.I.NearbyInteractable);
            
            DiscoverNotifier.Game.OnNearbyInteractableChanged.Subscribe(Refresh);
        }

        void OnDestroy()
        {
            DiscoverNotifier.Game.OnNearbyInteractableChanged.Unsubscribe(Refresh);
        }

        #endregion

        #region Methods

        void Refresh(Interactable interactable)
        {
            InteractionType type = interactable == null ? InteractionType.Talk : interactable.IconType;
            
            Preset preset = GetPresetFrom(type);
            if (preset != null) img.sprite = preset.sprite;
        }

        Preset GetPresetFrom(InteractionType type, InteractionType defaultIfNotFound = InteractionType.Talk)
        {
            foreach (Preset preset in presets)
            {
                if (preset.interactionType != type) continue;
                return preset;
            }
            
            // Rollback to default
            foreach (Preset preset in presets)
            {
                if (preset.interactionType != defaultIfNotFound) continue;
                return preset;
            }
            
            return null;
        }

        #endregion
        
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        [Serializable]
        class Preset
        {
            public InteractionType interactionType = InteractionType.None;
            public Sprite sprite;
        }
    }
}