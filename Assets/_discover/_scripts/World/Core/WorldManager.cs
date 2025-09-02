using Antura.Utilities;
using UnityEngine;

namespace Antura.Discover
{
    public class WorldManager : SingletonMonoBehaviour<WorldManager>
    {
        [Header("Runtime (read-only)")]
        public WorldController Current;

        protected override void Init()
        {
            // Beta: find the world root that already exists in the quest scene.
            if (!Current)
                Current = FindFirstObjectByType<WorldController>();

            if (!Current)
                Debug.LogWarning("[WorldManager] No WorldRoot found in the scene. Add one to your world root GameObject.");
        }

        /// Get a world service by type.
        public T Get<T>() where T : class, IWorldSystem
            => Current ? Current.Get<T>() : null;


        // === FUTURE (post-beta): world loading hooks ===
        // public void LoadWorld(WorldRoot prefab) { ... }
        // public void UnloadWorld() { ... }
        // Keep these stubs empty until you switch to loadable worlds.
    }
}
