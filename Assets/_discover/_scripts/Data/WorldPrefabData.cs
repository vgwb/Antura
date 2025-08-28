using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public enum WorldPrefabKit
    {
        None = 0,
        Antura = 1,
        Discover = 2,
        Hexplorando = 10,
        K_BlockyChars = 36,
        K_Car = 20,
        K_Castle = 21,
        K_CityCommercial = 22,
        K_CityIndustrial = 23,
        K_CityRoads = 24,
        K_CitySuburban = 25,
        K_Fantasy = 26,
        K_Food = 27,
        K_Furniture = 28,
        K_Holiday = 29,
        K_Nature = 30,
        K_Pirate = 31,
        K_Platformer = 32,
        K_Prototype = 38,
        K_RetroCastle = 37,
        K_Survival = 33,
        K_Train = 34,
        K_Watercraft = 35,
    }


    public enum WorldPrefabCategory
    {
        None = 0,    // fallback / uncategorized
        Animal = 40,   // animals, pets, wild creatures
        Audio = 170,  // sound emitters, music sources
        Building = 20,   // houses, walls, roofs, structural
        Character = 50,   // NPCs, humanoids
        Clothing = 150,  // hats, armor, costumes
        Collectible = 120,  // items to pick up (coins, keys, treasures)
        Container = 140,  // chests, boxes, barrels
        Decoration = 10,   // vases, paintings, ornaments
        Effect = 160,  // particles, visual FX
        Environment = 180,  // skyboxes, lighting profiles, terrain chunks
        Food = 30,   // edible items (fruit, bread, meat)
        Furniture = 130,  // chairs, tables, beds, cabinets
        Nature = 100,  // trees, rocks, plants, water elements
        Platform = 25,   // platforms, floors, walkways
        Prop = 90,   // misc props, interactive objects
        Sign = 80,   // signs, boards, indicators
        Street = 60,   // roads, lamps, benches, street furniture
        Tool = 110,  // tools, weapons, instruments
        Vehicle = 70,   // cars, trains, boats, planes
        Other = 999   // fallback for uncategorized / misc
    }

    public enum WorldPrefabTag
    {
        None = 0,

        // Environment / setting (30)
        EnvCity = 70,
        EnvDesert = 82,
        EnvFire = 85,
        EnvForest = 80,
        EnvGround = 30,
        EnvMountain = 50,
        EnvPlant = 83,
        EnvRock = 86,
        EnvSeaside = 60,
        EnvSkybox = 40,
        EnvSnow = 83,
        EnvTree = 81,
        EnvWater = 84,

        // Architecture / construction (110–179)
        ArchiBridge = 150,
        ArchiBuilding = 110,
        ArchiDoor = 180,
        ArchiFence = 160,
        ArchiLadder = 190,
        ArchiModularBuilding = 120,
        ArchiRoad = 140,
        ArchiWall = 170,
        ArchiWindow = 185,

        // Props & placeables (200–299)
        PropChest = 240,
        PropCollectible = 220,   // (consistent spelling)
        PropContainer = 260,
        PropDecoration = 200,
        PropFlag = 250,
        PropFood = 210,
        PropMovingObject = 280,
        PropResource = 270,   // was PickUpMaterial
        PropSign = 230,
        PropTool = 290,   // tools, weapons, instruments

        Platform = 300,   // gameplay platform



        // Level flow / editor helpers (400)
        GameAI = 400,   // has behaviour tree / AI
        GameBreakable = 410,
        GameCheckpoint = 420,
        GameClimbable = 425,
        GameHazard = 430,   // spikes, lava, etc.
        GameInteractive = 435,
        GameObstacle = 440,
        GameQuestItem = 445,
        GameSpawnPoint = 450,
        GameSpecial = 460,
        GameTeleport = 470,
        GameTrigger = 480,
        GameVolume = 490,

        // Vehicles (510)
        VehicleCar = 510,
        VehicleTrain = 520,
        VehicleBoat = 530,
        VehicleAirplane = 540,
        VehicleBicycle = 550,

        // Food sub-types (600)
        FoodFruit = 600,
        FoodVegetable = 610,
        FoodMeat = 620,
        FoodDrink = 630,
        FoodBread = 640,
        FoodDairy = 650,

        // VFX / Audio (700)
        FxEffect = 700,
        FxSoundFx = 710,
        FxMusic = 720,
        FxLightSource = 730,

        // Culture / region (800)
        RegionAsia = 800,
        RegionEuropea = 810,
        RegionAfrica = 820,
        RegionAmerica = 830,

        // Size / performance (900)
        SizeSmall = 900,
        SizeMedium = 910,
        SizeLarge = 920,
        SizeHighPoly = 951,

        // Theme
        ThemeModern = 1,
        ThemeMedieval = 2,
        ThemeFantasy = 3,
        ThemeFuturistic = 4,
        ThemeAncient = 5,

    }


    public class WorldPrefabData : MonoBehaviour
    {
        [Tooltip("Unique identifier for this prefab")]
        public string Id;

        [Header("Family")]
        public WorldPrefabKit Kit;
        public Countries Country = Countries.International;
        [Header("Category")]
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
