using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using ISTouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Antura
{
    /// <summary>
    /// We want to use this class for all future input handling in the project, to avoid coupling game code to the new Input System API and to
    /// provide a drop-in replacement for the legacy UnityEngine.Input API, backed by the new Input System.
    /// Mirrors the old behaviour where touches also drive the "mouse" pointer
    /// (the legacy Input.simulateMouseWithTouches), so pointer code works on both desktop and mobile.
    /// </summary>
    public static class InputCompat
    {
        #region Init

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            EnsureEnhancedTouch();
        }

        private static void EnsureEnhancedTouch()
        {
            if (!ETouch.EnhancedTouchSupport.enabled)
                ETouch.EnhancedTouchSupport.Enable();
        }

        #endregion

        #region Pointer (mouse + touch unified, like legacy simulateMouseWithTouches)

        /// <summary>Current pointer position (primary touch if present, otherwise mouse). Replaces Input.mousePosition.</summary>
        public static Vector3 mousePosition
        {
            get
            {
                EnsureEnhancedTouch();
                var touches = ETouch.Touch.activeTouches;
                if (touches.Count > 0)
                    return touches[0].screenPosition;
                if (Mouse.current != null)
                    return Mouse.current.position.ReadValue();
                return Vector3.zero;
            }
        }

        /// <summary>Replaces Input.GetMouseButton. Button 0 also reacts to touches.</summary>
        public static bool GetMouseButton(int button)
        {
            var control = GetMouseButtonControl(button);
            bool mouse = control != null && control.isPressed;
            if (button != 0)
                return mouse;
            return mouse || AnyTouch(t => IsActivePhase(t.phase));
        }

        /// <summary>Replaces Input.GetMouseButtonDown. Button 0 also reacts to touches.</summary>
        public static bool GetMouseButtonDown(int button)
        {
            var control = GetMouseButtonControl(button);
            bool mouse = control != null && control.wasPressedThisFrame;
            if (button != 0)
                return mouse;
            return mouse || AnyTouch(t => t.phase == ISTouchPhase.Began);
        }

        /// <summary>Replaces Input.GetMouseButtonUp. Button 0 also reacts to touches.</summary>
        public static bool GetMouseButtonUp(int button)
        {
            var control = GetMouseButtonControl(button);
            bool mouse = control != null && control.wasReleasedThisFrame;
            if (button != 0)
                return mouse;
            return mouse || AnyTouch(t => t.phase == ISTouchPhase.Ended || t.phase == ISTouchPhase.Canceled);
        }

        private static UnityEngine.InputSystem.Controls.ButtonControl GetMouseButtonControl(int button)
        {
            var mouse = Mouse.current;
            if (mouse == null)
                return null;
            switch (button)
            {
                case 0:
                    return mouse.leftButton;
                case 1:
                    return mouse.rightButton;
                case 2:
                    return mouse.middleButton;
                default:
                    return null;
            }
        }

        /// <summary>Replaces Input.mouseScrollDelta. Normalized so one wheel notch is ~1 on y.</summary>
        public static Vector2 mouseScrollDelta
        {
            get
            {
                if (Mouse.current == null)
                    return Vector2.zero;
                var raw = Mouse.current.scroll.ReadValue();
                return new Vector2(NormalizeScroll(raw.x), NormalizeScroll(raw.y));
            }
        }

        private static float NormalizeScroll(float value)
        {
            // Windows reports +-120 per notch, other platforms report small values.
            if (Mathf.Abs(value) >= 100f)
                return value / 120f;
            return value;
        }

        #endregion

        #region Axes

        private const float MouseAxisSensitivity = 0.1f; // legacy Input Manager default for mouse axes

        /// <summary>
        /// Replaces Input.GetAxis for the axes used in this project:
        /// "Mouse X", "Mouse Y", "Mouse ScrollWheel", "Horizontal", "Vertical".
        /// </summary>
        public static float GetAxis(string axisName)
        {
            switch (axisName)
            {
                case "Mouse X":
                    return PointerDelta().x * MouseAxisSensitivity;
                case "Mouse Y":
                    return PointerDelta().y * MouseAxisSensitivity;
                case "Mouse ScrollWheel":
                    return Mathf.Clamp(mouseScrollDelta.y, -1f, 1f) * 0.1f;
                case "Horizontal":
                    return ComposeMoveAxis(Key.A, Key.D, Key.LeftArrow, Key.RightArrow,
                        Gamepad.current != null ? Gamepad.current.leftStick.ReadValue().x : 0f);
                case "Vertical":
                    return ComposeMoveAxis(Key.S, Key.W, Key.DownArrow, Key.UpArrow,
                        Gamepad.current != null ? Gamepad.current.leftStick.ReadValue().y : 0f);
                default:
                    Debug.LogWarning($"[InputCompat] GetAxis: unsupported axis '{axisName}'");
                    return 0f;
            }
        }

        private static Vector2 PointerDelta()
        {
            EnsureEnhancedTouch();
            var touches = ETouch.Touch.activeTouches;
            if (touches.Count > 0)
                return touches[0].delta;
            if (Mouse.current != null)
                return Mouse.current.delta.ReadValue();
            return Vector2.zero;
        }

        private static float ComposeMoveAxis(Key negA, Key posA, Key negB, Key posB, float analog)
        {
            float value = analog;
            var kb = Keyboard.current;
            if (kb != null)
            {
                if (kb[negA].isPressed || kb[negB].isPressed)
                    value -= 1f;
                if (kb[posA].isPressed || kb[posB].isPressed)
                    value += 1f;
            }
            return Mathf.Clamp(value, -1f, 1f);
        }

        #endregion

        #region Touch

        /// <summary>Replaces Input.touchCount.</summary>
        public static int touchCount
        {
            get
            {
                EnsureEnhancedTouch();
                return ETouch.Touch.activeTouches.Count;
            }
        }

        /// <summary>Replaces Input.GetTouch.</summary>
        public static CompatTouch GetTouch(int index)
        {
            EnsureEnhancedTouch();
            return CompatTouch.From(ETouch.Touch.activeTouches[index]);
        }

        /// <summary>Replaces Input.touches. Allocates: prefer touchCount + GetTouch in hot paths.</summary>
        public static CompatTouch[] touches
        {
            get
            {
                EnsureEnhancedTouch();
                var active = ETouch.Touch.activeTouches;
                var result = new CompatTouch[active.Count];
                for (int i = 0; i < active.Count; i++)
                    result[i] = CompatTouch.From(active[i]);
                return result;
            }
        }

        private static bool AnyTouch(System.Func<ETouch.Touch, bool> predicate)
        {
            EnsureEnhancedTouch();
            var active = ETouch.Touch.activeTouches;
            for (int i = 0; i < active.Count; i++)
            {
                if (predicate(active[i]))
                    return true;
            }
            return false;
        }

        private static bool IsActivePhase(ISTouchPhase phase)
        {
            return phase == ISTouchPhase.Began
                || phase == ISTouchPhase.Moved
                || phase == ISTouchPhase.Stationary;
        }

        /// <summary>
        /// True if the given touch (or the mouse, when fingerId is omitted) is over a UI element.
        /// Use instead of EventSystem.current.IsPointerOverGameObject(fingerId):
        /// with InputSystemUIInputModule the id must be the touchId, which is what CompatTouch.fingerId carries.
        /// </summary>
        public static bool IsPointerOverUI(int fingerId = -1)
        {
            var es = EventSystem.current;
            if (es == null)
                return false;
            return es.IsPointerOverGameObject(fingerId);
        }

        #endregion

        #region Keyboard

        /// <summary>Replaces Input.anyKeyDown (keyboard or pointer press).</summary>
        public static bool anyKeyDown
        {
            get
            {
                var kb = Keyboard.current;
                if (kb != null && kb.anyKey.wasPressedThisFrame)
                    return true;
                return GetMouseButtonDown(0);
            }
        }

        public static bool GetKey(KeyCode keyCode)
        {
            var control = GetKeyControl(keyCode);
            return control != null && control.isPressed;
        }

        public static bool GetKeyDown(KeyCode keyCode)
        {
            var control = GetKeyControl(keyCode);
            return control != null && control.wasPressedThisFrame;
        }

        public static bool GetKeyUp(KeyCode keyCode)
        {
            var control = GetKeyControl(keyCode);
            return control != null && control.wasReleasedThisFrame;
        }

        public static bool GetKey(Key key)
        {
            var kb = Keyboard.current;
            return kb != null && key != Key.None && kb[key].isPressed;
        }

        public static bool GetKeyDown(Key key)
        {
            var kb = Keyboard.current;
            return kb != null && key != Key.None && kb[key].wasPressedThisFrame;
        }

        public static bool GetKeyUp(Key key)
        {
            var kb = Keyboard.current;
            return kb != null && key != Key.None && kb[key].wasReleasedThisFrame;
        }

        private static UnityEngine.InputSystem.Controls.KeyControl GetKeyControl(KeyCode keyCode)
        {
            var kb = Keyboard.current;
            if (kb == null)
                return null;
            var key = KeyCodeToKey(keyCode);
            return key == Key.None ? null : kb[key];
        }

        /// <summary>Maps legacy KeyCode values to new Input System Key values.</summary>
        public static Key KeyCodeToKey(KeyCode keyCode)
        {
            // Letters
            if (keyCode >= KeyCode.A && keyCode <= KeyCode.Z)
                return Key.A + (keyCode - KeyCode.A);
            // Top row digits
            if (keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9)
                return Key.Digit0 + (keyCode - KeyCode.Alpha0);
            // Keypad digits
            if (keyCode >= KeyCode.Keypad0 && keyCode <= KeyCode.Keypad9)
                return Key.Numpad0 + (keyCode - KeyCode.Keypad0);
            // Function keys
            if (keyCode >= KeyCode.F1 && keyCode <= KeyCode.F12)
                return Key.F1 + (keyCode - KeyCode.F1);

            switch (keyCode)
            {
                case KeyCode.Space:
                    return Key.Space;
                case KeyCode.Return:
                    return Key.Enter;
                case KeyCode.KeypadEnter:
                    return Key.NumpadEnter;
                case KeyCode.Escape:
                    return Key.Escape;
                case KeyCode.Tab:
                    return Key.Tab;
                case KeyCode.Backspace:
                    return Key.Backspace;
                case KeyCode.Delete:
                    return Key.Delete;
                case KeyCode.Insert:
                    return Key.Insert;
                case KeyCode.Home:
                    return Key.Home;
                case KeyCode.End:
                    return Key.End;
                case KeyCode.PageUp:
                    return Key.PageUp;
                case KeyCode.PageDown:
                    return Key.PageDown;
                case KeyCode.UpArrow:
                    return Key.UpArrow;
                case KeyCode.DownArrow:
                    return Key.DownArrow;
                case KeyCode.LeftArrow:
                    return Key.LeftArrow;
                case KeyCode.RightArrow:
                    return Key.RightArrow;
                case KeyCode.LeftShift:
                    return Key.LeftShift;
                case KeyCode.RightShift:
                    return Key.RightShift;
                case KeyCode.LeftControl:
                    return Key.LeftCtrl;
                case KeyCode.RightControl:
                    return Key.RightCtrl;
                case KeyCode.LeftAlt:
                    return Key.LeftAlt;
                case KeyCode.RightAlt:
                    return Key.RightAlt;
                case KeyCode.LeftCommand:
                    return Key.LeftMeta;   // also LeftApple
                case KeyCode.RightCommand:
                    return Key.RightMeta; // also RightApple
                case KeyCode.LeftWindows:
                    return Key.LeftMeta;
                case KeyCode.RightWindows:
                    return Key.RightMeta;
                case KeyCode.CapsLock:
                    return Key.CapsLock;
                case KeyCode.Numlock:
                    return Key.NumLock;
                case KeyCode.ScrollLock:
                    return Key.ScrollLock;
                case KeyCode.Pause:
                    return Key.Pause;
                case KeyCode.Print:
                    return Key.PrintScreen;
                case KeyCode.Menu:
                    return Key.ContextMenu;
                case KeyCode.Comma:
                    return Key.Comma;
                case KeyCode.Period:
                    return Key.Period;
                case KeyCode.Slash:
                    return Key.Slash;
                case KeyCode.Backslash:
                    return Key.Backslash;
                case KeyCode.Semicolon:
                    return Key.Semicolon;
                case KeyCode.Quote:
                    return Key.Quote;
                case KeyCode.LeftBracket:
                    return Key.LeftBracket;
                case KeyCode.RightBracket:
                    return Key.RightBracket;
                case KeyCode.Minus:
                    return Key.Minus;
                case KeyCode.Equals:
                    return Key.Equals;
                case KeyCode.BackQuote:
                    return Key.Backquote;
                case KeyCode.KeypadPlus:
                    return Key.NumpadPlus;
                case KeyCode.KeypadMinus:
                    return Key.NumpadMinus;
                case KeyCode.KeypadMultiply:
                    return Key.NumpadMultiply;
                case KeyCode.KeypadDivide:
                    return Key.NumpadDivide;
                case KeyCode.KeypadPeriod:
                    return Key.NumpadPeriod;
                case KeyCode.KeypadEquals:
                    return Key.NumpadEquals;
                default:
                    return Key.None;
            }
        }

        #endregion
    }

    /// <summary>
    /// Mirrors the legacy UnityEngine.Touch struct, backed by EnhancedTouch.
    /// fingerId carries the touchId so it can be passed to EventSystem.IsPointerOverGameObject
    /// when InputSystemUIInputModule is in use.
    /// </summary>
    public struct CompatTouch
    {
        public int fingerId;
        public Vector2 position;
        public Vector2 deltaPosition;
        public UnityEngine.TouchPhase phase;
        public int tapCount;

        internal static CompatTouch From(ETouch.Touch touch)
        {
            return new CompatTouch
            {
                fingerId = touch.touchId,
                position = touch.screenPosition,
                deltaPosition = touch.delta,
                phase = ToLegacyPhase(touch.phase),
                tapCount = touch.tapCount
            };
        }

        private static UnityEngine.TouchPhase ToLegacyPhase(ISTouchPhase phase)
        {
            switch (phase)
            {
                case ISTouchPhase.Began:
                    return UnityEngine.TouchPhase.Began;
                case ISTouchPhase.Moved:
                    return UnityEngine.TouchPhase.Moved;
                case ISTouchPhase.Stationary:
                    return UnityEngine.TouchPhase.Stationary;
                case ISTouchPhase.Ended:
                    return UnityEngine.TouchPhase.Ended;
                case ISTouchPhase.Canceled:
                    return UnityEngine.TouchPhase.Canceled;
                default:
                    return UnityEngine.TouchPhase.Canceled;
            }
        }
    }
}
