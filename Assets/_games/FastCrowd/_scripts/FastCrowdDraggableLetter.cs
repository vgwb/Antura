using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.FastCrowd
{
    [RequireComponent(typeof(StrollingLivingLetter))]
    [RequireComponent(typeof(LivingLetterController))]
    public class FastCrowdDraggableLetter : MonoBehaviour
    {
        StrollingLivingLetter letter;
        LetterCharacterController movement;
        DropAreaWidget currentDropAreaWidget;
        DropSingleArea currentDropArea;

        Vector3 rayOffset;
        bool isDragging = false;
        float currentY = 0;

        float overrideDirectionY;
        bool hasValidY = false;

        void Awake()
        {
            letter = GetComponent<StrollingLivingLetter>();
            movement = letter.GetComponent<LetterCharacterController>();
        }

        void OnDestroy()
        {

        }

        public void StartDragging(Vector3 dragOffset)
        {
            letter.SetCurrentState(letter.HangingState);
            var oldPos = letter.transform.position;
            currentY = oldPos.y;
            letter.transform.position = oldPos;

            rayOffset = dragOffset;

            movement.EnableSweep = false;

            isDragging = true;
        }

        public void EndDragging()
        {
            letter.SetCurrentState(letter.FallingState);

            if (currentDropArea != null)
                letter.DropOnArea(currentDropAreaWidget);

            isDragging = false;
            movement.EnableSweep = true;
        }

        void OnDrag()
        {
            if (letter.GetCurrentState() != letter.HangingState)
                letter.SetCurrentState(letter.HangingState);

            Vector3 oldPos;

            if (ComputePointedPosition(out oldPos, rayOffset))
            {
                oldPos.y = currentY;

                currentY = Mathf.Lerp(currentY, 1.5f, Time.deltaTime * 10.0f);

                movement.MoveTo(oldPos);
            }
        }

        bool ComputePointedPosition(out Vector3 output, Vector3 rayOffset)
        {
            var screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            var o = screenRay.origin;
            o -= rayOffset;
            screenRay.origin = o;

            var direction = screenRay.direction;

            if (Mathf.Abs(direction.y) > 0.1f)
            {
                overrideDirectionY = direction.y;
                hasValidY = true;
            }

            if (hasValidY)
            {
                direction.y = overrideDirectionY;

                float t = -screenRay.origin.y / direction.y;

                Vector3 pos = screenRay.origin + t * direction;
                pos.y = 0;

                pos = letter.crowd.walkableArea.GetNearestPoint(pos);
                pos.y = 0;

                output = pos;
                return true;
            }

            output = Vector3.zero;
            return false;
        }


        void Update()
        {
            rayOffset.x = Mathf.Lerp(rayOffset.x, 0, Time.deltaTime);
            rayOffset.z = Mathf.Lerp(rayOffset.z, 0, Time.deltaTime);

            if (isDragging)
                OnDrag();
            else
            {
                if (currentDropAreaWidget != null)
                {
                    currentDropAreaWidget.SetMatchingOutline(false, false);

                    currentDropAreaWidget = null;
                    currentDropArea = null;
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (letter.GetCurrentState() == letter.HangingState)
            {
                DropSingleArea singleArea = other.GetComponent<DropSingleArea>();
                if (singleArea)
                {
                    var dropArea = singleArea.transform.parent.GetComponent<DropAreaWidget>(); // dirty hack

                    if (dropArea == null)
                        return;

                    currentDropAreaWidget = dropArea;
                    currentDropArea = singleArea;

                    var activeData = dropArea.GetActiveData();
                    var draggedData = GetComponent<LivingLetterController>().Data;
                    bool matching = FastCrowdConfiguration.Instance.IsDataMatching(activeData, draggedData);

                    dropArea.SetMatchingOutline(true, matching);
                }
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (currentDropAreaWidget == null)
                OnTriggerEnter(other);
        }

        void OnTriggerExit(Collider other)
        {
            DropSingleArea dropArea = other.GetComponent<DropSingleArea>();
            if (dropArea && (dropArea == currentDropArea))
            {
                currentDropAreaWidget.SetMatchingOutline(false, false);

                currentDropAreaWidget = null;
                currentDropArea = null;
            }
        }
    }
}
