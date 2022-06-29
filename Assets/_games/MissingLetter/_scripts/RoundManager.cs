using Antura.Audio;
using Antura.Core;
using Antura.Helpers;
using Antura.LivingLetters;
using Antura.UI;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections;
using Antura.Database;
using Antura.Keeper;
using Antura.Language;
using Antura.Tutorial;
using UnityEngine;

namespace Antura.Minigames.MissingLetter
{

    public class RoundManager
    {
        [HideInInspector]
        public int m_iCurrentRound { get; private set; }

        #region API
        public IQuestionPack CurrentQuestionPack
        {
            get { return m_oCurrQuestionPack; }
        }

        public RoundManager(MissingLetterGame _game)
        {
            m_oGame = _game;
            m_iCurrentRound = -1;
        }

        public void Initialize()
        {
            m_oCurrQuestionPack = null;

            //set the answers and questions center position
            //TODO remove magic number 20, distance beetween camera and LL
            m_v3QstPos = m_oGame.m_oQuestionCamera.position + new Vector3(0, m_oGame.m_sQuestionOffset.fHeightOffset, 20);
            m_v3AnsPos = m_oGame.m_oAnswerCamera.position + new Vector3(0, m_oGame.m_sAnswerOffset.fHeightOffset, 20);
            m_oGame.m_oLetterPrefab.GetComponent<LetterBehaviour>().mfDistanceBetweenLetters = m_oGame.m_fDistanceBetweenLetters;

            int qstPoolSize = 3;
            qstPoolSize *= (MissingLetterConfiguration.Instance.Variation != MissingLetterVariation.Phrase) ? 1 : m_oGame.m_iMaxSentenceSize;

            m_oGame.m_oLetterPrefab.GetComponent<LetterBehaviour>().SetPositions(m_v3QstPos + Vector3.right * m_oGame.m_sQuestionOffset.fINOffset, m_v3QstPos, m_v3QstPos + Vector3.right * m_oGame.m_sQuestionOffset.fOUTOffset);
            m_oQuestionPool = new GameObjectPool(m_oGame.m_oLetterPrefab, qstPoolSize, false);

            int ansPoolSize = 15;
            m_oGame.m_oLetterPrefab.GetComponent<LetterBehaviour>().SetPositions(m_v3AnsPos + Vector3.right * m_oGame.m_sAnswerOffset.fINOffset, m_v3AnsPos, m_v3AnsPos + Vector3.right * m_oGame.m_sAnswerOffset.fOUTOffset);
            m_oAnswerPool = new GameObjectPool(m_oGame.m_oLetterPrefab, ansPoolSize, false);

            m_oEmoticonsController = new MissingLetterEmoticonsController(m_oGame.m_oEmoticonsController, m_oGame.m_oEmoticonsMaterials);
        }

        public void SetTutorial(bool _enabled)
        {
            this.m_bTutorialEnabled = _enabled;
        }

        public void NewRound()
        {
            m_iCurrentRound++;

            if (m_iCurrentRound >= m_oGame.m_iRoundsLimit)
            {
                m_oGame.SetCurrentState(m_oGame.ResultState);
                return;
            }

            // Force the game to end if you reach the max score
            if (m_oGame.CurrentScore >= m_oGame.STARS_3_THRESHOLD)
            {
                m_oGame.SetCurrentState(m_oGame.ResultState);
                return;
            }

            m_oGame.SetInIdle(false);
            ExitCurrentScene();

            if (MissingLetterConfiguration.Instance.Variation == MissingLetterVariation.Phrase)
            {
                NextSentenceQuestion();
            }
            else
            {
                NextWordQuestion();
            }

            if (m_oGame.GetCurrentState() == m_oGame.PlayState || m_bTutorialEnabled)
            {
                EnterCurrentScene();
                m_oGame.StartCoroutine(Utils.LaunchDelay(2.0f, m_oGame.SetInIdle, true));
            }
        }

        public void Terminate()
        {
            if (m_iCurrentRound < m_oGame.m_iRoundsLimit)
                ExitCurrentScene();
        }

        public GameObject GetCorrectLLObject()
        {
            return m_oCorrectAnswer;
        }

