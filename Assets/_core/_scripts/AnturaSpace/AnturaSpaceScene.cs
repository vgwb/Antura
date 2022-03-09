using System;
using Antura.AnturaSpace.UI;
using Antura.Audio;
using Antura.Core;
using Antura.FSM;
using Antura.Minigames;
using Antura.Tutorial;
using Antura.UI;
using System.Collections.Generic;
using System.Linq;
using Antura.Database;
using Antura.Dog;
using Antura.Helpers;
using Antura.Keeper;
using Antura.Profile;
using UnityEngine;

namespace Antura.AnturaSpace
{
    /// <summary>
    /// Manages the AnturaSpace scene.
    /// </summary>
    public class AnturaSpaceScene : SceneBase
    {
        private const int MaxSpawnedObjectsInScene = 5;

        [Header("References")]
        public AnturaLocomotion Antura;

        public AnturaSpaceUI UI;
        public ShopActionsManager ShopActionsManager;

        public Transform SceneCenter;
        public Pedestal RotatingBase;
        public Transform AttentionPosition;
        public GameObject PoofPrefab;

        public ThrowableObject NextObjectToCatch
        {
            get
            {
                var nextObject = spawnedObjects.FirstOrDefault(x => x.Catchable);
                if (nextObject == null)
                { return null; }
                return nextObject;
            }
        }

        public AnturaIdleState Idle;
        public AnturaCustomizationState Customization;
        public AnturaDrawingAttentionState DrawingAttention;
        public AnturaAnimateState Animate;
        public AnturaSleepingState Sleeping;
        public AnturaWaitingThrowState WaitingThrow;
        public AnturaCatchingState Catching;

        private StateMachineManager stateManager = new StateMachineManager();

        public AnturaState CurrentState
        {
            get { return (AnturaState)stateManager.CurrentState; }
            set { stateManager.CurrentState = value; }
        }

        public bool HasPlayerBones
        {
            get
            {
                return AppManager.I.Player.GetTotalNumberOfBones() > 0;
            }
        }

        public float AnturaHappiness { get; private set; }
        public bool InCustomizationMode { get; private set; }
        public float LastTimeCatching { get; set; }

        public Action onEatObject;
        public Action onHitObject;

        protected override void Init()
        {
            InitStates();

            UI.onEnterCustomization += OnEnterCustomization;
            UI.onExitCustomization += OnExitCustomization;

            Antura.onTouched += () =>
            {
                if (CurrentState != null)
                {
                    CurrentState.OnTouched();
                }
            };

            LastTimeCatching = Time.realtimeSinceStartup;
        }

        public void InitStates()
        {
            Idle = new AnturaIdleState(this);
            Customization = new AnturaCustomizationState(this);
            DrawingAttention = new AnturaDrawingAttentionState(this);
            Sleeping = new AnturaSleepingState(this);
            WaitingThrow = new AnturaWaitingThrowState(this);
            Catching = new AnturaCatchingState(this);
            Animate = new AnturaAnimateState(this);
        }

        protected override void Start()
        {
            base.Start();

            GlobalUI.ShowPauseMenu(false);
            //ShowBackButton();

            CurrentState = Idle;

            ShopActionsManager.Initialise();
            UI.Initialise();

            TutorialManager tutorialManager = gameObject.GetComponentInChildren<AnturaSpaceTutorialManager>();
            tutorialManager.HandleStart();

            if (!tutorialManager.IsRunning)
            {
                // Also play a welcome
                KeeperManager.I.PlayDialogue(new[]
                {
                    LocalizationDataId.AnturaSpace_Welcome_1,
                    LocalizationDataId.AnturaSpace_Welcome_2,
                    LocalizationDataId.AnturaSpace_Welcome_3
                }.GetRandom());
            }
        }

        private float anturaSpacePlayTime = 0.0f;

        public void Update()
        {
            AnturaHappiness -= Time.deltaTime / 40.0f;
            if (AnturaHappiness < 0)
            {
                AnturaHappiness = 0;
            }

            anturaSpacePlayTime += Time.deltaTime;

            stateManager.Update(Time.deltaTime);

            UI.BonesCount = AppManager.I.Player.GetTotalNumberOfBones();

            if (DraggedTransform != null && !Input.GetMouseButton(0))
            {
                AudioManager.I.PlaySound(Sfx.ThrowObj);
                DraggedTransform.GetComponent<ThrowableObject>().LetGo();
                DraggedTransform = null;
            }
        }

