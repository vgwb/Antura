using Antura.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// Chose and play sound effect on button click.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class OnClickButtonFX : MonoBehaviour
    {
        public Sfx SfxOnClick = Sfx.UIPopup;

        void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(playFx);
        }

        void OnDisable()
        {
            GetComponent<Button>().onClick.RemoveListener(playFx);
        }

        void playFx()
        {
            AudioManager.I.PlaySound(SfxOnClick);
        }
    }
}
