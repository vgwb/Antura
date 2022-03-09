using UnityEngine;
using System.Collections;
using TMPro;

namespace Antura.Minigames.SickLetters
{
    public class SickLettersDraggableDD : MonoBehaviour
    {

        [Header("Parameters")]

        public bool IsDot;
        public bool IsDiacritic => !IsDot;

        private Vector3 screenPoint;
        private Vector3 offset;

        [Header("State")]
        public SickLettersGame game;
        public bool deattached;


        public Vector3 fingerOffset;
        public TextMeshPro draggableText;

        public bool isCorrect;
        public bool isInVase, touchedVase, collidedWithVase;

        public bool isDragging;

        bool overPlayermarker;
        bool shake;
        bool release;

        [HideInInspector]
        public Rigidbody thisRigidBody;
        [HideInInspector]
        public BoxCollider boxCollider;
        Transform origParent;

        Vector3 origLocalRotation;

        bool _checkDDCollision;
        public bool checkDDCollision
        {
            set
            {
                _checkDDCollision = value;
                StartCoroutine(resetCheckDDCollision());
            }
            get
            {
                return _checkDDCollision;
            }

        }


        IEnumerator resetCheckDDCollision()
        {
            yield return new WaitForSeconds(1);
            _checkDDCollision = false;
        }

        void Start()
        {
            thisRigidBody = GetComponent<Rigidbody>();
            boxCollider = GetComponent<BoxCollider>();
        }

        void OnMouseDown()
        {
            origParent = transform.parent;
            origLocalRotation = transform.localEulerAngles;
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            if (game.disableInput)
                return;

            release = false;
            isDragging = true;

            transform.parent = null;

            if (isCorrect)
            {
                if (game.roundsCount > 0)
                    game.wrongDraggCount++;
                shake = true;
                draggableText.transform.SetParent(transform, true);
            }
            else
            {
                offset = gameObject.transform.position -
                         Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            }
        }

        void OnMouseDrag()
        {

            if (isDragging && game.disableInput)
                releaseDD();
            else if (game.disableInput)
                return;

            if (release)
                return;

            game.tut.repeatConter = game.tut.repeatMax;

            //transform.eulerAngles = origRotation;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = new Vector3(curPosition.x + fingerOffset.x, curPosition.y + fingerOffset.y, curPosition.z + fingerOffset.z);

        }

        void OnMouseUp()
        {
            if (game.disableInput)
                return;
            releaseDD();
        }


        public void releaseDD()
        {
            release = true;
            isDragging = shake = false;
            StartCoroutine(offToON());

            if (overPlayermarker)//pointer Still over LL
            {
                if (isCorrect)
                {
                    resetCorrectDD();
                }
                else
                {
                    resetWrongDD();
                }
            }
            else //pointer isn't over LL
            {
                transform.parent = null;
                thisRigidBody.isKinematic = false;
                thisRigidBody.useGravity = true;
                boxCollider.isTrigger = false;

                boxCollider.center = Vector3.zero;
                boxCollider.size = new Vector3(0.1f, 0.25f, 0.1f);

                if (game.wrongDDsOnLL() == 0 || isCorrect)
                    game.disableInput = true;
            }

            overPlayermarker = false;
            StartCoroutine(destroyIfStuck());
        }

        IEnumerator destroyIfStuck()
        {
            yield return new WaitForSeconds(2);
            if (!isInVase && touchedVase)
            {
                touchedVase = false;
                poofDD();
            }
        }
        void Update()
        {

            if (shake && game.wrongDraggCount <= 1)
                shakeTransform(game.scale.transform, game.scale.vaseStartPose);
        }

        void Setmarker(Collider other, bool markerStatus)
        {
            if (other.tag == "Player")
                overPlayermarker = markerStatus;
        }

