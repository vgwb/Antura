using Antura.Minigames;
using UnityEngine;

namespace Antura.Minigames.MixedLetters
{
    public class RotateButtonController : MonoBehaviour
    {
        public BoxCollider boxCollider;
        public DropZoneController dropZone;
        // Use this for initialization
        void Start()
        {
            IInputManager inputManager = MixedLettersConfiguration.Instance.Context.GetInputManager();
            inputManager.onPointerDown += OnPointerDown;
        }

        private void OnPointerDown()
        {
            Ray ray = Camera.main.ScreenPointToRay(MixedLettersConfiguration.Instance.Context.GetInputManager().LastPointerPosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider == boxCollider)
            {
                dropZone.OnRotateLetter();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDropZone(DropZoneController dropZone)
        {
            this.dropZone = dropZone;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