        public void OnAnswerClicked(LetterBehaviour clicked)
        {
            if (m_oGame.GetCurrentState() != m_oGame.PlayState && m_oGame.GetCurrentState() != m_oGame.TutorialState)
                return;

            m_oGame.SetInIdle(false);

            //refresh the data (for graphics)
            RestoreQuestion(isCorrectAnswer(clicked.LetterData));

            foreach (GameObject _obj in m_aoCurrentAnswerScene)
            {
                _obj.GetComponent<LetterBehaviour>().SetEnabledCollider(false);
            }

            //letter animation wait for ending dancing animation, wait animator fix
            if (isCorrectAnswer(clicked.LetterData))
            {
                TutorialUI.Clear(true); // Stopping here the tutorial, if needed
                clicked.PlayAnimation(LLAnimationStates.LL_still);
                clicked.mLetter.DoHorray();

                PlayParticleSystem(m_aoCurrentQuestionScene[0].transform.position + Vector3.up * 2);

                OnResponse(true);
            }
            else
            {
                clicked.PlayAnimation(LLAnimationStates.LL_still);
                OnResponse(false);
            }
        }

        //shuffle current answer order and tell to letter change pos
        public void ShuffleLetters(float duration)
        {
            if (m_oGame.IsInIdle())
            {
                m_oGame.SetInIdle(false);
                m_aoCurrentAnswerScene.Shuffle();
                for (int i = 0; i < m_aoCurrentAnswerScene.Count; ++i)
                {
                    float offsetDuration = UnityEngine.Random.Range(-2.0f, 0.0f);
                    m_aoCurrentAnswerScene[i].GetComponent<LetterBehaviour>().ChangePos(i, m_aoCurrentAnswerScene.Count, duration + offsetDuration);
                }
                m_oGame.StartCoroutine(Utils.LaunchDelay(duration, m_oGame.SetInIdle, true));
            }
        }

        #endregion

        #region PRIVATE_FUNCTION

        public LetterBehaviour CurrentLetterBehaviour;

