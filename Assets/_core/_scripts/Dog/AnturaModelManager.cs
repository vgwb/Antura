using Antura.Core;
using Antura.Database;
using Antura.Profile;
using Antura.Rewards;
using UnityEngine;
using System.Collections.Generic;

namespace Antura.Dog
{
    /// <summary>
    /// Handles loading and assignment of visual reward props appearing on Antura.
    /// </summary>
    // TODO convention: rename variables
    // TODO refactor: the class needs a complete refactoring
    public class AnturaModelManager : MonoBehaviour
    {
        // TODO refactor: remove static instance
        public static AnturaModelManager I;

        [Header("Bones Attach")]
        public Transform Dog_head;

        public Transform Dog_spine01;
        public Transform Dog_jaw;
        public Transform Dog_Tail3;
        public Transform Dog_R_ear04;
        public Transform Dog_L_ear04;

        [Header("Materials Owner")]
        public SkinnedMeshRenderer SkinnedMesh;

        public SkinnedMeshRenderer[] SkinnedMeshsTextureOnly;

        /// <summary>
        /// Pointer to transform used as parent for add reward model (and remove if already mounted yet).
        /// </summary>
        [HideInInspector]
        public Transform transformParent;

        #region Life cycle

        void Awake()
        {
            I = this;
        }

        void Start()
        {
            if (AppManager.I.Player != null)
            {
                var c = AppManager.I.Player.CurrentAnturaCustomizations;
                LoadAnturaCustomization(c);
            }
        }

        #endregion

        class LoadedModel
        {
            public RewardPack RewardPack;
            public GameObject GO;
        }

        private List<LoadedModel> LoadedModels = new List<LoadedModel>();
        private RewardPack LoadedTexturePack = null;
        private RewardPack LoadedDecalPack = null;

        #region API

        /// <summary>
        /// Loads the antura customization elements on Antura model.
        /// </summary>
        /// <param name="_anturaCustomization">The antura customization.</param>
        public void LoadAnturaCustomization(AnturaCustomization _anturaCustomization)
        {
            ClearLoadedRewardPacks();
            foreach (var propPack in _anturaCustomization.PropPacks)
            {
                LoadRewardPackOnAntura(propPack);
                ModelsManager.SwitchMaterial(LoadRewardPackOnAntura(propPack), propPack.GetMaterialPair());
            }
            LoadRewardPackOnAntura(_anturaCustomization.TexturePack);
            LoadRewardPackOnAntura(_anturaCustomization.DecalPack);
        }

        /// <summary>
        /// Saves the antura customization using the current model customization.
        /// </summary>
        /// <returns></returns>
        public AnturaCustomization SaveAnturaCustomization()
        {
            AnturaCustomization returnCustomization = new AnturaCustomization();
            foreach (LoadedModel loadedModel in LoadedModels)
            {
                returnCustomization.PropPacks.Add(loadedModel.RewardPack);
                returnCustomization.PropPacksIds.Add(loadedModel.RewardPack.UniqueId);
            }
            returnCustomization.TexturePack = LoadedTexturePack;
            returnCustomization.TexturePackId = LoadedTexturePack.UniqueId;
            returnCustomization.DecalPack = LoadedDecalPack;
            returnCustomization.DecalPackId = LoadedDecalPack.UniqueId;
            AppManager.I.Player.SaveAnturaCustomization(returnCustomization);

            return returnCustomization;
        }


        public GameObject LoadRewardPackOnAntura(RewardPack rewardPack)
        {
            if (rewardPack == null)
            { return null; }
            switch (rewardPack.BaseType)
            {
                case RewardBaseType.Prop:
                    return LoadRewardPropOnAntura(rewardPack);
                case RewardBaseType.Texture:
                    var newMaterial = MaterialManager.LoadTextureMaterial(rewardPack.BaseId, rewardPack.ColorId);
                    // Main mesh
                    var mats = SkinnedMesh.sharedMaterials;
                    mats[0] = newMaterial;
                    SkinnedMesh.sharedMaterials = mats;
                    LoadedTexturePack = rewardPack;
                    // Sup mesh for texture
                    foreach (var _renderer in SkinnedMeshsTextureOnly)
                    {
                        var materials = _renderer.sharedMaterials;
                        materials[0] = newMaterial;
                        _renderer.sharedMaterials = materials;
                    }
                    break;
                case RewardBaseType.Decal:
                    Material newDecalMaterial = MaterialManager.LoadTextureMaterial(rewardPack.BaseId, rewardPack.ColorId);
                    // Main mesh
                    Material[] decalMats = SkinnedMesh.sharedMaterials;
                    decalMats[1] = newDecalMaterial;
                    SkinnedMesh.sharedMaterials = decalMats;
                    // Sup mesh for texture
                    foreach (SkinnedMeshRenderer _renderer in SkinnedMeshsTextureOnly)
                    {
                        Material[] materials = _renderer.sharedMaterials;
                        materials[1] = newDecalMaterial;
                        _renderer.sharedMaterials = materials;
                    }
                    LoadedDecalPack = rewardPack;
                    break;
                default:
                    Debug.LogWarningFormat("Reward Type {0} not found!", rewardPack.BaseType);
                    break;
            }
            return null;
        }

