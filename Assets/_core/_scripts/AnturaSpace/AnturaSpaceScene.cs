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
using UnityEngine.Serialization;

namespace Antura.AnturaSpace
{
    /// <summary>
    /// Manages the AnturaSpace scene.
    /// </summary>
    public class AnturaSpaceScene : SceneBase
    {
        private const int MaxSpawnedObjectsInScene = 5;

        [Header("References")]
        public AnturaLocomotion AnturaMain;
        public AnturaLocomotion AnturaSide;

        public AnturaSpaceUI UI;
        public ShopActionsManager ShopActionsManager;

        public Transform SceneCenter;
        public Transform SideAnturaPivot;
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

            AnturaMain.PetSwitcher.LoadPet(AppManager.I.Player.PetData.SelectedPet);
            AnturaSide.PetSwitcher.LoadPet(AppManager.I.Player.PetData.SelectedPet == AnturaPetType.Dog ? AnturaPetType.Cat : AnturaPetType.Dog);
            AnturaSide.SetTarget(SideAnturaPivot,true);

            ReassignCallbacks();

            LastTimeCatching = Time.realtimeSinceStartup;
        }

        private void OnAnturaMainTouched()
        {
            if (CurrentState != null)
            {
                CurrentState.OnTouched();
            }
        }

        private void OnAnturaSideTouched()
        {
            if (ShopDecorationsManager.I.ShopContext != ShopContext.Closed
                 && ShopDecorationsManager.I.ShopContext != ShopContext.Hidden)
            {
                return;
            }

            AnturaMain.AnimController.State = AnturaAnimationStates.idle;
            AnturaSide.AnimController.State = AnturaAnimationStates.idle;

            AnturaSide.SetTarget(SceneCenter, false);
            AnturaMain.SetTarget(SideAnturaPivot, true);
            (AnturaMain, AnturaSide) = (AnturaSide, AnturaMain);

            AnturaMain.PetSwitcher.SwitchPet(alsoLoadInScene:false);

            ReassignCallbacks();
        }

        public void ReassignCallbacks()
        {
            AnturaMain.PetSwitcher.ModelManager.SetMainPet(true);
            AnturaSide.PetSwitcher.ModelManager.SetMainPet(false);

            AnturaMain.onTouched -= OnAnturaMainTouched;
            AnturaMain.onTouched -= OnAnturaSideTouched;

            AnturaSide.onTouched -= OnAnturaMainTouched;
            AnturaSide.onTouched -= OnAnturaSideTouched;

            AnturaMain.onTouched += OnAnturaMainTouched;
            AnturaSide.onTouched += OnAnturaSideTouched;
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

            // Handle side creature
            if (anturaSpacePlayTime > 2f && AnturaSide.HasReachedTarget)
            {
                AnturaSide.AnimController.State = AnturaAnimationStates.sleeping;
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
            if (FirstContactManager.I.IsPhaseUnlockedAndNotCompleted(FirstContactPhase.AnturaSpace_Exit))
            {
                FirstContactManager.I.CompletePhase(FirstContactPhase.AnturaSpace_Exit);
            }

            if (FirstContactManager.I.CurrentPhaseInSequence == FirstContactPhase.Assessment_Skip)
            {
                GlobalUI.ShowPrompt(LocalizationDataId.UI_Assessment_Skip,
                    () =>
                    {
                        FirstContactManager.I.CompletePhase(FirstContactPhase.Assessment_Skip);
                        // Skip to the assessment test playsession
                        var fakeAssessmentJP = new JourneyPosition(0, 0, 100);
                        AppManager.I.Player.SetCurrentJourneyPosition(fakeAssessmentJP);
                        AppManager.I.NavigationManager.GoToPlaySession(fakeAssessmentJP);

                    },
                    () =>
                    {
                        FirstContactManager.I.CompletePhase(FirstContactPhase.Assessment_Skip);
                        AppManager.I.NavigationManager.GoToNextScene();
                    });
            }
            else
            {
                AppManager.I.NavigationManager.GoToNextScene();
            }
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
            AnturaMain.BoneSmell();

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
            try
            {
                AnturaMain.PetSwitcher.ModelManager.SaveAnturaCustomization();
                AppManager.I.Services.Analytics.TrackCustomization(AppManager.I.Player.CurrentSingleAnturaCustomization,
                    anturaSpacePlayTime);
            }
            catch (Exception)
            {
                // Ignore this exception, can happen sometimes when exiting antura space, but it does not seem to be an issue as the save is triggered earlier still
            }
        }
    }
}