        void NextWordQuestion()
        {

            m_iRemovedLLDataIndex = 0;

            m_oCurrQuestionPack = MissingLetterConfiguration.Instance.Questions.GetNextQuestion();
            ILivingLetterData questionData = m_oCurrQuestionPack.GetQuestion();

            var _wrongAnswers = m_oCurrQuestionPack.GetWrongAnswers().ToList();
            var _correctAnswer = m_oCurrQuestionPack.GetCorrectAnswers().ToList().GetRandom();

            //Debug.Log("WRONG: " + m_oCurrQuestionPack.GetWrongAnswers().ToList().ToDebugStringNewline());
            //Debug.Log("CORRECT: " + m_oCurrQuestionPack.GetCorrectAnswers().ToList().ToDebugStringNewline());

            GameObject oQuestion = m_oQuestionPool.GetElement();

            LetterBehaviour qstBehaviour = oQuestion.GetComponent<LetterBehaviour>();
            qstBehaviour.Reset();
            qstBehaviour.LetterData = questionData;
            if (MissingLetterConfiguration.Instance.Variation == MissingLetterVariation.Image)
            {
                qstBehaviour.mLetter.LabelRender.SetTextUnfiltered("???");
                //qstBehaviour.LetterData = new LL_ImageData(questionData.Id);
                //qstBehaviour.mLetter.LabelRender.gameObject.SetActive(false);   // Hidden image
            }

            qstBehaviour.onEnterScene += qstBehaviour.Speak;
            qstBehaviour.onLetterBecameInvisible += OnQuestionLetterBecameInvisible;
            qstBehaviour.m_oDefaultIdleAnimation = LLAnimationStates.LL_idle;
            CurrentLetterBehaviour = qstBehaviour;
            m_aoCurrentQuestionScene.Add(oQuestion);

            m_oEmoticonsController.init(qstBehaviour.transform);

            //after insert in mCurrentQuestionScen
            if (MissingLetterConfiguration.Instance.VariationIsMissingLetter)
            {
                m_oRemovedLetter = RemoveLetterFromQuestion(_correctAnswer as LL_LetterData);
            }

            GameObject _correctAnswerObject = m_oAnswerPool.GetElement();
            LetterBehaviour corrAnsBheaviour = _correctAnswerObject.GetComponent<LetterBehaviour>();
            corrAnsBheaviour.Reset();

            corrAnsBheaviour.LetterData = _correctAnswer;
            if (MissingLetterConfiguration.Instance.Variation == MissingLetterVariation.Image)
                corrAnsBheaviour.LetterData = new LL_ImageData(_correctAnswer.Id);

            corrAnsBheaviour.onLetterBecameInvisible += OnAnswerLetterBecameInvisible;
            corrAnsBheaviour.onLetterClick += OnAnswerClicked;

            corrAnsBheaviour.m_oDefaultIdleAnimation = m_bTutorialEnabled ? LLAnimationStates.LL_still : LLAnimationStates.LL_idle;

            m_aoCurrentAnswerScene.Add(_correctAnswerObject);
            m_oCorrectAnswer = _correctAnswerObject;

            for (int i = 1; i < m_oGame.m_iNumberOfPossibleAnswers && i < _wrongAnswers.Count() + 1; ++i)
            {
                GameObject _wrongAnswerObject = m_oAnswerPool.GetElement();
                LetterBehaviour wrongAnsBheaviour = _wrongAnswerObject.GetComponent<LetterBehaviour>();
                wrongAnsBheaviour.Reset();

                var answer = _wrongAnswers[i - 1];

                /*
                if (answer is LL_LetterData)
                    ((LL_LetterData)answer).Position = questionLetterForm;
                    */

                wrongAnsBheaviour.LetterData = answer;
                if (MissingLetterConfiguration.Instance.Variation == MissingLetterVariation.Image)
                    wrongAnsBheaviour.LetterData = new LL_ImageData(answer.Id);

                wrongAnsBheaviour.onLetterBecameInvisible += OnAnswerLetterBecameInvisible;

                if (!m_bTutorialEnabled)
                {
                    wrongAnsBheaviour.onLetterClick += OnAnswerClicked;
                }
                wrongAnsBheaviour.m_oDefaultIdleAnimation = m_bTutorialEnabled ? LLAnimationStates.LL_still : LLAnimationStates.LL_idle;

                m_aoCurrentAnswerScene.Add(_wrongAnswerObject);
            }

            if (MissingLetterGame.VISUAL_DEBUG)
            {
                corrAnsBheaviour.mLetter.MarkData(MissingLetterGame.helpColor);
            }

            switch (MissingLetterConfiguration.Instance.Variation)
            {
                case MissingLetterVariation.LetterForm:
                case MissingLetterVariation.LetterInWord:
                    m_aoCurrentAnswerScene.Sort((a, b) =>
                    {
                        var first = (LL_LetterData)a.GetComponent<LetterBehaviour>().LetterData;
                        var second = (LL_LetterData)b.GetComponent<LetterBehaviour>().LetterData;

                        return Comparer<int>.Default.Compare(
                            (int)second.Form,
                            (int)first.Form);
                    });
                    break;
                case MissingLetterVariation.Phrase:
                case MissingLetterVariation.Image:
                    m_aoCurrentAnswerScene.Shuffle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            m_oCurrentCorrectAnswer = _correctAnswer;
        }

        void NextSentenceQuestion()
        {
            m_oCurrQuestionPack = MissingLetterConfiguration.Instance.Questions.GetNextQuestion();

            var phraseWords =  AppManager.I.VocabularyHelper.GetWordsFromPhraseText(((LL_PhraseData)m_oCurrQuestionPack.GetQuestion()).Data);
            if (LanguageSwitcher.I.IsLearningLanguageRTL())
                phraseWords.Reverse();

            var _correctAnswer = (LL_WordData)m_oCurrQuestionPack.GetCorrectAnswers().ToList()[0];
            var _wrongAnswers = m_oCurrQuestionPack.GetWrongAnswers().ToList();

            foreach (var _word in phraseWords)
            {
                GameObject oQuestion = m_oQuestionPool.GetElement();
                LetterBehaviour qstBehaviour = oQuestion.GetComponent<LetterBehaviour>();

                qstBehaviour.Reset();
                qstBehaviour.LetterData = _word.WD.ConvertToLivingLetterData();
                qstBehaviour.onLetterBecameInvisible += OnQuestionLetterBecameInvisible;
                qstBehaviour.m_oDefaultIdleAnimation = LLAnimationStates.LL_idle;
                qstBehaviour.SetInPhrase(m_oCurrQuestionPack.GetQuestion());
                CurrentLetterBehaviour = qstBehaviour;

                m_aoCurrentQuestionScene.Add(oQuestion);
            }

            //after insert in mCurrentQuestionScene
            m_iRemovedLLDataIndex = RemoveWordFromQuestion(phraseWords, _correctAnswer);
            m_aoCurrentQuestionScene[m_iRemovedLLDataIndex].GetComponent<LetterBehaviour>().onEnterScene += m_aoCurrentQuestionScene[m_iRemovedLLDataIndex].GetComponent<LetterBehaviour>().Speak;

            GameObject _correctAnswerObject = m_oAnswerPool.GetElement();
            LetterBehaviour corrAnsBheaviour = _correctAnswerObject.GetComponent<LetterBehaviour>();

            corrAnsBheaviour.Reset();
            corrAnsBheaviour.LetterData = _correctAnswer;
            corrAnsBheaviour.mLetter.TransformIntoImage();
            corrAnsBheaviour.onLetterBecameInvisible += OnAnswerLetterBecameInvisible;
            corrAnsBheaviour.onLetterClick += OnAnswerClicked;

            corrAnsBheaviour.m_oDefaultIdleAnimation = m_bTutorialEnabled ? LLAnimationStates.LL_still : LLAnimationStates.LL_idle;

            m_aoCurrentAnswerScene.Add(_correctAnswerObject);
            m_oCorrectAnswer = _correctAnswerObject;

            for (int i = 1; i < m_oGame.m_iNumberOfPossibleAnswers && i < _wrongAnswers.Count() + 1; ++i)
            {
                GameObject _wrongAnswerObject = m_oAnswerPool.GetElement();
                LetterBehaviour wrongAnsBheaviour = _wrongAnswerObject.GetComponent<LetterBehaviour>();
                wrongAnsBheaviour.Reset();

                var answer = _wrongAnswers[i - 1];

                wrongAnsBheaviour.LetterData = answer;
                wrongAnsBheaviour.onLetterBecameInvisible += OnAnswerLetterBecameInvisible;
                wrongAnsBheaviour.mLetter.TransformIntoImage();

                if (!m_bTutorialEnabled)
                {
                    wrongAnsBheaviour.onLetterClick += OnAnswerClicked;
                }
                wrongAnsBheaviour.m_oDefaultIdleAnimation = m_bTutorialEnabled ? LLAnimationStates.LL_still : LLAnimationStates.LL_idle;

                m_aoCurrentAnswerScene.Add(_wrongAnswerObject);
            }

            m_aoCurrentAnswerScene.Shuffle();

            m_oEmoticonsController.init(m_aoCurrentQuestionScene[m_iRemovedLLDataIndex].transform);

            m_oCurrentCorrectAnswer = _correctAnswer;
        }

        StringPart RemoveLetterFromQuestion(LL_LetterData letter)
        {
            LL_WordData word = (LL_WordData)m_oCurrQuestionPack.GetQuestion();

            LivingLetterController letterView = m_aoCurrentQuestionScene[0].GetComponent<LetterBehaviour>().mLetter;

            bool findSameForm = MissingLetterConfiguration.Instance.Variation == MissingLetterVariation.LetterForm;
            var parts = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).FindLetter(AppManager.I.DB, word.Data, letter.Data, findSameForm ? LetterEqualityStrictness.WithActualForm : LetterEqualityStrictness.Letter);
            //Debug.LogWarning("Looking for letter " + letter.Id + " into word " + word.Id);

            var partToRemove = parts[0];
            letterView.LabelRender.text = LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetWordWithMissingLetterText(word.Data, partToRemove);
            return partToRemove;
        }


