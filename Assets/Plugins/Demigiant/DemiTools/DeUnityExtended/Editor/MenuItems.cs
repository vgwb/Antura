// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2020/03/19

using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Demigiant.DemiTools.DeUnityExtended.Editor
{
    internal class MenuItems
    {
        #region CREATE

        #region UI

        enum UIButtonMode
        {
            Empty, Image, Text
        }

        [MenuItem("GameObject/UI/DeUIButton/Empty", false)]
        static void Create_UIButton(MenuCommand menuCommand)
        {
            DoCreate_UIButton(menuCommand, UIButtonMode.Empty);
        }
            
        [MenuItem("GameObject/UI/DeUIButton/With Child Image", false)]
        static void Create_UIButtonWImage(MenuCommand menuCommand)
        {
            DoCreate_UIButton(menuCommand, UIButtonMode.Image);
        }

        [MenuItem("GameObject/UI/DeUIButton/With Child Text", false)]
        static void Create_UIButtonWText(MenuCommand menuCommand)
        {
            DoCreate_UIButton(menuCommand, UIButtonMode.Text);
        }

        static void DoCreate_UIButton(MenuCommand menuCommand, UIButtonMode mode)
        {
            GameObject go = new GameObject("UIButton");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            // Setup size
            RectTransform rt = go.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(160, 30);
            // Add sub elements
            switch (mode) {
            case UIButtonMode.Image:
                AddUIButtonImage(go.transform);
                break;
            case UIButtonMode.Text:
                AddUIButtonText(go.transform);
                break;
            }
            // Setup button colors and navigation
            Image img = go.AddComponent<Image>();
            DeUIButton bt = go.AddComponent<DeUIButton>();
            // bt.toggledColors = ColorBlock.defaultColorBlock;
            // bt.toggledColors.colorMultiplier = 1;
            // bt.toggledColors.fadeDuration = 0.1f;
            // bt.toggledColors.normalColor = Color.white;
            // bt.toggledColors.highlightedColor = Color.white;
            // bt.toggledColors.pressedColor = Color.white;
            // bt.toggledColors.disabledColor = Color.white;
            Navigation nav = bt.navigation;
            nav.mode = Navigation.Mode.None;
            bt.navigation = nav;
            bt.targetGraphic = img;
            ColorBlock colors = bt.colors;
            colors.normalColor = new Color(0.1529412f, 0.4666667f, 0.9843138f, 1f);
            colors.highlightedColor = new Color(0.09803922f, 0.6196079f, 0.8313726f, 1f);
            colors.pressedColor = new Color(0f, 0.9725491f, 0.7294118f, 1f);
            bt.colors = colors;
            //
            Selection.activeObject = go;
        }
        static void AddUIButtonImage(Transform parent)
        {
            GameObject go = new GameObject("Image");
            go.transform.SetParent(parent, false);
            Image img = go.AddComponent<Image>();
            img.raycastTarget = false;
            GetOrCreateRectTransform(go);
        }
        static void AddUIButtonText(Transform parent)
        {
            GameObject go = new GameObject("Text");
            go.transform.SetParent(parent, false);
            TextMeshProUGUI tf = go.AddComponent<TextMeshProUGUI>();
            tf.raycastTarget = false;
            tf.alignment = TextAlignmentOptions.Center;
            tf.fontSize = 24;
            tf.text = "Button";
            GetOrCreateRectTransform(go);
        }

        #endregion
        
        #endregion

        #region Helpers

        static RectTransform GetOrCreateRectTransform(GameObject target, bool stretchToFill = true)
        {
            RectTransform rt = target.GetComponent<RectTransform>();
            if (rt == null) rt = target.AddComponent<RectTransform>();
            if (stretchToFill) StretchToFill(rt);
            return rt;
        }

        /// <summary>
        /// Sets the given RectTransform to stretch to the exact borders of its parent
        /// </summary>
        public static void StretchToFill(RectTransform rt)
        {
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMax = new Vector2(0, 0);
            rt.offsetMin = new Vector2(0, 0);
        }

        #endregion
    }
}