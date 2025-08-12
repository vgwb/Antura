using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Antura.Discover
{
    public enum AssetType
    {
        Image,
        Audio
    }

    public enum LicenseType
    {
        CC0,        // Public Domain equivalent
        CC_BY,      // Attribution required
        CC_BY_SA,   // Attribution + Share Alike
        Licensed    // Proprietary or custom license
    }

    [CreateAssetMenu(fileName = "AssetData", menuName = "Antura/Discover/Asset", order = 1)]
    public class AssetData : ScriptableObject
    {
        public string Title;
        public AssetType Type = AssetType.Image;

        [Header("Asset References")]
        public Sprite Image;
        public AudioClip Audio;

        [Header("License Information")]
        public LicenseType License = LicenseType.CC0;
        public string Copyright; // e.g., "Stefano Cecere, 2025"
        public string SourceUrl; // Original source link

        [TextArea]
        public string LicenseNotes;    // For custom terms or clarifications
    }
}
