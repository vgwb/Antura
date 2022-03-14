using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Antura.LivingLetters;

namespace Antura.Minigames.TakeMeHome
{
    public class TakeMeHomeLL : MonoBehaviour
    {

        public GameObject plane;
        public bool isDragged;
        public bool isMoving;
        private bool isPanicing, isFallingFromTube;
        public bool isDraggable;

        public Transform livingLetterTransform;
        public BoxCollider boxCollider;

        public LivingLetterController letter;

        Tweener moveTweener;
        Tweener rotationTweener;

        Vector3 holdPosition;
        Vector3 normalPosition;
        bool isResetting = false;

        private float cameraDistance;

        float maxY;

        bool dropLetter;
        bool clampPosition;
        public bool dragging = false;
        Vector3 dragOffset = Vector3.zero;
        Vector3 tubeSpawnPosition, ContentOffset;
        public bool respawn;

        public event Action onMouseUpLetter;

        Action endTransformToCallback;


        public List<TakeMeHomeTube> collidedTubes;

        [HideInInspector]
        public TakeMeHomeTube lastCollidedTube;

        void Awake()
        {
            normalPosition = transform.localPosition;
            livingLetterTransform = transform;
            holdPosition.x = normalPosition.x;
            isMoving = false;
            isDraggable = false;
            holdPosition.y = normalPosition.y;
            //lastTube = null;
            respawn = true;
            isResetting = false;
            isPanicing = false;
            collidedTubes = new List<TakeMeHomeTube>();

        }

        void Start()
        {
            ContentOffset = letter.contentTransform.position - transform.position;
        }

        public void Initialize(float _maxY, LivingLetterController _letter, Vector3 tubePosition)
        {
            tubeSpawnPosition = tubePosition - Vector3.up * 4;

            cameraDistance = (transform.position.z) - Camera.main.transform.position.z;

            //cameraDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
            letter = _letter;
            maxY = _maxY;

            dropLetter = false;
            isResetting = false;
            clampPosition = false;

        }

        public void PlayIdleAnimation()
        {
            letter.SetState(LLAnimationStates.LL_idle);

            //livingLetterTransform.localPosition = normalPosition;
        }

        public void PlayWalkAnimation()
        {
            letter.SetState(LLAnimationStates.LL_walking);
            letter.SetWalkingSpeed(LivingLetterController.WALKING_SPEED);

            //livingLetterTransform.localPosition = normalPosition;
        }

        public void PlayHoldAnimation()
        {
            letter.SetState(LLAnimationStates.LL_dragging);

            //livingLetterTransform.localPosition = holdPosition;
        }



        public void MoveTo(Vector3 position, float duration)
        {
            isMoving = true;
            PlayWalkAnimation();

            transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

            if (moveTweener != null)
            {
                moveTweener.Kill();
            }

            moveTweener = transform.DOLocalMove(position, duration).OnComplete(delegate ()
            {
                PlayIdleAnimation();
                if (endTransformToCallback != null)
                    endTransformToCallback();

                //play audio
                //TakeMeHomeConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(letter.Data, true);
                RotateTo(new Vector3(0, 180, 0), 0.5f);
                isMoving = false;
            });
        }

        public void sayLetter()
        {
            TakeMeHomeConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(letter.Data, true);
        }

        public void MoveBy(Vector3 position, float duration)
        {
            MoveTo(transform.position + position, duration);
        }


        void RotateTo(Vector3 rotation, float duration)
        {
            if (rotationTweener != null)
            {
                rotationTweener.Kill();
            }

            rotationTweener = transform.DORotate(rotation, duration);
        }

        void TransformTo(Transform transformTo, float duration, Action callback)
        {
            MoveTo(transformTo.localPosition, duration);
            RotateTo(transformTo.eulerAngles, duration);

            endTransformToCallback = callback;
        }







        public void OnPointerDown(Vector2 pointerPosition)
        {
            if (isMoving || !isDraggable)
                return;

            if (!dragging)
            {
                // if (lastTube) lastTube.deactivate();
                //lastTube = null;
                dragging = true;

                var data = letter.Data;

                TakeMeHomeConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(data, true);

                Vector3 mousePosition = new Vector3(pointerPosition.x, pointerPosition.y, cameraDistance);
                Vector3 world = Camera.main.ScreenToWorldPoint(mousePosition);
                dragOffset = world - transform.position;

                OnPointerDrag(pointerPosition);

                PlayHoldAnimation();
            }
        }

        Vector3 mousePos;

