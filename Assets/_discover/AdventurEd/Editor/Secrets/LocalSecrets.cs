using UnityEngine;

namespace AdventurEd.Editor
{
    [CreateAssetMenu(fileName = "LocalSecrets", menuName = "Antura/Local Secrets (Editor)")]
    public class LocalSecrets : ScriptableObject
    {
        [Header("API Keys (stored locally, not in git nor published)")]
        public string elevenLabsApiKey;

    }
}
