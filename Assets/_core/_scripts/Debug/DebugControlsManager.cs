// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2024/03/27

using System;
using System.Text;
using Antura.Core;
using Antura.Minigames.DiscoverCountry;
using Antura.Minigames.DiscoverCountry.Interaction;
using DG.DeExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Antura.Debugging
{
    /// <summary>
    /// Deals with all debug controls including cheats
    /// </summary>
    public class DebugControlsManager : MonoBehaviour
    {
        #region Serialized

        [SerializeField] GameObject _controlsKeysPanel;
        [SerializeField] TMP_Text _tfControlKeys;

        #endregion

        static readonly StringBuilder _Strb = new StringBuilder();
        // This returns error in discover mode because NavData.CurrentMiniGameData is null, comment for now and enable when it's working
        // bool activateLearnLanguageCheats { get { return AppManager.I.NavigationManager.CurrentMiniGameData.Code != MiniGameCode.Discover_Country; } }
        bool activateLearnLanguageCheats { get { return true; } }
        // This returns error in discover mode because NavData.CurrentMiniGameData is null, comment for now and enable when it's working
        // bool activateDiscoverCountriesCheats { get { return AppManager.I.NavigationManager.CurrentMiniGameData.Code == MiniGameCode.Discover_Country; } }
        bool activateDiscoverCountriesCheats { get { return true; } }

        #region Commands

        // TOOLS
        readonly DebugCommand[] _tools = new[] {
            new DebugCommand("Show this panel", CommandCondition.WhilePressed, KeyCode.C, KeyModifier.Shift, on => {
                _I._controlsKeysPanel.SetActive(on);
            })
        };
        // GLOBAL CHEATS
        readonly DebugCommand[] _globalCheats = new[] {
            new DebugCommand("Print Player Profile Info", CommandCondition.OnPress, KeyCode.P, KeyModifier.Shift, on => {
                AppManager.I.Player.PrintDebugInfo();
            }),
            new DebugCommand("Reload current scene", CommandCondition.OnPress, KeyCode.F5, on => {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }),
            new DebugCommand("Set timeScale to 10x (while pressed)", CommandCondition.WhilePressed, KeyCode.W, KeyModifier.Shift, on => {
                Time.timeScale = on ? 10 : 1;
            }),
            new DebugCommand("Set timeScale to 0.2x (while pressed)", CommandCondition.WhilePressed, KeyCode.S, KeyModifier.Shift, on => {
                Time.timeScale = on ? 0.2f : 1;
            }),
            new DebugCommand("Open reserved area bypassing the parental gate", CommandCondition.OnPress, KeyCode.R, KeyModifier.Shift, on => {
                AppManager.I.NavigationManager.GoToReservedArea();;
            }),
            new DebugCommand("Skip current scene", CommandCondition.OnPress, KeyCode.Space, KeyModifier.Shift, on => {
                DebugManager.I.SkipCurrentScene();
            }),
            new DebugCommand("Test local notification", CommandCondition.OnPress, KeyCode.T, KeyModifier.Ctrl, on => {
                AppManager.I.Services.Notifications.TestLocalNotification();
            }),
        };
        // ANTURA LEARN LANGUAGE CHEATS (active only if in that game mode)
        readonly DebugCommand[] _learnLanguageCheats = new[] {
            new DebugCommand("Add 50 bones", CommandCondition.OnPress, KeyCode.B, KeyModifier.Shift, on => {
                DebugManager.I.AddBones();
            }),
            new DebugCommand("End current minigame with 0 stars (fail)", CommandCondition.OnPress, KeyCode.Alpha0, on => {
                DebugManager.I.ForceCurrentMinigameEnd(0);
            }),
            new DebugCommand("End current minigame with 1 star", CommandCondition.OnPress, KeyCode.Alpha1, on => {
                DebugManager.I.ForceCurrentMinigameEnd(1);
            }),
            new DebugCommand("End current minigame with 2 stars", CommandCondition.OnPress, KeyCode.Alpha2, on => {
                DebugManager.I.ForceCurrentMinigameEnd(2);
            }),
            new DebugCommand("End current minigame with 3 stars", CommandCondition.OnPress, KeyCode.Alpha3, on => {
                DebugManager.I.ForceCurrentMinigameEnd(3);
            })
        };
        // ANTURA DISCOVER CHEATS
        readonly DebugCommand[] _discoverCountriesCheats = new[] {
            new DebugCommand("Print Quest Info", CommandCondition.OnPress, KeyCode.I, KeyModifier.Shift, on => {
                QuestManager.I.PrintDebugInfo();
            }),
            new DebugCommand("Activate Target Marker on random light beam", CommandCondition.OnPress, KeyCode.T, KeyModifier.Shift, on => {
                LightBeam beam = FindFirstObjectByType<LightBeam>(FindObjectsInactive.Include);
                if (beam == null) Debug.LogWarning("Couldn't find a target beam");
                else InteractionManager.I.ActivateWorldTargetIcon(true, beam.transform);
            }),
            new DebugCommand("Action END", CommandCondition.OnPress, KeyCode.E, KeyModifier.Shift, on => {
                ActionManager.I.DebugActionEnd();
            })
        };

        #endregion

        static DebugControlsManager _I;
        bool _isShiftDown, _isCtrlDown, _isAltDown, _isAppleDown, _isCommandDown;

        #region Unity

        void Awake()
        {
            if (_I != null)
            {
                Debug.LogError("DebugControlsManager already exists, destroying duplicate");
                Destroy(this.gameObject);
            }

            _I = this;
            Debug.Log("<color=#d8249c>DEBUG SHORTCUTS enabled:</color> press <b>SHIFT + C</b>");
        }

        void Start()
        {
            RefreshControlsKeysPanel();
            _controlsKeysPanel.SetActive(false);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDestroy()
        {
            if (_I == this)
                _I = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void Update()
        {
            _isShiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            _isCtrlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            _isAltDown = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            _isAppleDown = Input.GetKey(KeyCode.LeftApple) || Input.GetKey(KeyCode.RightApple);
            _isCommandDown = Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

            if (DebugManager.I.DebugPanelOpened)
                return;

            UpdateCommandList(_tools);
            UpdateCommandList(_globalCheats);
            if (activateLearnLanguageCheats)
                UpdateCommandList(_learnLanguageCheats);
            if (activateDiscoverCountriesCheats)
                UpdateCommandList(_discoverCountriesCheats);
        }

        void UpdateCommandList(DebugCommand[] commands)
        {
            foreach (DebugCommand command in commands)
            {
                CommandResult commandResult = GetCommandResult(command);
                if (commandResult == CommandResult.Ignore)
                    continue;
                command.onTick(commandResult == CommandResult.Activate);
            }
        }

        #endregion

        #region Methods

        void RefreshControlsKeysPanel()
        {
            _Strb.Length = 0;
            _Strb.Append("<b><size=120%>TOOLS</size></b>");
            foreach (DebugCommand command in _tools)
                AppendCommandInfo(_Strb, command);
            _Strb.Append("\n\n<b><size=120%>GLOBAL CHEATS</size></b>");
            foreach (DebugCommand command in _globalCheats)
                AppendCommandInfo(_Strb, command);
            if (activateLearnLanguageCheats)
            {
                _Strb.Append("\n\n<b><size=120%>LEARN LANGUAGE CHEATS</size></b>");
                foreach (DebugCommand command in _learnLanguageCheats)
                    AppendCommandInfo(_Strb, command);
            }
            if (activateDiscoverCountriesCheats)
            {
                _Strb.Append("\n\n<b><size=120%>DISCOVER COUNTRIES CHEATS</size></b>");
                foreach (DebugCommand command in _discoverCountriesCheats)
                    AppendCommandInfo(_Strb, command);
            }
            _tfControlKeys.text = _Strb.ToString();
            _Strb.Length = 0;
        }

        void AppendCommandInfo(StringBuilder to, DebugCommand command)
        {
            to.Append("\n<b><color=#59ce1a>");
            if (command.keyModifier != KeyModifier.None)
            {
                to.Append(command.keyModifier.ToString().Nicify()).Append('+');
            }
            to.Append(command.key).Append(" ").Append("</color></b>")
                .Append(command.label);
        }

        CommandResult GetCommandResult(DebugCommand command)
        {
            bool active = false;
            bool wasActive = command.isActive;
            CommandResult result = CommandResult.Ignore;
            bool isModifierActive = IsExclusiveKeyModifierDown(command.keyModifier);
            switch (command.condition)
            {
                case CommandCondition.OnPress:
                    active = isModifierActive && Input.GetKeyDown(command.key);
                    result = active ? CommandResult.Activate : CommandResult.Ignore;
                    break;
                case CommandCondition.OnRelease:
                    active = isModifierActive && Input.GetKeyUp(command.key);
                    result = active ? CommandResult.Activate : CommandResult.Ignore;
                    break;
                case CommandCondition.WhilePressed:
                    active = isModifierActive && Input.GetKey(command.key);
                    result = active && !wasActive ? CommandResult.Activate : !active && wasActive ? CommandResult.Deactivate : CommandResult.Ignore;
                    break;
                case CommandCondition.RepeatWhilePressed:
                    active = isModifierActive && Input.GetKey(command.key);
                    result = active ? CommandResult.Activate : wasActive ? CommandResult.Deactivate : CommandResult.Ignore;
                    break;
            }
            command.isActive = active;
            return result;
        }

        bool IsExclusiveKeyModifierDown(KeyModifier keyModifier)
        {
            switch (keyModifier)
            {
                case KeyModifier.Shift:
                    return _isShiftDown && !_isCtrlDown && !_isAltDown && !_isAppleDown && !_isCommandDown;
                case KeyModifier.Ctrl:
                    return !_isShiftDown && _isCtrlDown && !_isAltDown && !_isAppleDown && !_isCommandDown;
                case KeyModifier.Alt:
                    return !_isShiftDown && !_isCtrlDown && _isAltDown && !_isAppleDown && !_isCommandDown;
                case KeyModifier.Apple:
                    return !_isShiftDown && !_isCtrlDown && !_isAltDown && _isAppleDown && !_isCommandDown;
                case KeyModifier.Command:
                    return !_isShiftDown && !_isCtrlDown && !_isAltDown && !_isAppleDown && _isCommandDown;
                default:
                    return !_isShiftDown && !_isCtrlDown && !_isAltDown && !_isAppleDown && !_isCommandDown;
            }
        }

        #endregion

        #region Callbacks

        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            RefreshControlsKeysPanel();
        }

        #endregion

        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ INTERNAL ENUMS ██████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        enum KeyModifier
        {
            None, Shift, Alt, Ctrl, Apple, Command
        }

        enum CommandCondition
        {
            OnPress, OnRelease, WhilePressed, RepeatWhilePressed
        }

        enum CommandResult
        {
            Activate, Deactivate, Ignore
        }

        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        class DebugCommand
        {
            public readonly string label;
            public readonly CommandCondition condition;
            public readonly KeyCode key;
            public readonly KeyModifier keyModifier;
            public readonly Action<bool> onTick;
            public bool isActive;

            public DebugCommand(string label, CommandCondition condition, KeyCode key, Action<bool> onTick)
                : this(label, condition, key, KeyModifier.None, onTick) { }

            public DebugCommand(string label, CommandCondition condition, KeyCode key, KeyModifier keyModifier, Action<bool> onTick)
            {
                this.label = label;
                this.condition = condition;
                this.key = key;
                this.keyModifier = keyModifier;
                this.onTick = onTick;
            }
        }
    }
}
