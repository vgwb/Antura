using UnityEngine;
using System.Collections;
using TMPro;

namespace Antura.Minigames.DancingDots
{
    public class DancingDotsDraggableDot : MonoBehaviour
    {
        public static bool ALLOW_ANY_DROPZONE_FOR_ANY_LETTER = true;

        private Vector3 screenPoint;
        private Vector3 offset;

        public DancingDotsGame game;

        public bool isDot;
        [Range(0, 3)]
        public int dots;

        public DiacriticEnum diacritic;

        public Vector3 fingerOffset;
        public TextMeshPro draggableText;
        public bool isCorrect = false;

        public bool isDragging = false;
        private bool overDestinationMarker = false;
        private bool overPlayermarker = false;
        private float startX;
        private float startY;
        private float startZ;
        private Vector3 originalScale;

        private void Awake()
        {
            originalScale = transform.localScale;
            startX = transform.position.x;
            startY = transform.position.y;
            startZ = transform.position.z;
        }

        public void Reset()
        {
            transform.position = new Vector3(startX, startY, startZ);
            isDragging = false;
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            StartCoroutine(Coroutine_ScaleOverTime(1f));
        }

        void OnMouseDown()
        {
            if (game.disableInput)
                return;

            isDragging = true;
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position -
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        }

        void OnMouseDrag()
        {
            if (game.disableInput)
                return;

            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = new Vector3(curPosition.x + fingerOffset.x, curPosition.y + fingerOffset.y, -10);

        }

        void OnMouseUp()
        {
            if (game.disableInput)
                return;

            if (overDestinationMarker && isCorrect)
            {
                if (isDot)
                {
                    game.CorrectDot();
                }
                else
                {
                    game.CorrectDiacritic();
                }
                gameObject.SetActive(false);
            }
            else
            {
                if (overPlayermarker && !isCorrect)
                {
                    isDragging = false;

                    if (game.isTutRound)
                    {
                        Reset();
                        return;
                    }

                    game.WrongMove(transform.position);

                    gameObject.SetActive(false);
                }
                else
                {
                    isDragging = false;

                    //					StartCoroutine(GoToStartPosition3());
                }
            }

            overPlayermarker = false;
            overDestinationMarker = false;
        }

        IEnumerator Coroutine_ScaleOverTime(float time)
        {
            //originalScale = transform.localScale;
            Vector3 destinationScale = originalScale;

            float currentTime = 0.0f;
            do
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, destinationScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= time);
        }

        void Update()
        {

            if (!isDragging)
                Dance();

        }

        void Dance()
        {
            transform.position = new Vector3(
                startX + Mathf.PerlinNoise(Time.time, startX) * 2.0f + 1,
                startY + Mathf.PerlinNoise(Time.time, startY) * 2.0f + 1,
                startZ + Mathf.PerlinNoise(Time.time, startZ) * 2.0f + 1);
        }

        void Setmarker(Collider other, bool markerStatus)
        {
            if (other.tag == "Player")
                overPlayermarker = markerStatus;

            if (markerStatus && ALLOW_ANY_DROPZONE_FOR_ANY_LETTER && overPlayermarker)
                overDestinationMarker = true;

            if (isDot)
            {
                if (other.tag == DancingDotsGame.DANCING_DOTS)
                {
                    //Debug.Log(game.removeDiacritics(game.currentLetter));

                    if (ALLOW_ANY_DROPZONE_FOR_ANY_LETTER || other.GetComponent<DancingDotsDropZone>().letters.Contains(game.removeDiacritics(game.currentLetter))
                        && game.dotsCount == dots)
                    {
                        overDestinationMarker = markerStatus;
                    }
                }
            }
            else
            {
                if (other.tag == DancingDotsGame.DANCING_DIACRITICS)
                {
                    if (ALLOW_ANY_DROPZONE_FOR_ANY_LETTER || game.activeDiacritic.diacritic == diacritic)
                    {
                        overDestinationMarker = markerStatus;
                    }
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            Setmarker(other, true);
        }

        void OnTriggerStay(Collider other)
        {
            Setmarker(other, true);
        }

        void OnTriggerExit(Collider other)
        {
            Setmarker(other, false);
        }

    }

}
