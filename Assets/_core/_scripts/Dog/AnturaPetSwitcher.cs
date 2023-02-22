using System.Linq;
using Antura.Core;
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
        private AnturaPetType PetType {
            get
            {
                if (UseForcedPetType)
                {
                    return ForcedPetType;
                }

                return AppManager.I.Player.PetData.SelectedPet;
            }
        }
        public bool AutoSpawn;

        public AnturaModelManager[] LoadablePrefabs;
        private AnturaModelManager currentModelManager;

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
            if (AutoSpawn) LoadPet(PetType);
        }

        private void CheckPet()
        {
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

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (UseForcedPetType) return;

                if (AppManager.I.Player.PetData.SelectedPet == AnturaPetType.Dog) AppManager.I.Player.PetData.SelectedPet = AnturaPetType.Cat;
                else AppManager.I.Player.PetData.SelectedPet = AnturaPetType.Dog;
                LoadPet(AppManager.I.Player.PetData.SelectedPet);
            }
        }
    }
}
