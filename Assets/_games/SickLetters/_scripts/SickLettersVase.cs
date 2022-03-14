using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;

namespace Antura.Minigames.SickLetters
{
    public class SickLettersVase : MonoBehaviour
    {
        public Transform libra;
        Vector3 libraStartPos;
        public BoxCollider vaseCollider;
        public TextMeshPro _counter;
        public AnimationCurve bounceCurve;
        bool /*isFallingFromTop,*/ isOnTheGround;
        public int counter
        {
            set
            {
                _counter.text = value.ToString();
                //game.Context.GetOverlayWidget().SetStarsScore(value / (game.targetScale / 3));
            }
            get { return int.Parse(_counter.text); }
        }

        [HideInInspector]
        public Vector3 vaseStartPose;
        [HideInInspector]
        public Vector3 vaseStartRot;
        [HideInInspector]
        public SickLettersGame game;

        Rigidbody vaseRB;
        Tweener moveTweener;

        // Use this for initialization
        void Start()
        {
            vaseStartPose = transform.position;
            vaseStartRot = transform.eulerAngles;
            vaseRB = GetComponent<Rigidbody>();
            libraStartPos = libra.position;
        }


        /*void Update() {
            if (transform.position.y < 9.2f && isFallingFromTop)
            {

                StartCoroutine(bounceScale(0.95f));
                isFallingFromTop = false;
            }
        }*/

        SickLettersDraggableDD dd;

        void OnTriggerEnter(Collider coll)
        {
            checkEntry(coll);
            if (!isOnTheGround && coll.gameObject.name == "Ground")
            {
                SickLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.CrateLandOnground);
                isOnTheGround = true;
            }
        }

        void OnTriggerExit(Collider coll)
        {
            if (coll.tag == "Player")
            {
                dd = coll.gameObject.GetComponent<SickLettersDraggableDD>();

                if (!dd || dd.isInVase)
                    return;

                if (dd.isDragging)
                    dd.touchedVase = false;
            }
        }
        /*void OnTriggerStay(Collider coll)
        {
            checkEntry(coll);
        }*/

        private void checkEntry(Collider coll)
        {

            if (coll.tag == "Player")
            {
                dd = coll.gameObject.GetComponent<SickLettersDraggableDD>();

                if (dd.isDragging)
                    dd.touchedVase = true;
                if (!dd || dd.isDragging || dd.isInVase || dd.collidedWithVase)
                    return;


                dd.collidedWithVase = true;
                addNewDDToVas(dd);

            }
        }

        public void addNewDDToVas(SickLettersDraggableDD dd)
        {
            if (dd.isCorrect)
            {
                game.Poof(dd.transform);
                dd.resetCorrectDD();
                game.onWrongMove(dd.isCorrect);
                StartCoroutine(onDroppingCorrectDD());

            }
            else if (!dd.isInVase)
            {
                //dd.deattached = true;

                game.onCorrectMove(dd);

                //game.checkForNextRound();
            }

            StartCoroutine(bounceScale());
        }

        IEnumerator bounceScale(float maxDown = 0.25f)
        {
            Vector3 endPos = libraStartPos;
            if (libra.position.y > 1.17f)
                endPos = new Vector3(libraStartPos.x, libra.position.y - 0.05f, libraStartPos.z);

            transform.parent = libra;

            libra.DOMoveY(libra.position.y - maxDown, 0.2f).OnComplete(() =>
            {
                libra.DOJump(endPos, 0.1f, 1, 0.25f).OnComplete(() =>
                {
                    libra.DOJump(endPos, 0.05f, 1, 0.3f);
                });
            });

            yield return new WaitForSeconds(0.9f);

            transform.parent = null;
        }

