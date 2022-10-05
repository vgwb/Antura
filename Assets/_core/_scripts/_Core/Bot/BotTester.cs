using System;
using System.Collections;
using Antura.AnturaSpace;
using Antura.AnturaSpace.UI;
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
        public BotConfig Config;
        private BotConfig C => Config;

        public void Start()
        {
            if (C.AutoStart) StartBot();
        }

        private void StartBot()
        {
            StartCoroutine(BotFlowCO());
        }

        private void OnEnable()
        {
            if (C.AutoStart) StartBot();
        }

        private float timer = 0f;
        private AppScene prevScene;

        private IEnumerator BotFlowCO()
        {
            Application.runInBackground = true;

            while (true)
            {
                Time.timeScale = C.GameSpeed;

                var appScene = AppManager.I.NavigationManager.GetCurrentScene();

                GlobalUI.WidgetSubtitles.OnHintClicked();

                bool hasChangedScene = prevScene != appScene;
                prevScene = appScene;
                if (hasChangedScene) BotLog($"We are in {appScene}");

                switch (appScene)
                {
                    case AppScene.Home:
                    {
                        var homeScene = FindObjectOfType<HomeScene>();


                        // Choose language
                        if (AppManager.I.AppSettings.NativeLanguage != C.NativeLanguage)
                        {
                            Click(FindObjectOfType<EditionSelectorBtn>());
                            yield return new WaitForSeconds(C.Delay);
                            Click(FindObjectOfType<SelectNativeLanguageButton>());
                            yield return new WaitForSeconds(C.Delay);
                        }

                        // Play
                        var playerIcons = homeScene.ProfileSelectorUI.GetComponentsInChildren<PlayerIcon>(true);
                        if (playerIcons.Length > 0)
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
                        var stageMapsManager = FindObjectOfType<StageMapsManager>(true);
                        if (stageMapsManager != null)
                        {
                            var maxJourneyPos = AppManager.I.Player.MaxJourneyPosition;
                            var playerStageMap = stageMapsManager.StageMap(maxJourneyPos.Stage);
                            stageMapsManager.SelectPin(playerStageMap.PinForJourneyPosition(maxJourneyPos));
                            yield return new WaitForSeconds(C.Delay);
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

                    case AppScene.Mood:
                    {
                        var emoticons = GameObject.Find("Emoticons");
                        if (emoticons != null)
                        {
                            var moodButtons = GameObject.Find("Emoticons").GetComponentsInChildren<Button>(true);
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
                        Click(dailyRewardScene.claimButton);
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
                        timer += Time.deltaTime;
                        BotLog($"Timer: " + timer);
                        if (timer > C.MinigamePlayDelay)
                        {
                            var controller = FindObjectOfType<MiniGameController>(true);
                            if (controller.StarsScore == 0)
                            {
                                DebugManager.I.ForceCurrentMinigameEnd(3);
                            }
                        }

                        Click(GlobalUI.ContinueScreen.BtContinue);

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
                                }
                                break;

                            case FirstContactPhase.AnturaSpace_Customization:
                                var modsButton = FindObjectOfType<AnturaSpaceModsButton>();
                                if (modsButton != null) Click(modsButton);

                                foreach (var btn in FindObjectsOfType<AnturaSpaceCategoryButton>())
                                {
                                    Click(btn);
                                }

                                break;
                            case FirstContactPhase.AnturaSpace_Exit:
                                Click(GlobalUI.I.BackButton);
                                break;
                        }


                        yield return new WaitForSeconds(C.Delay);
                    }
                        break;
                }

                yield return null;
            }
        }

        private void Click(Component comp)
        {
            if (!comp.gameObject.activeInHierarchy) return;
            var button = comp.GetComponentInChildren<UIButton>(true);
            if (button == null) return;

            BotLog($"Clicking {button.name} for component {comp.name}");
            button.Bt.onClick.Invoke();
        }

        private void BotLog(string msg)
        {
            Debug.Log($"[BOT] {msg}");
        }
    }
}
