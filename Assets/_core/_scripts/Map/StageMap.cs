using Antura.Core;
using DG.DeExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using DG.DeInspektor.Attributes;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Antura.Map
{
    /// <summary>
    /// A single stage map.
    /// Contains all pins for the current stage.
    /// </summary>
    public class StageMap : MonoBehaviour
    {
        [Header("Stage")]
        public int stageNumber;

        [Header("Settings")]
        public Color color;
        public Transform cameraPivotStart;
        public Transform cameraPivotEnd;

        [Header("References")]
        public Transform dotsPivot;
        public Transform mapFloor;
        public GameObject pinPrefab;
        public GameObject dotPrefab;
        public GameObject ropePrefab;

        // Configuration
        private static float preferredDotsSpan = 5.0f;
        private static float startAppearDuration = 0.1f;
        private static float appearSpeedupMultiplier = 0.95f;
        private static float minAppearDuration = 0.05f;
        private static float pinAppearDuration = 2f;

        #region Properties

        // Current position of the PlayerPin in this stage map
        public int CurrentPinIndex { get; private set; }

        // Max position index PlayerPin can take in this stage map
        public int MaxUnlockedPinIndex { get; private set; }

        public void ForceCurrentPinIndex(int newIndex)
        {
            CurrentPinIndex = newIndex;
            //Debug.Log("STAGE " + stageNumber + "SET CURRENT PIN INDEX: " + CurrentPinIndex);
        }

        public Vector3 CurrentPlayerPosVector
        {
            get {
                return mapLocations[CurrentPinIndex].Position;
            }
        }

        public JourneyPosition CurrentPlayerPosJourneyPosition
        {
            get { return mapLocations[CurrentPinIndex].JourneyPos; }
        }

        public Pin FirstPin
        {
            get { return playPins[0]; }
        }

        public List<Pin> Pins
        {
            get {
                return playPins;
            }
        }

        public Pin PinForJourneyPosition(JourneyPosition jp)
        {
            return playPins.FirstOrDefault(p => p.JourneyPos.Equals(jp));
        }

        public Pin PinForIndex(int index)
        {
            return playPins.FirstOrDefault(p => p.pinIndex == index);
        }
        #endregion

        #region State

        // Pins: one per Play Session
        private List<Pin> playPins;

        [HideInInspector]
        private bool stageUnlocked;   // at least one PS for this stage is unlocked
        [HideInInspector]
        private bool wholeStageUnlocked;    // all PS and LB of this stage are unlocked

        #endregion

        public List<IMapLocation> mapLocations = new List<IMapLocation>();

        public int MapDirection => cameraPivotStart.root.localScale.x > 0 ? 1 : -1;

        #region Editor Utilities
#if UNITY_EDITOR
        [DeMethodButton("Rename Pins")]
        public void RenamePins()
        {
            var allPins = new List<Pin>(gameObject.GetComponentsInChildren<Pin>());
            for (var index = 0; index < allPins.Count; index++) {
                var pin = allPins[index];
                pin.gameObject.name = "Pin_" + stageNumber + "_" + (index + 1);
                EditorUtility.SetDirty(pin.gameObject);
            }
        }

        [DeMethodButton("Update Path")]
        public void UpdatePath()
        {
            DistributePins(false);
        }

        [DeMethodButton("Generate Path")]
        public void GeneratePath()
        {
            DistributePins(true);
        }

        private void DistributePins(bool zToo)
        {
            // DEPRECATED CODE (we use DistributePinsDuringPlay now)
            var pins = new List<Pin>(gameObject.GetComponentsInChildren<Pin>());
            var start_x = cameraPivotStart.localPosition.x * MapDirection;
            var end_x = cameraPivotEnd.localPosition.x * MapDirection;
            var total_pins = pins.Count;
            var delta_x = MapDirection * Mathf.Abs((end_x - start_x) / (total_pins - 1));

            var rnd = new System.Random(0);

            for (var index = 0; index < total_pins; index++) {
                var pin = pins[index];

                Undo.RecordObject(pin.gameObject, "Changed pin " + index);

                float pinZ = pin.transform.localPosition.z;
                if (zToo) {
                    // Distribute pins on Z following a random walk flow
                    const int MAX_Z = 25;
                    const int MIN_Z = -20;
                    const int Z_RND_RANGE = 20;
                    pinZ = index > 0
                        ? (pins[index - 1].transform.localPosition.z +
                           Mathf.Lerp((float)rnd.NextDouble(), -Z_RND_RANGE, Z_RND_RANGE))
                          : Mathf.Lerp((float) rnd.NextDouble(), MIN_Z, MAX_Z);
                    if (pinZ > MAX_Z) pinZ -= 2 * (pinZ - MAX_Z);
                    if (pinZ < MIN_Z) pinZ -= 2 * (pinZ - MIN_Z);
                }

                pin.transform.localPosition = new Vector3(index * delta_x + start_x, pin.transform.localPosition.y, pinZ);

            }

            // set the dotsPivot to the first Pin
            Undo.RecordObject(dotsPivot.gameObject, "Changed dotsPivot");
            dotsPivot.transform.localPosition = new Vector3(start_x, pins[0].transform.localPosition.y, pins[0].transform.localPosition.z);

            // sets and scale the floor plane
            Undo.RecordObject(mapFloor.gameObject, "Changed mapFloor");
            mapFloor.transform.localPosition = new Vector3(-(start_x - end_x) / 2 + start_x, 0, 0);
            mapFloor.transform.localScale = new Vector3((start_x - end_x) + 120, 70, 1);

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
#endif

        public void DistributePinsDuringPlay(bool zToo)
        {
            var pins = new List<PinPlaceholder>(gameObject.GetComponentsInChildren<PinPlaceholder>());
            var start_x = cameraPivotStart.transform.position.x;
            var end_x = cameraPivotEnd.transform.position.x;
            var total_pins = pins.Count;
            var delta_x = (end_x - start_x) / (total_pins - 1);

            var rnd = new System.Random(3);

            for (var index = 0; index < total_pins; index++)
            {
                var pin = pins[index];
                float pinZ = pin.transform.position.z;
                if (zToo)
                {
                    // Distribute pins on Z following a random walk flow
                    const int MAX_Z = 25;
                    const int MIN_Z = -20;
                    const int Z_RND_RANGE = 20;
                    pinZ = index > 0
                        ? (pins[index - 1].transform.localPosition.z +
                           Mathf.Lerp(-Z_RND_RANGE, Z_RND_RANGE, (float)rnd.NextDouble()))
                        : Mathf.Lerp(MIN_Z, MAX_Z, (float)rnd.NextDouble());
                    if (pinZ > MAX_Z) pinZ -= 2 * (pinZ - MAX_Z);
                    if (pinZ < MIN_Z) pinZ -= 2 * (pinZ - MIN_Z);
                }
                pin.transform.position = new Vector3(start_x + delta_x * index, pin.transform.position.y, pinZ);
                //Debug.Log("Placed " + pin.name + " at " + pin.transform.localPosition);
            }

            // set the dotsPivot to the first Pin
            dotsPivot.transform.position = new Vector3(start_x, pins[0].transform.position.y, pins[0].transform.position.z);
        }

        #endregion

        #region Initialisation

        public void Initialise(bool _stageUnlocked, bool _wholeStageUnlocked)
        {
            stageUnlocked = _stageUnlocked;
            wholeStageUnlocked = _wholeStageUnlocked;
        }

        public IEnumerator LazyInitialiseCO()
        {
            // Generate enough pins
            var pinPlaceholders = gameObject.GetComponentsInChildren<PinPlaceholder>().ToList();
            var allPlaySessionStates = GetAllPlaySessionStatesForStage(stageNumber);
            int nMoreNeeded = allPlaySessionStates.Count - pinPlaceholders.Count;
            if (nMoreNeeded > 0)
            {
                for (int iPin = 0; iPin < nMoreNeeded; iPin++)
                {
                    var newPlaceholder = Instantiate(pinPlaceholders[0]);
                    pinPlaceholders.Add(newPlaceholder);
                    newPlaceholder.transform.SetParent(pinPlaceholders[0].transform.parent);
                }
            }
            else if (nMoreNeeded < 0)
            {
                int iMax = pinPlaceholders.Count - 1;
                for (int iPin = iMax; iPin >= iMax-(-nMoreNeeded)+1; iPin--)
                {
                    var placeholder = pinPlaceholders[iPin];
                    pinPlaceholders.RemoveAt(iPin);
                    Destroy(placeholder.gameObject);
                }
            }

            // Re-align all pin placeholders
            DistributePinsDuringPlay(true);

            // Create all pins from placeholders
            playPins = new List<Pin>();
            for (var jp_i = 0; jp_i < pinPlaceholders.Count; jp_i++)
            {
                var pinPlaceholder = pinPlaceholders[jp_i];
                var pinGO = Instantiate(pinPrefab);
                var pin = pinGO.GetComponent<Pin>();
                pin.transform.position = pinPlaceholder.transform.position;
                playPins.Add(pin);
                Destroy(pinPlaceholder);

                if (jp_i % 5 == 0) yield return null;
            }

            playPins.Sort((pinA, pinB) => -MapDirection * (int)(pinA.transform.position.x - pinB.transform.position.x));

            int playerPosIndexCount = 0;
            JourneyPosition assignedJourneyPosition = new JourneyPosition(stageNumber, 1, 1);

            // Initialise all pins
            for (int jp_i = 0; jp_i < playPins.Count; jp_i++)
            {
                var pin = playPins[jp_i];
                pin.Initialise(playerPosIndexCount++, assignedJourneyPosition);
                mapLocations.Add(pin);

                pin.SetLocked();
                var psState = allPlaySessionStates.Find(x => x.psData.GetJourneyPosition().Equals(assignedJourneyPosition));
                if (psState != null) {
                    pin.SetPlaySessionState(psState);
                }

                // Dialogues added to all JPs of the stage
                playPins[jp_i].gameObject.AddComponent<IntroDialogues>();

                // Advance to the next journey pos
                //Debug.Log("Current jp: " + assignedJourneyPosition);
                assignedJourneyPosition = AppManager.I.JourneyHelper.FindNextJourneyPosition(assignedJourneyPosition);
                //Debug.Log("Next jp: " + assignedJourneyPosition);

                pin.InitPinHidden();
            }

            UnlockPlaySessions();
            //yield return new WaitForSeconds(2.0f);
        }

        public IEnumerator LazyInitialiseOptionalsCO()
        {
            // Load additional visuals
            for (int jp_i = 0; jp_i < playPins.Count; jp_i++)
            {
                var pin = playPins[jp_i];

                // Create visual dots and a rope
                if (jp_i > 0)
                {
                    CreateVisualsBetweenPins(playPins[jp_i], playPins[jp_i - 1]);
                    if (jp_i % 5 == 0) yield return null;
                }

                pin.InitOptionalsHidden();
            }
            //yield return new WaitForSeconds(2.0f);
        }

        #region Appear Animation

        private bool hasAppeared = false;

        public void Appear(JourneyPosition fromPos, JourneyPosition toPos)
        {
            StartCoroutine(AppearCO(fromPos, toPos));
        }

        private IEnumerator AppearCO(JourneyPosition fromPos, JourneyPosition toPos)
        {
            if (hasAppeared) {
                yield break;
            }

            hasAppeared = true;

            //Debug.Log("Animating from " + fromPos + " to " + toPos);

            // First, let all the available dots appear, up to FROM
            FlushAppear(fromPos);

            // Then, let the remaining ones appear in order, up to TO
            int upToPosIndex = StageMapsManager.GetPosIndexFromJourneyPosition(this, toPos);
            float duration = startAppearDuration;
            foreach (var pin in playPins) {
                // Then the pins
                if (!pin.Appeared && pin.pinIndex <= upToPosIndex) {
                    // First the dots that connect the pins
                    foreach (var dot in pin.dots) {
                        if (!dot.Appeared) {
                            dot.Appear(0.0f, duration);
                            yield return new WaitForSeconds(duration);
                            duration *= appearSpeedupMultiplier;
                            if (duration <= minAppearDuration) { duration = minAppearDuration; }
                        }
                    }

                    pin.Appear(pinAppearDuration);
                    yield return new WaitForSeconds(pinAppearDuration);
                    duration *= appearSpeedupMultiplier;
                    if (duration <= minAppearDuration) { duration = minAppearDuration; }
                }
            }

        }

        public void FlushAppear(JourneyPosition upToJourneyPos)
        {
            //Debug.Log("FLUSH TO " + upToJourneyPos);
            int upToPosIndex = StageMapsManager.GetPosIndexFromJourneyPosition(this, upToJourneyPos);
            foreach (var pin in playPins) {
                // Stop when we find locked pins
                if (pin.isLocked) { break; }

                if (pin.pinIndex > upToPosIndex) {
                    break;
                }

                // First the dots
                foreach (var dot in pin.dots) {
                    dot.FlushAppear();
                }

                // Then the pin itself
                pin.FlushAppear();
            }
        }

        #endregion

        private void CreateVisualsBetweenPins(Pin pinFront, Pin pinBack)
        {
            // @note: rope and dots always belong to the FRONT dot
            Vector3 pFront = pinFront.transform.position;
            Vector3 pBack = pinBack.transform.position;

            var distance = Vector3.Distance(pFront, pBack);
            int nDots = Mathf.FloorToInt(distance / preferredDotsSpan) - 1;  // -1 as we have the two pins as start and end
            float actualDotsSpan = distance / nDots;
            var dir = Vector3.Normalize(pFront - pBack);

            // Create and stretch the rope between pins of the same Learning Block
            if (pinBack.journeyPosition.LearningBlock == pinFront.journeyPosition.LearningBlock)
            {
                GameObject ropeGo = Instantiate(ropePrefab);
                ropeGo.transform.SetParent(dotsPivot);
                var rope = ropeGo.GetComponent<Rope>();
                rope.name = "MapRope_" + pinFront.pinIndex;
                pinFront.rope = rope;
                rope.transform.position = pinBack.transform.position;
                rope.transform.position += pinBack.currentPinGO.transform.up * 4;
                rope.transform.LookAt(pinFront.transform.position + pinFront.currentPinGO.transform.up * 4);
                rope.transform.Rotate(Vector3.forward, -90f);
                rope.transform.Rotate(0, 4, 0);
                rope.transform.SetLocalScaleZ((distance / 20f) * 1.1f);
            }

            // Create the dots
            for (int dot_i = 1; dot_i <= nDots; dot_i++) {
                // Create a new dot
                var dotPos = pBack + dir * dot_i * actualDotsSpan;
                var dotRot = Quaternion.Euler(90, 0, 0);
                GameObject dotGo = Instantiate(dotPrefab, dotPos, dotRot);
                dotGo.transform.SetParent(dotsPivot);
                var dot = dotGo.GetComponent<Dot>();
                pinFront.dots.Add(dot);
            }
        }

        private void UnlockPlaySessions()
        {
            if (!stageUnlocked) {
                // All is locked
                MaxUnlockedPinIndex = 0;
            } else if (wholeStageUnlocked) {
                // All is unlocked
                playPins.ForEach(pin => pin.SetUnlocked());
                MaxUnlockedPinIndex = playPins.Last().pinIndex;
            } else {
                // Part of the stage is locked
                var maxJp = AppManager.I.Player.MaxJourneyPosition;
                playPins.ForEach(pin => {
                    if (pin.JourneyPos.IsMinorOrEqual(maxJp)) {
                        pin.SetUnlocked();
                        MaxUnlockedPinIndex = pin.pinIndex;
                    }
                });
            }
        }

        public void ResetStageOnShow(bool playerIsHere)
        {
            //Debug.Log("Stage " + name + " player here? " + playerIsHere);
            foreach (var pin in playPins) {
                pin.Select(playerIsHere && Equals(pin.JourneyPos, CurrentPlayerPosJourneyPosition));
            }
        }

        #endregion

        #region Play Session State

        /// <summary>
        ///     Returns a list of all play session data with its score (if a score exists) for the given stage
        /// </summary>
        /// <param name="_stage"></param>
        /// <returns></returns>
        private List<PlaySessionState> GetAllPlaySessionStatesForStage(int _stage)
        {
            // Get all PS for this stage
            var allPlaySessionData = AppManager.I.DB.GetAllPlaySessionData().Where(ps => ps.Stage == _stage).ToList();

            // Get all available scores for this stage
            var allScoreData = AppManager.I.ScoreHelper.GetCurrentScoreForPlaySessionsOfStage(_stage);

            // Build a structure containing both
            var playSessionStateList = new List<PlaySessionState>();
            for (var i = 0; i < allPlaySessionData.Count; i++) {
                //var data = AppManager.I.DB.GetPlaySessionDataById(scoreData_list[i].ElementId);
                var scoreData = allScoreData.FirstOrDefault(sc => sc.ElementId == allPlaySessionData[i].Id);
                playSessionStateList.Add(new PlaySessionState(allPlaySessionData[i], scoreData));
                //Debug.Log(scoreData_list[i].ElementId + " SCORE " + scoreData_list[i].Stars);
            }
            return playSessionStateList;
        }

        #endregion

        #region Show / Hide

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            if (hasAppeared) {
                FlushAppear(AppManager.I.Player.MaxJourneyPosition);
            }
        }

        #endregion

    }
}