        public void FixedUpdate()
        {
            stateManager.UpdatePhysics(Time.fixedDeltaTime);
        }

        public void ShowBackButton()
        {
            GlobalUI.ShowBackButton(true, OnExit);
        }
        public void HideBackButton()
        {
            GlobalUI.ShowBackButton(false);
        }

        void OnExit()
        {
            AppManager.I.NavigationManager.GoToNextScene();
        }

        void OnEnterCustomization()
        {
            HideBackButton();

            AudioManager.I.PlaySound(Sfx.UIButtonClick);
            InCustomizationMode = true;
            CurrentState = Customization;
        }

        void OnExitCustomization()
        {
            if (!tutorialManager.IsRunning || tutorialManager.CurrentRunningPhase == FirstContactPhase.AnturaSpace_Exit)
            {
                ShowBackButton();
            }

            AudioManager.I.PlaySound(Sfx.UIButtonClick);
            InCustomizationMode = false;
            CurrentState = Idle;
        }


        #region Throwable actions

        public Transform DraggedTransform { get; private set; }
        public Transform ObjectSpawnPivotTr;
        private List<ThrowableObject> spawnedObjects = new List<ThrowableObject>();

        public bool CanSpawnMoreObjects
        {
            get { return spawnedObjects.Count < MaxSpawnedObjectsInScene; }
        }

        public ThrowableObject ThrowObject(ThrowableObject ObjectPrefab)
        {
            if (DraggedTransform != null)
            { return null; }

            if (CanSpawnMoreObjects)
            {
                AudioManager.I.PlaySound(Sfx.ThrowObj);

                var throwableObject = SpawnNewObject(ObjectPrefab);
                throwableObject.SimpleThrow();
                return throwableObject;
            }
            return null;
        }

        public ThrowableObject DragObject(ThrowableObject ObjectPrefab)
        {
            if (DraggedTransform != null)
            { return null; }

            if (CanSpawnMoreObjects)
            {
                ShopDecorationsManager.I.SetContextNewPlacement();
                var throwableObject = SpawnNewObject(ObjectPrefab);
                DraggedTransform = throwableObject.transform;
                throwableObject.Drag();
                throwableObject.OnRelease += ShopDecorationsManager.I.SetContextPurchase;
                return throwableObject;
            }
            return null;
        }

        private ThrowableObject SpawnNewObject(ThrowableObject ObjectPrefab)
        {
            Antura.BoneSmell();

            var newObjectGo = Instantiate(ObjectPrefab.gameObject);
            newObjectGo.SetActive(true);
            newObjectGo.transform.position = ObjectSpawnPivotTr.position;
            var throwableObject = newObjectGo.GetComponent<ThrowableObject>();
            spawnedObjects.Add(throwableObject);
            return throwableObject;
        }

        public void EatObject(ThrowableObject throwableObject)
        {
            if (spawnedObjects.Remove(throwableObject))
            {
                AudioManager.I.PlaySound(Sfx.EggMove);
                var poof = Instantiate(PoofPrefab).transform;
                poof.position = throwableObject.transform.position;

                foreach (var ps in poof.GetComponentsInChildren<ParticleSystem>())
                {
                    var main = ps.main;
                    main.scalingMode = ParticleSystemScalingMode.Hierarchy;
                }

                poof.localScale = poof.localScale * 0.5f;
                poof.gameObject.AddComponent<AutoDestroy>().duration = 2;
                AudioManager.I.PlaySound(Sfx.Poof);
                AnturaHappiness += 0.2f;
                if (AnturaHappiness > 1)
                {
                    AnturaHappiness = 1;
                }

                if (onEatObject != null)
                    onEatObject();

                Destroy(throwableObject.gameObject);
            }
        }

        public void HitObject(ThrowableObject throwableObject)
        {
            if (spawnedObjects.Remove(throwableObject))
            {
                AudioManager.I.PlaySound(Sfx.EggMove);

                AnturaHappiness += 0.2f;
                if (AnturaHappiness > 1)
                {
                    AnturaHappiness = 1;
                }


                if (onHitObject != null)
                    onHitObject();

                throwableObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 20, ForceMode.Impulse);
            }
        }

        #endregion

        public void TriggerSceneExit()
        {
            AnturaModelManager.I.SaveAnturaCustomization();
            AppManager.I.Services.Analytics.TrackCustomization(AppManager.I.Player.CurrentAnturaCustomizations, anturaSpacePlayTime);
        }
    }
}
