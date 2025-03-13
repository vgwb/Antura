// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2020/10/22

using UnityEngine;
using UnityEngine.UI;

namespace Demigiant.DemiTools.DeUnityExtended
{
    public static class DeUIUtils
    {
        // Used by DeUI Selectables to indicate selectability retrieval direction
        public enum Direction
        {
            None, Up, Down, Left, Right
        }
        
        #region Public Methods

        /// <summary>
        /// Changes the pivot of a RectTransform without changing its world position
        /// </summary>
        /// Based on code by jomrhart: http://answers.unity.com/answers/1545950/view.html
        public static void ChangePivotOnly(RectTransform rt, Vector2 pivot)
        {
            Vector3 deltaPosition = rt.pivot - pivot;
            deltaPosition.Scale(rt.rect.size);
            deltaPosition.Scale(rt.localScale);
            deltaPosition = rt.localRotation * deltaPosition;
            rt.pivot = pivot;
            rt.localPosition -= deltaPosition;
        }

        /// <summary>
        /// Finds the first valid <see cref="Selectable"/> connected to the Navigation property of the given one,
        /// skipping eventual invalid Selectables and going through their internal Navigation to find a valid set one in the given direction.
        /// A Selectable is considered valid either if NULL (which means navigation ends there) or both active in hierarchy and interactable. 
        /// </summary>
        public static Selectable FindNearestNavSelectableUp(Selectable selectable, bool horizontalIsLeftToRight)
        {
            return FindNearestNavSelectableVertically(selectable, true, horizontalIsLeftToRight);
        }
        /// <summary>
        /// Finds the first valid <see cref="Selectable"/> connected to the Navigation property of the given one,
        /// skipping eventual invalid Selectables and going through their internal Navigation to find a valid set one in the given direction.
        /// A Selectable is considered valid either if NULL (which means navigation ends there) or both active in hierarchy and interactable. 
        /// </summary>
        public static Selectable FindNearestNavSelectableDown(Selectable selectable, bool horizontalIsLeftToRight)
        {
            return FindNearestNavSelectableVertically(selectable, false, horizontalIsLeftToRight);
        }
        /// <summary>
        /// Finds the first valid <see cref="Selectable"/> connected to the Navigation property of the given one,
        /// skipping eventual invalid Selectables and going through their internal Navigation to find a valid set one in the given direction.
        /// A Selectable is considered valid either if NULL (which means navigation ends there) or both active in hierarchy and interactable. 
        /// </summary>
        public static Selectable FindNearestNavSelectableLeft(Selectable selectable)
        {
            return FindNearestNavSelectableHorizontally(selectable, false);
        }
        /// <summary>
        /// Finds the first valid <see cref="Selectable"/> connected to the Navigation property of the given one,
        /// skipping eventual invalid Selectables and going through their internal Navigation to find a valid set one in the given direction.
        /// A Selectable is considered valid either if NULL (which means navigation ends there) or both active in hierarchy and interactable. 
        /// </summary>
        public static Selectable FindNearestNavSelectableRight(Selectable selectable)
        {
            return FindNearestNavSelectableHorizontally(selectable, true);
        }

        #endregion

        #region Methods

        static Selectable FindNearestNavSelectableVertically(Selectable selectable, bool isUp, bool horizontalIsLeftToRight)
        {
            Selectable next = isUp ? selectable.navigation.selectOnUp : selectable.navigation.selectOnDown;
            while (!IsValidNavSelectable(next)) {
                // First look in default horizontal direction
                Selectable horNext = FindNearestNavSelectableHorizontally(next, horizontalIsLeftToRight);
                while (!IsValidNavSelectable(horNext)) {
                    horNext = FindNearestNavSelectableHorizontally(next, horizontalIsLeftToRight);
                }
                if (horNext != null) return horNext;
                // Then in opposite one
                horNext = FindNearestNavSelectableHorizontally(next, !horizontalIsLeftToRight);
                while (!IsValidNavSelectable(horNext)) {
                    horNext = FindNearestNavSelectableHorizontally(next, !horizontalIsLeftToRight);
                }
                if (horNext != null) return horNext;
                next = isUp ? next.navigation.selectOnUp : next.navigation.selectOnDown;
            }
            return next;
        }

        static Selectable FindNearestNavSelectableHorizontally(Selectable selectable, bool isRight)
        {
            Selectable next = isRight ? selectable.navigation.selectOnRight : selectable.navigation.selectOnLeft;
            while (!IsValidNavSelectable(next)) {
                next = isRight ? next.navigation.selectOnRight : next.navigation.selectOnLeft;
            }
            return next;
        }

        static bool IsValidNavSelectable(Selectable selectable)
        {
            return selectable == null || selectable.IsInteractable() && selectable.gameObject.activeInHierarchy;
        }

        #endregion
    }
}