        public IEnumerator onDroppingCorrectDD()
        {

            if (game.roundsCount == 0)
                yield break;

            game.disableInput = true;

            StartCoroutine(game.antura.bark());

            yield return new WaitForSeconds(0.5f);

            game.LLPrefab.LLStatus = letterStatus.angry;
            game.LLPrefab.letterView.DoAngry();
            SickLettersConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(game.LLPrefab.letterView.Data, true, soundType: SickLettersConfiguration.Instance.GetVocabularySoundType());



            yield return new WaitForSeconds(1.5f);
            game.LLPrefab.LLStatus = letterStatus.idle;

            vaseRB.constraints = RigidbodyConstraints.None;
            vaseRB.isKinematic = false;



            StartCoroutine(dropVase());

            if (game.scale.counter > game.maxReachedCounter)
                game.maxReachedCounter = game.scale.counter;

            game.scale.counter = 0;

            yield return new WaitForSeconds(3);

            game.Poof(transform);
            vaseRB.isKinematic = true;
            transform.position = vaseStartPose + Vector3.up * 20;
            transform.eulerAngles = vaseStartRot;


            yield return new WaitForSeconds(1.5f);
            game.antura.sleep();

            StartCoroutine(summonVase());

            yield return new WaitForSeconds(1f);

            if (game.wrongDDsOnLL() > 0)
                game.disableInput = false;
        }

        public IEnumerator dropVase(float delay = 0, bool moveCam = false)
        {
            //if (game.roundsCount == 0)
            //  yield break;

            yield return new WaitForSeconds(delay);

            vaseRB.constraints = RigidbodyConstraints.None;
            vaseRB.isKinematic = false;

            libra.DOMove(libraStartPos, 0.5f).OnComplete(() =>
            {
                libra.DOPunchPosition(Vector3.up, 1, 7);
            });

            foreach (SickLettersDraggableDD dd in game.allWrongDDs)
            {
                if (dd && dd.isInVase)
                {
                    dd.boxCollider.isTrigger = false;
                    dd.thisRigidBody.isKinematic = false;
                    dd.thisRigidBody.useGravity = true;
                    dd.poofDD(Mathf.Clamp(Random.value * 10f, 1.5f, 3));
                }
            }

            if (moveCam)
            {
                yield return new WaitForSeconds(0.65f);
                StartCoroutine(game.slCamera.rotatCamera(20f));
            }

            yield return new WaitForSeconds(1);
            while (libra.position.y > libraStartPos.y + 0.01f)
            {
                libra.position = Vector3.Lerp(libra.position, libraStartPos, Time.deltaTime * 5);
                yield return null;
            }
        }

        public IEnumerator summonVase()
        {
            if (game.StateManager.CurrentState != game.PlayState)
                yield break;
            vaseRB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

            vaseRB.isKinematic = false;
            vaseRB.useGravity = true;
            //isFallingFromTop = true;
            isOnTheGround = false;

            yield return new WaitForSeconds(0.65f);
            StartCoroutine(bounceScale(0.95f));
            SickLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.OK);
        }

        public void flyVas(float delay = 0)
        {
            StartCoroutine(coFlyVase(delay));
        }

        IEnumerator coFlyVase(float delay)
        {
            yield return new WaitForSeconds(delay);

            foreach (SickLettersDraggableDD dd in game.allWrongDDs)
            {
                if (dd && dd.isInVase)
                {
                    dd.transform.parent = transform;
                    dd.thisRigidBody.isKinematic = true;
                }
            }

            vaseRB.isKinematic = true;

            libra.DOMove(libraStartPos, 0.2f).OnComplete(() =>
            {
                libra.DOPunchPosition(Vector3.up, 1, 7);
            });

            while (true)
            {
                vaseRB.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 30, 10 * Time.deltaTime), transform.position.z);
                game.slCamera.transform.eulerAngles = new Vector3(Mathf.LerpAngle(game.slCamera.transform.eulerAngles.x, -10, 4 * Time.deltaTime), game.slCamera.transform.eulerAngles.y, game.slCamera.transform.eulerAngles.z);
                yield return null;
            }
        }
    }
}
