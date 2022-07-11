using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.EditorUtilities;
using Antura.Keeper;
using Antura.LivingLetters;
using Antura.Tutorial;
using Antura.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Antura.Minigames.SickLetters
{

    public class SickLettersGame : MiniGameController
    {
        public SickLettersLLPrefab LLPrefab;
        public SickLettersAntura antura;
        public SickLettersVase scale;
        public GameObject buttonRepeater;
        public GameObject DropZonesGO;
        public Transform[] safeDropZones;
        public UnityEngine.Animation hole;
        public SickLettersTutorial tut;
        public bool lastMoveIsCorrect;

        public SickLettersCamera slCamera;
        public SickLettersGameManager manager;

        public int maxRoundsCount = 6, roundsCount = 1, wrongDraggCount = 0;
        [HideInInspector]
        public bool disableInput;

        #region Score

        public override int MaxScore => STARS_3_THRESHOLD;

        private int STARS_1_THRESHOLD => Mathf.RoundToInt(targetScale / 3f);
        private int STARS_2_THRESHOLD => Mathf.RoundToInt(targetScale * 2f / 3f);
        private int STARS_3_THRESHOLD => Mathf.RoundToInt(targetScale);

        public int CurrentStars
        {
            get
            {
                if (CurrentScore < STARS_1_THRESHOLD)
                    return 0;
                if (CurrentScore < STARS_2_THRESHOLD)
                    return 1;
                if (CurrentScore < STARS_3_THRESHOLD)
                    return 2;
                return 3;
            }
        }

        #endregion

        public bool TutorialEnabled => GetConfiguration().TutorialEnabled;

        public int gameDuration = 120, targetScale = 45, maxReachedCounter;
        public float vaseWidth = 5.20906f;
        public bool LLCanDance = false, with7arakat;
        public int numberOfWrongDDs = 3;

        public SickLettersDraggableDD[] Draggables;

        [HideInInspector]
        public SickLettersDropZone[] DropZones;

        [HideInInspector]
        public List<SickLettersDraggableDD> allWrongDDs = new List<SickLettersDraggableDD>();

        public IntroductionGameState IntroductionState { get; private set; }
        public QuestionGameState QuestionState { get; private set; }
        public PlayGameState PlayState { get; private set; }
        public ResultGameState ResultState { get; private set; }

        public QuestionsManager questionManager;

        protected override void OnInitialize(IGameContext context)
        {
            IntroductionState = new IntroductionGameState(this);
            QuestionState = new QuestionGameState(this);
            PlayState = new PlayGameState(this);
            ResultState = new ResultGameState(this);
            questionManager = new QuestionsManager();

            hole.gameObject.SetActive(false);

            manager = GetComponent<SickLettersGameManager>();
            DropZones = DropZonesGO.GetComponentsInChildren<SickLettersDropZone>();
            tut = GetComponent<SickLettersTutorial>();

            scale.game = this;
            scale.transform.localScale = new Vector3(vaseWidth, scale.transform.localScale.y, scale.transform.localScale.z);
        }

        protected override FSM.IState GetInitialState()
        {
            return IntroductionState;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return SickLettersConfiguration.Instance;
        }

        public SickLettersDraggableDD createNewDragable(GameObject go)
        {
            return Instantiate(go).GetComponent<SickLettersDraggableDD>();
        }

        public Transform Poof(Transform t)
        {
            SickLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Poof);
            var puffGo = GameObject.Instantiate(LLPrefab.GetComponent<LivingLetterController>().poofPrefab);
            puffGo.AddComponent<AutoDestroy>().duration = 2;
            puffGo.SetActive(true);
            puffGo.transform.position = t.position - Vector3.forward * 2;

            ParticleSystem[] PSs = puffGo.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in PSs)
            {
                var main = ps.main;
                main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            }

            puffGo.transform.localScale *= t.lossyScale.y * 1.2f / 3f;

            return puffGo.transform;
        }

        public bool checkForNextRound()
        {

            if (checkSucess())
                return false;

            if (StateManager.CurrentState == ResultState)
                return false;


            if (wrongDDsOnLL() == 0)
            {
                if (roundsCount == maxRoundsCount)
                {
                    SetCurrentState(ResultState);
                    return false;
                }

                roundsCount++;
                disableInput = true;
                LLPrefab.letterAnimator.SetBool("dancing", false);
                LLPrefab.letterAnimator.Play("LL_idle_1", -1);

                if (lastMoveIsCorrect)
                {
                    LLPrefab.letterView.DoHorray();
                    SickLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Win);
                    Context.GetAudioManager().PlayVocabularyData(LLPrefab.letterView.Data, true, soundType: SickLettersConfiguration.Instance.GetVocabularySoundType());
                    LLPrefab.jumpOut(1.5f);
                }
                else
                {
                    LLPrefab.jumpOut(0.5f);
                    if (roundsCount > 0)
                        Context.GetAudioManager().PlayVocabularyData(LLPrefab.letterView.Data, true, soundType: SickLettersConfiguration.Instance.GetVocabularySoundType());
                }
                if (roundsCount == 1)
                {
                    Context.GetOverlayWidget().Initialize(true, true, false);
                    Context.GetOverlayWidget().SetClockDuration(gameDuration);
                    scale.counter = 0;

                    SickLettersConfiguration.Instance.Context.GetAudioManager().PlayMusic(Music.Theme10);
                }

                return true;
            }
            else
            {
                tut.doTutorial();
                return false;
            }


        }

        public bool checkSucess()
        {
            if (scale.counter == targetScale)
            {
                manager.sucess();
                return true;
            }
            else
                return false;
        }


        public int wrongDDsOnLL()
        {
            int i = 0;
            foreach (SickLettersDraggableDD dd in LLPrefab.thisLLWrongDDs)
            {
                if (dd && dd.transform.root.GetComponent<LivingLetterController>())
                    i++;
            }

            return i;
        }


        private void SetDifficulty(float diff, float vaseWidth, bool LLCanDance, bool with7arakat)
        {
            if (diff > 0.666f)
                scale.transform.localScale = new Vector3(vaseWidth, scale.transform.localScale.y, 7.501349f);
            else
                scale.transform.localScale = new Vector3(vaseWidth, scale.transform.localScale.y, scale.transform.localScale.z);

            this.LLCanDance = LLCanDance;
            this.with7arakat = with7arakat;
            numberOfWrongDDs = targetScale / 6;
        }

        float prevDiff = -1;
        public void ProcessDifficulty(float diff)
        {
            if (prevDiff == diff)
                return;
            prevDiff = diff;

            if (diff < 0.333f)
                SetDifficulty(diff, 5.20906f, false, false);
            else if (diff < 0.666f)
                SetDifficulty(diff, 4.0f, false, true);
            else
                SetDifficulty(diff, 3.0f, false, true);
        }

        public void onWrongMove(bool isDDCorrect = false)
        {
            lastMoveIsCorrect = false;
            goodCommentCounter = 0;
            Context.GetLogManager().OnAnswered(LLPrefab.letterView.Data, false);
            if (isDDCorrect)
            {
                KeeperManager.I.PlayDialogue("Keeper_Bad_" + Random.Range(1, 6), KeeperMode.LearningNoSubtitles);
                TutorialUI.MarkNo(scale.transform.position - Vector3.forward * 2 + Vector3.up, TutorialUI.MarkSize.Big);
                Context.GetAudioManager().PlaySound(Sfx.KO);
            }

        }


        int goodCommentCounter;
        public void onCorrectMove(SickLettersDraggableDD dd)
        {
            if (goodCommentCounter == 3 || !lastMoveIsCorrect)
            {
                KeeperManager.I.PlayDialogue("Keeper_Good_" + UnityEngine.Random.Range(1, 13), KeeperMode.LearningNoSubtitles);
                goodCommentCounter = 0;
            }

            scale.counter++;
            goodCommentCounter++;
            lastMoveIsCorrect = true;
            dd.deattached = true;


            if (!dd.touchedVase)
                dd.boxCollider.isTrigger = false;

            TutorialUI.MarkYes(scale.transform.position - Vector3.forward * 2 + Vector3.up, TutorialUI.MarkSize.Big);
            Context.GetAudioManager().PlaySound(Sfx.OK);
            Context.GetLogManager().OnAnswered(LLPrefab.letterView.Data, true);

            if (scale.counter > maxReachedCounter)
            {
                Context.GetOverlayWidget().SetStarsThresholds(STARS_1_THRESHOLD, STARS_2_THRESHOLD, STARS_3_THRESHOLD);
                CurrentScore = scale.counter;
            }

            dd.isInVase = true;
            dd.gameObject.tag = "Finish";

            checkForNextRound();
        }

        public void RepeatAudio()
        {
            if (LLPrefab.letterView && LLPrefab.letterView.Data != null && !disableInput)
                Context.GetAudioManager().PlayVocabularyData(LLPrefab.letterView.Data, true, soundType: SickLettersConfiguration.Instance.GetVocabularySoundType());
        }


