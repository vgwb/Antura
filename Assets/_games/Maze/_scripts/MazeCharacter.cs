using System;
using Antura.LivingLetters;
using Antura.Minigames.Tobogan;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Helpers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Antura.Minigames.Maze
{
    public delegate void VoidDelegate();

    public class MazeCharacter : MonoBehaviour
    {
        private const float VERTICAL_DISTANCE_FROM_CAMERA = 0.2f;
        private const float MIN_XZ_DISTANCE_FROM_CAMERA = 1f;
        private const float MAX_XZ_DISTANCE_FROM_CAMERA = 2f;

        private const float CELEBRATION_PATH_MIDPOINT_X_ANCHOR = 0.2f;
        private const float CELEBRATION_PATH_MIDPOINT_Z_ANCHOR = -0.2f;

        private const float CELEBRATION_PATH_ENDPOINT_X_ANCHOR = -0.5f;
        private const float CELEBRATION_PATH_ENDPOINT_Z_ANCHOR = 0f;
        private const float CELEBRATION_PATH_ENDPOINT_DISTANCE_FROM_CAMERA = 6.5f;
        private const float CELEBRATION_PATH_DURATION = 2.5f;

        private const float FLEE_PATH_MIDPOINT_X_ANCHOR = -0.6f;
        private const float FLEE_PATH_MIDPOINT_Z_ANCHOR = 0.2f;

        private const float FLEE_PATH_ENDPOINT_X_ANCHOR = 0.33f;
        private const float FLEE_PATH_ENDPOINT_Z_ANCHOR = -0.33f;
        private const float FLEE_PATH_ENDPOINT_DISTANCE_FROM_CAMERA = 5f;
        private const float FLEE_PATH_DURATION = 2.5f;

        private const float DELAY_TO_PRONOUNCE_LETTER = 0.1f;
        private const float DELAY_BETWEEN_LETTER_SOUND_AND_CHECKMARK = 0.2f;
        private const float DELAY_BETWEEN_CHECKMARK_AND_EXIT = 1.33f;

        private enum LLState
        {
            Normal, Braked, Impacted, Ragdolling
        }

        private LLState _state;
        private LLState State
        {
            get
            {
                return _state;
            }

            set
            {
                if (_state != value)
                {
                    _state = value;

                    switch (_state)
                    {
                        case LLState.Ragdolling:
                            ragdoll.SetRagdoll(true, rocket.GetComponent<Rigidbody>().velocity);

                            foreach (Collider collider in ragdoll.GetComponentsInChildren<Collider>())
                            {
                                collider.enabled = true;
                            }
                            break;
                    }
                    stateTime = 0f;
                }
            }
        }
        private float stateTime;

        public enum LoseState
        {
            None, OutOfBounds, Incomplete
        }

        public LoseState loseState;

        public List<Vector3> characterWayPoints;

        public LivingLetterController LL;
        private Transform LLParent;

        public MeshCollider myCollider;
        public List<GameObject> particles;

        public List<GameObject> Fruits;

        public bool characterIsMoving;

        public MazeDot dot = null;

        public Transform nextPosition;

        public Vector3 initialPosition;
        public Quaternion initialRotation;
        private Vector3 targetPos;

        public List<GameObject> _fruits;

        public int CurrentStrokeIndex => currentFruitList;
        public int currentFruitList = 0;

        public int startFruitIndex;

#pragma warning disable 0219
#pragma warning disable 0414
        private bool startCheckingForCollision = false;
        public bool donotHandleBorderCollision = false;
#pragma warning restore 0414
#pragma warning restore 0219

        public bool isFleeing = false;

        public bool isAppearing = false;
        public GameObject rocket;

        private GameObject blinkingTarget;

        private MazeLetter mazeLetter;

        public GameObject winParticleVFX;

        public LivingLetterRagdoll ragdoll;

        private IAudioSource rocketMoveSFX;

        private bool showedCheckmarkUponVictory = false;
        private System.Action OnMarkStamp;

        private IAudioSource letterPronounciation;
        private bool pronouncedLetter = false;
        private Tweener celebrationPathTweener;
        private Tweener brakeYoyoTweener;
        private bool markedEndTimeOfLetterPronounciation = false;
        private float endTimeOfLetterPronounciation;

        private Vector3 raycastCheckTarget;

        public bool finishedRound;

        public void SetMazeLetter(MazeLetter mazeLetter)
        {
            this.mazeLetter = mazeLetter;
        }

        void Start()
        {
            LL.SetState(LLAnimationStates.LL_rocketing);

            LLParent = ragdoll.transform.parent;

            isFleeing = false;
            characterIsMoving = false;
            characterWayPoints = new List<Vector3>();

            GetComponent<Collider>().enabled = false;

            foreach (Collider _collider in rocket.GetComponentsInChildren<Collider>())
            {
                _collider.enabled = false;
            }
        }

        private float GetFrustumHeightAtDistance(float distanceFromCamera)
        {
            return 2.0f * distanceFromCamera * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }

        private float GetFrustumWidth(float frustumHeight)
        {
            return frustumHeight * Camera.main.aspect;
        }

        public void SpawnOffscreen()
        {
            var frustumHeight = GetFrustumHeightAtDistance(VERTICAL_DISTANCE_FROM_CAMERA);
            var frustumWidth = GetFrustumWidth(frustumHeight);

            var xDisplacement = Random.Range(MIN_XZ_DISTANCE_FROM_CAMERA, MAX_XZ_DISTANCE_FROM_CAMERA);
            xDisplacement *= -1f;

            var zDisplacement = Random.Range(MIN_XZ_DISTANCE_FROM_CAMERA, MAX_XZ_DISTANCE_FROM_CAMERA);
            zDisplacement *= -1f;

            var cameraPosition = Camera.main.transform.position;

            Vector3 startPoint = new Vector3(cameraPosition.x + (frustumWidth / 2 * Mathf.Sign(xDisplacement)) + xDisplacement,
                                                cameraPosition.y - VERTICAL_DISTANCE_FROM_CAMERA, cameraPosition.z + (frustumHeight / 2 * Mathf.Sign(zDisplacement)) + zDisplacement);

            transform.position = startPoint;
        }

        public void ToggleParticlesVisibility(bool value)
        {
            foreach (GameObject particle in particles)
            { particle.SetActive(value); }
        }

        private void ResetRocket()
        {
            var rocketRigidBody = rocket.GetComponent<Rigidbody>();
            rocketRigidBody.isKinematic = true;
            rocketRigidBody.useGravity = false;
            rocketRigidBody.velocity = Vector3.zero;
            rocketRigidBody.angularVelocity = Vector3.zero;
            rocket.transform.localPosition = Vector3.zero;
            rocket.transform.localRotation = Quaternion.Euler(Vector3.zero);
            rocket.GetComponent<SphereCollider>().enabled = false;
        }

        private void ResetLivingLetter()
        {
            ragdoll.transform.parent = LLParent;
            ragdoll.SetRagdoll(false, Vector3.zero);

            ragdoll.transform.localPosition = Vector3.zero;
            ragdoll.transform.localRotation = Quaternion.Euler(Vector3.zero);

            foreach (Collider _collider in ragdoll.GetComponentsInChildren<Collider>())
            {
                _collider.enabled = false;
            }

            LL.SetState(LLAnimationStates.LL_rocketing);
        }

        private void Reset()
        {
            ResetRocket();
            ResetLivingLetter();

            loseState = LoseState.None;

            myCollider.enabled = true;

            GetComponent<CapsuleCollider>().enabled = true;
        }

        public void initialize()
        {
            Reset();

            initialPosition = transform.position;
            targetPos = initialPosition;

            initialRotation = transform.rotation;

            characterWayPoints.Add(initialPosition);
            SetFruitsList();

            AnimateWaitingForward();
            raycastCheckTarget = transform.position;
            raycastCheckTarget.y = TrackBounds.instance.transform.position.y;

            myCollider.enabled = true;
            finishedRound = false;
        }

        public void AnimateWaitingForward()
        {
            var startRotation = _fruits[startFruitIndex].transform.rotation.eulerAngles;
            transform.DORotate(startRotation, 0.5f).OnComplete(() =>
            {
                transform.DOMove(transform.position - transform.TransformVector(Vector3.forward), 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            });
        }

        public void CreateFruits(List<GameObject> fruitsLists)
        {
            foreach (GameObject fruitsList in fruitsLists)
            {
                for (int i = 0; i < fruitsList.transform.childCount; i++)
                {
                    Transform child = fruitsList.transform.GetChild(i);

                    var arrow = child.gameObject.GetComponent<MazeArrow>();
                    child.gameObject.name = $"fruit_{(i)}";

                    arrow.arrowMesh = child.GetComponentInChildren<MeshRenderer>();
                }
            }

            Fruits = fruitsLists;
        }

        private void SetFruitsList()
        {
            if (Fruits.Count == 0)
            {
                return;
            }

            // Fruits to collect:
            _fruits = new List<GameObject>();

            for (int i = 0; i < Fruits[currentFruitList].transform.childCount; i++)
            {
                GameObject child = Fruits[currentFruitList].transform.GetChild(i).gameObject;
                MazeArrow mazeArrow = child.gameObject.GetComponent<MazeArrow>();

                if (i == startFruitIndex)
                {
                    mazeArrow.HighlightAsLaunchPosition();
                }
                else if (i > startFruitIndex)
                {
                    mazeArrow.Reset();
                }

                _fruits.Add(child);
            }

            foreach (GameObject fruit in _fruits)
            {
                fruit.GetComponent<BoxCollider>().enabled = true;
            }

            MazeGame.instance.RefreshFruitColliderSizes(startFruitIndex);
        }

        void OnTriggerEnter(Collider other)
        {
            if (donotHandleBorderCollision || !characterIsMoving)
            {
                return;
            }

            //print("Colliding with: " + other.gameObject.name);

            // DEPRECATED: We now do not care about the character hitting fruits
            /*if (other.gameObject.name.IndexOf("fruit_") == 0)
            {
                other.enabled = false;

                //we hit a fruit make sure it is in order:
                int index = int.Parse(other.gameObject.name.Substring(6));

                if (index == 0)
                {
                    Debug.LogWarning("Character Hit fruit " + index);
                    return;
                }
                else if (index == currentFruitIndex)
                {
                    //lerp
                    _fruits[currentFruitIndex].GetComponent<MazeArrow>().pingPong = false;
                    _fruits[currentFruitIndex].GetComponent<MazeArrow>().tweenToColor = true;

                    currentFruitIndex++;
                    Debug.LogWarning("Character Hit fruit " + currentFruitIndex);
                    MazeGame.instance.RefreshFruitColliderSizes(currentFruitIndex);

                    if (index == 0)
                    {
                        if (blinkingTarget != null)
                        {
                            Destroy(blinkingTarget);
                            blinkingTarget = null;
                        }
                    }
                }
            }*/
        }

        public void waitAndRestartScene()
        {
            //if (particles) particles.SetActive(false);
            foreach (GameObject particle in particles)
                particle.SetActive(false);
            //stop for a second and restart the level:
            StartCoroutine(waitAndPerformCallback(3, () =>
            {
                donotHandleBorderCollision = true;
                characterIsMoving = false;
                transform.DOKill(false);
                ToggleParticlesVisibility(true);

                MazeGame.instance.ColorCurrentLinesAsIncorrect();

            },
                () =>
                {
                    MazeGame.instance.lostCurrentLetter();
                }));
        }

        //corutine to handle pausing a bit then resuming
        IEnumerator waitAndPerformCallback(float seconds, VoidDelegate init, VoidDelegate callback)
        {
            init();

            yield return new WaitForSeconds(seconds);

            callback();
        }

        public bool isComplete()
        {
            if (currentFruitList == Fruits.Count - 1)
            {
                if (dot == null)
                {
                    return true;
                }
                else
                {
                    return dot.isClicked;
                }
            }
            else
            {
                return false;
            }
        }

        public void setClickedDot()
        {
            ToggleParticlesVisibility(false);
            MazeGame.instance.moveToNext(true);
        }

        public void nextPath()
        {
            if (currentFruitList == Fruits.Count - 1)
            {
                return;
            }

            transform.parent.Find("MazeLetter").GetComponent<MazeLetter>().isDrawing = false;
            currentFruitList++;

            reachedFruitIndex = 0;
            startFruitIndex = 0;
            SetFruitsList();

            Vector3 initPos = _fruits[0].transform.position + new Vector3(0f, 0.6f, 0f);

            initialPosition = initPos;
            targetPos = initialPosition;

            initialRotation = transform.rotation;

            characterWayPoints = new List<Vector3>();
            characterWayPoints.Add(initialPosition);

            transform.LookAt(_fruits[0].transform.position + new Vector3(0f, 0.6f, 0f));

            ToggleParticlesVisibility(true);
            transform.DOMove(_fruits[0].transform.position + new Vector3(0f, 0.6f, 0f), 1).OnComplete(() =>
            {
                ToggleParticlesVisibility(false);

                var firstArrowRotation = _fruits[0].transform.rotation.eulerAngles;
                transform.DORotate(firstArrowRotation, 0.5f).OnComplete(() =>
                {
                    transform.DOMove(transform.position - transform.TransformVector(Vector3.forward), 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                });
            });

            myCollider.enabled = true;
            finishedRound = false;
        }

        public void resetToCurrent()
        {
            //Debug.LogError("RETURNING TO FRUIT INDEX " + startFruitIndex);
            reachedFruitIndex = startFruitIndex;

            transform.DOKill(false);
            donotHandleBorderCollision = false;
            transform.parent.Find("MazeLetter").GetComponent<MazeLetter>().isDrawing = false;
            transform.position = _fruits[startFruitIndex].transform.position + new Vector3(0f, 0.6f, 0f);

            initialPosition = transform.position;
            targetPos = initialPosition;

            initialRotation = transform.rotation;

            characterWayPoints = new List<Vector3>();
            characterWayPoints.Add(initialPosition);

            SetFruitsList();

            ToggleParticlesVisibility(false);

            var firstArrowRotation = _fruits[0].transform.rotation.eulerAngles;
            firstArrowRotation.x += 90f;
            firstArrowRotation.y += 90f;

            transform.DORotate(firstArrowRotation, 0.5f).OnComplete(() =>
            {
                transform.DOMove(transform.position - transform.TransformVector(Vector3.forward), 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            });

            myCollider.enabled = true;
            finishedRound = false;
            characterIsMoving = false;
            GetComponent<Collider>().enabled = false;
        }

        public bool canStartDrawing()
        {
            if (_fruits == null || MazeGame.instance.isShowingAntura)
            { return false; }

            if (_fruits.Count == 0)
            { return false; }

            float distance = Camera.main.transform.position.y;
            Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(distance));
            pos = Camera.main.ScreenToWorldPoint(pos);

            //float mag = (pos - _fruits[0].transform.position).sqrMagnitude;

            if (((pos - _fruits[startFruitIndex].transform.position).sqrMagnitude) <= 1)
            {
                MazeGame.instance.appendToLine(_fruits[startFruitIndex].transform.position);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MoveTween(bool wrong)
        {
            if (wrong)
            {
                // Average distance:
                float distance = 0;
                for (int i = 1; i < characterWayPoints.Count; ++i)
                    distance += (characterWayPoints[i] - characterWayPoints[i - 1]).sqrMagnitude;

                float time = distance * 2;
                if (time > 2)
                    time = 2;

                if (loseState == LoseState.OutOfBounds)
                {
                    time = 0.33f;
                }

                // Set initial angle:
                var characterWaypointsArray = characterWayPoints.ToArray();
                if (characterWayPoints.Count >= 2) transform.LookAt(characterWayPoints[1]);

                transform.DOPath(characterWaypointsArray, time, PathType.Linear, PathMode.Ignore, resolution: 2).OnWaypointChange((int index) =>
                {
                    if (index + 1 < characterWayPoints.Count)
                    {
                        transform.LookAt(characterWayPoints[index + 1]);
                    }
                }).OnComplete(OnPathMoveComplete);
            }
            else
            {
                StartCoroutine(FollowCorrectPathCO());
            }
        }

        private IEnumerator FollowCorrectPathCO()
        {
            var shapeData = MazeGame.instance.currentNewMazeLetter.GetComponent<NewMazeLetter>().shapeData;
            var spline = shapeData.Strokes[CurrentStrokeIndex].Spline;

            var originalParent = transform.parent;

            // Check start and end spline values based on current and reached
            var arrowStart = _fruits[startFruitIndex].GetComponent<MazeArrow>();
            var arrowEnd = _fruits[reachedFruitIndex].GetComponent<MazeArrow>();
            float t = 0f;
            var speed = 5f;
            var diff = arrowEnd.splineValue - arrowStart.splineValue;
            while (t < 1f)
            {
                t += Time.deltaTime * speed / diff;
                var lerp = Mathf.Lerp(arrowStart.splineValue, arrowEnd.splineValue, t);
                ShapeManager.PlaceObjectOnSpline(gameObject, arrowStart.transform.parent.gameObject, spline, lerp, 0.1f);
                yield return null;
            }

            transform.parent = originalParent;
            OnPathMoveComplete();
        }

        public static bool LOSE_ON_SINGLE_PATH = false;

        public bool HasCompletedPath => reachedFruitIndex == _fruits.Count - 1;

        private void OnPathMoveComplete()
        {
            if (loseState == LoseState.None && !LOSE_ON_SINGLE_PATH && !HasCompletedPath)
            {
                characterWayPoints.Clear();

                startFruitIndex = reachedFruitIndex;

                Debug.LogWarning("Path not completed yet (current " + reachedFruitIndex + " / " + (_fruits.Count-1) + ")");

                // Wait for the player to finish
                characterIsMoving = false;
                LL.SetState(LLAnimationStates.LL_rocketing);
                AnimateWaitingForward();

                Fruits[currentFruitList].transform.GetChild(startFruitIndex).GetComponentInChildren<MazeArrow>().HighlightAsLaunchPosition();
                //MazeGame.instance.currentNewMazeLetter.GetComponent<NewMazeLetterBuilder>().AddDotAndHideArrow(_fruits[startFruitIndex].transform);
                return;
            }

            finishedRound = true;

            transform.parent.Find("MazeLetter").GetComponent<MazeLetter>().isDrawing = false;

            //arrived!
            //transform.rotation = initialRotation;
            if (HasCompletedPath)
            {
                startFruitIndex = reachedFruitIndex;

                Debug.LogWarning("Path COMPLETED!");

                // if (particles) particles.SetActive(false);
                foreach (GameObject particle in particles)
                    particle.SetActive(false);
                GetComponent<Collider>().enabled = false;
                characterIsMoving = false;
                ToggleParticlesVisibility(false);
                transform.DOKill(false);
                MazeGame.instance.moveToNext(true);

                if (currentFruitList == Fruits.Count - 1)
                {
                    if (dot != null)
                    {
                        dot.GetComponent<BoxCollider>().enabled = true;
                    }
                }
            }
            else
            {
                if (loseState != LoseState.OutOfBounds)
                {
                    for (int i = startFruitIndex+1; i < _fruits.Count; i++)
                    {
                        _fruits[i].GetComponent<MazeArrow>().MarkAsUnreached(i == startFruitIndex+1);
                    }

                    Vector3 direction = _fruits[startFruitIndex+1].transform.position - rocket.transform.position;
                    Vector3 rotatedVector = direction;
                    var piOverTwo = Mathf.PI / 2;
                    rotatedVector.x = direction.x * Mathf.Cos(piOverTwo) - direction.z * Mathf.Sin(piOverTwo);
                    rotatedVector.z = direction.x * Mathf.Sin(piOverTwo) + direction.z * Mathf.Cos(piOverTwo);
                    rotatedVector.y = 0f;
                    rotatedVector.Normalize();
                    rotatedVector *= 1.5f;
                    rotatedVector.y = 2f;

                    Tutorial.TutorialUI.MarkNo((_fruits[startFruitIndex+1].transform.position + rocket.transform.position) / 2 + rotatedVector, Tutorial.TutorialUI.MarkSize.Normal);

                    rocketMoveSFX.Stop();

                    MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.KO);

                    if (!MazeGame.instance.isTutorialMode)
                    {
                        MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Lose);
                    }

                    loseState = LoseState.Incomplete;
                }
                else
                {
                    OnRocketImpactedWithBorder();
                }

                //Debug.LogError("LOSING WITH START " + startFruitIndex + " REACHED " + reachedFruitIndex);

                MazeGame.instance.OnLoseLife();
            }
        }

        public void HighlightStartFruit()
        {
            _fruits[startFruitIndex].GetComponent<MazeArrow>().Unhighlight();
            _fruits[startFruitIndex].GetComponent<MazeArrow>().HighlightAsReached();

            MazeGame.instance.currentNewMazeLetter.GetComponent<NewMazeLetterBuilder>().HideDotAndShowArrow(_fruits[startFruitIndex].transform);

            MazeGame.instance.RefreshFruitColliderSizes(startFruitIndex+1);
        }

        private void OnRocketImpactedWithBorder()
        {
            GetComponent<CapsuleCollider>().enabled = false;
            myCollider.enabled = false;
            mazeLetter.GetComponent<BoxCollider>().enabled = false;

            ragdoll.transform.SetParent(rocket.transform, true);

            rocket.GetComponent<SphereCollider>().enabled = true;

            var rocketRigidBody = rocket.GetComponent<Rigidbody>();
            rocketRigidBody.isKinematic = false;
            rocketRigidBody.useGravity = true;

            var rocketRotation = rocket.transform.rotation.eulerAngles.y;

            var velocity = new Vector3(Mathf.Sin(rocketRotation * Mathf.Deg2Rad), 0f, Mathf.Cos(rocketRotation * Mathf.Deg2Rad));
            velocity *= 10f;
            velocity.y = 20f;

            rocketRigidBody.velocity = Vector3.zero;
            rocketRigidBody.angularVelocity = Vector3.zero;

            rocketRigidBody.AddForce(velocity, ForceMode.VelocityChange);
            rocketRigidBody.AddRelativeTorque(new Vector3(Random.Range(-40f, 40f), Random.Range(-40f, 40f), Random.Range(-40f, 40f)) * 100f);

            State = LLState.Impacted;

            rocketMoveSFX.Stop();

            MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.CrateLandOnground);
        }


        public int reachedFruitIndex = 0;
        public void MoveOnCurrentFruits()
        {
            /*characterWayPoints.Clear();
            for (int iFruit = startFruitIndex; iFruit <= reachedFruitIndex; iFruit++)
            {
                characterWayPoints.Add(_fruits[iFruit].transform.position + new Vector3(0, 0.5f, 0));
            }*/

            //Debug.LogError("Move on fruits from " + (currentFruitIndex-1) + " to " + reachedFruitIndex + ": " + characterWayPoints.ToJoinedString());

            // Start the highlight line
            //MazeGame.instance.addLine(MazeGame.instance.correctPathColor);

            charPosForDrawing = transform.position;
            PerformMovement(false);
        }

        public void PerformMovement(bool wrong)
        {
            if (characterIsMoving)
            {
                return;
            }

            transform.DOKill(false);
            characterIsMoving = true;

            GetComponent<Collider>().enabled = true;

            foreach (GameObject particle in particles)
            {
                particle.SetActive(true);
            }

            myCollider.enabled = false;

            // Test with tweens:
            MoveTween(wrong);

            rocketMoveSFX = MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.RocketMove);
        }

        public void calculateMovementAndRotation()
        {
            Vector3 previousPosition = targetPos;
            float distance = (0.1f) - Camera.main.transform.position.y;
            targetPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -distance);
            targetPos = Camera.main.ScreenToWorldPoint(targetPos);

            if (previousPosition != initialPosition && previousPosition != targetPos)
            {
                MazeGame.instance.appendToLine(targetPos);
            }

            if (previousPosition != targetPos)
            {
                characterWayPoints.Add(targetPos + new Vector3(0, 0.5f, 0));
                var newDrawingToolPosition = targetPos + new Vector3(0, 0.5f, 0);
                MazeGame.instance.drawingTool.transform.position = newDrawingToolPosition;

                // @note: this is DEPRECATED, we now check the errors in MiniDrawingTOols
                /*

                var raycastSource = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.y - raycastCheckTarget.y));
                raycastSource = Camera.main.ScreenToWorldPoint(raycastSource);

                if (MazeGame.instance.pointsList.Count >= 2)
                {
                    RaycastHit hitInfo;

                    raycastCheckTarget = MazeGame.instance.pointsList[MazeGame.instance.pointsList.Count - 2];
                    raycastCheckTarget.y = TrackBounds.instance.transform.position.y;

                    if (Physics.Raycast(raycastSource, raycastCheckTarget - raycastSource, out hitInfo, Vector3.Distance(raycastSource, raycastCheckTarget), LayerMask.GetMask("TrackBounds")))
                    {
                        var collisionPoint = hitInfo.point;
                        Debug.LogError(hitInfo.collider.name);

                        var adjustedLinePoint = Camera.main.WorldToScreenPoint(collisionPoint);
                        adjustedLinePoint = new Vector3(adjustedLinePoint.x, adjustedLinePoint.y, -distance);
                        adjustedLinePoint = Camera.main.ScreenToWorldPoint(adjustedLinePoint);
                        MazeGame.instance.AdjustLastPointOfLine(adjustedLinePoint);

                        var pointOfImpact = Camera.main.WorldToScreenPoint(collisionPoint);
                        pointOfImpact = new Vector3(pointOfImpact.x, pointOfImpact.y, Camera.main.transform.position.y - transform.position.y - 2f);
                        pointOfImpact = Camera.main.ScreenToWorldPoint(pointOfImpact);

                        mazeLetter.OnPointerOverTrackBounds(pointOfImpact);
                    }
                }
                */
            }

            // Completed drawing at the end
            /*if ((_fruits[_fruits.Count - 1].transform.position - targetPos).sqrMagnitude < 0.1f)
            {
               ToggleParticlesVisibility(true);
               //initMovement();
               MazeGame.instance.timer.StopTimer();
               MoveOnCurrentFruits();
            }*/
        }

        public void Appear()
        {
            ToggleParticlesVisibility(true);
            isAppearing = true;

            List<Vector3> trajectoryPoints = new List<Vector3>();

            var startPivot = Fruits[0].transform.GetChild(0).gameObject.transform;
            var finalPosition = startPivot.position + new Vector3(0f, 0.6f, 0f);

            rocketMoveSFX = MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.RocketMove);
            rocketMoveSFX.Play();
            float duration = 1;
            if (!MazeGame.instance.isTutorialMode)
            {
                duration = 1;
                trajectoryPoints.Add(-startPivot.right * 15);
                transform.position = trajectoryPoints[0];
            }
            else
            {
                duration = 3;
                trajectoryPoints.Add(transform.position);

                int numTrajectoryPoints = 7;
                float yDecrement = (finalPosition.y - transform.position.y) / (numTrajectoryPoints + 1);

                float[] trajectoryPointXAnchors = { -0.7f, 0f, 0.7f, 0f, -0.75f, -0.4f, 0f };
                float[] trajectoryPointZAnchors = { 0f, 0.8f, 0f, -0.8f, -0.5f, 0.7f, 0.85f, 1.2f };

                for (int i = 0; i < numTrajectoryPoints; i++)
                {
                    Vector3 trajectoryPoint = new Vector3();
                    trajectoryPoint.y = transform.position.y + (i + 1) * yDecrement;

                    var frustumHeight = GetFrustumHeightAtDistance(Camera.main.transform.position.y - trajectoryPoint.y);
                    var frustumWidth = GetFrustumWidth(frustumHeight);

                    trajectoryPoint.x = frustumWidth * 0.5f * trajectoryPointXAnchors[i];
                    trajectoryPoint.z = frustumHeight * 0.5f * trajectoryPointZAnchors[i];

                    trajectoryPoints.Add(trajectoryPoint);
                }

            }
            trajectoryPoints.Add(finalPosition);

            transform.DOPath(trajectoryPoints.ToArray(), duration, PathType.CatmullRom, PathMode.Ignore).OnWaypointChange((int index) =>
            {
                if (index + 1 < trajectoryPoints.Count)
                {
                    LookAt(trajectoryPoints[index + 1], true);
                }

            }).OnComplete(() =>
            {
                ToggleParticlesVisibility(false);
                isAppearing = false;
                rocketMoveSFX.Stop();

                //transform.rotation = initialRotation;
                MazeGame.instance.ShowLetterTutorialAndInit();
            });

        }

        public void Flee()
        {
            StartCoroutine(Flee_Coroutine());
        }

        private IEnumerator Flee_Coroutine()
        {
            yield return new WaitForSeconds(0.25f);

            finishedRound = true;
            isFleeing = true;

            List<Vector3> fleePathPoints = new List<Vector3>();

            var cameraPosition = Camera.main.transform.position;

            var frustumHeight = GetFrustumHeightAtDistance(FLEE_PATH_ENDPOINT_DISTANCE_FROM_CAMERA);
            var frustumWidth = GetFrustumWidth(frustumHeight);

            Vector3 endPoint = new Vector3(cameraPosition.x + (frustumWidth / 2) * FLEE_PATH_ENDPOINT_X_ANCHOR,
                                           cameraPosition.y - FLEE_PATH_ENDPOINT_DISTANCE_FROM_CAMERA,
                                           cameraPosition.z + (frustumHeight / 2) * FLEE_PATH_ENDPOINT_Z_ANCHOR);

            Vector3 midPoint = transform.position + endPoint;
            midPoint *= 0.5f;

            frustumHeight = GetFrustumHeightAtDistance(cameraPosition.y - midPoint.y);
            frustumWidth = GetFrustumWidth(frustumHeight);

            midPoint = new Vector3(cameraPosition.x + (frustumWidth / 2) * FLEE_PATH_MIDPOINT_X_ANCHOR,
                                   midPoint.y,
                                   cameraPosition.z + (frustumHeight / 2) * FLEE_PATH_MIDPOINT_Z_ANCHOR);


            fleePathPoints.Add(transform.position);
            fleePathPoints.Add(midPoint);
            fleePathPoints.Add(endPoint);

            LL.Init(MazeGame.instance.currentLL);

            var fleePathPointsArray = fleePathPoints.ToArray();

            transform.DOKill();

            transform.DOLookAt(fleePathPointsArray[1], 0.33f, AxisConstraint.None, Vector3.forward);

            transform.DOPath(fleePathPointsArray, FLEE_PATH_DURATION, PathType.Linear).OnWaypointChange((int index) =>
            {
                if (index < fleePathPoints.Count - 1)
                {
                    transform.DOLookAt(fleePathPointsArray[index + 1], 0.33f, AxisConstraint.None, Vector3.forward);
                }

            }).OnComplete(() =>
            {
                //wait then show cracks:
                StartCoroutine(waitAndPerformCallback(3.5f, () =>
                {
                    MazeGame.instance.showAllCracks();
                    donotHandleBorderCollision = true;
                    characterIsMoving = false;
                    transform.DOKill(false);

                    rocket.GetComponent<SphereCollider>().enabled = true;

                    var rocketRigidBody = rocket.GetComponent<Rigidbody>();
                    rocketRigidBody.isKinematic = true;

                    State = LLState.Ragdolling;

                    ragdoll.deleteOnRagdollHit = false;

                    MazeGame.instance.ColorCurrentLinesAsIncorrect();

                    var tickPosition = transform.position;
                    tickPosition.z -= 1f;
                    tickPosition.x -= 2f;
                    tickPosition.y -= 3f;
                    Tutorial.TutorialUI.MarkNo(tickPosition, Tutorial.TutorialUI.MarkSize.Normal);
                    MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.KO);

                    if (!MazeGame.instance.isTutorialMode)
                    {
                        MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Lose);
                    }
                },
                () =>
                {
                    MazeGame.instance.lostCurrentLetter();
                }));
            });
        }

        private void LookAt(Vector3 target, bool forceHorizontal = false)
        {
            transform.LookAt(target);

            if (forceHorizontal)
            {
                var eulerAngles = transform.rotation.eulerAngles;
                eulerAngles.x = 0f;
                transform.rotation = Quaternion.Euler(eulerAngles);
            }
        }

        public void Celebrate(System.Action OnCelebrationOver, System.Action OnMarkStamp)
        {
            this.OnMarkStamp = OnMarkStamp;
            List<Vector3> celebrationPathPoints = new List<Vector3>();

            var cameraPosition = Camera.main.transform.position;

            var frustumHeight = GetFrustumHeightAtDistance(CELEBRATION_PATH_ENDPOINT_DISTANCE_FROM_CAMERA);
            var frustumWidth = GetFrustumWidth(frustumHeight);

            Vector3 endPoint = new Vector3(cameraPosition.x + (frustumWidth / 2) * CELEBRATION_PATH_ENDPOINT_X_ANCHOR,
                                           cameraPosition.y - CELEBRATION_PATH_ENDPOINT_DISTANCE_FROM_CAMERA,
                                           cameraPosition.z + (frustumHeight / 2) * CELEBRATION_PATH_ENDPOINT_Z_ANCHOR);

            Vector3 midPoint = transform.position + endPoint;
            midPoint *= 0.5f;

            frustumHeight = GetFrustumHeightAtDistance(cameraPosition.y - midPoint.y);
            frustumWidth = GetFrustumWidth(frustumHeight);

            midPoint = new Vector3(cameraPosition.x + (frustumWidth / 2) * CELEBRATION_PATH_MIDPOINT_X_ANCHOR,
                                   midPoint.y,
                                   cameraPosition.z + (frustumHeight / 2) * CELEBRATION_PATH_MIDPOINT_Z_ANCHOR);

            celebrationPathPoints.Add(transform.position);
            celebrationPathPoints.Add(midPoint);
            celebrationPathPoints.Add(endPoint);

            var offscreenPoint = endPoint + (endPoint - midPoint).normalized * 6f;

            celebrationPathPoints.Add(offscreenPoint);

            LL.Init(MazeGame.instance.currentLL);
            LL.Horraying = true;

            bool braked = false;

            celebrationPathTweener = transform.DOPath(celebrationPathPoints.ToArray(), CELEBRATION_PATH_DURATION, PathType.CatmullRom, PathMode.Ignore).OnWaypointChange((int index) =>
            {
                if (index == celebrationPathPoints.Count - 3)
                {
                    var rotationQuaterion = Quaternion.LookRotation(celebrationPathPoints[index + 1] - transform.position);
                    var eulerAngles = rotationQuaterion.eulerAngles;
                    eulerAngles.z -= 90f;
                    transform.DORotate(eulerAngles, 0.33f);
                }
                else if (index < celebrationPathPoints.Count - 2)
                {
                    transform.DOLookAt(celebrationPathPoints[index + 1], 0.33f, AxisConstraint.None, Vector3.forward);
                }
                else if (index == celebrationPathPoints.Count - 2 && !braked)
                {
                    braked = true;

                    celebrationPathTweener.Pause();
                    State = LLState.Braked;
                    winParticleVFX.SetActive(true);
                    brakeYoyoTweener = transform.DOMove(transform.position + new Vector3(-0.5f, 0.5f, -0.5f) * 0.33f, 0.75f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                }

            }).OnComplete(() =>
            {
                ToggleParticlesVisibility(false);
                gameObject.SetActive(false);
                OnCelebrationOver();
            });
        }

        private void FixedUpdate()
        {
            switch (_state)
            {
                case LLState.Normal:
                    break;
                case LLState.Braked:
                    if (stateTime > DELAY_TO_PRONOUNCE_LETTER && !pronouncedLetter)
                    {
                        letterPronounciation = MazeConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(
                            MazeGame.instance.currentLL,
                            soundType: MazeConfiguration.Instance.GetVocabularySoundType()
                        );
                        pronouncedLetter = true;
                    }
                    else if (pronouncedLetter && (letterPronounciation == null || !letterPronounciation.IsPlaying))
                    {
                        if (!markedEndTimeOfLetterPronounciation)
                        {
                            endTimeOfLetterPronounciation = Time.time;
                            markedEndTimeOfLetterPronounciation = true;
                        }

                        if (Time.time - endTimeOfLetterPronounciation > DELAY_BETWEEN_LETTER_SOUND_AND_CHECKMARK && !showedCheckmarkUponVictory)
                        {
                            var tickPosition = transform.position;
                            tickPosition.z -= 1.5f;
                            tickPosition.x -= 0.5f;

                            Tutorial.TutorialUI.MarkYes(tickPosition, Tutorial.TutorialUI.MarkSize.Big);
                            MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.StampOK);

                            showedCheckmarkUponVictory = true;
                            if (OnMarkStamp != null)
                                OnMarkStamp();
                        }

                        if (Time.time - endTimeOfLetterPronounciation > (DELAY_BETWEEN_LETTER_SOUND_AND_CHECKMARK + DELAY_BETWEEN_CHECKMARK_AND_EXIT))
                        {
                            State = LLState.Normal;

                            brakeYoyoTweener.Kill();
                            celebrationPathTweener.Play();
                        }
                    }

                    break;
                case LLState.Impacted:
                    if (stateTime >= 0.33f)
                    {
                        State = LLState.Ragdolling;
                    }
                    break;
            }

            stateTime += Time.fixedDeltaTime;
        }

        private Vector3 charPosForDrawing;
        public void Draw()
        {
            var previousPosition = charPosForDrawing;
            charPosForDrawing = transform.position;
            if (previousPosition != charPosForDrawing)
            {
                MazeGame.instance.appendToLine(previousPosition);
            }
        }
    }
}
