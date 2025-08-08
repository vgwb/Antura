using UnityEngine;
using UnityEngine.EventSystems;

namespace Antura.Discover.Activities
{
    [RequireComponent(typeof(PianoKey))]
    public class PianoKeyInputRelay : MonoBehaviour, IPointerClickHandler
    {
        public ActivityPiano activity;
        private PianoKey key;

        private void Awake()
        {
            key = GetComponent<PianoKey>();
            if (activity == null)
                activity = FindObjectOfType<ActivityPiano>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (activity != null && activity.enabled && activity.playMode == PianoPlayMode.Repeat)
            {
                activity.OnKeyPressed(key.noteName);
            }
        }
    }
}
