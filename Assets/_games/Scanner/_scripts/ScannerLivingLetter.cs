using UnityEngine;
using System.Collections;
using System;
using Antura.Audio;
using Antura.LivingLetters;
using Antura.Minigames;

namespace Antura.Minigames.Scanner
{

    public class ScannerLivingLetter : MonoBehaviour
    {

        public ScannerGame game;
        public enum LLStatus { None, Sliding, StandingOnBelt, RunningFromAntura, Lost, Won, Happy, Sad, Flying, Poofing, Falling };
        public GameObject livingLetter;
        public float slideSpeed = 2f;
        public float flightSpeed = 10f;
        public bool facingCamera;
        public LLStatus status = LLStatus.None;
        private float turnAngle;
        private Vector3 startingPosition;
        private Quaternion startingRotation;

        public Transform collForScan;
        public Transform fallOffPoint;
        public Transform midPoint;

        public LivingLetterController LLController;
        public GameObject rainbowJet;
        public SkinnedMeshRenderer sm;
        [HideInInspector]
        public Material mat;
        public float slidingTime;

        //		public event Action <ScannerLivingLetter> onReset;
        public event Action<ScannerLivingLetter> onStartFallOff;
        public event Action<ScannerLivingLetter> onFallOff;
        public event Action<ScannerLivingLetter> onPassedMidPoint;
        public event Action<ScannerLivingLetter> onFlying;

        public BoxCollider bodyCollider;

        [HideInInspector]
        public bool gotSuitcase;
        //        private Transform originalParent;
        private float fallOffX;
        private float midPointX;
        private bool passedMidPoint = false;

        void Awake()
        {
            status = LLStatus.None;
            LLController = livingLetter.GetComponent<LivingLetterController>();
            startingPosition = transform.position;
            startingRotation = LLController.transform.rotation;
        }

        public void Reset(bool stopCO = true)
        {
            if (stopCO)
                StopAllCoroutines();
            rainbowJet.SetActive(false);

            if (game.gameActive)
            {
                status = LLStatus.None;
                LLController.Falling = false;
                LLController.SetState(LLAnimationStates.LL_still);
                gotSuitcase = false;
                LLController.transform.rotation = startingRotation;
                transform.position = startingPosition;

                fallOffX = fallOffPoint.position.x;
                midPointX = midPoint.position.x;
                passedMidPoint = false;

                turnAngle = facingCamera ? 180 : 0;
                gameObject.SetActive(true);

                gameObject.GetComponent<SphereCollider>().enabled = true; // enable feet collider
                bodyCollider.enabled = false; // disable body collider

                showLLMesh(true);

            }
        }

        public void StartSliding()
        {
            LLController.Falling = true;
            status = LLStatus.Sliding;
        }

