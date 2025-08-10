using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Antura.Discover.Achievements.UI
{
    /// <summary>
    /// Small tile showing a card in a grid. Handles lock overlay, greyscale, and sound icon.
    /// </summary>
    public class CardTile : MonoBehaviour
    {
        [Header("UI")]
        public Image image;
        public Image lockOverlay;
        public Image soundIcon;
        public TextMeshProUGUI titleText;

        [Header("Locked Greyscale")]
        public Material greyscaleMaterial;

        private CardDefinition def;
        private CardState state;
        private Action<CardDefinition> onClick;

        public void Bind(CardDefinition def, CardState state, Action<CardDefinition> onClick)
        {
            this.def = def;
            this.state = state;
            this.onClick = onClick;

            if (image)
                image.sprite = def.Image;

            bool isLocked = state == null || !state.Unlocked;

            if (lockOverlay)
                lockOverlay.enabled = isLocked;
            if (soundIcon)
                soundIcon.enabled = def.Audio != null;

            if (greyscaleMaterial != null && image != null)
                image.material = isLocked ? greyscaleMaterial : null;

        }

        public void OnTileClicked()
        {
            onClick?.Invoke(def);
        }
    }
}
