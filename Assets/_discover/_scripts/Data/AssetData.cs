using UnityEditor.EditorTools;
using UnityEngine;

namespace Antura.Discover
{
    public enum AssetType
    {
        Image,
        Audio,
        Model3D
    }

    public enum LicenseType
    {
        CC0,        // Public Domain equivalent
        CC_BY,      // Attribution required
        CC_BY_SA,   // Attribution + Share Alike
        Licensed    // Proprietary or custom license
    }

    [CreateAssetMenu(fileName = "AssetData", menuName = "Antura/Discover/Asset", order = 1)]
    public class AssetData : IdentifiedData
    {
        public string Title;
        public AssetType Type = AssetType.Image;
        [Tooltip("Country prefix for localized assets. Use 'Global' for assets not specific to any country.")]
        public Countries Country = Countries.Global; // Default to global if not specified

        [Header("Asset Reference")]
        public Sprite Image;
        public AudioClip Audio;
        public GameObject Model3D;

        [Header("License Information")]
        public LicenseType License = LicenseType.CC0;
        public string Copyright; // e.g., "Stefano Cecere, 2025"
        public string SourceUrl; // Original source link

        [TextArea]
        public string LicenseNotes;    // For custom terms or clarifications
    }
}