        public void ClearLoadedRewardPacks()
        {
            foreach (var item in LoadedModels)
            {
                Destroy(item.GO);
            }
            LoadedModels.Clear();
        }

        public void ClearLoadedRewardsWithoutDestroy()
        {
            for(int i=0; i<LoadedModels.Count; i++)
            {
                LoadedModels[i].GO.SetActive(false);
                LoadedModels.RemoveAt(i);
            }

            if (LoadedModels.Count > 0) //for some reason, sometimes the loop left 1 item loaded, so we call this very same method to clean it again
                ClearLoadedRewardsWithoutDestroy();
        }

        /// <summary>
        /// Clears the loaded reward in category.
        /// </summary>
        /// <param name="_categoryId">The category identifier.</param>
        public void ClearLoadedRewardInCategory(string _categoryId)
        {
            LoadedModel lm = LoadedModels.Find(m => m.RewardPack.Category == _categoryId);
            if (lm != null)
            {
                Destroy(lm.GO);
                LoadedModels.Remove(lm);
            }
        }

        /// <summary>
        /// Sets the reward material colors.
        /// </summary>
        /// <param name="_gameObject">The game object.</param>
        /// <param name="rewardPackUnlockData">The reward pack.</param>
        /// <returns></returns>
        public GameObject SetRewardMaterialColors(GameObject _gameObject, RewardPack rewardPack)
        {
            ModelsManager.SwitchMaterial(_gameObject, rewardPack.GetMaterialPair());
            //actualRewardsForCategoryColor.Add()
            return _gameObject;
        }

        /// <summary>
        /// Loads the reward on model.
        /// </summary>
        /// <param name="rewardPackUnlockData">The identifier.</param>
        /// <returns></returns>
        private GameObject LoadRewardPropOnAntura(RewardPack rewardPack)
        {
            RewardProp prop = rewardPack.RewardBase as RewardProp;
            if (prop == null)
            {
                Debug.LogFormat("Prop {0} not found!", rewardPack.BaseId);
                return null;
            }

            // Check if we already loaded a reward of this category
            LoadedModel loadedModel = LoadedModels.Find(lm => lm.RewardPack.Category == prop.Category);
            if (loadedModel != null)
            {
                Destroy(loadedModel.GO);
                LoadedModels.Remove(loadedModel);
            }

            // Load Model
            string boneParent = prop.BoneAttach;
            GameObject rewardModel = null;
            switch (boneParent)
            {
                case "dog_head":
                    rewardModel = ModelsManager.MountModel(prop.ID, Dog_head);
                    break;
                case "dog_spine01":
                    rewardModel = ModelsManager.MountModel(prop.ID, Dog_spine01);
                    break;
                case "dog_jaw":
                    rewardModel = ModelsManager.MountModel(prop.ID, Dog_jaw);
                    break;
                case "dog_Tail4":
                    rewardModel = ModelsManager.MountModel(prop.ID, Dog_Tail3);
                    break;
                case "dog_R_ear04":
                    rewardModel = ModelsManager.MountModel(prop.ID, Dog_R_ear04);
                    break;
                case "dog_L_ear04":
                    rewardModel = ModelsManager.MountModel(prop.ID, Dog_L_ear04);
                    break;
            }

            // Set materials
            ModelsManager.SwitchMaterial(rewardModel, rewardPack.GetMaterialPair());

            // Save on LoadedModel List
            LoadedModels.Add(new LoadedModel() { RewardPack = rewardPack, GO = rewardModel });
            return rewardModel;
        }

        #endregion

        #region events

        void OnEnable()
        {
            if (AppManager.I == null || AppManager.I.RewardSystemManager == null)
                return;
            AppManager.I.RewardSystemManager.OnRewardSelectionChanged += RewardSystemManager_OnRewardItemChanged;
            PlayerProfileManager.OnProfileChanged += PlayerProfileManager_OnProfileChanged;
        }

        private void PlayerProfileManager_OnProfileChanged()
        {
            LoadAnturaCustomization(AppManager.I.Player.CurrentAnturaCustomizations);
        }

        private void RewardSystemManager_OnRewardItemChanged(RewardPack rewardPack)
        {
            LoadRewardPackOnAntura(rewardPack);
            //rewardPack.SetNew(false);

            AppManager.I.RewardSystemManager.SaveRewardsUnlockDataChanges();
            SaveAnturaCustomization();
        }

        void OnDisable()
        {
            AppManager.I.RewardSystemManager.OnRewardSelectionChanged -= RewardSystemManager_OnRewardItemChanged;
            PlayerProfileManager.OnProfileChanged -= PlayerProfileManager_OnProfileChanged;
        }

        #endregion
    }
}
