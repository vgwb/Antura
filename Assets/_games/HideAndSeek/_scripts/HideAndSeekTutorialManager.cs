using Antura.Audio;
using Antura.LivingLetters;
using Antura.Tutorial;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Minigames.HideAndSeek
{
    public class HideAndSeekTutorialManager : MonoBehaviour
    {
        void OnEnable()
        {
            foreach (var a in ArrayLetters)
            {
                a.GetComponent<HideAndSeekLetterController>().onLetterTouched += CheckResult;
            }

            foreach (var a in ArrayTrees)
            {
                a.GetComponent<HideAndSeekTreeController>().onTreeTouched += MoveObject;
            }
            SetupTutorial();

            phase = 0;
        }

        void OnDisable()
        {
            foreach (var a in ArrayLetters)
            {
                if (a != null)
                {
                    a.GetComponent<HideAndSeekLetterController>().onLetterTouched -= CheckResult;
                }
            }

            foreach (var a in ArrayTrees)
            {
                if (a != null)
                {
                    a.GetComponent<HideAndSeekTreeController>().onTreeTouched -= MoveObject;
                }
            }
        }

        void Update()
        {
            if (timeFinger > 0 && Time.time > timeFinger)
                ShowFinger();
        }

        private ILivingLetterData GetCorrectAnswer()
        {
            return currentQuestion.GetCorrectAnswers().ToList()[0];
        }

        void SetupTutorial()
        {
            currentQuestion = HideAndSeekConfiguration.Instance.Questions.GetNextQuestion();
            List<ILivingLetterData> letterList = new List<ILivingLetterData>();

            ILivingLetterData right = currentQuestion.GetCorrectAnswers().ToList()[0];
            ILivingLetterData wrong = currentQuestion.GetWrongAnswers().ToList()[0];

            if (HideAndSeekConfiguration.Instance.Variation == HideAndSeekVariation.Image)
            {
                right = new LL_ImageData(right.Id);
                wrong = new LL_ImageData(wrong.Id);
            }

            letterList.Add(right);
            letterList.Add(wrong);

            // Set the wrong answer
            ArrayLetters[0].transform.position = ArrayPlaceholder[0].transform.position;
            ArrayLetters[0].GetComponent<HideAndSeekLetterController>().id = 3;

            ArrayLetters[0].GetComponentInChildren<LivingLetterController>().Init(wrong);
            ArrayTrees[0].GetComponent<SphereCollider>().enabled = true;
            ArrayLetters[0].GetComponent<CapsuleCollider>().enabled = false;


            // Set the correct answer
            ArrayLetters[1].transform.position = ArrayPlaceholder[1].transform.position;
            ArrayLetters[1].GetComponent<HideAndSeekLetterController>().id = 5;

            ArrayLetters[1].GetComponentInChildren<LivingLetterController>().Init(right);

            StartCoroutine(WaitTutorial());
        }

        private IEnumerator WaitTutorial()
        {
            bool canContinue = false;
            game.PlayTutorial(() => canContinue = true);
            while (!canContinue) yield return null;

            var winInitialDelay = 1f;
            yield return new WaitForSeconds(winInitialDelay);

            var answerAudio = game.Context.GetAudioManager().PlayVocabularyData(GetCorrectAnswer());

            yield return new WaitForSeconds(answerAudio.Duration);

            buttonRepeater.SetActive(true);

            ShowFinger();
        }

        void ShowFinger()
        {
            Vector3 offset = new Vector3(0f, 3f, -1.5f);
            Vector3 offsetCentral = new Vector3(0f, 3f, -2f);
            Vector3 offsetFirst = new Vector3(0.5f, 3f, -2f);


            switch (phase)
            {
                case 0:
                    TutorialUI.ClickRepeat(ArrayTrees[0].transform.position + offsetFirst, animDuration, 1);
                    break;
                case 1:
                    TutorialUI.ClickRepeat(ArrayLetters[0].transform.position + offset, animDuration, 1);
                    break;
                case 2:
                    TutorialUI.ClickRepeat(ArrayTrees[1].transform.position + offsetCentral, animDuration, 1);
                    break;
                case 3:
                    TutorialUI.ClickRepeat(ArrayLetters[1].transform.position + offset, animDuration, 1);
                    break;

            }

            timeFinger = Time.time + animDuration + timeToWait;
        }

        void MoveObject(int id)
        {
            HideAndSeekConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.BushRustlingOut);
            if (ArrayLetters.Length > 0)
            {
                script = ArrayLetters[GetIdFromPosition(id)].GetComponent<HideAndSeekLetterController>();
                script.MoveTutorial();
            }

            if (GetIdFromPosition(id) == 0)
            {
                ArrayTrees[0].GetComponent<SphereCollider>().enabled = false;
                ArrayTrees[1].GetComponent<SphereCollider>().enabled = true; // skip to phase 2
                phase = 2;
                TutorialUI.Clear(false);
                timeFinger = Time.time + animDuration + timeToWait;
            }


            if (GetIdFromPosition(id) == 1)
            {
                ArrayTrees[1].GetComponent<SphereCollider>().enabled = false;
                phase = 3;
                TutorialUI.Clear(false);
                timeFinger = Time.time + animDuration + timeToWait;
            }

        }

        void CheckResult(int id)
        {
            letterInAnimation = GetIdFromPosition(id);
            HideAndSeekLetterController script = ArrayLetters[letterInAnimation].GetComponent<HideAndSeekLetterController>();
            if (script.view.Data.Id == GetCorrectAnswer().Id)
            {
                script.PlayResultAnimation(true);
                AudioManager.I.PlaySound(Sfx.Win);
                AudioManager.I.PlaySound(Sfx.OK);
                script.GetComponent<EmoticonsAnimator>().DoCorrect();
                StartCoroutine(GoToPlay());
                phase = -1;
                buttonRepeater.SetActive(false);
            }
            else
            {
                script.PlayResultAnimation(false);
                ArrayTrees[1].GetComponent<SphereCollider>().enabled = true;
                phase = 2;
                TutorialUI.Clear(false);
                AudioManager.I.PlaySound(Sfx.Lose);
                AudioManager.I.PlaySound(Sfx.KO);
                script.GetComponent<EmoticonsAnimator>().DoWrong();
                timeFinger = Time.time + animDuration + timeToWait;
            }

        }

        private IEnumerator GoToPlay()
        {
            var winInitialDelay = 3f;
            yield return new WaitForSeconds(winInitialDelay);

            foreach (GameObject x in ArrayLetters)
            {
                if (x.activeSelf)
                {
                    x.GetComponent<LivingLetterController>().Poof();
                    AudioManager.I.PlaySound(Sfx.Poof);
                    x.SetActive(false);
                    x.GetComponent<CapsuleCollider>().enabled = true;
                }
            }

            var delay = 1f;
            yield return new WaitForSeconds(delay);

            game.SetCurrentState(game.PlayState);

        }

        int GetIdFromPosition(int index)
        {
            for (int i = 0; i < ArrayLetters.Length; ++i)
            {
                if (ArrayLetters[i].GetComponent<HideAndSeekLetterController>().id == index)
                {
                    return i;
                }
            }
            return -1;
        }

        public void RepeatAudio()
        {
            game.Context.GetAudioManager().PlayVocabularyData(GetCorrectAnswer());
        }

        public GameObject[] ArrayTrees;
        public GameObject[] ArrayLetters;
        public Transform[] ArrayPlaceholder;

        private HideAndSeekLetterController script;

        private int letterInAnimation = -1;

        private IQuestionPack currentQuestion;

        public HideAndSeekGame game;

        int phase;

        public float timeToWait = 0f;//
        float timeFinger = -1f;

        float animDuration = 1f;//

        public GameObject buttonRepeater;
    }
}
