// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2022/04/13

using UnityEngine.UI;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Demigiant.DemiTools.DeUnityExtended
{
    /// <summary>
    /// Global manager and global event notifier for DeUI elements
    /// </summary>
    public static class DeUIManager
    {
        #region EVENTS

        /// <summary>Dispatched when a DeUI object is selected</summary>
        public static ActionEvent<Selectable> OnSelected = new ActionEvent<Selectable>("DeUIManager.OnSelected");
        /// <summary>Dispatched when a DeUI object is hovered/entered/highlighted (including when selected)</summary>
        public static ActionEvent<Selectable> OnHighlighted = new ActionEvent<Selectable>("DeUIManager.OnHighlighted");
        /// <summary>Dispatched when <see cref="currHighlighted"/> changes</summary>
        public static ActionEvent<Selectable> OnHighlightedChanged = new ActionEvent<Selectable>("DeUIManager.OnHighlightedChanged");

        #endregion
        
        /// <summary>Currently selected DeUI object (can be NULL also if a normal UI object is selected)</summary>
        public static Selectable currSelected { get; private set; }
        /// <summary>Currently highlighted DeUI object (can be NULL also if a normal UI object is highlighted)</summary>
        public static Selectable currHighlighted {
            get { return _currHighlighted; }
            set {
                if (value != null && _currHighlighted == value) return;
                _currHighlighted = value;
                OnHighlightedChanged.Dispatch(value);
            }
        }
        /// <summary>Current DeUI object with mouse hover state (can be NULL also if a normal UI object has a mouse hover state)</summary>
        public static Selectable currHovered { get; private set; }
        static Selectable _currHighlighted;

        #region Internal Methods

        internal static void Selected(Selectable selectable)
        {
            if (selectable == null) {
                currSelected = currHighlighted = null;
                return;
            }
            
            currSelected = currHighlighted = selectable;
            OnSelected.Dispatch(selectable);
            OnHighlighted.Dispatch(selectable);
        }
        
        public static void Deselected(Selectable selectable)
        {
            if (selectable != currSelected) return;

            currSelected = null;
            currHighlighted = currHovered;
        }

        internal static void PointerEntered(Selectable selectable)
        {
            if (selectable == null) {
                currHighlighted = currHovered = null;
                return;
            }
            
            currHighlighted = currHovered = selectable;
            OnHighlighted.Dispatch(selectable);
        }

        internal static void PointerExited(Selectable selectable)
        {
            if (currHovered == selectable) currHovered = null;
            if (selectable == null || currHighlighted == selectable) currHighlighted = currSelected;
        }

        #endregion
    }
}