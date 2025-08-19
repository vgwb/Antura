using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public enum WorldPrefabCategory
    {
        None = 0,
        Environment = 10,      // Worlds, maps, terrain, sky
        Architecture = 20,     // Buildings, modular kits, castles
        Props = 30,            // Placeables, decorations, signs, fences, pickups
        Characters = 40,       // Player, NPCs, animals
        Vehicles = 50,         // Cars, trains, boats, etc.
        Vfx = 60,              // Particles, special effects
        Gameplay = 70,         // Triggers, helpers, invisible walls
        Audio = 80,            // Audio emitters, speakers
        Other = 100            // Catch-all
    }

    public enum WorldPrefabTag
    {
        None = 0,
        // Environment specifics
        WorldScene = 10,
        Map = 20,
        Ground = 30,
        Skybox = 40,
        Mountain = 50,
        Seaside = 60,
        City = 70,

        // Architecture specifics
        Building = 110,
        ModularBuilding = 120,
        Castle = 130,
        Road = 140,
        Bridge = 150,
        CityKit = 160,
        CastleKit = 170,
        ModularBuildingsKit = 180,

        // Props and placeables
        Decoration = 210,
        CollectableItem = 220,
        Sign = 230,
        InfoSign = 240,
        ArrowSign = 250,
        StreetSign = 260,
        Fence = 270,
        Wall = 280,
        InvisibleWall = 290,
        Chest = 300,
        Backpack = 310,
        Ball = 320,
        Flag = 330,
        TargetPlatform = 340,
        PickUpMaterial = 350,
        MovingObject = 360,
        ActionObject = 370,

        // Characters
        Player = 410,
        NPC = 420,
        Antura = 430,
        LL = 440,

        // Vehicles
        Car = 510,
        Train = 520,
        Boat = 530,
        Watercraft = 540,

        // VFX / Audio
        WinParticles = 610,
        ParticleFX = 620,
        AudioEmitter = 630,
        PlaySound3D = 640,

        // UI / Helpers
        DialogueUI = 710,
        HomeUI = 720,
        HelperUI = 730

    }

    public class WorldPrefabData : MonoBehaviour
    {
        [Tooltip("Unique identifier for this prefab")]
        [SerializeField] private string Id;

        [Tooltip("Where does this prefab come from?")]
        [SerializeField] private string Source;

        [Header("Filtering")]
        [SerializeField] private WorldPrefabCategory Category;
        [SerializeField] private List<WorldPrefabTag> Tags = new List<WorldPrefabTag>();


#if UNITY_EDITOR
        // Editor-only helpers used by your prefab creation tool.
        public void __EditorSetPrefabId(string id) => Id = id;
        public void __EditorSetCategory(WorldPrefabCategory cat) => Category = cat;
        public void __EditorSetTags(IEnumerable<WorldPrefabTag> set)
        {
            Tags.Clear();
            if (set == null)
                return;
            foreach (var t in set)
            {
                if (!Tags.Contains(t))
                    Tags.Add(t);
            }
        }

#endif
    }
}