#if UNITY_EDITOR
        [ContextMenu("Recreate Shape Letters Data")]
        public void RecreateShapeLettersData()
        {
            // Recreate shape letter data from the current scene
            foreach (var letterData in AppManager.I.DB.GetAllLetterData())
            {
                var path = "Assets/_config/Resources/arabic/ShapeData/Letters/";
                var asset = CustomAssetUtility.CreateAsset<ShapeLetterData>(path, $"shapedata_{letterData.Id}");

                string s = $"DROP ZONES FOR {letterData.Isolated}";
                var zones = new List<Vector2>();
                foreach (SickLettersDropZone dz in DropZones)
                {
                    var letters = dz.letters.Split(' ');
                    var found = letters.Any(x => x.Trim() == letterData.GetStringForDisplay());
                    if (found)
                    {
                        var v = new Vector2(dz.transform.localPosition.x, dz.transform.localPosition.z);
                        s += "\n" + v;
                        //  s += "\n" + dz.name + ": " + dz.letters.Split(' ').ToJoinedString();
                        zones.Add(v);
                    }
                }

                asset.EmptyZones = zones.ToArray();
                EditorUtility.SetDirty(asset);

                Debug.LogError(s);
            }
            AssetDatabase.SaveAssets();

        }
#endif
    }

}
