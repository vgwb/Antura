using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Antura.AnturaSpace;
using Antura.AnturaSpace.UI;
using Antura.Book;
using Antura.Database.Management;
using Antura.Debugging;
using Antura.GamesSelector;
using Antura.Helpers;
using Antura.Map;
using Antura.Minigames;
using Antura.Minigames.Egg;
using Antura.Minigames.FastCrowd;
using Antura.Profile;
using Antura.ReservedArea;
using Antura.Rewards;
using Antura.Scenes;
using Antura.Teacher.Test;
using Antura.Test;
using Antura.Tutorial;
using Antura.UI;
using Antura.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Antura.Core
{
    public enum BotStep
    {
        PlayAllGamesInBook,
        PlayJourney,
    }

    public class BotTester : MonoBehaviour
    {
        public static BotTester I;

        public BotConfig Config;
        private BotConfig C => Config;

        public void Start()
        {
            I = this;
            if (C.AutoStart)
            {
                C.BotEnabled = true;
            }
            else
            {
                C.BotEnabled = false;
            }
        }

        private Coroutine co;
        private float runCheck = 0f;

        private float timer = 0f;
        private AppScene prevScene;

        private List<string> playedMinigameVariations = new List<string>();
        private List<EditionCombo> playedCombos = new List<EditionCombo>();
        private List<BotStep> botSteps = new List<BotStep>();

        private void Update()
        {
            var runCheckLimit = 20f;
            if (C.BotEnabled && (co == null || runCheck >= runCheckLimit))
            {
                if (runCheck >= runCheckLimit)
                {
                    BotLog("Bot was stopped abruptly or timed out! Restarting.");
                }
                RestartBot();
                runCheck = 0f;
            }
            runCheck += Time.deltaTime;

            // Reset timescale when we disable the bot
            if (!C.BotEnabled && Time.timeScale > 1f)
            {
                Time.timeScale = 1f;
            }
        }

        private void OnApplicationQuit()
        {
            C.BotEnabled = false;
        }

        private void RestartBot()
        {
            if (co != null) StopCoroutine(co);
            co = StartCoroutine(BotFlowCO());
        }

        private void StopBot()
        {
            C.BotEnabled = false;
            if (co != null) StopCoroutine(co);
        }

        private bool completedFirstTime = false;

        private IEnumerator BotFlowCO()
        {
            Application.runInBackground = true;

            if (!completedFirstTime && C.DeleteExistingProfiles)
            {
                BotLog($"Deleted all player profiles");
                DebugManager.I.ResetAll(true);
                DebugManager.I.GoToHome();
            }

#if UNITY_EDITOR
            if (!completedFirstTime && (C.StartTeacherTester || C.CheckLearningMissingAudio))
            {
                var op = SceneManager.LoadSceneAsync("manage_Database", new LoadSceneParameters(LoadSceneMode.Additive));
                while (!op.isDone) yield return null;

                // Avoids duplicates
                var eventSystem = FindObjectOfType<EventSystem>();
                Destroy(eventSystem);

                if (C.StartTeacherTester)
                {
                    BotLog($"Starting Teacher Tester");
                    var databaseTester = FindObjectOfType<DatabaseTester>();
                    databaseTester.ToTeacherTester();

                    var teacherTester = FindObjectOfType<TeacherTester>();
                    teacherTester.DoTestCompleteJourney();
                    while (teacherTester.IsExecuting) yield return null;
                    BotLog($"Completed Teacher Tester");
                }

                if (C.CheckLearningMissingAudio)
                {
                    var missingAudioFileChecker = FindObjectOfType<MissingAudioFileChecker>();
                    missingAudioFileChecker.LanguageToCheck = AppManager.I.ContentEdition.LearningLanguage;
                    BotLog("Checking missing audio: DB against Project");
                    missingAudioFileChecker.CheckDbAgaistProject();
                    BotLog("Checking missing audio: Project against DB");
                    missingAudioFileChecker.CheckProjectAgainstDb();
                }

                op = SceneManager.UnloadSceneAsync("manage_Database");
                while (!op.isDone) yield return null;
                DebugManager.I.GoToHome();
            }
#endif
            completedFirstTime = true;

            if (C.PlayAllGamesInBook) botSteps.Add(BotStep.PlayAllGamesInBook);
            if (C.PlayJourney) botSteps.Add(BotStep.PlayJourney);

            while (true)
            {
                while (!C.BotEnabled) yield return null;
                while (AppManager.I.NavigationManager.IsTransitioningScenes) yield return null;

                Time.timeScale = C.GameSpeed;

                var appScene = AppManager.I.NavigationManager.GetCurrentScene();

                GlobalUI.WidgetSubtitles.OnHintClicked();

                bool hasChangedScene = prevScene != appScene;
                prevScene = appScene;
                if (hasChangedScene) BotLog($"Entering scene: {appScene}");

                switch (appScene)
                {
                    case AppScene.Home:
                    {
                        var homeScene = FindObjectOfType<HomeScene>();

                        // Choose language
                        if (C.TestEditionCombos)
                        {
                            bool foundCombo = false;
                            foreach (var combo in C.Combos)
                            {
                                if (playedCombos.Contains(combo)) continue;

                                BotLog("Switching to learning combo " + combo);
                                Click(FindObjectOfType<EditionSelectorBtn>());
                                yield return new WaitForSeconds(C.Delay);
                                Click(FindObjectOfType<SelectNativeLanguageButton>());
                                yield return new WaitForSeconds(C.Delay);

                                var nativeLangBtns = FindObjectsOfType<SelectNativeLanguageButton>();
                                foreach (var nativeLangBtn in nativeLangBtns)
                                {
                                    if (nativeLangBtn.LanguageCode == combo.NativeLanguage)
                                    {
                                        Click(nativeLangBtn);
                                        break;
                                    }
                                }
                                yield return new WaitForSeconds(3f);

                                var learningContentBtns = FindObjectsOfType<SelectLearningContentButton>();
                                foreach (var learningContentBtn in learningContentBtns)
                                {
                                    if (learningContentBtn.ContentId == combo.LearningContent)
                                    {
                                        Click(learningContentBtn);
                                        break;
                                    }
                                }
                                yield return new WaitForSeconds(C.Delay);

                                foundCombo = true;
                                playedCombos.Add(combo);
                                break;
                            }

                            if (!foundCombo)
                            {
                                BotLog("Completed all combos. Stopping");
                                StopBot();
                                yield break;
                            }
                        }

                        var playerIcons = homeScene.ProfileSelectorUI.GetComponentsInChildren<PlayerIcon>(true);
                        if (C.UseDemoProfile)
                        {
                            if (!AppManager.I.PlayerProfileManager.IsDemoUserExisting())
                            {
                                DebugManager.I.GoToReservedArea();
                                yield return new WaitForSeconds(1f);
                                var profilesPanel = FindObjectOfType<ProfilesPanel>();
                                yield return profilesPanel.CreateDemoPlayer();
                                DebugManager.I.GoToHome();
                            }
                        }
                        if (homeScene == null) homeScene = FindObjectOfType<HomeScene>();

                        // Play
                        if (C.UseDemoProfile)
                        {
                            var desiredPlayerIcon = playerIcons.FirstOrDefault(x => x.HatImage != null && x.HatImage.gameObject.activeInHierarchy);
                            Click(desiredPlayerIcon);
                            yield return new WaitForSeconds(2f);
                            if (homeScene == null) homeScene = FindObjectOfType<HomeScene>();

                            Click(homeScene.ProfileSelectorUI.transform.Find("BT Play"));
                        }
                        else if (!C.CreateNewProfile && playerIcons.Length > 0)
                        {
                            // Existing player
                            var desiredPlayerIcon = playerIcons[0];
                            Click(desiredPlayerIcon);
                            yield return new WaitForSeconds(C.Delay);
                            Click(homeScene.ProfileSelectorUI.transform.Find("BT Play"));
                        }
                        else
                        {
                            // New player
                            Click(homeScene.ProfileSelectorUI.GetComponent<ProfileSelectorUI>().BtAdd);
                        }

                    }
                        break;

                    case AppScene.Map:
                    {
                        // Go to the BOOK
                        if (botSteps.Contains(BotStep.PlayAllGamesInBook))
                        {
                            var openBookBtn = FindObjectOfType<OpenPlayerBookScene>();
                            if (openBookBtn != null) openBookBtn.OnPointerClick(new PointerEventData(EventSystem.current));
                            yield return new WaitForSeconds(C.Delay);

                            Click("MiniGames");
                            yield return new WaitForSeconds(C.Delay);

                            var listContainer = GameObject.Find("List Container");
                            if (listContainer)
                            {
                                bool gameChosen = false;
                                var allMinigames = listContainer.GetComponentsInChildren<ItemMainMiniGame>(true);
                                foreach (var minigame in allMinigames)
                                {
                                    foreach (var variation in minigame.mainGameInfo.variations)
                                    {
                                        var key = $"{minigame.mainGameInfo.MainId}_{variation.data.Variation}";
                                        if (playedMinigameVariations.Contains(key)) continue;

                                        BotLog("Testing minigame variation " + key);
                                        // This is the next minigame to play
                                        playedMinigameVariations.Add(key);

                                        Click(minigame.GetComponentInChildren<Button>(true));
                                        yield return new WaitForSeconds(C.Delay);
                                        yield return new WaitForSeconds(1f);

                                        var allVariations = GameObject.Find("Variations Container").GetComponentsInChildren<ItemMiniGame>(true);
                                        var variationItem = allVariations.FirstOrDefault(x => x.miniGameInfo.data.Variation == variation.data.Variation);
                                        //if (variationItem == null) continue;

                                        Click(variationItem.GetComponentInChildren<Button>(true));
                                        yield return new WaitForSeconds(C.Delay);

                                        Click(GameObject.Find("Btn Start Game"));
                                        yield return new WaitForSeconds(C.Delay);
                                        gameChosen = true;
                                        if (gameChosen) break;
                                    }
                                    if (gameChosen) break;
                                }

                                if (!gameChosen)
                                {
                                    BotLog("Completed minigame phase. Played " + playedMinigameVariations.Count + " minigames");
                                    botSteps.Remove(BotStep.PlayAllGamesInBook);
                                }
                            }
                        }
                        else
                        // Or, play the journey instead
                        if (botSteps.Contains(BotStep.PlayJourney))
                        {
                            var stopAtJp = AppManager.I.JourneyHelper.GetFinalJourneyPosition();
                            if (C.EnableStopBeforeJP)
                            {
                                stopAtJp = C.StopBeforeJP;
                            }
                            if (AppManager.I.Player.MaxJourneyPosition.IsGreaterOrEqual(stopAtJp))
                            {
                                BotLog($"We reached the target Journey Position " + stopAtJp);
                                botSteps.Remove(BotStep.PlayJourney);
                            }
                            else
                            {
                                var stageMapsManager = FindObjectOfType<StageMapsManager>(true);
                                if (stageMapsManager != null)
                                {
                                    var targetJourneyPos = AppManager.I.Player.MaxJourneyPosition;
                                    var playerStageMap = stageMapsManager.StageMap(targetJourneyPos.Stage);
                                    stageMapsManager.SelectPin(playerStageMap.PinForJourneyPosition(targetJourneyPos));
                                    BotLog($"Selected pin fr Journey Position " + targetJourneyPos);
                                    yield return new WaitForSeconds(1f);
                                    yield return new WaitForSeconds(C.Delay);
                                }
                            }
                        }
                    }
                        break;

                    case AppScene.GameSelector:
                    {
                        var gamesSelector = FindObjectOfType<GamesSelector.GamesSelector>(true);
                        if (gamesSelector != null)
                        {
                            var bubbles = gamesSelector.GetComponentsInChildren<GamesSelectorBubble>();
                            foreach (GamesSelectorBubble gamesSelectorBubble in bubbles)
                            {
                                if (!gamesSelectorBubble.IsOpen) gamesSelector.HitBubble(gamesSelectorBubble);
                                yield return new WaitForSeconds(C.Delay);
                            }
                        }
                    }
                        break;

                    case AppScene.PlayerCreation:
                    {
                        var genderSelector = GameObject.Find("Gender selector");
                        var skinSelector = GameObject.Find("SkinColor selector");
                        var avatarSelector = GameObject.Find("Avatar selector");
                        var hairColorSelector = GameObject.Find("HairColor selector");
                        var bgColorSelector = GameObject.Find("BgColor Category");
                        var ageSelector = GameObject.Find("Age Category");
                        var btnContinue = GameObject.Find("BT Continue");

                        if (genderSelector != null && genderSelector.activeInHierarchy)
                        {
                            var buttons = genderSelector.GetComponentsInChildren<UIButton>();
                            Click(buttons.GetRandom());
                            yield return new WaitForSeconds(C.Delay);
                        }
                        else if (bgColorSelector != null && bgColorSelector.activeInHierarchy)
                        {
                            var buttons = bgColorSelector.GetComponentsInChildren<UIButton>();
                            Click(buttons.GetRandom());
                            yield return new WaitForSeconds(C.Delay);
                        }
                        else if (ageSelector != null && ageSelector.activeInHierarchy)
                        {
                            var buttons = ageSelector.GetComponentsInChildren<UIButton>();
                            Click(buttons.GetRandom());
                            yield return new WaitForSeconds(C.Delay);
                        }
                        else if (hairColorSelector != null && hairColorSelector.activeInHierarchy)
                        {
                            var buttons = hairColorSelector.GetComponentsInChildren<UIButton>();
                            Click(buttons.GetRandom());
                            yield return new WaitForSeconds(C.Delay);
                        }
                        else if (avatarSelector != null && avatarSelector.activeInHierarchy)
                        {
                            var buttons = avatarSelector.GetComponentsInChildren<UIButton>();
                            Click(buttons.GetRandom());
                            yield return new WaitForSeconds(C.Delay);
                        }
                        else if (skinSelector != null && skinSelector.activeInHierarchy)
                        {
                            var buttons = skinSelector.GetComponentsInChildren<UIButton>();
                            Click(buttons.GetRandom());
                            yield return new WaitForSeconds(C.Delay);
                        }

                        if (btnContinue != null)
                        {
                            Click(btnContinue.transform);
                            yield return new WaitForSeconds(C.Delay);
                        }

                    }
                        break;
                    case AppScene.Mood:
                    {
                        var emoticons = GameObject.Find("Emoticons");
                        if (emoticons != null)
                        {
                            var moodButtons = emoticons.GetComponentsInChildren<Button>(true);
                            var btn = moodButtons.GetRandom();
                            Click(btn);
                            yield return new WaitForSeconds(2f); // Or it breaks for some reason
                        }
                        yield return new WaitForSeconds(C.Delay);
                    }
                        break;

                    case AppScene.DailyReward:
                    {
                        timer += Time.deltaTime;
                        var dailyRewardScene = FindObjectOfType<DailyRewardScene>(true);
                        if (dailyRewardScene != null)
                        {
                            dailyRewardScene.UnlockNewReward();
                            //Click(dailyRewardScene.claimButton);
                        }
                        yield return new WaitForSeconds(C.Delay);
                    }
                        break;

                    case AppScene.Rewards:
                    {
                        Click(GlobalUI.ContinueScreen.BtContinue);
                        yield return new WaitForSeconds(C.Delay);
                    }
                        break;

                    case AppScene.MiniGame:
                    {
                        var controller = FindObjectOfType<MiniGameController>(true);
                        switch (controller.GetType().Name)
                        {
                            case "EggGame":
                            {
                                var buttons = FindObjectsOfType<EggButton>();
                                if (buttons.Length > 0)
                                {
                                    var btn = buttons.GetRandom();
                                    var nClicks = buttons.Length == 1 ? 10 : 1;
                                    for (int i = 0; i < nClicks; i++)
                                    {
                                        Click(btn);
                                        yield return new WaitForSeconds(0.2f);
                                    }
                                }
                            }
                                break;

                            /*
                            case "FastCrowdGame":
                            {
                                var ll = FindObjectOfType<StrollingLivingLetter>();
                                if (ll != null)
                                {
                                    var drops = FindObjectsOfType<DropAreaWidget>();
                                    if (drops.Length > 0)
                                    {
                                        ll.DropOnArea(drops[0]);
                                        yield return new WaitForSeconds(0.2f);
                                    }
                                }
                            }
                                break;
                                */
                            default:
                            {
                                // Skip the game
                                //BotLog($"Timer: " + timer);
                                if (timer > C.MinigamePlayDelay)
                                {
                                    if (controller != null && controller.StarsScore == 0)
                                    {
                                        DebugManager.I.ForceCurrentMinigameEnd(3);
                                        timer = 0f;
                                    }
                                }
                            }
                                break;
                        }

                        Click(GlobalUI.ContinueScreen.BtContinue);

                        timer += C.Delay;
                        yield return new WaitForSeconds(C.Delay);

                        var minigameRunLimit = 300;
                        if (timer > minigameRunLimit)
                        {
                            BotError($"Minigame timeout, could not be completed in {minigameRunLimit} seconds! Back to MAP.");
                            timer = 0f;
                            DebugManager.I.GoToMap();
                            yield return new WaitForSeconds(C.Delay);
                        }
                    }
                        break;


                    case AppScene.PlaySessionResult:
                    {
                        Click(GlobalUI.ContinueScreen.BtContinue);
                        yield return new WaitForSeconds(C.Delay);
                    }
                        break;

                    case AppScene.AnturaSpace:
                    {
                        var phase = FirstContactManager.I.CurrentPhaseInSequence;
                        switch (phase)
                        {

                            case FirstContactPhase.AnturaSpace_TouchAntura:
                                var manager = FindObjectOfType<AnturaSpaceTutorialManager>();
                                if (manager != null)
                                {
                                    // @note: sometimes it can block here, so it's better to just wait a bit more
                                    manager.HandleAnturaTouched();
                                    yield return new WaitForSeconds(2f);
                                    GlobalUI.WidgetSubtitles.OnHintClicked();
                                    yield return new WaitForSeconds(2f);
                                    GlobalUI.WidgetSubtitles.OnHintClicked();
                                    yield return new WaitForSeconds(2f);
                                }
                                break;

                            case FirstContactPhase.AnturaSpace_Customization:
                                var modsButton = FindObjectOfType<AnturaSpaceModsButton>();
                                if (modsButton != null) Click(modsButton);

                                foreach (var btn in FindObjectsOfType<AnturaSpaceCategoryButton>())
                                {
                                    Click(btn);
                                }

                                foreach (var btn in FindObjectsOfType<AnturaSpaceItemButton>())
                                {
                                    Click(btn);
                                }

                                foreach (var btn in FindObjectsOfType<AnturaSpaceSwatchButton>())
                                {
                                    Click(btn);
                                }
                                break;
                            case FirstContactPhase.AnturaSpace_Exit:
                                Click(GlobalUI.I.BackButton);
                                break;

                            case FirstContactPhase.AnturaSpace_Shop:
                                Click(GameObject.Find("Btn Shop Bones"));
                                yield return new WaitForSeconds(C.Delay);
                                var shopActionThrow = FindObjectOfType<ShopAction_Throw>();
                                if (shopActionThrow != null)
                                {
                                    DoOnce(shopActionThrow.PerformAction);
                                    yield return new WaitForSeconds(C.Delay);
                                }
                                Click(GameObject.Find("BT Yes"));

                                // FAKE TUTORIAL COMPLETION (too hard to drag)
                                var tutManager = FindObjectOfType<AnturaSpaceTutorialManager>();
                                tutManager.FakeAdvanceTutorialShop();

                                break;

                            case FirstContactPhase.AnturaSpace_Photo:
                            case FirstContactPhase.Finished:
                                Click(GameObject.Find("ShopActionUI_Photo"));
                                yield return new WaitForSeconds(C.Delay);
                                Click(GameObject.Find("BT Yes"));
                                yield return new WaitForSeconds(C.Delay);
                                break;
                        }

                        // Try to exit if possible
                        Click(GlobalUI.I.BackButton);

                        yield return new WaitForSeconds(C.Delay);
                    }
                        break;
                }

                yield return null;
                runCheck = 0f;
            }
        }

        private List<Action> performedActions = new List<Action>();

        private void DoOnce(Action performAction)
        {
            if (performedActions.Contains(performAction)) return;
            performedActions.Add(performAction);
            performAction.Invoke();
        }

        private bool Click(string goName)
        {
            var go = GameObject.Find(goName);
            if (go == null)
            {
                BotLog($"Could not find any go with name {goName}");
                return false;
            }
            return Click(go);
        }

        private bool Click(GameObject go)
        {
            if (go == null) return false;
            return Click(go.transform);
        }

        private bool Click(Component comp)
        {
            if (comp == null) return false;
            if (!comp.gameObject.activeInHierarchy) return false;
            var button = comp.GetComponentInChildren<Button>(true);
            if (button == null) return false;

            BotDebug($"Clicking {button.name} for component {comp.name}");
            button.onClick.Invoke();
            return true;
        }

        private void BotDebug(string msg)
        {
            if (C.DebugMode)
            {
                Debug.Log($"[BOT] {msg}");
            }
        }

        public void BotLog(string msg)
        {
            Debug.Log($"[BOT] {msg}");
        }

        public void BotError(string msg)
        {
            Debug.LogError($"[BOT] {msg}");
        }
    }
}
