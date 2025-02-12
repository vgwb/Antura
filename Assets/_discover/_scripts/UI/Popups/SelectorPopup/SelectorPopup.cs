using System;
using System.Collections.Generic;
using Antura.Minigames.DiscoverCountry.Popups;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Action = Antlr.Runtime.Misc.Action;

namespace Antura.Minigames.DiscoverCountry
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SelectorPopup : MonoBehaviour
    {
        #region EVENTS

        public ActionEvent OnClosing = new("SelectorPopup.OnClosing");

        #endregion
        
        #region Serialized

        [SerializeField] int shortBtWidth = 130;
        [SerializeField] int defaultBtWidth = 342;
        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] TMP_Text tfTitle;
        [DeEmptyAlert]
        [SerializeField] Button btPrefab;
        [DeEmptyAlert]
        [SerializeField] Button btClose;
        [DeEmptyAlert]
        [SerializeField] ScrollRect scrollRect;
        [DeEmptyAlert]
        [SerializeField] GridLayoutGroup gridLayoutGroup;

        #endregion

        Action<int> onSelect;
        CanvasGroup cg;
        readonly List<Button> bts = new();
        Tween openTween;

        #region Unity

        void Awake()
        {
            cg = this.GetComponent<CanvasGroup>();
            
            openTween = this.transform.DOScale(0, GlobalPopups.ShowTime).From().SetAutoKill(false).Pause().SetUpdate(true)
                .SetEase(Ease.OutBack)
                .OnComplete(() => cg.interactable = true)
                .OnRewind(() => this.gameObject.SetActive(false));
            
            btPrefab.gameObject.SetActive(false);
            cg.interactable = false;
            
            btClose.onClick.AddListener(Close);
        }

        void OnDestroy()
        {
            openTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Open(string title, List<string> values, Action<int> onSelected)
        {
            Clear();

            tfTitle.text = title;
            onSelect = onSelected;

            cg.interactable = false;
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
                int index = i;
                Vector2 size = gridLayoutGroup.cellSize;
                size.x = btWidth;
                gridLayoutGroup.cellSize = size;
                bt.GetComponentInChildren<TMP_Text>().text = values[i];
                bt.onClick.AddListener(() => OnClick(index));
                bt.gameObject.SetActive(true);
            }

            scrollRect.verticalNormalizedPosition = 1;
            openTween.Restart();
            this.gameObject.SetActive(true);
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
            openTween.Rewind();
        }

        void Close()
        {
            cg.interactable = false;
            openTween.PlayBackwards();
            OnClosing.Dispatch();
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