using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Antura.Language;
using Antura.LivingLetters;
using Antura.UI;
using TMPro;

namespace Antura.Minigames.SickLetters
{

    public enum letterStatus { idle, angry, horry}

    public class SickLettersLLPrefab : MonoBehaviour
    {
        public Transform shadow;
        public TextMeshPro dotlessLetter, correctDot, correctDiac;
        public SickLettersDraggableDD correctDotCollider, correctDiacCollider;
        public SickLettersGame game;
        public LivingLetterController letterView;
        public letterStatus LLStatus = letterStatus.idle;
        public Animator letterAnimator;
        public List<SickLettersDraggableDD> thisLLWrongDDs = new List<SickLettersDraggableDD>();


        private SkinnedMeshRenderer[] LLMesh;
        Vector3 statPos, shadowStartSize;


        void Start()
        {
            shadowStartSize = shadow.localScale;
            shadow.localScale = Vector3.zero;
            LLMesh = GetComponentsInChildren<SkinnedMeshRenderer>();
            letterView = GetComponent<LivingLetterController>();
            letterAnimator = GetComponent<Animator>();
            statPos = transform.position;
            
        }


        public void jumpIn()
        {
            StartCoroutine(coJumpIn());
        }


        public void jumpOut(float delay = 0, bool endGame = false) {
            StartCoroutine(coJumpOut(delay, endGame));
        }

        IEnumerator coJumpIn()
        {
            showLLMesh(true);
            getNewLetterData();
            scatterDDs();
            StartCoroutine(fadShadow());

            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<CapsuleCollider>().isTrigger = false;
            letterView.Falling = true;
            yield return new WaitForSeconds(0.30f);

            
            letterView.OnJumpEnded();
            letterAnimator.SetBool("dancing", game.LLCanDance);
            

            if (game.roundsCount > 0)
                game.disableInput = false;

            yield return new WaitForSeconds(1f);

            if (game.roundsCount == 0)
            {
                game.tut.doTutorial(thisLLWrongDDs[Random.Range(0, thisLLWrongDDs.Count-1)].transform);
            }
            else
                SickLettersConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(letterView.Data, true, soundType: SickLettersConfiguration.Instance.GetVocabularySoundType());          
            
        }

        IEnumerator coJumpOut(float delay, bool endGame)
        {

            letterAnimator.SetBool("dancing", false);
            yield return new WaitForSeconds(delay );
            letterAnimator.Play("LL_idle_1", -1);
            game.manager.holeON();
            yield return new WaitForSeconds(0.25f);

            letterView.Falling = true;
            GetComponent<CapsuleCollider>().isTrigger = true;

            yield return new WaitForSeconds(.475f);
            game.Poof(transform).position += Vector3.up * 15f - Vector3.forward;
            showLLMesh(false);
            yield return new WaitForSeconds(.75f);

            if (!endGame)
            {
                transform.position = new Vector3(statPos.x, 29.04f, statPos.z);
                StartCoroutine(coJumpIn());
            }
        }

        Vector3 correctDiacriticPos;
        public void getNewLetterData()
        {
            ILivingLetterData newLetter = game.questionManager.getNewLetter();
            letterView.Init(newLetter);
            letterView.Label.GetComponent<TextRender>().SetLetterData(newLetter);


            //game.LLPrefab.dotlessLetter.text = newLetter.TextForLivingLetter;

            string letterWithoutDiac = removeDiacritics(newLetter.TextForLivingLetter);

            dotlessLetter.GetComponent<TextRender>().SetText(letterWithoutDiac, LanguageUse.Learning);

            //Deal with dotless letters
            if (!game.LettersWithDots.Contains(letterWithoutDiac))
            {
                correctDot.text = "";
                correctDotCollider.GetComponent<BoxCollider>().enabled = false;
            }

            //Deal with letters with dots
            else
            {
                correctDot.text = letterWithoutDiac;
                correctDotCollider.GetComponent<BoxCollider>().enabled = true;
            }

            //Deal with Diacritics if any
            if (letterWithoutDiac != newLetter.TextForLivingLetter)
            {
                Debug.Log(newLetter.TextForLivingLetter + " " + letterWithoutDiac +" " + letterView.Label.mesh.vertexCount);

                StopCoroutine(processCorrectDiacPose());
                StartCoroutine(processCorrectDiacPose());

                //dotlessLetter.GetComponent<TextRender>().setText(letterWithoutDiac, true);

                correctDiacCollider.GetComponent<BoxCollider>().enabled = true;
            }
            else
                correctDiacCollider.GetComponent<BoxCollider>().enabled = false;

        }