        int RemoveWordFromQuestion(List<VocabularyHelper.WordDataWrapper> phraseWords, LL_WordData word)
        {
            var index = phraseWords.FindIndex(x => x.Selected);
            LivingLetterController tmp = m_aoCurrentQuestionScene[index].GetComponent<LetterBehaviour>().mLetter;
            tmp.LabelRender.text = "";
            return index;
        }


        void RestoreQuestion(bool result)
        {
            LivingLetterController letterView = m_aoCurrentQuestionScene[m_iRemovedLLDataIndex].GetComponent<LetterBehaviour>().mLetter;

            foreach (GameObject _obj in m_aoCurrentQuestionScene)
            {
                _obj.GetComponent<LetterBehaviour>().Refresh();
            }

            if (result)
                m_oEmoticonsController.EmoticonPositive();
            else
                m_oEmoticonsController.EmoticonNegative();

            //change restored color letter with tag
            Color32 markColor = result ? new Color32(0x4C, 0xAF, 0x50, 0xFF) : new Color32(0xDD, 0x2C, 0x00, 0xFF);
            string color = result ? "#4CAF50" : "#DD2C00";

            switch (MissingLetterConfiguration.Instance.Variation)
            {
                case MissingLetterVariation.Phrase:
                    letterView.LabelRender.text = "<color=" + color + ">" + letterView.LabelRender.text + "</color>";
                    break;
                case MissingLetterVariation.LetterForm:
                case MissingLetterVariation.LetterInWord:
                    var word = (LL_WordData)m_oCurrQuestionPack.GetQuestion();
                    letterView.LabelRender.text = LanguageSwitcher.LearningHelper.GetWordWithMarkedLetterText(word.Data, m_oRemovedLetter, markColor, MarkType.SingleLetter);
                    break;
                case MissingLetterVariation.Image:
                    break;
            }
        }

