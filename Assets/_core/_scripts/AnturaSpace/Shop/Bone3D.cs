using Antura.Core;
using UnityEngine;

namespace Antura.UI
{
    public class Bone3D : MonoBehaviour
    {
        public bool autoSpawn = true;
        public bool skipSwitching = false;
        private GameObject loadedBoneGo;

        public void OnEnable()
        {
            if (autoSpawn) Switch();
        }

        public void Switch()
        {
            if (skipSwitching) return;
            if (loadedBoneGo != null) Destroy(loadedBoneGo);
            loadedBoneGo = Instantiate(Resources.Load<GameObject>($"{AppManager.I.Player.PetData.SelectedPet}/Biscuit/bonePrefab"), transform);
        }
    }
}
