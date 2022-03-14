using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Antura.Dog;
using Antura.Audio;
using DG.Tweening;

namespace Antura.Minigames.Scanner
{
    public class ScannerAntura : MonoBehaviour
    {


        public float chargeAnimSpeed = 1.5f, camShakeSpeed = 2;
        public bool isInScene;

        public int scaredCounter;
        public ScannerGame game;
        public Transform sceneCamera;
        public Transform stopPose, chargeEndPose;
        public float movingSpeed = 8, chargeSpeed = 11;
        public int timesCanAppear = 1;
        public List<ScannerLivingLetter> fallenLL = new List<ScannerLivingLetter>();
        public ParticleSystem stars;
        public Material redMat;
        private AnturaAnimationController antura;
        private Animator anturaAnimator;
        bool canBeScared;
        int thisRound;

        void Start()
        {
            antura = GetComponent<AnturaAnimationController>();
            anturaAnimator = GetComponent<Animator>();
            antura.SetWalkingSpeed(1);
            antura.State = AnturaAnimationStates.walking;

            transform.position += Vector3.right * 25;

            StartCoroutine(handleAnturasEvents());

            thisRound = game.roundsManager.numberOfRoundsPlayed;
        }


        void Update()
        {
            if (thisRound != game.roundsManager.numberOfRoundsPlayed)
            {
                thisRound = game.roundsManager.numberOfRoundsPlayed;
                fallenLL.Clear();
            }
        }

        IEnumerator handleAnturasEvents()
        {
            yield return new WaitForSeconds(2);

            while (timesCanAppear > 0)
            {
                if (!game.tut.isTutRound)
                {
                    int d = Random.Range(25, 50);
                    print(d);
                    yield return new WaitForSeconds(d);

                    if (game.roundsManager.numberOfRoundsPlayed >= 6)
                        break;

                    StartCoroutine(enterTheScene());

                    timesCanAppear--;
                }
                yield return null;
            }
        }

        public IEnumerator enterTheScene()
        {
            scaredCounter = 0;
            isInScene = true;

            game.trapDoor.SetBool("TrapDown", false);
            game.trapDoor.SetBool("TrapUp", true);

            antura.transform.eulerAngles = Vector3.up * 90;
            yield return new WaitForSeconds(0);

            antura.State = AnturaAnimationStates.walking;
            antura.SetWalkingSpeed(1);

            while (transform.position.x > stopPose.position.x + 0.01f)
            {
                transform.position -= Vector3.right * movingSpeed * Time.deltaTime;
                yield return null;
            }

            antura.SetWalkingSpeed(0);
            antura.State = AnturaAnimationStates.idle;

            StartCoroutine(bark());

            yield return null;
        }

        public IEnumerator bark(float delay = 0)
        {
            canBeScared = true;
            antura.IsExcited = true;
            yield return new WaitForSeconds(0.75f);
            antura.IsExcited = false;

            if (scaredCounter != 0)
            { /*StartCoroutine(leaveScene(true));*/ yield break; }

            anturaAnimator.SetTrigger("doShout");
            AudioManager.I.PlaySound(Sfx.DogBarking);
            yield return new WaitForSeconds(1f);

            if (scaredCounter != 0)
            { /*StartCoroutine(leaveScene(true));*/ yield break; }

            yield return new WaitForSeconds(1f);
            //antura.OnJumpStart();
            yield return new WaitForSeconds(0.5f);
            //antura.OnJumpEnded();
            yield return new WaitForSeconds(1.0f);

            if (scaredCounter != 0)
            {  /*StartCoroutine(leaveScene(true));*/ yield break; }

            yield return new WaitForSeconds(1.0f);

            if (scaredCounter != 0)
            { /*StartCoroutine(leaveScene(true));*/ yield break; }

            yield return new WaitForSeconds(1.0f);
            anturaAnimator.SetTrigger("doShout");
            AudioManager.I.PlaySound(Sfx.DogBarking);
            yield return new WaitForSeconds(1f);

            if (scaredCounter != 0)
            { /*StartCoroutine(leaveScene(true));*/ yield break; }

            yield return new WaitForSeconds(1f);

            if (scaredCounter != 0)
            { /*StartCoroutine(leaveScene(true));*/ yield break; }

            antura.DoBurp();
            AudioManager.I.PlaySound(Sfx.DogBarking);
            yield return new WaitForSeconds(0.5f);
            antura.IsAngry = true;
            yield return new WaitForSeconds(1f);

            if (scaredCounter != 0)
            { /*StartCoroutine(leaveScene(true));*/ yield break; }

            yield return new WaitForSeconds(1f);
            if (game.tut.isTutRound && game.tut.tutStep == 0)
                StartCoroutine(bark());
            else
                StartCoroutine(charge());

            yield return null;
        }


        IEnumerator charge()
        {
            canBeScared = false;
            antura.IsAngry = false;
            antura.IsExcited = false;
            antura.SetWalkingSpeed(1);
            anturaAnimator.CrossFade("dog_charge_start", 0.3f);
            yield return new WaitForSeconds(0.75f);
            anturaAnimator.CrossFade("dog_charge_start", 0.2f);
            yield return new WaitForSeconds(0.75f);
            //antura.DoCharge(() => { StartCoroutine(chargeMove()); })
            anturaAnimator.CrossFade("dog_charge_start", 0.2f);
            yield return new WaitForSeconds(0.75f);
            StartCoroutine(chargeMove());
            antura.State = AnturaAnimationStates.walking;
            //anturaAnimator.CrossFade("dog_charge_run", 0.2f);
            anturaAnimator.speed = chargeAnimSpeed;
        }

