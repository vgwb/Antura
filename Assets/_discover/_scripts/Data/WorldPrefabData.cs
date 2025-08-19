using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public enum WorldPrefabCategory
    {
        None = 0,    // fallback / uncategorized
        Decoration = 10,   // vases, paintings, ornaments
        Building = 20,   // houses, walls, roofs, structural
        Food = 30,   // edible items (fruit, bread, meat)
        Animal = 40,   // animals, pets, wild creatures
        Character = 50,   // NPCs, humanoids
        Street = 60,   // roads, lamps, benches, street furniture
        Vehicle = 70,   // cars, trains, boats, planes
        Sign = 80,   // signs, boards, indicators
        Prop = 90,   // misc props, interactive objects
        Nature = 100,  // trees, rocks, plants, water elements
        Tool = 110,  // tools, weapons, instruments
        Collectible = 120,  // items to pick up (coins, keys, treasures)
        Furniture = 130,  // chairs, tables, beds, cabinets
        Container = 140,  // chests, boxes, barrels
        Clothing = 150,  // hats, armor, costumes
        Effect = 160,  // particles, visual FX
        Audio = 170,  // sound emitters, music sources
        Environment = 180,  // skyboxes, lighting profiles, terrain chunks
        Other = 999   // fallback for uncategorized / misc
    }

    public enum WorldPrefabTag
    {
        None = 0,

        // Historical / thematic
        Medieval = 1,
        Modern = 2,
        Futuristic = 3,
        Ancient = 4,

        // Environment / setting (30–89)
        Ground = 30,
        Skybox = 40,
        Mountain = 50,
        Seaside = 60,
        City = 70,
        Forest = 80,
        Tree = 81,
        Desert = 82,
        Snow = 83,
        Water = 84,
        Fire = 85,
        Rock = 86,

        // Architecture / construction (110–179)
        Building = 110,
        ModularBuilding = 120,
        Road = 140,
        Bridge = 150,
        Fence = 160,
        Wall = 170,

        // Props & placeables (200–299)
        Decoration = 200,
        Food = 210,
        Collectible = 220,   // (consistent spelling)
        Sign = 230,
        Chest = 240,
        Flag = 250,
        Container = 260,
        Resource = 270,   // was PickUpMaterial
        MovingObject = 280,
        Platform = 290,   // gameplay platform (jumpable/movable)

        // Food sub-types (300–359)
        Fruit = 300,
        Vegetable = 310,
        Meat = 320,
        Drink = 330,
        Bread = 340,
        Dairy = 350,

        // Level flow / editor helpers (400–469)
        SpawnPoint = 400,
        Checkpoint = 410,
        Teleport = 420,
        Door = 430,
        Ladder = 440,
        Waypoint = 450,
        Trigger = 460,
        Volume = 461,

        // Vehicles (510–559)
        Car = 510,
        Train = 520,
        Boat = 530,
        Airplane = 540,
        Bicycle = 550,

        // Gameplay / logic (600–629)
        QuestItem = 600,
        Interactive = 610,
        AI = 620,   // has behaviour tree / AI
        Static = 621,

        // Nav / physics (640–679)
        NavWalkable = 640,
        NavObstacle = 641,
        Breakable = 652,
        Hazard = 653,   // spikes, lava, etc.
        Climbable = 654,

        // VFX / Audio (700–739)
        Effect = 700,   // visual effects, general
        SoundFx = 710,
        Music = 720,
        LightSource = 730,

        // Culture / region (800–839)
        Asian = 800,
        European = 810,
        African = 820,
        American = 830,

        // Size / performance (900–959)
        Small = 900,
        Medium = 910,
        Large = 920,
        HighPoly = 951,

    }


    public class WorldPrefabData : MonoBehaviour
    {
        [Tooltip("Unique identifier for this prefab")]
        public string Id;

        [Tooltip("Where does this prefab come from?")]
        public string Source;

        [Header("Filtering")]
        public WorldPrefabCategory Category;
        public List<WorldPrefabTag> Tags = new List<WorldPrefabTag>();


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
