using Antura.Audio;
using Antura.Database;
using Antura.Keeper;
using Antura.Tutorial;
using UnityEngine;
using System.Collections;
using TMPro;

namespace Antura.Minigames.DancingDots
{
    public class DancingDotsTutorial : MonoBehaviour
    {

        public TextMeshPro hintDot;
        public DancingDotsDiacriticPosition[] targetDDs;
        //public Vector3[] path;
        public float repeatDelay = 3;

        private bool doTutOnDots;

        DancingDotsGame gameManager;
        Transform source, target;
        Vector3 targetPosition;
        DancingDotsDraggableDot currentDD;

        void Awake()
        {
            gameManager = GetComponent<DancingDotsGame>();
        }
        void Start()
        {
            StartCoroutine(UpdateTutorialCo());

            //warm up
            TutorialUI.DrawLine(-100 * Vector3.up, -100 * Vector3.up, TutorialUI.DrawLineMode.Arrow);
        }


        public IEnumerator DoTutorial()
        {
            if (!gameManager.isTutRound)
                yield break;

            yield return sayTut(0f);

            Debug.Log("Tutorial started");

            doTutOnDots = false;

            foreach (DancingDotsDraggableDot dd in gameManager.dragableDots)
                if (dd.isCorrect && dd.gameObject.activeSelf)
                {
                    currentDD = dd;
                    doTutOnDots = true;
                    source = currentDD.transform;
                    break;
                }


            if (!doTutOnDots)
            {

                foreach (DancingDotsDraggableDot dd in gameManager.dragableDiacritics)
                    if (dd.isCorrect)
                    {
                        currentDD = dd;
                        source = currentDD.transform;
                        break;
                    }

                foreach (DancingDotsDiacriticPosition dd in targetDDs)
                    if (dd.gameObject.activeInHierarchy)
                    {
                        target = dd.transform;
                        break;
                    }
            }

        }

        IEnumerator UpdateTutorialCo()
        {
            while (true)
            {
                if (gameManager.isTutRound)
                {
                    if (currentDD)
                    {

                        yield return new WaitForSeconds(repeatDelay);


                        if (currentDD.isDragging || !gameManager.isTutRound)
                        {
                            yield return null;
                            continue;
                        }

                        if (doTutOnDots)
                            targetPosition = hintDot.transform.TransformPoint(Vector3.Lerp(hintDot.mesh.vertices[0], hintDot.mesh.vertices[2], 0.5f));
                        else
                            targetPosition = target.position;

                        TutorialUI.DrawLine(source.position - Vector3.forward * 2, targetPosition - Vector3.forward * 2, TutorialUI.DrawLineMode.FingerAndArrow);

                        //yield return new WaitForSeconds(repeatDelay/2);
                    }
                }
                yield return null;
            }
        }

        IEnumerator sayTut(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameManager.PlayTutorial();
        }
    }
}
