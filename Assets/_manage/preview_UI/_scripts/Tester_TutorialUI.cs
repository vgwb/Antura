using System;
using System.Collections.Generic;
using Antura.Tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Test
{
    /// <summary>
    /// Helper class to test the TutorialUI
    /// <seealso cref="TutorialUI"/>
    /// </summary>
    public class Tester_TutorialUI : MonoBehaviour
    {
        public enum DrawMode
        {
            StraightLine,
            FullCurve,
            Click,
            ClickRepeat,
            RandomMark
        }

        public float CameraDistance = 20;
        public float MinPointDistance = 0.3f;
        [Header("References")]
        public Dropdown DrawModeDropdown;
        public Toggle FingerToggle, ArrowToggle, PersistentToggle, OverlayToggle;
        public Toggle NormalToggle, BigToggle, HugeToggle;

        DrawMode drawMode = DrawMode.StraightLine;
        bool isDraggingMode { get { return drawMode == DrawMode.FullCurve || drawMode == DrawMode.StraightLine; } }
        readonly List<Vector3> storedPs = new List<Vector3>();
        bool isDragging;

        #region Unity

        void Start()
        {
            // Fill DrawModeDropdown
            DrawModeDropdown.ClearOptions();
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            foreach (DrawMode dm in Enum.GetValues(typeof(DrawMode))) options.Add(new Dropdown.OptionData(dm.ToString()));
            DrawModeDropdown.AddOptions(options);

            RefreshUI();
        }

        void Update()
        {
            // Keyboard overrides
            if (Input.GetKeyDown(KeyCode.Space)) TutorialUI.Click(MouseWorldPosition());

            // Regular mouse interactions
            if (isDraggingMode) Update_Dragging();
            else Update_Click();
        }

        void Update_Dragging()
        {
            if (Input.GetMouseButtonDown(0)) {
                isDragging = true;
                storedPs.Add(MouseWorldPosition());
            } else if (isDragging) {
                Vector3 p = MouseWorldPosition();
                if (Vector3.Distance(storedPs[storedPs.Count - 1], p) >= MinPointDistance) storedPs.Add(p);
            }

            if (Input.GetMouseButtonUp(0)) {
                isDragging = false;
                if (storedPs.Count > 1) storedPs.RemoveAt(storedPs.Count - 1);
                storedPs.Add(MouseWorldPosition());
                TutorialUI.DrawLineMode mode =
                    FingerToggle.isOn ? ArrowToggle.isOn ? TutorialUI.DrawLineMode.FingerAndArrow : TutorialUI.DrawLineMode.Finger
                    : ArrowToggle.isOn ? FingerToggle.isOn ? TutorialUI.DrawLineMode.FingerAndArrow : TutorialUI.DrawLineMode.Arrow
                    : TutorialUI.DrawLineMode.LineOnly;
                switch (drawMode) {
                case DrawMode.StraightLine:
                    TutorialUI.DrawLine(storedPs[0], storedPs[storedPs.Count - 1], mode, PersistentToggle.isOn, OverlayToggle.isOn)
                        .OnComplete(()=> Debug.Log("DrawLine Complete"));
                    break;
                case DrawMode.FullCurve:
                    TutorialUI.DrawLine(storedPs.ToArray(), mode, PersistentToggle.isOn, OverlayToggle.isOn)
                        .OnComplete(()=> Debug.Log("DrawLine Complete"));
                    break;
                }
                storedPs.Clear();
            }
        }

        void Update_Click()
        {
            if (!Input.GetMouseButtonUp(0)) return;

            switch (drawMode) {
            case DrawMode.Click:
                TutorialUI.Click(MouseWorldPosition());
                break;
            case DrawMode.ClickRepeat:
                TutorialUI.ClickRepeat(MouseWorldPosition());
                break;
            case DrawMode.RandomMark:
                TutorialUI.MarkSize markSize = NormalToggle.isOn ? TutorialUI.MarkSize.Normal : BigToggle.isOn ? TutorialUI.MarkSize.Big : TutorialUI.MarkSize.Huge;
                if (UnityEngine.Random.value < 0.5f) TutorialUI.MarkYes(MouseWorldPosition(), markSize);
                else TutorialUI.MarkNo(MouseWorldPosition(), markSize);
                break;
            }
        }

        #endregion

        #region Methods

        void RefreshUI()
        {
            bool dragOptionsActive = isDraggingMode;
            FingerToggle.gameObject.SetActive(dragOptionsActive);
            ArrowToggle.gameObject.SetActive(dragOptionsActive);
            PersistentToggle.gameObject.SetActive(dragOptionsActive);
            OverlayToggle.gameObject.SetActive(dragOptionsActive);
            NormalToggle.gameObject.SetActive(drawMode == DrawMode.RandomMark);
            BigToggle.gameObject.SetActive(drawMode == DrawMode.RandomMark);
            HugeToggle.gameObject.SetActive(drawMode == DrawMode.RandomMark);
        }

        #endregion

        #region Helpers

        Vector3 MouseWorldPosition()
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + CameraDistance));
        }

        #endregion

        #region Callbacks

        public void OnDrawModeChanged()
        {
            drawMode = (DrawMode)DrawModeDropdown.value;
            RefreshUI();
        }

        public void OnClear(bool _destroy)
        {
            TutorialUI.Clear(_destroy);
        }

        #endregion
    }
}