        public void OnPointerDrag(Vector2 pointerPosition)
        {
            if (dragging)
            {
                dropLetter = false;

                Vector3 mousePosition = new Vector3(pointerPosition.x, pointerPosition.y, cameraDistance);

                /*transform.position =*/
                mousePos = Camera.main.ScreenToWorldPoint(mousePosition);

                /*transform.position =*/
                ClampPositionToStage(transform.position - dragOffset);
            }
        }

        public void OnPointerUp()
        {
            if (dragging)
            {
                dragging = false;
                dropLetter = true;


                //check if position should clamp:
                if (transform.position.x > 5.4f)// && transform.position.y > maxY)
                    clampPosition = true;

                PlayIdleAnimation();

                if (onMouseUpLetter != null)
                {
                    onMouseUpLetter();
                }


            }
        }

        void Drop(float delta)
        {
            Vector3 dropPosition = transform.position;

            dropPosition += Physics.gravity * delta;


            if (clampPosition)
                transform.position = ClampPositionToStage(dropPosition);
            else
                transform.position = dropPosition;//ClampPositionToStage(dropPosition);

            //free fall:
            if (!clampPosition)
            {
                if (respawn && transform.position.y < (maxY - 20))
                {
                    TakeMeHomeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Splat);
                    //transform.position =
                    isPanicing = false;

                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    transform.position = tubeSpawnPosition;
                    clampPosition = true;
                    transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    transform.DOScale(0.8f, 0.5f);
                    StartCoroutine(playLandinganimation(0.25f));

                }
            }

            if (isFallingFromTube)
            {
                StartCoroutine(playLandinganimation(0.25f));
                isFallingFromTube = false;
            }
        }

        IEnumerator playLandinganimation(float delay)
        {

            letter.GetComponent<Animator>().Play("LL_fall_down");
            letter.Falling = true;
            yield return new WaitForSeconds(delay);
            letter.Falling = false;
            letter.OnJumpEnded();

            letter.GetComponent<Animator>().CrossFade("LL_land", 0.3f);

        }

