using System;
using Antura.Core;
using Antura.Database;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// Represents a MiniGame variation badge
    /// </summary>
    public class UIMinigameVariationBadge : MonoBehaviour
    {
        public TextRender Text;
        public SpriteRenderer Icon;
        public Image UIIcon;

        public void Assign(MiniGameData miniGameData)
        {
            bool hasBadge = !string.IsNullOrEmpty(miniGameData.Badge) || !string.IsNullOrEmpty(miniGameData.BadgeLocId);
            if (hasBadge)
            {
                if (!string.IsNullOrEmpty(miniGameData.Badge))
                {
                    var sprite = AppManager.I.AssetManager.GetBadgeIcon(miniGameData);
                    if (Icon != null)
                        Icon.sprite = sprite;
                    if (UIIcon != null)
                        UIIcon.sprite = sprite;
                    if (Icon != null)
                        Icon.enabled = true;
                    if (UIIcon != null)
                        UIIcon.enabled = true;
                    Text.gameObject.SetActive(false);
                    Text.enabled = false;
                }
                else
                {
                    if (UIIcon != null)
                        UIIcon.enabled = false;
                    if (Icon != null)
                        Icon.enabled = false;
                    Text.gameObject.SetActive(true);
                    Text.enabled = true;
                    Text.SetSentence((LocalizationDataId)Enum.Parse(typeof(LocalizationDataId), miniGameData.BadgeLocId));
                }
            }
            else
            {
                if (UIIcon != null)
                    UIIcon.enabled = false;
                if (Icon != null)
                    Icon.enabled = false;
                Text.enabled = false;
            }
        }
    }
}
