using System.Linq;
using Antura.AnturaSpace;
using Antura.Core;
using Antura.Profile;
using Antura.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Dog
{
    /// <summary>
    /// Allows switching to multiple models of Antura. Used as an entry point for all Antura pet models.
    /// </summary>
    public class AnturaPetSwitcher : MonoBehaviour
    {
        public bool UseForcedPetType;
        public AnturaPetType ForcedPetType = AnturaPetType.Dog;
        private AnturaPetType PetType
        {
            get
            {
                if (UseForcedPetType)
                {
                    return ForcedPetType;
                }

                if (AppManager.I.Player == null)
                {
                    return AnturaPetType.Dog;
                    // No profile fix
                }

                return AppManager.I.Player.PetData.SelectedPet;
            }
        }
        public bool AutoSpawn;

        public AnturaModelManager[] LoadablePrefabs;
        private AnturaModelManager currentModelManager;

        public AnturaModelManager StartingModelManager;

        public AnturaModelManager ModelManager
        {
            get
            {
                if (currentModelManager == null)
                {
                    LoadPet(PetType);
                }
                return currentModelManager;
            }
        }

        public AnturaAnimationController AnimController
        {
            get
            {
                if (currentModelManager == null)
                {
                    LoadPet(PetType);
                }
                return currentModelManager.GetComponent<AnturaAnimationController>();
            }
        }

        public Transform Pivot;
        private bool initialised;

        public void Awake()
        {
            if (AutoSpawn)
                LoadPet(PetType);
            if (StartingModelManager != null)
                currentModelManager = StartingModelManager;
        }

        public void LoadPet(AnturaPetType petType)
        {
            if (currentModelManager != null)
            {
                Destroy(currentModelManager.gameObject);
            }

            var prefab = LoadablePrefabs.FirstOrDefault(x => x.PetType == petType);
            currentModelManager = Instantiate(prefab, Pivot);
            var currentPetTr = currentModelManager.transform;
            currentPetTr.localPosition = Vector3.zero;
            currentPetTr.localRotation = Quaternion.identity;
        }

        public void SwitchPet(bool alsoLoadInScene)
        {
            if (AppManager.I.Player.PetData.SelectedPet == AnturaPetType.Dog)
                AppManager.I.Player.PetData.SelectedPet = AnturaPetType.Cat;
            else
                AppManager.I.Player.PetData.SelectedPet = AnturaPetType.Dog;

            if (alsoLoadInScene)
            {
                LoadPet(AppManager.I.Player.PetData.SelectedPet);
            }

            AppManager.I.Player.Save();

            foreach (var bone3D in FindObjectsByType<Bone3D>(FindObjectsSortMode.None))
            {
                bone3D.Switch();
            }
            foreach (var uiBone in FindObjectsByType<UIBone>(FindObjectsSortMode.None))
            {
                uiBone.Switch();
            }
            foreach (var shopActionUI in FindObjectsByType<ShopActionUI>(FindObjectsSortMode.None))
            {
                shopActionUI.RetriggerRender();
            }
        }

        void OnEnable()
        {
            if (AppManager.I == null || AppManager.I.RewardSystemManager == null)
                return;
            PlayerProfileManager.OnProfileChanged += PlayerProfileManager_OnProfileChanged;
        }
        void OnDisable()
        {
            PlayerProfileManager.OnProfileChanged -= PlayerProfileManager_OnProfileChanged;
        }
        private void PlayerProfileManager_OnProfileChanged()
        {
            // LoadPet(AppManager.I.Player.PetData.SelectedPet);
        }

    }
}
