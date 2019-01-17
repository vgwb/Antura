using UnityEngine;
using System.Collections;
using Antura.Audio;
using Antura.Tutorial;

namespace Antura.Minigames.SickLetters
{
    public class SickLettersTutorial : MonoBehaviour {

        public Vector3[] path;
        public bool draw = false;
        public float tutorialStartDelay;
        public int  repeatMax = 3, repeatConter = 0;
        float repeatDely = 3;
        // Use this for initialization
        SickLettersGame game;
        SickLettersDraggableDD curDD;

        void Start() {
            game = GetComponent<SickLettersGame>();

            Debug.Log("Enabled tutorial? " + game.TutorialEnabled);

            if (!game.TutorialEnabled)
            {
                game.disableInput = false;
                game.roundsCount = 1;
            }
            else
                StartCoroutine(coDoTutorial());
        }

        public void doTutorial(Transform start = null)
        {

            if (game.roundsCount > 0)
            {
                game.buttonRepeater.SetActive(true);
                return;
            }

            if (start)
                path[0] = start.position;

            else
            {
                //TutorialUI.Clear(true);
                foreach (SickLettersDraggableDD dd in game.LLPrefab.thisLLWrongDDs)
                    if (!dd.deattached && dd.transform.parent)
                    {
                        path[0] = dd.transform.position;
                        break;
                    }
            }

            repeatConter = 0;

        }

        void onTutStart()
        {
            game.disableInput = false;
            AudioManager.I.PlayDialogue(Database.LocalizationDataId.SickLetters_lettername_Tuto, ()=>{ game.buttonRepeater.SetActive(true); });//, () => { WidgetSubtitles.I.gameObject.SetActive(false); });
            //WidgetSubtitles.I.DisplaySentence(Db.LocalizationDataId.SickLetters_Tuto, 5, false);
        }

        void onTutEnd()
        {
            TutorialUI.Clear(true);
            
        }

        IEnumerator coDoTutorial(Transform start = null)
        {
            yield return new WaitForSeconds(tutorialStartDelay);

            onTutStart();

            while (game.roundsCount <= 0)
            {
                /*if (game.roundsCount > 0)
                {
                    TutorialUI.Clear(true);
                    break;
                }*/

                repeatConter++;
                TutorialUI.DrawLine(path, TutorialUI.DrawLineMode.FingerAndArrow);
                yield return new WaitForSeconds(repeatDely);

            }

            onTutEnd();
        }


    }
}