using Antura.Audio;
using Antura.LivingLetters;
using Antura.Helpers;
using DG.Tweening;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.HideAndSeek
{
    public class HideAndSeekGameManager : MonoBehaviour
    {
        void OnEnable()
        {
            foreach (var a in ArrayLetters)
            {
                a.GetComponent<HideAndSeekLetterController>().game = game;
                a.GetComponent<HideAndSeekLetterController>().onLetterTouched += CheckResult;
                a.GetComponent<HideAndSeekLetterController>().onLetterReturned += OnLetterReturned;
            }

            foreach (var a in ArrayTrees)
            {
                a.GetComponent<HideAndSeekTreeController>().onTreeTouched += MoveObject;
            }
        }

        void OnDisable()
        {
            foreach (var a in ArrayLetters)
            {
                if (a == null)
                {
                    continue;
                }

                a.GetComponent<HideAndSeekLetterController>().onLetterTouched -= CheckResult;
                a.GetComponent<HideAndSeekLetterController>().onLetterReturned -= OnLetterReturned;
            }

            foreach (var a in ArrayTrees)
            {
                if (a != null)
                {
                    a.GetComponent<HideAndSeekTreeController>().onTreeTouched -= MoveObject;
                }
            }
        }

        void Start()
        {
            for (int i = 0; i < MAX_OBJECT; ++i)
            {
                UsedPlaceholder[i] = false;
            }
            AnturaEnterScene();
        }

        float anturaEnterTimer = 5;
        void Update()
        {
            if (StartNewRound && game.inGame && Time.time > time + timeToWait)
            {
                NewRound();
            }

            if (game.inGame && !Antura.IsFollowing)
            {
                anturaEnterTimer -= Time.deltaTime;

                if (anturaEnterTimer < 0)
                {
                    anturaEnterTimer = Random.Range(5, 10);
                    AnturaEnterScene();
                }
            }
        }

        void MoveObject(int id)
        {
            if (ArrayLetters.Length > 0)
            {
                HideAndSeekConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.BushRustlingOut);
                script = ArrayLetters[GetIdFromPosition(id)].GetComponent<HideAndSeekLetterController>();
                if (script.Move())
                {
                    LockTree(id, true);
                }
            }
        }

        int GetIdFromPosition(int index)
        {
            for (int i = 0; i < ArrayLetters.Length; ++i)
            {
                if (ArrayLetters[i].GetComponent<HideAndSeekLetterController>().id == index)
                    return i;
            }
            return -1;
        }

        private ILivingLetterData GetCorrectAnswer()
        {
            //correctAnswer is the first answer
            return currentQuestion.GetCorrectAnswers().ToList()[0];
        }

        public void RepeatAudio()
        {
            game.Context.GetAudioManager().PlayVocabularyData(GetCorrectAnswer());
        }


        private IEnumerator DelayAnimation()
        {
            game.PlayState.gameTime.Stop();

            var initialDelay = 3f;
            yield return new WaitForSeconds(initialDelay);

            foreach (GameObject x in ArrayLetters)
            {
                x.GetComponent<LivingLetterController>().Poof();
                AudioManager.I.PlaySound(Sfx.Poof);
                x.SetActive(false);
            }

            var delay = 0.5f;
            yield return new WaitForSeconds(delay);

            StartNewRound = true;
            SetTime();
        }

        void OnLetterReturned(int id)
        {
            if (isRoundRunning && !game.isTimesUp)
                LockTree(id, false);
        }

        void CheckResult(int id)
        {
            if (game.isTimesUp)
                return;

            letterInAnimation = GetIdFromPosition(id);
            HideAndSeekLetterController script = ArrayLetters[letterInAnimation].GetComponent<HideAndSeekLetterController>();
            if (script.view.Data.Id == GetCorrectAnswer().Id)
            {
                isRoundRunning = false;
                LockTrees();
                LockLetters(true);
                StartCoroutine(DelayAnimation());
                script.PlayResultAnimation(true);
                script.GetComponent<EmoticonsAnimator>().DoCorrect();
                game.OnResult(GetCorrectAnswer(), true);
                buttonRepeater.SetActive(false);
                AudioManager.I.PlaySound(Sfx.Win);
                AudioManager.I.PlaySound(Sfx.OK);
            }
            else
            {
                AudioManager.I.PlaySound(Sfx.KO);
                game.OnResult(GetCorrectAnswer(), false);
                RemoveLife();
                script.PlayResultAnimation(false);
                script.GetComponent<EmoticonsAnimator>().DoWrong();
                if (lifes == 0)
                {
                    isRoundRunning = false;
                    LockTrees();
                    LockLetters(true);
                    AudioManager.I.PlaySound(Sfx.Lose);
                    StartCoroutine(DelayAnimation());
                    buttonRepeater.SetActive(false);
                }
            }
        }

        void RemoveLife()
        {
            switch (--lifes)
            {
                case 2:
                    game.Context.GetOverlayWidget().SetLives(2);
                    break;
                case 1:
                    game.Context.GetOverlayWidget().SetLives(1);
                    break;
                case 0:
                    game.Context.GetOverlayWidget().SetLives(0);
                    break;
            }
        }

        void SetFullLife()
        {
            lifes = 3;
            game.Context.GetOverlayWidget().SetLives(3);
        }

        public void SetTime()
        {
            time = Time.time;
        }

        public void LockTrees()
        {
            for (int i = 0; i < MAX_OBJECT; ++i)
            {
                ArrayTrees[i].GetComponent<SphereCollider>().enabled = false;
            }
        }

        public void LockTree(int id, bool toLock)
        {
            ArrayTrees[id].GetComponent<SphereCollider>().enabled = !toLock;
        }

        void LockLetters(bool toLock)
        {
            for (int i = 0; i < MAX_OBJECT; ++i)
            {
                ArrayLetters[i].GetComponent<CapsuleCollider>().enabled = !toLock;
            }
        }

        public void ClearRound()
        {
            for (int i = 0; i < MAX_OBJECT; ++i)
            {
                ArrayLetters[i].SetActive(true);
                ArrayLetters[i].transform.position = originLettersPlaceholder.position;
                ArrayLetters[i].GetComponent<HideAndSeekLetterController>().ResetLetter();
                UsedPlaceholder[i] = false;
            }
        }

        public void NewRound()
        {
            ClearRound();

            isRoundRunning = true;
            currentQuestion = HideAndSeekConfiguration.Instance.Questions.GetNextQuestion();
            StartNewRound = false;
            SetFullLife();
            FreePlaceholder = MAX_OBJECT;

            ActiveTrees = new List<GameObject>();

            List<ILivingLetterData> letterList = new List<ILivingLetterData>();
            foreach (var letter in currentQuestion.GetCorrectAnswers())
            {
                ILivingLetterData data = letter;
                if (HideAndSeekConfiguration.Instance.Variation == HideAndSeekVariation.Image)
                    data = new LL_ImageData(data.Id);

                letterList.Add(data);
            }

            int numWrong = Mathf.RoundToInt(2 + game.Difficulty * 4);

            foreach (var letter in currentQuestion.GetWrongAnswers())
            {
                if (numWrong-- == 0)
                    break;

                ILivingLetterData data = letter;
                if (HideAndSeekConfiguration.Instance.Variation == HideAndSeekVariation.Image)
                    data = new LL_ImageData(data.Id);

                letterList.Add(data);
            }

            ActiveLetters = letterList.Count;

            for (int i = 0; i < ActiveLetters; ++i)
            {
                int index = getRandomPlaceholder();
                if (index != -1)
                {

                    ActiveTrees.Add(ArrayTrees[index]);
                    Vector3 hiddenPosition = new Vector3(ArrayPlaceholder[index].transform.position.x, ArrayPlaceholder[index].transform.position.y - 3f, ArrayPlaceholder[index].transform.position.z + 3f);
                    ArrayLetters[i].transform.position = hiddenPosition;
                    HideAndSeekLetterController scriptComponent = ArrayLetters[i].GetComponent<HideAndSeekLetterController>();
                    scriptComponent.SetStartPosition(ArrayPlaceholder[index].transform.position);
                    scriptComponent.id = index;
                    SetLetterMovement(index, scriptComponent);
                    ArrayLetters[i].GetComponentInChildren<LivingLetterController>().Init(letterList[i]);

                    ArrayLetters[i].transform.DOMove(ArrayPlaceholder[index].transform.position, 0.5f);
                }
            }

            LockLetters(false);

            StartCoroutine(DisplayRound_Coroutine());

        }

        public void SetLetterMovement(int placeholder, HideAndSeekLetterController script)
        {
            if (placeholder == 1)
                script.SetMovement(MovementType.OnlyRight);
            else if (placeholder == 2)
                script.SetMovement(MovementType.OnlyLeft);
            else
                script.SetMovement(MovementType.Normal);
        }

        private IEnumerator DisplayRound_Coroutine()
        {
            foreach (GameObject tree in ActiveTrees)
            {
                tree.GetComponent<SphereCollider>().enabled = true;
            }

            var winInitialDelay = 0.5f;
            yield return new WaitForSeconds(winInitialDelay);

            game.Context.GetAudioManager().PlayVocabularyData(GetCorrectAnswer());
            game.PlayState.gameTime.Start();

            buttonRepeater.SetActive(true);
        }

        public int getRandomPlaceholder()
        {
            int result = 0;
            int position = Random.Range(0, FreePlaceholder--);

            for (int i = 0; i < UsedPlaceholder.Length; ++i)
            {
                if (UsedPlaceholder[i] == true)
                    continue;
                if (result == position)
                {
                    UsedPlaceholder[i] = true;
                    return i;
                }
                result++;
            }
            return -1;
        }

        void AnturaEnterScene()
        {
            var path = anturaPaths.GetRandom();

            Antura.FollowPath(path);
        }


        #region VARIABLES
        bool isRoundRunning = false;
        bool StartNewRound = true;
        int lifes;
        int ActiveLetters;
        private const int MAX_OBJECT = 7;
        private int FreePlaceholder;

        public AnturaPathFollower Antura;

        public GameObject[] ArrayTrees;
        private List<GameObject> ActiveTrees;

        public Transform[] ArrayPlaceholder;
        private bool[] UsedPlaceholder = new bool[MAX_OBJECT];

        public Transform originLettersPlaceholder;

        public GameObject[] ArrayLetters;

        private int letterInAnimation = -1;

        private HideAndSeekLetterController script;

        public HideAndSeekGame game;

        private IQuestionPack currentQuestion;

        public float timeToWait = 1.0f;
        private float time;

        public GameObject buttonRepeater;

        public AnturaPath[] anturaPaths;
        #endregion
    }
}
