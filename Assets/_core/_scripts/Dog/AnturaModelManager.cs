using System;
using Antura.Core;
using Antura.Database;
using Antura.Profile;
using Antura.Rewards;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Antura.Dog
{
    /// <summary>
    /// Handles loading and assignment of visual reward props appearing on Antura.
    /// </summary>
    // TODO convention: rename variables
    // TODO refactor: the class needs a complete refactoring
    public class AnturaModelManager : MonoBehaviour
    {
        [Header("Pet")]
        public AnturaPetType PetType;

        [Header("Bones Attach")]
        public Transform RootBone;

        public Transform JawBone; // @note: Needed for bone catching

        private List<SkinnedMeshRenderer> propSMRs = new List<SkinnedMeshRenderer>();

        [Header("Materials Owner")]
        public SkinnedMeshRenderer SkinnedMesh;

        public SkinnedMeshRenderer[] SkinnedMeshsTextureOnly;

        /// <summary>
        /// Pointer to transform used as parent for add reward model (and remove if already mounted yet).
        /// </summary>
        [HideInInspector]
        public Transform transformParent;

        #region Life cycle

        void Start()
        {
            if (AppManager.I.Player != null)
            {
                var c = AppManager.I.Player.AnturaCustomization(PetType);
                LoadAnturaCustomization(c);
            }
        }

        public void AssignPropSkinnedMeshRenderers()
        {
            propSMRs.RemoveAll(x => x == null);

            // Skinned mesh renderer support
            SkinnedMeshRenderer targetRenderer = SkinnedMesh;
            var boneMap = new Dictionary<string,Transform>();
            foreach(var bone in targetRenderer.bones)
                boneMap[bone.gameObject.name] = bone;

            foreach (SkinnedMeshRenderer additionalSmr in propSMRs)
            {
                var newBones = new Transform[additionalSmr.bones.Length];
                for( int i = 0; i < additionalSmr.bones.Length; ++i )
                {
                    GameObject bone = additionalSmr.bones[i].gameObject;
                    if(!boneMap.TryGetValue(bone.name, out newBones[i]))
                    {
                        Debug.Log($"Unable to map bone {bone.name} to target skeleton.");
                        break;
                    }
                }
                additionalSmr.bones = newBones;
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
            returnCustomization.PetType = AppManager.I.Player.PetData.SelectedPet;
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
                    var newMaterial = MaterialManager.LoadTextureMaterial(PetType, rewardPack.BaseId, rewardPack.ColorId);
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
                    Material newDecalMaterial = MaterialManager.LoadTextureMaterial(PetType, rewardPack.BaseId, rewardPack.ColorId);
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

            // Load the new Model
            var targetBone = RootBone;
            if (prop.BoneAttach.ToLower() != "none")
            {
                targetBone = RecursiveFind(RootBone, prop.BoneAttach);
                if (targetBone == null)
                {
                    Debug.LogWarning($"Could not find bone {prop.BoneAttach} to attach the prop to.");
                    targetBone = RootBone; // Fallback
                }
            }
            var rewardModel = ModelsManager.MountModel(PetType, prop.ID, targetBone);

            var smrs = rewardModel.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (smrs.Length > 0)
            {
                rewardModel.transform.SetParent(RootBone, false);
                foreach (var smr in rewardModel.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    propSMRs.Add(smr);
                }
                AssignPropSkinnedMeshRenderers();
            }

            // Set materials
            ModelsManager.SwitchMaterial(rewardModel, rewardPack.GetMaterialPair());

            // Save on LoadedModel List
            LoadedModels.Add(new LoadedModel() { RewardPack = rewardPack, GO = rewardModel });
            return rewardModel;
        }

        private Transform RecursiveFind(Transform rootBone, string childName)
        {
            foreach (Transform childTr in rootBone.transform)
            {
                if (childTr.name.Equals(childName, StringComparison.OrdinalIgnoreCase))
                {
                    return childTr;
                }

                if (childTr.childCount > 0)
                {
                    var found = RecursiveFind(childTr, childName);
                    if (found != null) return found;
                }
            }
            return null;
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
            LoadAnturaCustomization(AppManager.I.Player.CurrentSingleAnturaCustomization);
        }

        private void RewardSystemManager_OnRewardItemChanged(RewardPack rewardPack)
        {
            LoadRewardPackOnAntura(rewardPack);
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
