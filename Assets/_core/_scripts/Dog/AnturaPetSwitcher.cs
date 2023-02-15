using System.Linq;
using UnityEngine;

namespace Antura.Dog
{
    /// <summary>
    /// Allows switching to multiple models of Antura. Used as an entry point for all Antura pet models.
    /// </summary>
    public class AnturaPetSwitcher : MonoBehaviour
    {
        public AnturaPetType PetType = AnturaPetType.Dog;
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

        public void Awake()
        {
            if (AutoSpawn) LoadPet(PetType);
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
                if (ModelManager.PetType == AnturaPetType.Dog) LoadPet(AnturaPetType.Cat);
                else LoadPet(AnturaPetType.Dog);
            }
        }
    }
}