        void EnterCurrentScene()
        {
            int _pos = 0;
            foreach (GameObject _obj in m_aoCurrentQuestionScene)
            {
                _obj.GetComponent<LetterBehaviour>().EnterScene(_pos, m_aoCurrentQuestionScene.Count());
                ++_pos;
            }

            _pos = 0;

            for (int i = 0; i < m_aoCurrentAnswerScene.Count; ++i)
            {
                GameObject _obj = m_aoCurrentAnswerScene[i];

                _obj.GetComponent<LetterBehaviour>().EnterScene(i, m_aoCurrentAnswerScene.Count());
            }
        }

        void ExitCurrentScene()
        {
            if (m_oCurrQuestionPack != null)
            {

                foreach (GameObject _obj in m_aoCurrentQuestionScene)
                {
                    _obj.GetComponent<LetterBehaviour>().ExitScene();
                }
                m_aoCurrentQuestionScene.Clear();

                foreach (GameObject _obj in m_aoCurrentAnswerScene)
                {
                    _obj.GetComponent<LetterBehaviour>().ExitScene();
                }
                m_aoCurrentAnswerScene.Clear();
            }
        }


        void OnQuestionLetterBecameInvisible(GameObject _obj)
        {
            m_oQuestionPool.FreeElement(_obj);
            _obj.GetComponent<LetterBehaviour>().onLetterBecameInvisible -= OnQuestionLetterBecameInvisible;
        }

        void OnAnswerLetterBecameInvisible(GameObject _obj)
        {
            m_oAnswerPool.FreeElement(_obj);
            _obj.GetComponent<LetterBehaviour>().onLetterBecameInvisible -= OnAnswerLetterBecameInvisible;
        }

        private bool isCorrectAnswer(ILivingLetterData data)
        {
            return DataMatchingHelper.IsDataMatching(data, m_oCurrentCorrectAnswer, LetterEqualityStrictness.WithVisualForm);
        }

        //call after clicked answer animation
        private void OnResponse(bool correct)
        {
            if (correct)
            {
                AudioManager.I.PlaySound(Sfx.LetterHappy);
                DoWinAnimations();
            }
            else
            {
                AudioManager.I.PlaySound(Sfx.LetterSad);
                DoLoseAnimations();
            }

            if (onAnswered != null)
            {
                m_oGame.OnResult(correct);
                m_oGame.StartCoroutine(AnswerReactionCO(correct));
            }
        }

        private IEnumerator AnswerReactionCO(bool correct)
        {
            yield return new WaitForSeconds(2.0f);
            if (MissingLetterConfiguration.Instance.Variation == MissingLetterVariation.Phrase)
            {
                var question = m_oGame.m_oRoundManager.CurrentQuestionPack.GetQuestion();
                MissingLetterConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(question, keeperMode: KeeperMode.NativeNoSubtitles, soundType: MissingLetterConfiguration.Instance.GetVocabularySoundType());
                yield return new WaitForSeconds(2.0f);
            }
            onAnswered?.Invoke(correct);
        }

        private void PlayParticleSystem(Vector3 _pos)
        {
            m_oGame.m_oParticleSystem.SetActive(true);
            m_oGame.m_oParticleSystem.transform.position = _pos;
            m_oGame.m_oParticleSystem.GetComponent<ParticleSystem>().Play();
            m_oGame.StartCoroutine(Utils.LaunchDelay(1.5f, delegate
            { m_oGame.m_oParticleSystem.SetActive(false); }));
        }