        void Update()
        {
            if (dropLetter)
            {
                Drop(Time.deltaTime);
            }

            Vector3 targetScale;
            Vector3 targetPosition = transform.position;

            if (lastCollidedTube != null)
            {
                float yScale = 1.3f * (1 + 0.1f * Mathf.Cos(Time.realtimeSinceStartup * 3.14f * 6));

                targetScale = 0.55f * new Vector3(1 / yScale, yScale, 1);

                if (Vector3.Distance(lastCollidedTube.transform.position, mousePos) > 4.5f)
                    targetPosition = mousePos - ContentOffset + Vector3.up * 0.9f;
                else
                {
                    targetPosition = lastCollidedTube.enterance.position;
                    //targetPosition.y = Mathf.Lerp(targetPosition.y, maxY, 2.0f * Mathf.Abs(targetPosition.x - transform.position.x));
                }

            }
            else
            {
                targetScale = Vector3.one * 0.8f;
                if (dragging)
                    targetPosition = mousePos - ContentOffset + Vector3.up * 0.9f;
            }
            if (!dragging)
                return;

            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 15.0f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition, 25.0f * Time.deltaTime);
        }

        Vector3 ClampPositionToStage(Vector3 unclampedPosition)
        {
            Vector3 clampedPosition = unclampedPosition;



            if (!dragging)
                clampedPosition.y = clampedPosition.y < maxY ? maxY : clampedPosition.y;

            if (clampedPosition.y == maxY)
            {
                dropLetter = false;
                clampPosition = false;
                isResetting = false;

            }

            return clampedPosition;
        }

        private void moveUp()
        {
            if (collidedTubes.Count == 0)
                return;
            //if (lastTube == null)
            //	return;

            if (moveTweener != null)
            {
                moveTweener.Kill();
            }
            isResetting = true;
            transform.DOScale(0.1f, 0.1f);

            Vector3 targetPosition = collidedTubes[collidedTubes.Count - 1].transform.Find("Cube").position;
            ;// lastTube.transform.FindChild("Cube").position;
            collidedTubes[collidedTubes.Count - 1].deactivate(this);


            moveTweener = transform.DOLocalMove(targetPosition/*transform.position + lastTube.transform.up*5 + new Vector3(0,0,20)*/, 0.3f).OnComplete(delegate ()
            {
                //PlayIdleAnimation();
                if (endTransformToCallback != null)
                    endTransformToCallback();

                /*if(lastTube != null)
                {
                    lastTube.hideWinParticles();
                }*/


                //lastTube = null;

                foreach (TakeMeHomeTube tube in collidedTubes)
                {
                    tube.deactivate(this);
                    tube.hideWinParticles();
                }
                collidedTubes.Clear();

                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                transform.position = tubeSpawnPosition;
                transform.DOScale(0.8f, 0.5f);
                clampPosition = true;
                dropLetter = true;
                isMoving = false;
                //Drop(Time.deltaTime);

            });
        }

        public void panicAndRun()
        {
            //wait few milliseconds then move:
            if (isPanicing)
                return;
            isPanicing = true;
            isMoving = true;
            isDraggable = false;
            dropLetter = false;

            // StartCoroutine(waitForSeconds(1, ()=> {

            RotateTo(new Vector3(0, -90, 0), 0.5f);


            letter.SetState(LLAnimationStates.LL_walking);
            letter.SetWalkingSpeed(LivingLetterController.RUN_SPEED);

            if (moveTweener != null)
            {
                moveTweener.Kill();
            }

            moveTweener = transform.DOLocalMove(new Vector3(5.2f, -3.44f, -15), 0.5f).OnComplete(delegate ()
            {

                PlayIdleAnimation();
                respawn = false;
                clampPosition = false;



                dropLetter = true;

                isMoving = false;
            });

            //  }));



        }


        public void followTube(bool win)
        {

            if (isResetting)
                return;

            Debug.Log("following tube");
            isResetting = true;
            isMoving = true;
            isDraggable = false;
            dropLetter = false;
            isFallingFromTube = true;

            moveUp();

            if (win)
            {
                //lastTube.showWinParticles();
                if (collidedTubes.Count > 1)
                {
                    collidedTubes[collidedTubes.Count - 1].showWinParticles();
                }
            }

            //else
            //  StartCoroutine(playLandinganimation(.5f));







        }

        IEnumerator waitForSeconds(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();

        }

        IEnumerator waitForSecondsAndJump(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            letter.SetState(LLAnimationStates.LL_walking);
            letter.SetWalkingSpeed(LivingLetterController.RUN_SPEED);

            if (moveTweener != null)
            {
                moveTweener.Kill();
            }

            moveTweener = transform.DOLocalMove(transform.position - (new Vector3(5, 0, 0)), 1).OnComplete(delegate ()
            {

                clampPosition = false;
                transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                transform.DOScale(0.8f, 0.5f);
                dropLetter = true;
                isMoving = false;
            });
        }


        public void EnableCollider(bool enable)
        {
            boxCollider.enabled = enable;
        }

        void OnTriggerEnter(Collider other)
        {
            if (isResetting)
                return;

            TakeMeHomeTube tube = other.gameObject.GetComponent<TakeMeHomeTube>();
            if (!tube || collidedTubes.IndexOf(tube) != -1)
                return;

            if (collidedTubes.Count > 0)
                collidedTubes[collidedTubes.Count - 1].deactivate(this);

            lastCollidedTube = tube;
            collidedTubes.Add(tube);
            tube.activate(this);

            /*
                        if (!dragging) {
                            if (lastTube) lastTube.deactivate();
                            lastTube = null;
                            return;
                        }

                        TakeMeHomeTube tube = other.gameObject.GetComponent<TakeMeHomeTube> ();
                        if (!tube)
                            return;
                        Debug.Log("entering tube: " + tube.gameObject.name);
                        if (lastTube) lastTube.deactivate();
                        lastTube = tube;


                        tube.shake ();
                        tube.activate();*/
        }




        void OnTriggerExit(Collider other)
        {
            TakeMeHomeTube tube = other.gameObject.GetComponent<TakeMeHomeTube>();
            if (!tube)
                return;
            lastCollidedTube = null;
            popTube(tube);

            /*
                        Debug.Log("exiting: " + other.gameObject.name);
                       // if (!isDraggable) return;

                        TakeMeHomeTube tube = other.gameObject.GetComponent<TakeMeHomeTube> ();
                        if(tube)
                        {
                            tube.deactivate();
                            Debug.Log("exiting tube: " + tube.gameObject.name);
                        }



                        if (!tube || lastTube != tube)
                            return;
                        if (lastTube) lastTube.deactivate();
                        lastTube = null;*/
        }

        void popTube(TakeMeHomeTube tube)
        {
            tube.deactivate(this);
            collidedTubes.Remove(tube);

            if (collidedTubes.Count > 0)
                collidedTubes[collidedTubes.Count - 1].activate(this);
        }
    }

}
