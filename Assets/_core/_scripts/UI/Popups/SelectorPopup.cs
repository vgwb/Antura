using System;
using System.Collections.Generic;
using Demigiant.DemiTools;
using DG.DeExtensions;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class SelectorPopup : AbstractGlobalPopup
    {
        #region Serialized

        [Header("Specific")]
        [SerializeField] int shortBtWidth = 130;
        [SerializeField] int defaultBtWidth = 342;
        [SerializeField] Color specialItemsColor = Color.red;
        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] Button btPrefab;
        [DeEmptyAlert]
        [SerializeField] ScrollRect scrollRect;
        [DeEmptyAlert]
        [SerializeField] GridLayoutGroup gridLayoutGroup;

        #endregion

        ColorBlock defItemsColorBlock;
        ColorBlock specialItemsColorBlock;
        Action<int> onSelect;
        readonly List<Button> bts = new();

        #region Unity

        protected override void Awake()
        {
            base.Awake();
            
            defItemsColorBlock = btPrefab.colors;
            specialItemsColorBlock = ColorBlock.defaultColorBlock;
            specialItemsColorBlock.normalColor = specialItemsColorBlock.selectedColor = specialItemsColorBlock.disabledColor = specialItemsColor;
            specialItemsColorBlock.highlightedColor = specialItemsColor.ChangeBrightness(1.2f);
            specialItemsColorBlock.pressedColor = specialItemsColor.ChangeBrightness(1.3f);
            
            btPrefab.gameObject.SetActive(false);
        }

        #endregion

        #region Public Methods

        public void Open(string title, List<string> values, Action<int> onSelected, bool hasCloseButton, params int[] specialItemsIndexes)
        {
            Clear();

            onSelect = onSelected;
            this.gameObject.SetActive(true);
            btClose.gameObject.SetActive(hasCloseButton);

            int btWidth = GetMaxValuesLen(values) < 3 ? shortBtWidth : defaultBtWidth;
            
            int tot = values.Count;
            while (bts.Count < tot)
            {
                Button bt = Instantiate(btPrefab, btPrefab.transform.parent);
                bts.Add(bt);
            }
            for (int i = 0; i < tot; i++)
            {
                Button bt = bts[i];
                bool isSpecial = Array.IndexOf(specialItemsIndexes, i) != -1;
                bt.colors = isSpecial ? specialItemsColorBlock : defItemsColorBlock;
                int index = i;
                Vector2 size = gridLayoutGroup.cellSize;
                size.x = btWidth;
                gridLayoutGroup.cellSize = size;
                bt.GetComponentInChildren<TMP_Text>().text = values[i];
                bt.onClick.AddListener(() => OnClick(index));
                bt.gameObject.SetActive(true);
            }

            scrollRect.verticalNormalizedPosition = 1;
            
            BaseOpen(title);
        }

        #endregion

        #region Methods

        void Clear()
        {
            onSelect = null;
            foreach (Button bt in bts)
            {
                bt.gameObject.SetActive(false);
                bt.onClick.RemoveAllListeners();
            }
            
            BaseClear();
        }

        int GetMaxValuesLen(List<string> values)
        {
            int max = 0;
            foreach (string s in values)
            {
                if (s.Length > max) max = s.Length;
            }
            return max;
        }

        #endregion

        #region Callbacks

        void OnClick(int index)
        {
            Close();
            if (onSelect != null) onSelect(index);
        }

        #endregion
    }
}