        IAudioSource wordSound;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.E) && LLController.Data != null)
            {
                if (wordSound == null)
                {
                    wordSound = game.Context.GetAudioManager().PlayVocabularyData(LLController.Data, true);
                }
                if (!wordSound.IsPlaying)
                {
                    wordSound = game.Context.GetAudioManager().PlayVocabularyData(LLController.Data, true);
                }

                wordSound.Position = 0;
            }
            if (wordSound != null)
            {
                wordSound.Position = Mathf.Clamp(wordSound.Position + Time.deltaTime / 10, 0, 0.5f);
            }

            if (status == LLStatus.Sliding)
            {
                transform.Translate(slideSpeed * Time.deltaTime, -slideSpeed * Time.deltaTime / 2, 0);
            }
            else if (status == LLStatus.StandingOnBelt)
            {
                transform.Translate(game.beltSpeed * Time.deltaTime, 0, 0);
            }
            else if (status == LLStatus.Flying)
            {
                transform.Translate(Vector2.up * flightSpeed * Time.deltaTime);
            }
            else if (status == LLStatus.Falling)
            {
                transform.Translate(Vector2.down * flightSpeed * Time.deltaTime);
            }

            if (livingLetter.transform.position.x > fallOffX && status == LLStatus.StandingOnBelt)
            {
                StartCoroutine(co_FallOff());
            }
            else if (livingLetter.transform.position.x > midPointX && !passedMidPoint)
            {
                passedMidPoint = true;
                onPassedMidPoint(this);
            }
        }


        IEnumerator co_FlyAway()
        {
            //            letterObjectView.DoSmallJump();
            onFlying(this);
            status = LLStatus.Happy;

            LLController.State = LLAnimationStates.LL_dancing;
            LLController.DoDancingWin();
            //letterObjectView.DoSmallJump();
            // Rotate in case not facing the camera
            StartCoroutine(RotateGO(livingLetter, new Vector3(0, 180, 0), 1f));
            yield return new WaitForSeconds(2f);

            //			// building anticipation
            //			letterObjectView.Crouching = true;
            //			yield return new WaitForSeconds(1f);
            //			letterObjectView.Crouching = false;

            // Starting flight
            LLController.DoHorray();
            yield return new WaitForSeconds(0.75f);
            rainbowJet.SetActive(true);
            //yield return new WaitForSeconds(0.15f);
            status = LLStatus.Flying;

            LLController.SetState(LLAnimationStates.LL_still);
            yield return new WaitForSeconds(2f);
            //            Reset();
        }

        IEnumerator co_Lost()
        {
            if (status == LLStatus.StandingOnBelt)
            {
                status = LLStatus.Sad;
                LLController.DoAngry();
                yield return new WaitForSeconds(1.5f);
            }
            Debug.Log(status);
            if (status != LLStatus.Flying || status != LLStatus.Falling || status != LLStatus.None)
            {
                LLController.Poof();
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
            transform.position = new Vector3(-100, -100, -100); // Move offscreen


        }

        IEnumerator co_FallOff()
        {
            gotSuitcase = false;
            status = LLStatus.None;
            onStartFallOff(this);
            LLController.SetState(LLAnimationStates.LL_idle);
            LLController.DoSmallJump();
            StartCoroutine(RotateGO(livingLetter, new Vector3(90, 90, 0), 1f));
            yield return new WaitForSeconds(0.25f);
            AudioManager.I.PlaySound(Sfx.LetterSad);
            yield return new WaitForSeconds(0.25f);
            LLController.Falling = true;
            status = LLStatus.Falling;

            yield return new WaitForSeconds(0.1f);

            LLController.Poof();
            showLLMesh(false);

            yield return new WaitForSeconds(0.9f);

            onFallOff(this);
        }

        void OnMouseUp()
        {
            LLController.SetState(LLAnimationStates.LL_tickling);
            game.Context.GetAudioManager().PlaySound(Sfx.LL_Annoyed);
        }

        public void RoundLost()
        {
            gotSuitcase = false;
            StopAllCoroutines();
            //            letterObjectView.SetState(LLAnimationStates.LL_idle);
            StartCoroutine(co_Lost());
        }

        public void RoundWon()
        {
            StartCoroutine(co_FlyAway());
        }

        public void CorrectMove()
        {
            StopAllCoroutines();
            //			letterObjectView.SetState(LLAnimationStates.LL_idle);
            LLController.DoHorray();
        }

        public void WrongMove()
        {
            StopAllCoroutines();
            LLController.SetState(LLAnimationStates.LL_idle);
            LLController.DoAngry();
        }


        IEnumerator RotateGO(GameObject go, Vector3 toAngle, float inTime)
        {
            var fromAngle = go.transform.rotation;
            var destAngle = Quaternion.Euler(toAngle);
            for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
            {
                go.transform.rotation = Quaternion.Lerp(fromAngle, destAngle, t);
                yield return null;
            }
        }

        IEnumerator AnimateLL()
        {

            yield return new WaitForSeconds(1f);

            int index = -1;
            LLAnimationStates[] animations =
            {
                LLAnimationStates.LL_idle,
                LLAnimationStates.LL_dancing
            };

            do
            {
                int oldIndex = index;
                do
                {
                    index = UnityEngine.Random.Range(0, animations.Length);
                } while (index == oldIndex);
                LLController.SetState(animations[index]);
                yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 4f));
            } while (status == LLStatus.StandingOnBelt);
        }

        void OnTriggerEnter(Collider other)
        {
            if (status == LLStatus.Sliding)
            {
                if (other.tag == ScannerGame.TAG_BELT)
                {
                    //                    transform.parent = other.transform;
                    status = LLStatus.StandingOnBelt;
                    gameObject.GetComponent<SphereCollider>().enabled = false; // disable feet collider
                    bodyCollider.enabled = true; // enable body collider
                    LLController.Falling = false;
                    StartCoroutine(RotateGO(livingLetter, new Vector3(0, turnAngle, 0), 1f));
                    StartCoroutine(AnimateLL());
                }
            }
        }

        public void showLLMesh(bool show)
        {
            SkinnedMeshRenderer[] LLMesh = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer sm in LLMesh)
            {
                sm.enabled = show;
            }
            LLController.contentTransform.gameObject.SetActive(show);
        }

        public void setColor(Color col)
        {
            if (!mat)
            {
                mat = sm.material;
            }
            mat.SetColor("_Color", col);
        }
    }
}