        //win animation: quesion high five, correct answer dancing other horray
        private void DoWinAnimations()
        {
            for (int i = 0; i < m_aoCurrentAnswerScene.Count; ++i)
            {
                if (isCorrectAnswer(m_aoCurrentAnswerScene[i].GetComponent<LetterBehaviour>().LetterData))
                {
                    m_aoCurrentAnswerScene[i].GetComponent<LetterBehaviour>().PlayAnimation(LLAnimationStates.LL_dancing);
                    m_aoCurrentAnswerScene[i].GetComponent<LetterBehaviour>().mLetter.DoDancingWin();
                    m_aoCurrentAnswerScene[i].GetComponent<LetterBehaviour>().LightOn();
                }
                else
                {
                    //random delay poof of wrong answer
                    m_oGame.StartCoroutine(Utils.LaunchDelay(UnityEngine.Random.Range(0, 0.5f), PoofLetter, m_aoCurrentAnswerScene[i]));
                }
            }
            m_aoCurrentQuestionScene[m_iRemovedLLDataIndex].GetComponent<LetterBehaviour>().LightOn();

            if (MissingLetterConfiguration.Instance.Variation == MissingLetterVariation.Image)
            {
                m_aoCurrentQuestionScene[m_iRemovedLLDataIndex].GetComponent<LetterBehaviour>().LetterData = new LL_ImageData(
                    m_aoCurrentQuestionScene[m_iRemovedLLDataIndex].GetComponent<LetterBehaviour>().LetterData.Id
                    );
                m_aoCurrentQuestionScene[m_iRemovedLLDataIndex].GetComponent<LetterBehaviour>().mLetter.LabelRender.gameObject.SetActive(true);
            }

            for (int i = 0; i < m_aoCurrentQuestionScene.Count; ++i)
            {
                m_aoCurrentQuestionScene[i].GetComponent<LetterBehaviour>().mLetter.DoHighFive();
            }
        }

        //poof the gameobject letter
        private void PoofLetter(GameObject letter)
        {
            if (!letter.GetComponent<LetterBehaviour>())
            {
                Debug.LogWarning("Cannot poof letter " + letter.name);
                return;
            }

            letter.GetComponent<LetterBehaviour>().mLetter.Poof();
            letter.transform.position = Vector3.zero;
            AudioManager.I.PlaySound(Sfx.Poof);
        }

        //lose animation: quesion and correct answer angry other crouch
        private void DoLoseAnimations()
        {
            for (int i = 0; i < m_aoCurrentQuestionScene.Count; ++i)
            {
                m_aoCurrentQuestionScene[i].GetComponent<LetterBehaviour>().mLetter.DoAngry();
            }

            for (int i = 0; i < m_aoCurrentAnswerScene.Count; ++i)
            {
                if (m_aoCurrentAnswerScene[i].GetComponent<LetterBehaviour>().LetterData.Id == m_oCurrQuestionPack.GetCorrectAnswers().ElementAt(0).Id)
                {
                    m_aoCurrentAnswerScene[i].GetComponent<LetterBehaviour>().mLetter.DoAngry();
                }
                else
                {
                    m_aoCurrentAnswerScene[i].GetComponent<LetterBehaviour>().mLetter.Crouching = true;
                }
            }

            if (MissingLetterConfiguration.Instance.Variation == MissingLetterVariation.Image)
            {
                m_aoCurrentQuestionScene[m_iRemovedLLDataIndex].GetComponent<LetterBehaviour>().LetterData = new LL_ImageData(
                    m_aoCurrentQuestionScene[m_iRemovedLLDataIndex].GetComponent<LetterBehaviour>().LetterData.Id
                );
                m_aoCurrentQuestionScene[m_iRemovedLLDataIndex].GetComponent<LetterBehaviour>().mLetter.LabelRender.gameObject.SetActive(true);
            }
        }
        #endregion

        #region VARS

        private MissingLetterGame m_oGame;

        private int m_iRemovedLLDataIndex;

        private IQuestionPack m_oCurrQuestionPack;
        private ILivingLetterData m_oCurrentCorrectAnswer;
        private StringPart m_oRemovedLetter;

        private GameObjectPool m_oAnswerPool;
        private GameObjectPool m_oQuestionPool;

        private List<GameObject> m_aoCurrentQuestionScene = new List<GameObject>();
        private List<GameObject> m_aoCurrentAnswerScene = new List<GameObject>();
        private GameObject m_oCorrectAnswer;

        private Vector3 m_v3AnsPos;
        private Vector3 m_v3QstPos;

        public event Action<bool> onAnswered;

        private MissingLetterEmoticonsController m_oEmoticonsController;

        private bool m_bTutorialEnabled;

        #endregion

    }
}