        string removeDiacritics(string letter)
        {
            //nasb
            if (letter.Contains("ً"))
            {
                correctDiac.text = "ً";
                return letter.Replace("ً", string.Empty);      
            }
            //jarr
            else if (letter.Contains("ٍ"))
            {
                correctDiac.text = "ٍ";
                return letter.Replace("ٍ", string.Empty);
            }
            //damm
            else if (letter.Contains("ٌ"))
            {
                correctDiac.text = "ٌ";
                return letter.Replace("ٌ", string.Empty);
            }
            //kasra
            else if (letter.Contains("ِ"))
            {
                correctDiac.text = "ِ";
                return letter.Replace("ِ", string.Empty);
            }
            //fatha
            else if (letter.Contains("َ"))
            {
                correctDiac.text = "َ";
                return letter.Replace("َ", string.Empty);
            }
            //damma
            else if (letter.Contains("ُ"))
            {
                correctDiac.text = "ُ";
                return letter.Replace("ُ", string.Empty);
            }
            //shadda
            else if (letter.Contains("ّ"))
            {
                correctDiac.text = "ّ";
                return letter.Replace("ّ", string.Empty);

            }
            //sukon
            else if (letter.Contains("ْ"))
            {
                correctDiac.text = "ْ";
                return letter.Replace("ْ", string.Empty);
            }
            else
            {
                correctDiac.text = "";
                return letter;
            }
        }

        IEnumerator processCorrectDiacPose() {
            while (true)
            {
                if (letterView.Label.mesh.vertexCount > 0)
                {
                    correctDiacriticPos = letterView.Label.transform.TransformPoint(Vector3.Lerp(letterView.Label.mesh.vertices[4], letterView.Label.mesh.vertices[6], 0.5f));

                    if (correctDiacCollider.transform.childCount == 0)
                    {
                        correctDiacCollider.transform.position = correctDiacriticPos;
                        correctDiac.transform.position = correctDiacriticPos;
                    }

                    Debug.DrawRay(correctDiacriticPos, -Vector3.forward * 10, Color.blue);
                    yield return null;
                }
                yield return null;
            }
        }

        int i = 0;
        public void scatterDDs(bool isSimpleLetter = true)
        {
            i = 0;
            string letter = "x";
            thisLLWrongDDs.Clear();

            if (isSimpleLetter)
               letter = game.LLPrefab.dotlessLetter.text;

            foreach (SickLettersDropZone dz in game.DropZones)
            {
                if (dz.letters.Contains(letter))
                {
                    if (i < game.Draggables.Length)
                    {
                        if (game.Draggables[i].diacritic != Diacritic.None && i>=game.numerOfWringDDs/*!game.with7arakat*/)
                        {
                            i++;
                            continue;
                        }
                        SickLettersDraggableDD newDragable = game.createNewDragable(game.Draggables[i].gameObject);
                        newDragable.transform.parent = dz.transform;
                        newDragable.transform.localPosition = Vector3.zero;
                        newDragable.transform.localEulerAngles = new Vector3(0, -90, 0);
                        //newDragable.setInitPos(newDragable.transform.localPosition);
                        newDragable.checkDDCollision = true;
                        //newDragable.isAttached = true;

                        thisLLWrongDDs.Add(newDragable);
                        game.allWrongDDs.Add(newDragable);

                        i++;
                    }
                }                
            }

            if (i == 0)
                scatterDDs(false);
        }

        void showLLMesh(bool show)
        {
            foreach (SkinnedMeshRenderer sm in LLMesh)
                sm.enabled = show;
            correctDot.gameObject.SetActive(show);
            correctDiac.gameObject.SetActive(show);
            dotlessLetter.gameObject.SetActive(show);
            if (!show)
                shadow.localScale = Vector3.zero;
        }

        IEnumerator fadShadow()
        {
            while(shadow.localScale.x < shadowStartSize.x - 0.01f)
            {
                shadow.localScale = Vector3.Lerp(shadow.localScale, shadowStartSize, Time.deltaTime*3.5f);
                yield return null;
            }
        }
    }
}