        public void resetWrongDD()
        {
            transform.SetParent(origParent);
            ApplyEmptyZone();
            collidedWithVase = touchedVase = false;

            thisRigidBody.isKinematic = true;
            boxCollider.enabled = true;
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector3(0.6f, 3.89f, 0.6f);
            boxCollider.center = Vector3.zero + Vector3.up * -1.62f;

            if (game.wrongDDsOnLL() > 0)
                game.disableInput = false;
        }
        public void resetCorrectDD()
        {
            transform.parent = origParent;
            thisRigidBody.isKinematic = true;
            thisRigidBody.useGravity = false;
            boxCollider.enabled = true;

            draggableText.transform.SetParent(origParent, true);
            draggableText.transform.localPosition = new Vector3(-0.5f, 0.5f, 0);
            draggableText.transform.localEulerAngles = new Vector3(90, 0.0f, 90);
            draggableText.transform.localScale = Vector3.one;

            boxCollider.size = new Vector3(1, 1, 0.75f);
            boxCollider.isTrigger = true;
            transform.localEulerAngles = origLocalRotation;

            collidedWithVase = touchedVase = false;

            if (game.wrongDDsOnLL() > 0)
                game.disableInput = false;
        }

        void OnTriggerEnter(Collider other)
        {
            Setmarker(other, true);
            if (isCorrect && !isDragging)
                checkDDsOverlapping(other);
        }

        void OnTriggerStay(Collider other)
        {
            Setmarker(other, true);
            if (isCorrect && !isDragging)
            {
                checkDDsOverlapping(other);
            }
        }

        void OnTriggerExit(Collider other)
        {
            Setmarker(other, false);
        }

        void OnCollisionEnter(Collision coll)
        {
            if (coll.gameObject.tag == "Marker")
            {
                touchedVase = true;
            }
            if (coll.gameObject.tag == "Obstacle")
            {
                poofOnCollision(coll);
            }
        }

        void OnCollisionStay(Collision coll)
        {
            if (deattached)
                poofOnCollision(coll);
        }

        void checkDDsOverlapping(Collider coll)
        {
            SickLettersDraggableDD dd = coll.gameObject.GetComponent<SickLettersDraggableDD>();
            if (dd && dd.checkDDCollision && !dd.isCorrect && !dd.isDragging && dd.transform.parent)
                foreach (Transform t in game.safeDropZones)
                    if (t.childCount == 0)
                    {
                        dd.transform.parent = t;
                        dd.transform.localPosition = Vector3.zero;
                        break;
                    }
        }

        void poofOnCollision(Collision coll)
        {
            if (coll.gameObject.tag == "Obstacle")
            {
                poofDD();
            }
        }

        public void poofDD(float delay = 0)
        {
            StartCoroutine(coPoof(delay));
        }

        IEnumerator coPoof(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (!this)
                yield break;

            game.Poof(transform);

            if (game.roundsCount == 0 && !isInVase)
            {
                if (isCorrect)
                    resetCorrectDD();
                else
                    resetWrongDD();

                game.onWrongMove(isCorrect);
                game.tut.doTutorial();
                yield break;
            }

            if (!isInVase)
            {
                game.onWrongMove(isCorrect);
            }

            if (isCorrect)
            {
                resetCorrectDD();
                StartCoroutine(game.scale.onDroppingCorrectDD());
            }
            else
            {
                if (!deattached)
                {
                    deattached = true;
                    game.checkForNextRound();
                }

                Destroy(gameObject, 0.0f);
            }
        }

        void shakeTransform(Transform t, Vector2 startPose)
        {
            t.position = new Vector3(startPose.x + Mathf.Sin(Time.time * 20f) / 10, t.position.y, t.position.z);
        }

        IEnumerator offToON()
        {
            boxCollider.enabled = false;
            yield return new WaitForSeconds(.1f);
            boxCollider.enabled = true;
        }

        private Vector2 emptyZone;
        public void SetEmptyZone(Vector2 emptyZone)
        {
            this.emptyZone = emptyZone;
            ApplyEmptyZone();
        }

        public void ApplyEmptyZone()
        {
            float scaleMultiplier = 0.2f;
            var transform1 = transform;
            transform1.localPosition = new Vector3(-emptyZone.y * scaleMultiplier + 0.2f, 0, emptyZone.x * scaleMultiplier);
            transform1.position += Vector3.forward * -0.9f;  // A bit to the front so they are always visible
            transform1.localEulerAngles = new Vector3(0, -90, 0);
        }
    }

}
