using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Antura.AnturaSpace;
using Antura.AnturaSpace.UI;
using Antura.Book;
using Antura.Debugging;
using Antura.GamesSelector;
using Antura.Helpers;
using Antura.Map;
using Antura.Minigames;
using Antura.Profile;
using Antura.Rewards;
using Antura.Scenes;
using Antura.Tutorial;
using Antura.UI;
using Antura.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Core
{
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

        private List<string> PlayedMinigameVariations = new List<string>();

        private void Update()
        {
            var runCheckLimit = 20f;
            if (C.BotEnabled && (co == null || runCheck >= runCheckLimit))
            {
                if (runCheck >= runCheckLimit)
                {
                    BotLog("Bot was stopped abruptly! Restarting.");
                }
                RestartBot();
                runCheck = 0f;
            }
            runCheck += Time.deltaTime;
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

        private IEnumerator BotFlowCO()
        {
            Application.runInBackground = true;

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
                        /*while (AppManager.I.AppSettings.NativeLanguage != C.NativeLanguage)
                        {
                            Click(FindObjectOfType<EditionSelectorBtn>());
                            yield return new WaitForSeconds(C.Delay);
                            Click(FindObjectOfType<SelectNativeLanguageButton>());
                            yield return new WaitForSeconds(C.Delay);

                            var nativeLangBtns = FindObjectsOfType<SelectNativeLanguageButton>();
                            foreach (var nativeLangBtn in nativeLangBtns)
                            {
                                if (nativeLangBtn.LanguageCode == C.NativeLanguage)
                                {
                                    Click(nativeLangBtn);
                                    break;
                                }
                            }
                            yield return new WaitForSeconds(C.Delay);

                        }*/

                        // Play
                        var playerIcons = homeScene.ProfileSelectorUI.GetComponentsInChildren<PlayerIcon>(true);
                        if (!C.CreateNewProfile && playerIcons.Length > 0)
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
                        if (C.PlayAllGamesInBook)
                        {
                            Click(FindObjectOfType<OpenPlayerBookScene>());
                            yield return new WaitForSeconds(C.Delay);
                            yield return new WaitForSeconds(C.Delay);
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
                                        if (PlayedMinigameVariations.Contains(key)) continue;

                                        BotLog("Testing minigame variation " + key);
                                        // This is the next minigame to play
                                        PlayedMinigameVariations.Add(key);

                                        Click(minigame.GetComponentInChildren<Button>(true));
                                        yield return new WaitForSeconds(C.Delay);
                                        yield return new WaitForSeconds(1f);

                                        var allVariations = GameObject.Find("Variations Container").GetComponentsInChildren<ItemMiniGame>(true);
                                        var variationItem = allVariations.FirstOrDefault(x => x.miniGameInfo.data.Variation == variation.data.Variation);

                                        Click(variationItem.GetComponentInChildren<Button>(true));
                                        yield return new WaitForSeconds(C.Delay);

                                        Click(GameObject.Find("Btn Start Game"));
                                        yield return new WaitForSeconds(C.Delay);
                                        gameChosen = true;
                                        if (gameChosen) break;
                                    }
                                    if (gameChosen) break;
                                }
                            }
                        }

                        if (C.PlayJourney)
                        {
                            if (C.EnableStopBeforeJP)
                            {
                                if (AppManager.I.Player.MaxJourneyPosition.Equals(C.StopBeforeJP))
                                {
                                    BotLog($"We reached the target Journey Position");
                                    StopBot();
                                    yield break;
                                }
                            }

                            var stageMapsManager = FindObjectOfType<StageMapsManager>(true);
                            if (stageMapsManager != null)
                            {
                                var targetJourneyPos = AppManager.I.Player.MaxJourneyPosition;
                                var playerStageMap = stageMapsManager.StageMap(targetJourneyPos.Stage);
                                stageMapsManager.SelectPin(playerStageMap.PinForJourneyPosition(targetJourneyPos));
                                BotLog($"Selected pin fr Journey Position " + targetJourneyPos);
                                yield return new WaitForSeconds(C.Delay);
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
                        BotLog($"Timer: " + timer);
                        if (timer > C.MinigamePlayDelay)
                        {
                            var controller = FindObjectOfType<MiniGameController>(true);
                            if (controller != null && controller.StarsScore == 0)
                            {
                                DebugManager.I.ForceCurrentMinigameEnd(3);
                                timer = 0f;
                            }
                        }

                        Click(GlobalUI.ContinueScreen.BtContinue);

                        timer += C.Delay;
                        yield return new WaitForSeconds(C.Delay);
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
                        // TODO: perform the tutorial too
                        var phase = FirstContactManager.I.CurrentPhaseInSequence;
                        switch (phase)
                        {
                            case FirstContactPhase.AnturaSpace_TouchAntura:
                                var manager = FindObjectOfType<AnturaSpaceTutorialManager>();
                                if (manager != null)
                                {
                                    manager.HandleAnturaTouched();
                                    yield return new WaitForSeconds(0.1f);
                                    GlobalUI.WidgetSubtitles.OnHintClicked();
                                    yield return new WaitForSeconds(0.1f);
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
                                break;
                            case FirstContactPhase.AnturaSpace_Photo:
                                Click(GameObject.Find("ShopActionUI_Photo"));
                                Click(GameObject.Find("BT Yes"));
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

            BotLog($"Clicking {button.name} for component {comp.name}");
            button.onClick.Invoke();
            return true;
        }

        private void BotLog(string msg)
        {
            Debug.Log($"[BOT] {msg}");
        }
    }
}
