using System;
using System.Collections;
using System.Collections.Generic;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using DG.Tweening;
using Antura.Teacher;
using UnityEngine;

namespace Antura.GamesSelector
{
    /// <summary>
    /// Handles the GamesSelector logic, showing bubbles for all mini-games that should be played next.
    ///
    /// This can be instantiated in any scene.
    /// To create and start it automatically, just call the static <see cref="Show"/> method.
    /// When the user has opened all bubbles and the app should move on, the <see cref="OnComplete"/> event will be dispatched.
    /// </summary>
    [RequireComponent(typeof(GamesSelectorTrailsManager))]
    public class GamesSelector : MonoBehaviour
    {
        #region Events

        public static event Action OnComplete;

        static void DispatchOnComplete()
        {
            if (OnComplete != null)
            {
                OnComplete();
            }
        }

        #endregion

        [Tooltip("Automatically grabs the minigames list from the teacher on startup, unless Show was called directly")]
        public bool AudoLoadMinigamesOnStartup = true;

        [Tooltip("Distance to keep from camera")]
        public float DistanceFromCamera = 14;

        [Tooltip("Delay between the moment the last bubble is opened and the OnComplete event is dispatched")]
        public float EndDelay = 2;

        [Header("Debug")]
        public bool SimulateForDebug; // If TRUE creates games list for debug

        const string ResourceID = "GamesSelector";
        const string ResourcePath = "Prefabs/UI/" + ResourceID;
        private static GamesSelector instance;
        private GamesSelectorTrailsManager trailsManager;
        private GamesSelectorTutorial tutorial;
        private List<MiniGameData> games;
        private GamesSelectorBubble mainBubble;
        private readonly List<GamesSelectorBubble> bubbles = new List<GamesSelectorBubble>();
        private TrailRenderer currTrail;
        private Camera cam;
        private Transform camT;
        private bool cutAllowed = true;
        private bool isDragging;
        private int totOpenedBubbles;
        private Sequence showTween;

        #region Unity

        void Awake()
        {
            instance = this;
            trailsManager = GetComponent<GamesSelectorTrailsManager>();
            tutorial = GetComponentInChildren<GamesSelectorTutorial>(true);
        }

        void Start()
        {
            if (mainBubble == null)
            {
                mainBubble = GetComponentInChildren<GamesSelectorBubble>();
                mainBubble.gameObject.SetActive(false);
            }
            if (cam == null)
            {
                cam = Camera.main;
                camT = Camera.main.transform;
            }

            if (AudoLoadMinigamesOnStartup && games == null)
            {
                AutoLoadMinigames();
            }
        }

        void OnDestroy()
        {
            if (instance == this)
            { instance = null; }
            StopAllCoroutines();
            showTween.Kill(true);
            OnComplete -= GoToMinigame;
        }

        void Update()
        {
            if (Time.timeScale <= 0)
            {
                // Prevent actions when pause menu is open
                if (isDragging)
                {
                    StopDrag();
                }
                return;
            }

            if (!Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0))
            {
                return;
            }

