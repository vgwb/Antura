using Antura.Audio;
using Antura.Core;
using Antura.UI;
using UnityEngine;

namespace Antura.Map
{
    /// <summary>
    /// Controls clicks on the Map.
    /// </summary>
    public class MapClickController : MonoBehaviour
    {
        public StageMapsManager stageMapsManager;
        public MapCameraController mapCameraController;

        void Update()
        {
            // Touch movement controls
            if (AppManager.I.ModalWindowActivated) { return; }
            if (SceneTransitioner.IsShown) return;

            if (Input.GetMouseButtonUp(0) && !GlobalUI.I.IsFingerOverUI() && !mapCameraController.IsFollowingFinger) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                int layerMask = 1 << 15;
                if (Physics.Raycast(ray, out hit, 500, layerMask)) {
                    if (hit.collider.CompareTag("Pin")) {
                        AudioManager.I.PlaySound(Sfx.UIButtonClick);

                        var pin = hit.collider.transform.gameObject.GetComponent<Pin>();
                        stageMapsManager.SelectPin(pin);
                    }
                }
            }
        }

    }
}