        IEnumerator chargeMove()
        {
            AudioManager.I.PlaySound(Sfx.DogBarking);
            sceneCamera.DOShakePosition(2.5f, camShakeSpeed);

            while (transform.position.x > chargeEndPose.position.x + 0.01f)
            {
                transform.position -= Vector3.right * chargeSpeed * 1.75f * Time.deltaTime;

                yield return null;
            }

            anturaAnimator.speed = 1;
            StartCoroutine(leaveScene());
        }


        public void beScared()
        {
            StartCoroutine(co_beScared());
        }
        IEnumerator co_beScared()
        {
            if (scaredCounter == 0 && canBeScared)
            {
                if (game.tut.tutStep == 0)
                    game.tut.tutStep = -1;

                scaredCounter = 1;
                antura.IsExcited = false;
                canBeScared = false;
                anturaAnimator.SetBool("idle", false);
                //StartCoroutine(flashRed(sm));
                flashRedGO(gameObject);
                anturaAnimator.CrossFade("dog_suck_end", 0.3f);
                AudioManager.I.PlaySound(Sfx.Dog_Scared);
                yield return new WaitForSeconds(0.3f);
                anturaAnimator.SetBool("idle", true);
                anturaAnimator.CrossFade("dog_stand_sad_breath", 0.2f);
                antura.IsSad = true;
                stars.Play();
                yield return new WaitForSeconds(2.5f);
                stars.Stop(true);
                yield return new WaitForSeconds(1.5f);
                StartCoroutine(leaveScene(true));
                antura.IsSad = false;
            }
        }

        public IEnumerator leaveScene(bool wasScared = false, float delay = 0f)
        {
            if (game.tut.isTutRound && game.tut.tutStep == -1)
                game.tut.setupTutorial(1);

            if (wasScared)
            {
                AudioManager.I.PlaySound(Sfx.DogBarking);
                scaredCounter = 0;
            }
            yield return new WaitForSeconds(delay);
            //anturaAnimator.Play("dog_turn180", 1);
            if (wasScared)
            {
                antura.State = AnturaAnimationStates.bitingTail;
                //anturaAnimator.SetBool("bitingTail", true);
                yield return new WaitForSeconds(.6f);
            }
            else
                antura.transform.eulerAngles = Vector3.up * 270;

            antura.SetWalkingSpeed(1);
            antura.State = AnturaAnimationStates.walking;
            while (transform.position.x < stopPose.position.x + 20)
            {
                transform.position += Vector3.right * chargeSpeed * Time.deltaTime;
                yield return null;
            }
            game.trapDoor.SetBool("TrapUp", false);
            game.trapDoor.SetBool("TrapDown", true);

            isInScene = false;
        }

        float sildingDelay;
        IEnumerator throwLL(ScannerLivingLetter ll, float slideTime)
        {


            Rigidbody rb;
            rb = ll.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.forward * Random.Range(6, 10) + Vector3.up * Random.Range(12, 15), ForceMode.Impulse);
            ll.LLController.OnJumpStart();
            ll.LLController.OnJumpMaximumHeightReached();

            ll.slidingTime = slideTime;

            yield return new WaitForSeconds(0.45f);
            ll.LLController.Poof();
            ll.showLLMesh(false);

            yield return new WaitForSeconds(1.55f);
            rb.isKinematic = true;
            rb.useGravity = false;

            StartCoroutine(llReset(ll, slideTime));

        }

        void OnTriggerEnter(Collider coll)
        {
            ScannerLivingLetter ll = coll.transform.root.GetComponent<ScannerLivingLetter>();

            if (ll && !ll.gotSuitcase && ll.status == ScannerLivingLetter.LLStatus.StandingOnBelt)
            {
                if (!fallenLL.Contains(ll))
                {
                    ll.status = ScannerLivingLetter.LLStatus.None;
                    fallenLL.Add(ll);
                    AudioManager.I.PlaySound(Sfx.LetterSad);
                    StartCoroutine(throwLL(ll, calculateDelay()));
                }

            }
        }

        float calculateDelay()
        {
            float slideTime = 0;
            for (int i = 0; i < game.scannerLL.Count; i++)
            {
                if (game.scannerLL[i].slidingTime > slideTime)
                    slideTime = game.scannerLL[i].slidingTime;
            }


            if (game.scannerLL.Count == 3)
                return slideTime + 8;
            else
                return slideTime + 5;
        }

        IEnumerator llReset(ScannerLivingLetter ll, float slideTime)
        {
            //yield return new WaitForSeconds(delay);
            while (Time.time < slideTime)
                yield return null;

            if (fallenLL.IndexOf(ll) >= 0 && ll.status == ScannerLivingLetter.LLStatus.None)
            {
                ll.Reset();
                ll.StartSliding();
                fallenLL[fallenLL.IndexOf(ll)] = null;
            }
        }

        public void flashRedGO(GameObject go)
        {
            Renderer[] r = go.GetComponentsInChildren<Renderer>();

            for (int j = 0; j < r.Length; j++)
                StartCoroutine(flashRed(r[j]));
        }

        public IEnumerator flashRed(Renderer r)
        {

            List<Material> newMats = new List<Material>();
            Material[] startMats;

            startMats = r.materials;

            for (int m = 0; m < startMats.Length; m++)
                newMats.Add(redMat);


            for (int i = 0; i < 2; i++)
            {
                r.materials = newMats.ToArray();
                yield return new WaitForSeconds(0.15f);
                r.materials = startMats;
                yield return new WaitForSeconds(0.15f);
            }

        }

    }
}