            Vector3 mouseP = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane + 10));
            if (Input.GetMouseButtonDown(0))
            {
                // Start drag/click
                isDragging = true;
                currTrail = trailsManager.Spawn(mouseP);
            }
            if (isDragging)
            {
                Update_Dragging(mouseP);
            }
            if (Input.GetMouseButtonUp(0))
            {
                // Stop drag/click
                StopDrag();
            }
        }

        void Update_Dragging(Vector3 _mouseP)
        {
            trailsManager.SetPosition(currTrail, _mouseP);
            if (cutAllowed)
            {
                Update_CheckHitBubble(_mouseP);
            }
        }

        void Update_CheckHitBubble(Vector3 _mouseP)
        {
            _mouseP += -camT.forward * 3;
            RaycastHit hit;
            if (!Physics.Raycast(new Ray(_mouseP, camT.forward), out hit))
            {
                return;
            }

            GamesSelectorBubble hitBubble = null;
            foreach (GamesSelectorBubble bubble in bubbles)
            {
                if (hit.transform != bubble.Cover.transform)
                {
                    continue;
                }
                hitBubble = bubble;
                break;
            }
            if (hitBubble == null)
            {
                return;
            }

            HitBubble(hitBubble);
        }

        public void HitBubble(GamesSelectorBubble hitBubble)
        {
            if (tutorial.isPlaying)
            {
                tutorial.Stop();
            }
            hitBubble.Open();
            totOpenedBubbles++;
            if (totOpenedBubbles == bubbles.Count)
            {
                // All bubbles opened: final routine
                cutAllowed = false;
                StartCoroutine(CO_EndCoroutine());
            }
        }

        void LateUpdate()
        {
            // Adapt to camera
            transform.rotation = camT.rotation;
            transform.position = camT.position + camT.forward * DistanceFromCamera;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Instantiates and starts the GamesSelector routine,
        /// or just resets and starts it if it's already present in the scene.
        /// The selector will automatically adapt to the position of the main camera (or the given one).
        /// </summary>
        /// <param name="_games">The list of selected games, ordered as they will happen in the session</param>
        /// <param name="_cam">Camera to use. If NULL uses main camera automatically</param>
        public static void Show(List<MiniGameData> _games, Camera _cam = null)
        {
            GamesSelector gs;
            if (instance != null)
            {
                gs = instance;
            }
            else
            {
                GameObject go = Instantiate(Resources.Load<GameObject>(ResourcePath));
                go.name = ResourceID;
                gs = go.GetComponent<GamesSelector>();
            }
            gs.SimulateForDebug = false;
            gs.games = _games;
            gs.cam = _cam == null ? Camera.main : _cam;
            gs.camT = gs.cam.transform;
            gs.ResetAndLayout();
            gs.StartCoroutine(gs.CO_AnimateEntrance());
        }

        #endregion

        #region Methods

        void ResetAndLayout()
        {
            // Reset
            if (mainBubble == null)
            {
                mainBubble = this.GetComponentInChildren<GamesSelectorBubble>();
            }
            foreach (GamesSelectorBubble bubble in bubbles)
            {
                if (bubble != mainBubble)
                {
                    Destroy(bubble.gameObject);
                }
            }
            bubbles.Clear();

            // Layout
            const float bubblesDist = 0.1f;
            int totBubbles = games.Count;
            float bubbleW = mainBubble.Main.GetComponent<Renderer>().bounds.size.x;
            float area = totBubbles * bubbleW + (totBubbles - 1) * bubblesDist;
            float startX = -area * 0.5f + bubbleW * 0.5f;
            //if (!LanguageSwitcher.LearningRTL) startX *= -1;

            for (int i = 0; i < totBubbles; ++i)
            {
                int iRTL = LanguageSwitcher.LearningRTL ? totBubbles - i - 1 : i;

                MiniGameData mgData = games[iRTL];
                GamesSelectorBubble bubble = i == 0 ? mainBubble : (GamesSelectorBubble)Instantiate(mainBubble, this.transform);
                bubble.Setup(mgData, startX + (bubbleW + bubblesDist) * i);
                bubbles.Add(bubble);
            }
        }

        void StopDrag()
        {
            isDragging = false;
            if (currTrail != null)
            {
                trailsManager.Despawn(currTrail);
                currTrail = null;
            }
        }

        #region MiniGame Selection Getters

        void AutoLoadMinigames()
        {
            OnComplete += GoToMinigame;

            // TODO refactor: the current list of minigames should be injected by the navigation manager instead
            var minigames = AppManager.I.NavigationManager.CurrentPlaySessionMiniGames;
            if (minigames.Count > 0)
            {
                Show(minigames);
            }
        }

        // TODO refactor: this should be injected
        void GoToMinigame()
        {
            AppManager.I.NavigationManager.GoToNextScene();
        }

        #endregion

        IEnumerator CO_AnimateEntrance()
        {
            foreach (GamesSelectorBubble bubble in bubbles)
            {
                bubble.gameObject.SetActive(false);
            }
            yield return null;
            yield return null;

            showTween = DOTween.Sequence();
            for (int i = 0; i < bubbles.Count; ++i)
            {
                GamesSelectorBubble bubble = bubbles[i];
                bubble.gameObject.SetActive(true);
                showTween.Insert(i * 0.05f, bubble.transform.DOScale(0.0001f, 0.6f).From().SetEase(Ease.OutElastic, 1, 0))
                    .InsertCallback(i * 0.1f, () => AudioManager.I.PlaySound(Sfx.BalloonPop));
            }
            yield return showTween.WaitForCompletion();

            if (totOpenedBubbles == 0)
            {
                tutorial.Play(bubbles);
            }
        }

        IEnumerator CO_EndCoroutine()
        {
            yield return new WaitForSeconds(EndDelay);
            if (DebugConfig.I.DebugLogEnabled)
            {
                Debug.Log("<b>GamesSelector</b> > Complete");
            }
            DispatchOnComplete();
        }

        #endregion
    }
}
