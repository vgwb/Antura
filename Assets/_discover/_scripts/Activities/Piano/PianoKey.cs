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

        public AudioClip[] baseSemitoneClips = new AudioClip[12];
        public AudioSource audioSource;
        public int baseOctave = 4;

        public TextMeshProUGUI label;
        public Image background;
        public Color normalColor = Color.white;
        public Color highlightColor = new Color(1f, 0.95f, 0.3f, 1f);

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
            Play();
        }

        public void Play(float velocity = 1f)
        {
            var clip = GetClipFor(noteName);
            if (clip == null)
                return;
            // float octaveShift = Mathf.Pow(2f, (octave - baseOctave));
            // float semiShift = Mathf.Pow(2f, NoteUtils.SemitoneOf(noteName) / 12f);
            audioSource.pitch = 1;
            audioSource.PlayOneShot(clip);
            Debug.Log($"Playing note: {NoteUtils.DisplayName(noteName)} at octave {octave} with pitch {audioSource.pitch}");
        }

        private AudioClip GetClipFor(NoteName name)
        {
            int idx = NoteUtils.SemitoneOf(name) % 12 + (octave - baseOctave) * 12;
            if (baseSemitoneClips == null || baseSemitoneClips.Length < 12)
                return null;
            return baseSemitoneClips[idx];
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
