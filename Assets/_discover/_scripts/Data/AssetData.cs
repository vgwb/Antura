using UnityEditor.EditorTools;
using UnityEngine;

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "AssetData", menuName = "Antura/Discover/Asset", order = 1)]
    public class AssetData : IdentifiedData
    {
        public Status Status = Status.Draft;
        public AssetType Type = AssetType.Image;
        public Countries Country = Countries.International;

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
