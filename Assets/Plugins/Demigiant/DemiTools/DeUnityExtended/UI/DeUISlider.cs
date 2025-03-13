// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2022/04/12

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Demigiant.DemiTools.DeUnityExtended
{
    /// <summary>
    /// Advanced <see cref="Slider"/> with extra options and,
    /// if navigation is set to explicit, being able to find nearest selectable if set one is not valid
    /// </summary>
    public class DeUISlider : Slider
    {
        #region Serialized
#pragma warning disable 0649
        
        [SerializeField] DeUIUtils.Direction _selectOtherOnDisable = DeUIUtils.Direction.None; // If set and this is disabled, tries to select another selectable in the given direction
        [SerializeField] GameObject _showOnSelected; // Eventual gameObject to show when selected
        
#pragma warning restore 0649
        #endregion
        
        public bool isSelected { get { return EventSystem.current != null && EventSystem.current.currentSelectedGameObject == this.gameObject; } }

        #region Unity

        protected override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying) return;

            if (_showOnSelected != null && !isSelected) _showOnSelected.SetActive(false);
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
        
#if UNITY_STANDALONE
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            DeUIManager.PointerEntered(this);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            DeUIManager.PointerExited(this);
        }
        
#endif
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            if (_showOnSelected != null) _showOnSelected.SetActive(true);
            DeUIManager.Selected(this);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            if (_showOnSelected != null) _showOnSelected.SetActive(false);
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
    }
}