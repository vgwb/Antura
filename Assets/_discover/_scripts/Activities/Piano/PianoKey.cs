using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Antura.Discover.Activities
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class PianoKey : MonoBehaviour, IPointerClickHandler
    {
        public NoteName noteName;
        public int octave;

        // Removed per-key clips; each key queries the keyboard
        public PianoKeyboard keyboard;

        public AudioSource audioSource;
        public int baseOctave = 4;

        public TextMeshProUGUI label;
        public Image background;
        public Color normalColor = Color.white;
        public Color highlightColor = new Color(1f, 0.95f, 0.3f, 1f);

        [Tooltip("Seconds to flash highlightColor when pressed")]
        public float pressFlashSeconds = 0.2f;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            if (!audioSource)
                audioSource = gameObject.AddComponent<AudioSource>();
            if (!background)
                background = GetComponent<Image>();
            SetLabelActive(false);
        }

        public void SetLabelActive(bool on)
        {
            if (label)
                label.gameObject.SetActive(on);
            if (label)
                label.text = NoteUtils.DisplayName(noteName);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Flash(pressFlashSeconds);
            Play();
        }

        public void Play(float velocity = 1f)
        {
            var clip = GetClipFor(noteName);
            if (clip == null)
                return;
            audioSource.pitch = 1f; // never change pitch
            audioSource.PlayOneShot(clip);
        }

        private AudioClip GetClipFor(NoteName name)
        {
            if (keyboard == null || keyboard.baseSemitoneClips == null || keyboard.baseSemitoneClips.Length == 0)
                return null;
            int idx = NoteUtils.SemitoneOf(name) % keyboard.baseSemitoneClips.Length; // cycle if needed
            return keyboard.baseSemitoneClips[idx];
        }

        public void Flash(float seconds = 0.25f)
        {
            if (!isActiveAndEnabled)
                return;
            StopAllCoroutines();
            StartCoroutine(FlashRoutine(seconds));
        }

        System.Collections.IEnumerator FlashRoutine(float seconds)
        {
            var old = background ? background.color : Color.white;
            if (background)
                background.color = highlightColor;
            yield return new WaitForSecondsRealtime(seconds);
            if (background)
                background.color = old;
        }
    }
}
