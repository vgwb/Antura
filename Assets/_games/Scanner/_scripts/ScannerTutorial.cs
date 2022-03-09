using Antura.Audio;
using Antura.Tutorial;
using UnityEngine;
using System.Collections;
using Antura.Keeper;

namespace Antura.Minigames.Scanner
{
    public class ScannerTutorial : MonoBehaviour
    {

        public Transform scannerDevice;
        public ScannerScrollBelt UpperBelt, lowerBelt;
        [HideInInspector]
        public float originalLLOnBeltSpeed;
        public float startDelay = 8, repeatDelay = 3;

        public int tutStep = 0;
        public bool playTut = true;

        ScannerGame game;
        Transform source, target;
        Vector3 targetPosition;
        ScannerSuitcase currentSuitcases;
        ScannerLivingLetter currentLL;
        ScannerAntura antura;

        float scannerStartZPos;
        int llCounter = 0;

        bool isScannerReady
        {
            get
            {
                if (scannerDevice.transform.position.z < scannerStartZPos + 0.1f)
                    return true;
                else
                    return false;
            }
        }

        public bool isTutRound
        {
            get
            {
                if (game.roundsManager.numberOfRoundsPlayed <= 0 &&
                    game.TutorialEnabled)
                {
                    return true;
                }
                else
                {
                    game.beltSpeed = originalLLOnBeltSpeed;
                    return false;
                }
            }
        }

        void Awake()
        {
            game = GetComponent<ScannerGame>();
        }
        void Start()
        {
            if (ScannerConfiguration.Instance.Variation == ScannerVariation.OneWord || isTutRound)
                startDelay = 8;
            else if (ScannerConfiguration.Instance.nCorrect == 3)
                startDelay = 25;
            else if (ScannerConfiguration.Instance.nCorrect == 4)
                startDelay = 24;
            else
                startDelay = 25.5f;

            StartCoroutine(coDoTutorial());
            originalLLOnBeltSpeed = game.beltSpeed;
            //if (ScannerConfiguration.Instance.Difficulty >= 0.5f)
            game.beltSpeed = 2;


            game.disableInput = true;
            //warm up
            TutorialUI.DrawLine(-100 * Vector3.up, -100 * Vector3.up, TutorialUI.DrawLineMode.Arrow);

            scannerStartZPos = scannerDevice.transform.position.z;

            foreach (ScannerSuitcase sc in game.suitcases)
            {
                sc.onCorrectDrop += resetTut;
            }

            antura = game.antura.GetComponent<ScannerAntura>();
            //foreach (ScannerLivingLetter ll in game.scannerLL)
            //  ll.facingCamera = true;
        }


        public void setupTutorial(int step = 0, ScannerLivingLetter targetLL = null)
        {

            if (!isTutRound)
            {
                return;
            }

            Debug.Log("Tutorial started");

            if (targetLL)
            {
                currentLL = targetLL;
            }
            else
            {
                currentLL = game.scannerLL[0];
                target = currentLL.transform;
            }

            tutStep = step;
            if (step <= 1)
            {
                source = scannerDevice;

            }
            else if (step == 2)
            {
                matchLLToSS(targetLL);
            }
        }

        void onTutorialStart()
        {
            //AudioManager.I.PlayDialogue(Database.LocalizationDataId.Scanner_Tuto);
            game.beltSpeed = 0;
            game.disableInput = false;
            StartCoroutine(sayTut(1));

        }

        void onTutorialEnd()
        {
            TutorialUI.Clear(true);

            game.beltSpeed = originalLLOnBeltSpeed;
            if (ScannerConfiguration.Instance.Variation == ScannerVariation.OneWord)
            {
                game.Context.GetOverlayWidget().Initialize(true, false, true);
                game.Context.GetOverlayWidget().SetMaxLives(game.allowedFailedMoves);
            }
            else
            {
                game.Context.GetOverlayWidget().Initialize(true, false, false);
            }
            game.Context.GetOverlayWidget().SetStarsThresholds(game.STARS_1_THRESHOLD, game.STARS_2_THRESHOLD, game.STARS_3_THRESHOLD);
        }


        IEnumerator coDoTutorial()
        {
            yield return new WaitForSeconds(startDelay - 3);
            StartCoroutine(antura.enterTheScene());
            yield return new WaitForSeconds(3);
            resetTut(null, null);
            onTutorialStart();

            while (isTutRound && playTut)
            {
                if (pauseTut())
                {
                    yield return null;
                    continue;
                }

                if (tutStep == 0)
                {
                    //Debug.Log(llCounter+"<<<");
                    target = game.antura.transform;
                    if (target)
                    {
                        TutorialUI.DrawLine(source.position - Vector3.forward * 2, target.position +
                            new Vector3(0, scannerDevice.position.y - target.position.y, -2),
                            TutorialUI.DrawLineMode.FingerAndArrow);
                    }
                    else
                    {
                        TutorialUI.Clear(true);
                    }
                }
                else if (tutStep == 1)
                {
                    //Debug.Log(llCounter+"<<<");
                    target = getNewTarget();
                    if (target)
                    {
                        TutorialUI.DrawLine(source.position - Vector3.forward * 2, target.position +
                            new Vector3(0, scannerDevice.position.y - target.position.y, -2),
                            TutorialUI.DrawLineMode.FingerAndArrow);
                    }
                    else
                    {
                        TutorialUI.Clear(true);
                    }
                }
                else
                {
                    //Debug.Log(Time.deltaTime + " b");
                    TutorialUI.DrawLine(source.position - Vector3.forward * 2, target.transform.position +
                        new Vector3(5f, 3, -2), TutorialUI.DrawLineMode.FingerAndArrow);
                }

                yield return new WaitForSeconds(repeatDelay);

            }

            onTutorialEnd();
        }

        void matchLLToSS(ScannerLivingLetter targetLL)
        {
            currentLL = targetLL;

            foreach (ScannerSuitcase sc in game.suitcases)
                if (targetLL.LLController.Data.Id == sc.wordId)
                {
                    currentSuitcases = sc;
                    source = sc.transform;
                    target = targetLL.transform;
                    break;
                }
        }

        void resetTut(GameObject g, ScannerLivingLetter sll)
        {
            llCounter++;
            setupTutorial();
        }

        Transform getNewTarget()
        {
            int i = 0;
            foreach (ScannerLivingLetter ll in game.scannerLL)
                if (ll.gotSuitcase == false)
                {
                    i++;
                    //Debug.LogWarning(ll.transform.position +" "+ll.transform+""+ ll.gotSuitcase);
                    currentLL = ll;
                    target = currentLL.LLController.transform;
                    return target;
                }

            if (i == 0)
            {
                Debug.Log("xxxx");
                target = null;
                return null;
            }

            return currentLL.transform;

        }
        IEnumerator sayTut(float delay)
        {
            ///TODO FIX
            // AudioManager.I.PlayDialogue(Database.LocalizationDataId.Scanner_Tuto_Antura);
            while (tutStep == 0 || pauseTut())
            {
                yield return null;
            }

            yield return new WaitForSeconds(delay);
            KeeperManager.I.PlayDialogue(ScannerConfiguration.Instance.TutorialLocalizationId);
        }

        bool pauseTut()
        {
            if (tutStep == -1 || llCounter > game.scannerLL.Count ||
                !isScannerReady ||
                /*(currentLL && currentLL.status != ScannerLivingLetter.LLStatus.StandingOnBelt) ||*/
                !target ||
                (currentSuitcases && (currentSuitcases.isDragging || !currentSuitcases.isReady) && tutStep == 2) ||
                !isTutRound ||
                !isScannerReady)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
