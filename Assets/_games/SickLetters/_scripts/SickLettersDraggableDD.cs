using UnityEngine;
using System.Collections;
using TMPro;

namespace Antura.Minigames.SickLetters
{
	public class SickLettersDraggableDD : MonoBehaviour {

		private Vector3 screenPoint;
		private Vector3 offset;

        public SickLettersGame game;
		public bool isDot, deattached;
		[Range (0, 3)] public int dots;

		public Diacritic diacritic;

		public Vector3 fingerOffset;
		public TextMeshPro draggableText;

        public bool isCorrect;
        public bool isNeeded = false, isInVase = false, touchedVase = false, collidedWithVase;

        public bool isDragging = false;

		bool overPlayermarker = false;
        bool shake = false;
        bool release = false;

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
            //Reset();
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

			

            //origParent = transform.parent;
            //origLocalRotation = transform.localEulerAngles;
            

            transform.parent = null;

            if (isCorrect)
            {
                if(game.roundsCount > 0)
                    game.wrongDraggCount++;
                shake = true;
                //draggableText.transform.parent = transform;
                draggableText.transform.SetParent(transform, true);
            }

            else
                offset = gameObject.transform.position - 
				    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

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
			transform.position = new Vector3 (curPosition.x + fingerOffset.x, curPosition.y + fingerOffset.y, curPosition.z+ fingerOffset.z);

		}

		void OnMouseUp()
        {
            if (game.disableInput)
                return;
            releaseDD();
        }


        bool destroy;
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
                //if (!isTouchingVase)
                {
                    thisRigidBody.isKinematic = false;
                    thisRigidBody.useGravity = true;
                    boxCollider.isTrigger = false;
                }

                boxCollider.center = Vector3.zero;
                boxCollider.size = new Vector3(0.1f, 0.25f, 0.1f);

                //if (isTouchingVase)
                //  game.scale.addNewDDToVas(this);

                //if (game.scale.vaseCollider.bounds.Contains(transform.position))

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

        /*public void setInitPos(Vector3 initPos)
        {
            //startX = initPos.x;
            //startY = initPos.y;
            //startZ = initPos.z;
        }*/

        
			
		void Update() {
  
            if (shake && game.wrongDraggCount <= 1)
                shakeTransform(game.scale.transform, 20, 10, game.scale.vaseStartPose);
        }


        void Setmarker(Collider other, bool markerStatus)
        {
            if (other.tag == "Player")
                overPlayermarker = markerStatus;
		}
        public void resetWrongDD()
        {
            transform.parent = origParent;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = origLocalRotation;
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

            //draggableText.transform.parent = origParent;
            draggableText.transform.SetParent(origParent, true);
            draggableText.transform.localPosition = new Vector3(-0.5f, 0.5f,0);
            draggableText.transform.localEulerAngles = new Vector3(90, 0.0f, 90);
            draggableText.transform.localScale = Vector3.one;

            boxCollider.size = new Vector3(1, 1, 0.75f); //(1,1,1.21f);
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
            //Debug.LogError(coll.gameObject.name);
            if (coll.gameObject.tag == "Marker")
            {
                touchedVase = true;
            }
            if (coll.gameObject.tag == "Obstacle")
            {
                
                poofOnCollision(coll);
            }
            /*else if (!isDragging &&!isInVase && coll.gameObject.tag == "Finish")
            {
                game.scale.addNewDDToVas(this);
                thisRigidBody.isKinematic = true;
                
            }*/
        }
        void OnCollisionStay(Collision coll)
        {
            if(deattached)
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

        IEnumerator coPoof(float delay) {
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

        void shakeTransform(Transform t, float speed, float amount, Vector2 startPose)
        {
            t.position = new Vector3(startPose.x + Mathf.Sin(Time.time * 20f) / 10, t.position.y, t.position.z);

        }

        IEnumerator offToON()
        {
            //yield return new WaitForSeconds(.25f);
            boxCollider.enabled = false;
            yield return new WaitForSeconds(.1f);
            boxCollider.enabled = true;
        }
    }

}
