using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.Language;
using Antura.LivingLetters;
using Antura.UI;
using TMPro;

namespace Antura.Minigames.SickLetters
{

    public enum letterStatus { idle, angry, horry }

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


        public void jumpOut(float delay = 0, bool endGame = false)
        {
            StartCoroutine(coJumpOut(delay, endGame));
        }

        IEnumerator coJumpIn()
        {
            showLLMesh(true);
            getNewLetterData();
            scatterWrongDDs();
            StartCoroutine(fadeShadow());

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
                game.tut.doTutorial(thisLLWrongDDs[Random.Range(0, thisLLWrongDDs.Count - 1)].transform);
            }
            else
                SickLettersConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(letterView.Data, true, soundType: SickLettersConfiguration.Instance.GetVocabularySoundType());

        }

        IEnumerator coJumpOut(float delay, bool endGame)
        {

            letterAnimator.SetBool("dancing", false);
            yield return new WaitForSeconds(delay);
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

            //newLetter = new LL_LetterData(AppManager.I.DB.GetLetterDataById("lam"));

            letterView.Init(newLetter);
            letterView.LabelRender.SetLetterData(newLetter);

            string extractedDiacritic;
            string letterWithoutDiac;
            ExtractDiacritics(newLetter.TextForLivingLetter, out letterWithoutDiac, out extractedDiacritic);
            correctDiac.text = extractedDiacritic;

            // @note: TODO: this Text UI  uses a special font without dots
            dotlessLetter.GetComponent<TextRender>().SetText(letterWithoutDiac, LanguageUse.Learning);

            var letterData = (newLetter as LL_LetterData).Data;

            // Deal with Dots if any
            if (letterData.HasDot)
            {
                // @note: this Text UI uses a special font with dots only and no letters
                correctDot.text = letterWithoutDiac;
                correctDotCollider.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                correctDot.text = "";
                correctDotCollider.GetComponent<BoxCollider>().enabled = false;
            }

            // Deal with Diacritics if any
            if (letterData.HasDiacritic)// letterWithoutDiac != newLetter.TextForLivingLetter)
            {
                StopCoroutine(processCorrectDiacPose());
                StartCoroutine(processCorrectDiacPose());
                correctDiacCollider.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                correctDiacCollider.GetComponent<BoxCollider>().enabled = false;
            }
        }

        // TODO: move to LanguageHelper and make this a "RemoveAllSymbols" instead
        // this could remove diacritics & accents
        void ExtractDiacritics(string letter, out string letterWithoutDiacritic, out string extractedDiacritic)
        {
            //nasb
            if (letter.Contains("ً"))
            {
                extractedDiacritic = "ً";
                letterWithoutDiacritic = letter.Replace("ً", string.Empty);
            }
            //jarr
            else if (letter.Contains("ٍ"))
            {
                extractedDiacritic = "ٍ";
                letterWithoutDiacritic = letter.Replace("ٍ", string.Empty);
            }
            //damm
            else if (letter.Contains("ٌ"))
            {
                extractedDiacritic = "ٌ";
                letterWithoutDiacritic = letter.Replace("ٌ", string.Empty);
            }
            //kasra
            else if (letter.Contains("ِ"))
            {
                extractedDiacritic = "ِ";
                letterWithoutDiacritic = letter.Replace("ِ", string.Empty);
            }
            //fatha
            else if (letter.Contains("َ"))
            {
                extractedDiacritic = "َ";
                letterWithoutDiacritic = letter.Replace("َ", string.Empty);
            }
            //damma
            else if (letter.Contains("ُ"))
            {
                extractedDiacritic = "ُ";
                letterWithoutDiacritic = letter.Replace("ُ", string.Empty);
            }
            //shadda
            else if (letter.Contains("ّ"))
            {
                extractedDiacritic = "ّ";
                letterWithoutDiacritic = letter.Replace("ّ", string.Empty);
            }
            //sukon
            else if (letter.Contains("ْ"))
            {
                extractedDiacritic = "ْ";
                letterWithoutDiacritic = letter.Replace("ْ", string.Empty);
            }
            else
            {
                extractedDiacritic = "";
                letterWithoutDiacritic = letter;
            }
        }

        IEnumerator processCorrectDiacPose()
        {
            while (true)
            {
                if (letterView.LabelRender.mesh.vertexCount > 0)
                {
                    correctDiacriticPos = letterView.LabelRender.transform.TransformPoint(Vector3.Lerp(letterView.LabelRender.mesh.vertices[4], letterView.LabelRender.mesh.vertices[6], 0.5f));

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

        int foundWrongDDCount = 0;
        public void scatterWrongDDs(bool useLetter = true)
        {
            foundWrongDDCount = 0;
            thisLLWrongDDs.Clear();

            Vector2[] emptyZones;
            if (useLetter)
            {
                var shapeData = AppManager.I.AssetManager.GetShapeLetterData((letterView.Data as LL_LetterData).Data);
                emptyZones = shapeData.EmptyZones;
            }
            else
            {
                // Fallback
                emptyZones = new[] { new Vector2(-0.055f, 1.054f) };
            }

            for (int iZone = 0; iZone < emptyZones.Length; iZone++)
            {

                if (foundWrongDDCount >= game.Draggables.Length)
                    continue;

                if (game.Draggables[foundWrongDDCount].IsDiacritic && foundWrongDDCount >= game.numberOfWrongDDs)
                {
                    foundWrongDDCount++;
                    continue;
                }

                SickLettersDraggableDD newDragable = game.createNewDragable(game.Draggables[foundWrongDDCount].gameObject);
                newDragable.transform.SetParent(game.DropZonesGO.transform);
                newDragable.SetEmptyZone(emptyZones[iZone]);
                newDragable.checkDDCollision = true;
                thisLLWrongDDs.Add(newDragable);
                game.allWrongDDs.Add(newDragable);
                foundWrongDDCount++;
            }

            if (foundWrongDDCount == 0)
            {
                scatterWrongDDs(false);
            }
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

        IEnumerator fadeShadow()
        {
            while (shadow.localScale.x < shadowStartSize.x - 0.01f)
            {
                shadow.localScale = Vector3.Lerp(shadow.localScale, shadowStartSize, Time.deltaTime * 3.5f);
                yield return null;
            }
        }
    }
}
