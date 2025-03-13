// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2020/10/19

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Demigiant.DemiTools.DeUnityExtended
{
    /// <summary>
    /// Advanced <see cref="Button"/> with extra options including Toggle mode and,
    /// if navigation is set to explicit, being able to find nearest selectable if set one is not valid
    /// </summary>
    public class DeUIButton : Button
    {
        enum ToggleMode
        {
            ManualOnOff, // Can always be manually toggled both ON and OFF
            NoManualOff, // Can only be manually toggled ON
            ManualOffIfNoGroup // Can be manually toggled OFF only if not in a group
        }
        
        /// <summary>Dispatched globally when a button is clicked or submitted</summary>
        public static event Handler_OnClickOrSubmit OnClickOrSubmit;
        public delegate void Handler_OnClickOrSubmit(DeUIButton bt);
        static void Dispatch_OnClickOrSubmit(DeUIButton bt) { if (OnClickOrSubmit != null) OnClickOrSubmit(bt); }

        #region Serialized
#pragma warning disable 0649

        // Hidden in Inspector since it uses regular Button.interactable value to be set
        // (required to leave toggled button non-interactable even if set to interactable)
        [SerializeField] bool _isInteractable = true;
        [SerializeField] bool _keepSelected = true; // If TRUE keeps the button selected even after it becomes non-interactable
        [SerializeField] DeUIUtils.Direction _selectOtherOnDisable = DeUIUtils.Direction.None; // If set and this is disabled, tries to select another selectable in the given direction
        [SerializeField] GameObject _showOnSelected; // Eventual gameObject to show when selected
        [SerializeField] Graphic _secondaryTargetGraphic; // Eventual secondary graphic object whose color to control with mouse events 
        [SerializeField] Graphic _hoverTargetGraphic; // Eventual tertiary graphic object whose color can be customized on hover 
        [SerializeField] Color _hoverTargetColor = Color.white; // Color assigned to eventual _hoverTargetGraphic on hover
        [SerializeField] bool _isToggle;
        [SerializeField] bool _isOn;
        [SerializeField] bool _borderOnlyWhenOff = false; // If TRUE only shows border of image if toggled (must be set to Sliced and have a border)
        [SerializeField] bool _isToggleGroup;
        [SerializeField] string _groupId = "";
        [SerializeField] ToggleMode _toggleMode = ToggleMode.ManualOffIfNoGroup;
        [SerializeField] Graphic _toggle_secondaryTarget; // If not NULL, uses this as a target for the following _toggle_[...] options when toggled ON
        [SerializeField] bool _toggle_activateIfToggled;
        [SerializeField] bool _toggle_changeColor;
        [SerializeField] Color _toggle_color = Color.white;
        [SerializeField] Sprite _toggle_sprite;
        [SerializeField] string _toggle_text;
        public ColorBlock toggledColors = ColorBlock.defaultColorBlock;
        public UnityEvent<bool> onToggle; // Sent both when toggle is toggled ON and OFF
        public UnityEvent onToggleOn; // Only sent when toggle is toggled ON
        public UnityEvent onPress;
        public UnityEvent onRelease;

#pragma warning restore 0649
        #endregion

        public bool isToggle { get { return _isToggle; } }
        public bool isToggleGroup { get { return _isToggleGroup; } }
        public bool isOn { get { return _isOn; } }
        public bool isPressed { get; private set; }
        public bool isSelected { get { return EventSystem.current != null && EventSystem.current.currentSelectedGameObject == this.gameObject; } }
        public new bool interactable {
            get => _isInteractable;
            set {
                Init();
                bool wasSelected = isSelected;
                // Commented to replace new version when I introduced _toggleMode
                // _isInteractable = base.interactable = _isToggle && _isToggleGroup && _isOn && !string.IsNullOrEmpty(_groupId) ? false : value;
                _isInteractable = base.interactable
                    = _isToggle && _isOn && (_toggleMode == ToggleMode.NoManualOff || _isValidToggleGroup && _toggleMode != ToggleMode.ManualOnOff)
                        ? false
                        : value;
                if (_keepSelected && wasSelected) Select();
            }
        }
        
        bool _initialized;
        static readonly Dictionary<string,List<DeUIButton>> _toggleGroupToButtons = new Dictionary<string, List<DeUIButton>>();
        bool _isValidToggleGroup;
        // bool _defsSet;
        ColorBlock _defColors;
        string _defText;
        bool _hasSecondaryTargetGraphic, _hasHoverTargetGraphic;
        bool _hasToggleBorderTargetImage, _hasToggleTargetImage, _hasToggleTargetGraphic, _hasToggleTargetText;
        Image _borderTargetImage, _toggleTargetImage;
        TMP_Text _toggleTargetTextField;
        Color _hoverTargetDefaultColor, _toggleTargetDefaultColor;

        #region Unity + INIT

        void Init()
        {
            if (_initialized) return;

            _initialized = true;

            _isValidToggleGroup = _isToggle && _isToggleGroup && !string.IsNullOrEmpty(_groupId);
            
            _defColors = this.colors;
            _hasToggleBorderTargetImage = (_borderTargetImage = targetGraphic as Image) != null;
            _hasToggleTargetGraphic = _toggle_secondaryTarget != null;
            _hasToggleTargetImage = _hasToggleTargetGraphic && (_toggleTargetImage = _toggle_secondaryTarget as Image) != null;
            _hasToggleTargetText = _hasToggleTargetGraphic && (_toggleTargetTextField = _toggle_secondaryTarget as TMP_Text) != null;
            if (_hasToggleTargetText) _defText = _toggleTargetTextField.text;
            if (_hasToggleTargetGraphic) _toggleTargetDefaultColor = _toggle_secondaryTarget.color;
            
            _hasSecondaryTargetGraphic = _secondaryTargetGraphic != null;
            _hasHoverTargetGraphic = _hoverTargetGraphic != null;
            if (_hasSecondaryTargetGraphic) _secondaryTargetGraphic.color = targetGraphic.color;
            if (_hasHoverTargetGraphic) _hoverTargetDefaultColor = _hoverTargetGraphic.color;
        }

        protected override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying) return;

            Init();
            if (_isValidToggleGroup) AddToToggleGroup();
            if (_showOnSelected != null && !isSelected) _showOnSelected.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            if (!Application.isPlaying) return;

            if (_isToggle) DoToggle(_isOn, false, false, true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (_showOnSelected != null) _showOnSelected.SetActive(false);
            if (_selectOtherOnDisable != DeUIUtils.Direction.None) {
                EventSystem currentES = EventSystem.current;
                if (currentES != null) {
                    Selectable other;
                    switch (_selectOtherOnDisable) {
                    case DeUIUtils.Direction.Up:
                        other = DeUIUtils.FindNearestNavSelectableUp(this, true);
                        if (other != null) currentES.SetSelectedGameObject(other.gameObject);
                        break;
                    case DeUIUtils.Direction.Down:
                        other = DeUIUtils.FindNearestNavSelectableDown(this, true);
                        if (other != null) currentES.SetSelectedGameObject(other.gameObject);
                        break;
                    case DeUIUtils.Direction.Left:
                        other = DeUIUtils.FindNearestNavSelectableLeft(this);
                        if (other != null) currentES.SetSelectedGameObject(other.gameObject);
                        break;
                    case DeUIUtils.Direction.Right:
                        other = DeUIUtils.FindNearestNavSelectableRight(this);
                        if (other != null) currentES.SetSelectedGameObject(other.gameObject);
                        break;
                    }
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.onClick.RemoveAllListeners();
            if (_isValidToggleGroup) RemoveFromToggleGroup();
        }

        void LateUpdate()
        {
            if (!_hasSecondaryTargetGraphic) return;
            if (_secondaryTargetGraphic.canvasRenderer.GetColor() != targetGraphic.canvasRenderer.GetColor()) {
                _secondaryTargetGraphic.canvasRenderer.SetColor(targetGraphic.canvasRenderer.GetColor());
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            isPressed = true;
            if (onPress != null) onPress.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            isPressed = false;
            if (onRelease != null) onRelease.Invoke();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            bool wasInteractable = IsInteractable();
            base.OnPointerClick(eventData);
            if (eventData.button == PointerEventData.InputButton.Left) EvaluateClickOrSubmit(wasInteractable);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            if (!isSelected) return; // Prevent submit if not selected
            bool wasInteractable = IsInteractable();
            base.OnSubmit(eventData);
            EvaluateClickOrSubmit(wasInteractable);
        }
        
        void EvaluateClickOrSubmit(bool wasInteractable)
        {
            if (!IsInteractable() && !wasInteractable) return;
            if (_isToggle) DoToggle(!_isOn);
            Dispatch_OnClickOrSubmit(this);
        }
        
#if UNITY_STANDALONE

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (_hasHoverTargetGraphic) _hoverTargetGraphic.color = _hoverTargetColor;
            DeUIManager.PointerEntered(this);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (_hasHoverTargetGraphic) _hoverTargetGraphic.color = _hoverTargetDefaultColor;
            DeUIManager.PointerExited(this);
        }
        
#endif

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            if (_showOnSelected != null) _showOnSelected.SetActive(true);
            if (_hasHoverTargetGraphic) _hoverTargetGraphic.color = _hoverTargetColor;
            DeUIManager.Selected(this);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            if (_showOnSelected != null) _showOnSelected.SetActive(false);
            if (_hasHoverTargetGraphic) _hoverTargetGraphic.color = _hoverTargetDefaultColor;
            DeUIManager.Deselected(this);
        }
        
        public override Selectable FindSelectableOnUp()
        {
            switch (navigation.mode) {
            case Navigation.Mode.Explicit: return DeUIUtils.FindNearestNavSelectableUp(this, true);
            default: return base.FindSelectableOnUp();
            }
        }

        public override Selectable FindSelectableOnDown()
        {
            switch (navigation.mode) {
            case Navigation.Mode.Explicit: return DeUIUtils.FindNearestNavSelectableDown(this, true);
            default: return base.FindSelectableOnDown();
            }
        }
        
        public override Selectable FindSelectableOnLeft()
        {
            switch (navigation.mode) {
            case Navigation.Mode.Explicit: return DeUIUtils.FindNearestNavSelectableLeft(this);
            default: return base.FindSelectableOnLeft();
            }
        }

        public override Selectable FindSelectableOnRight()
        {
            switch (navigation.mode) {
            case Navigation.Mode.Explicit: return DeUIUtils.FindNearestNavSelectableRight(this);
            default: return base.FindSelectableOnRight();
            }
        }

        #endregion

        #region Public Methods

        public void Toggle(bool toggleOn, bool dispatchEvents = true)
        {
            if (!_isToggle) {
                Debug.LogWarning(string.Format("DeUIButton({0}).Toggle ► Button is not a toggle", this.name), this);
                return;
            }
            DoToggle(toggleOn, false, dispatchEvents);
        }

        #endregion

        #region Methods

        void DoToggle(bool toggleOn, bool ignoreGroup = false, bool dispatchEvents = true, bool forceRefresh = false)
        {
            if (!forceRefresh && _isOn == toggleOn) return; // Already set

            Init();
            
            // Group management
            // (disable ON button if toggleMode requests it and enable OFF ones)
            if (toggleOn && !ignoreGroup && _isValidToggleGroup) {
                List<DeUIButton> others = _toggleGroupToButtons[_groupId];
                for (int i = 0; i < others.Count; ++i) {
                    DeUIButton bt = others[i];
                    if (bt == this) continue;
                    bt.DoToggle(false, true, dispatchEvents);
                    bt.interactable = true;
                }
                interactable = interactable && _toggleMode == ToggleMode.ManualOnOff;
            }
            // Non-group
            // Disable if ON and toggleMode requests it
            if (toggleOn && _toggleMode == ToggleMode.NoManualOff) interactable = false;
            
            // Toggle
            _isOn = toggleOn;
            this.colors = toggleOn ? toggledColors : _defColors;
            if (_borderOnlyWhenOff && _hasToggleBorderTargetImage) {
                if (!toggleOn) _borderTargetImage.type = Image.Type.Sliced;
                _borderTargetImage.fillCenter = toggleOn;
            }
            if (_hasToggleTargetImage && _toggle_sprite != null) {
                _toggleTargetImage.overrideSprite = _isOn ? _toggle_sprite : null;
            }
            if (_hasToggleTargetText && !string.IsNullOrEmpty(_toggle_text)) {
                _toggleTargetTextField.text = _isOn ? _toggle_text : _defText;
            }
            if (_hasToggleTargetGraphic) {
                if (_toggle_activateIfToggled) _toggle_secondaryTarget.gameObject.SetActive(isOn);
                if (_toggle_changeColor) _toggle_secondaryTarget.color = isOn ? _toggle_color : _toggleTargetDefaultColor;
            }
            if (dispatchEvents) {
                if (isOn && onToggleOn != null) onToggleOn.Invoke();
                if (onToggle != null) onToggle.Invoke(isOn);
            }
        }

        void AddToToggleGroup()
        {
            if (_toggleGroupToButtons.ContainsKey(_groupId)) _toggleGroupToButtons[_groupId].Add(this);
            else _toggleGroupToButtons.Add(_groupId, new List<DeUIButton>() { this });
        }

        void RemoveFromToggleGroup()
        {
            if (!_toggleGroupToButtons.ContainsKey(_groupId)) return;
            List<DeUIButton> li = _toggleGroupToButtons[_groupId];
            li.Remove(this);
            if (li.Count == 0) _toggleGroupToButtons.Remove(_groupId);
        }

        #endregion
    }
}