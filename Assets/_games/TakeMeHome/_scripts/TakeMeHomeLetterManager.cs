using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.TakeMeHome
{

    public class TakeMeHomeLetterManager : MonoBehaviour
    {
        public GameObject plane;
        public LivingLetterController LLPrefab;

        TakeMeHomeLL dragging;

        TakeMeHomeLL _letter;

        private TakeMeHomeGame game;

        void Start()
        {
            game = GetComponent<TakeMeHomeGame>();

            game.Context.GetInputManager().onPointerDown += OnPointerDown;
            game.Context.GetInputManager().onPointerUp += OnPointerUp;
            game.Context.GetInputManager().onPointerDrag += OnPointerDrag;
        }

        void OnPointerDown()
        {
            if (_letter != null)
            {
                var pointerPosition = game.Context.GetInputManager().LastPointerPosition;
                var screenRay = Camera.main.ScreenPointToRay(pointerPosition);

                RaycastHit hitInfo;
                if (_letter.GetComponent<Collider>().Raycast(screenRay, out hitInfo, Camera.main.farClipPlane))
                {
                    dragging = _letter;

                    _letter.OnPointerDown(pointerPosition);
                }
            }
        }

        void OnPointerUp()
        {

            dragging = null;
            if (_letter != null)
                _letter.OnPointerUp();


        }

        void OnPointerDrag()
        {
            if (dragging != null && _letter == dragging)
            {
                var pointerPosition = game.Context.GetInputManager().LastPointerPosition;
                _letter.OnPointerDrag(pointerPosition);
            }
        }

        public void removeLetter()
        {
            if (_letter != null)
            {
                Destroy(_letter.gameObject);
                _letter = null;
            }
        }

        //uses fast crowd letter management and dragging:
        public TakeMeHomeLL spawnLetter(ILivingLetterData data)
        {
            //

            LivingLetterController letterObjectView = Instantiate(LLPrefab);
            letterObjectView.gameObject.SetActive(true);
            letterObjectView.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            letterObjectView.transform.SetParent(transform, true);
            Vector3 newPosition = GetComponent<TakeMeHomeGame>().LLSpawnPosition.position;// = walkableArea.GetFurthestSpawn(letters); // Find isolated spawn point

            letterObjectView.transform.position = newPosition;
            //letterObjectView.transform.rotation = Quaternion.identity
            letterObjectView.Init(data);

            var ll = letterObjectView.gameObject.AddComponent<TakeMeHomeLL>();
            ll.Initialize(plane.transform.position.y, letterObjectView, GetComponent<TakeMeHomeGame>().spawnTube.transform.position);

            /*/var livingLetter = letterObjectView.gameObject.AddComponent<FastCrowdLivingLetter>();
			//livingLetter.crowd = this;

			letterObjectView.gameObject.AddComponent<FastCrowdDraggableLetter>();*/
            letterObjectView.gameObject.AddComponent<Rigidbody>().isKinematic = true;

            foreach (var collider in letterObjectView.gameObject.GetComponentsInChildren<Collider>())
                collider.isTrigger = true;

            var characterController = letterObjectView.gameObject.AddComponent<CharacterController>();
            characterController.height = 6;
            characterController.center = Vector3.up * 3;
            characterController.radius = 1.5f;



            _letter = (ll);

            return ll;
        }
    }
}
