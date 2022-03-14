using UnityEngine;
using System;
using System.Collections;
using TMPro;

namespace Antura.Minigames.Scanner
{

    public class ScannerSuitcase : MonoBehaviour
    {

        public ScannerGame game;
        public bool isDragging = false, isReady;
        private Vector3 screenPoint;
        private Vector3 offset;
        private float startX;
        private float startY;
        private float startZ;
        private Collider player;
        private Transform originalParent;
        private BoxCollider thisCollider;
        private Rigidbody thisRigidbody;
        private Vector3 originalColliderSize, originalColliderCenter, shadowStartPos;

        public Vector3 fingerOffset;
        public float scale = 0.5f;
        public bool isCorrectAnswer = false;
        public TextMeshPro drawing;
        public GameObject shadow;

        [HideInInspector]
        public string wordId;


        bool overPlayermarker = false;

        public event Action<GameObject, ScannerLivingLetter> onCorrectDrop;
        public event Action<GameObject, ScannerLivingLetter> onWrongDrop;

        IEnumerator Coroutine_ScaleOverTime(float time)
        {
            Vector3 originalScale = shadow.transform.localScale;//transform.localScale;
            Vector3 destinationScale = new Vector3(7.6f, 14.167f, 14.167f);//new Vector3(1.0f, 1.0f, 1.0f);

            float currentTime = 0.0f;
            do
            {
                shadow.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= time);
        }


        IEnumerator dropSuitcase()
        {
            shadow.transform.localScale = Vector3.zero;
            shadow.transform.position = shadowStartPos;
            transform.position = new Vector3(startX, 20, startZ);

            float r = (UnityEngine.Random.value / 2f) + (UnityEngine.Random.value / 2f);

            yield return new WaitForSeconds(r);
            thisRigidbody.isKinematic = false;
            thisRigidbody.useGravity = true;
            thisRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

            StartCoroutine(Coroutine_ScaleOverTime(1f));

            yield return new WaitForSeconds(1.5f);
            isReady = true;
            thisRigidbody.isKinematic = true;
            thisCollider.size = originalColliderSize;
            thisCollider.center = originalColliderCenter;

        }

        IEnumerator resetSuitCasePos()
        {
            while (transform.position.y > startY + 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(startX, startY, startZ), 0.25f);
                shadow.transform.position = new Vector3(transform.position.x, shadow.transform.position.y, transform.position.z);
                yield return null;
            }

            isReady = true;
            thisRigidbody.isKinematic = true;
            thisCollider.size = originalColliderSize;
            thisCollider.center = originalColliderCenter;
        }


        void Start()
        {
            originalParent = transform.parent;
            startX = transform.position.x;
            startY = transform.position.y;
            startZ = transform.position.z;

            shadowStartPos = shadow.transform.position;
            thisCollider = GetComponentInChildren<BoxCollider>();
            thisRigidbody = GetComponent<Rigidbody>();
            //Reset();

            originalColliderSize = thisCollider.size;
            originalColliderCenter = thisCollider.center;
            shadow.transform.localScale = Vector3.zero;
            transform.localPosition = Vector3.up * 20;
        }

        public void Reset(bool newRound = true)
        {
            groundHit = isReady = false;
            transform.parent = originalParent;

            //transform.position = new Vector3(startX, startY, startZ);
            thisRigidbody.isKinematic = true;



            thisCollider.size = new Vector3(originalColliderSize.x, 1, originalColliderSize.z);
            thisCollider.center = new Vector3(originalColliderCenter.x, 0, originalColliderCenter.z);
            isDragging = false;
            //transform.localScale = new Vector3(0.25f,0.25f,0.25f);
            transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            shadow.SetActive(true);
            //shadow.transform.position = shadowStartPos;

            if (newRound)
            {
                StartCoroutine(dropSuitcase());
            }
            else
            {
                StartCoroutine(resetSuitCasePos());
            }

        }

        void OnMouseDown()
        {
            if (game.disableInput || !isReady || game.tut.tutStep == 0)
                return;


            //shadow.SetActive(false);
            isDragging = true;
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position -
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

            //transform.localScale = new Vector3(scale,scale,scale);


        }

        void OnMouseDrag()
        {
            if (isDragging)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
                transform.position =
                    new Vector3(curPosition.x + fingerOffset.x, curPosition.y + fingerOffset.y, Mathf.Clamp(startZ + transform.position.y / 1.80f, -100, -11.5f));
                shadow.transform.position = new Vector3(transform.position.x, shadow.transform.position.y, transform.position.z);

            }

        }

        private ScannerLivingLetter lastDetectedLL;
        bool groundHit;

        void OnCollisionEnter(Collision other)
        {
            if (game.roundsManager.playSuitcaseSound)
            {
                if (!groundHit && other.gameObject.tag == "Obstacle")
                {
                    game.roundsManager.playSuitcaseSound = false;
                    groundHit = true;
                    //ScannerConfiguration.Instance.Context.GetAudioManager().StopSounds();
                    ScannerConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.CrateLandOnground);
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                lastDetectedLL = other.transform.root.GetComponent<ScannerLivingLetter>();
                overPlayermarker = true;
                player = other;
            }


        }

        void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                lastDetectedLL = other.transform.root.GetComponent<ScannerLivingLetter>();
                overPlayermarker = true;
                player = other;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                overPlayermarker = false;
            }
        }

        void OnMouseUp()
        {
            if (game.disableInput || !isReady)
                return;

            if (overPlayermarker && lastDetectedLL && lastDetectedLL.status == ScannerLivingLetter.LLStatus.StandingOnBelt)
            {
                shadow.transform.localScale = Vector3.zero;
                ScannerLivingLetter LL = player.transform.parent.GetComponent<ScannerLivingLetter>();
                if (isCorrectAnswer && LL.LLController.Data.Id == wordId)
                {
                    LL.gotSuitcase = true;
                    transform.parent = player.transform;
                    transform.localPosition = new Vector3(5.5f, 1, -2);
                    onCorrectDrop(gameObject, LL);
                    transform.localScale = new Vector3(scale, scale, scale);
                    game.GetComponent<ScannerTutorial>().tutStep = 1;

                }
                else
                {
                    onWrongDrop(gameObject, LL);
                }
            }
            else
            {
                Reset(false);
            }
            lastDetectedLL = null;
            isDragging = false;
            overPlayermarker = false;
        }

    }
}
