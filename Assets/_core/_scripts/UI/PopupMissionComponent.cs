using Antura.Audio;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Antura.UI
{
    // TODO refactor: not clear where this is used
    public class PopupMissionComponent : MonoBehaviour
    {
        public Image CompletedCheck;
        public TextMeshProUGUI TitleLable;
        public TextMeshProUGUI MainLable;
        public Image Draw;
        public Button ContinueButton;

        public class Data
        {
            public string Title;
            public string MainTextToDisplay;
            public Sprite DrawSprite;
            public PopupType Type;

            /// <summary>
            /// Auto close popup after seconds indicated. If -1 autoclose is disabled and appear close button for that.
            /// </summary>
            public float AutoCloseTime = -1;
        }

        public enum PopupType
        {
            New_Mission,
            Mission_Completed
        }

        private Vector2 HidePosition;
        private Vector2 ShowPosition;

        private Sequence sequence;
        private TweenParams tParms;
        private TweenCallback pendingCallback = null;

        //float timeScaleAtMenuOpen = 1;


        void Start()
        {
            HidePosition = new Vector2(0, -750);
            ShowPosition = new Vector2(0, 0);
            GetComponent<RectTransform>().anchoredPosition = HidePosition;
            //ContinueButton.gameObject.SetActive(AutoCloseTime < 0);
        }

        public void Show(Data _data, TweenCallback _callback = null)
        {
            AudioManager.I.PlaySound(Sfx.UIPopup);

            MainLable.text = _data.MainTextToDisplay;
            // Preset for animation
            CompletedCheck.rectTransform.DOScale(6, 0);
            CompletedCheck.DOFade(0, 0);
            // Animation sequence
            sequence = DOTween.Sequence().SetUpdate(true);
            tParms = new TweenParams()
                .SetEase(Ease.InOutBack);
            //timeScaleAtMenuOpen = Time.timeScale;
            Time.timeScale = 0; // not working
            sequence.Append(GetComponent<RectTransform>().DOAnchorPos(ShowPosition, 0.3f).SetAs(tParms));
            TitleLable.text = _data.Title;
            // Complete check animation.
            if (_data.Type == PopupType.Mission_Completed)
            {
                AudioManager.I.PlaySound(Sfx.StampOK);
                sequence.Insert(0.3f, CompletedCheck.DOFade(1, 0.1f));
                sequence.Append(CompletedCheck.rectTransform.DOScale(1, 0.3f).SetAs(tParms));
                //                    .OnComplete(delegate()
                //                    {
                //                        AudioManager.I.PlaySound(Sfx.Win);
                //                    });
            }
            // Draw
            if (_data.DrawSprite)
            {
                Draw.gameObject.SetActive(true);
                Draw.sprite = _data.DrawSprite;
            }
            else
            {
                Draw.gameObject.SetActive(false);
            }
            // Autoclose
            if (_data.AutoCloseTime >= 0)
            {
                sequence.InsertCallback(_data.AutoCloseTime, delegate
                { Close(sequence, tParms, _callback); });
            }
            else
            {
                pendingCallback = null; // reset
                if (_callback != null)
                {
                    pendingCallback = _callback;
                }
            }
        }

        public void Close()
        {
            Close(sequence, tParms, pendingCallback);
        }

        /// <summary>
        /// Close popup with actal sequence and callback.
        /// </summary>
        /// <param name="_sequence"></param>
        /// <param name="_tParms"></param>
        /// <param name="_callback"></param>
        void Close(Sequence _sequence, TweenParams _tParms, TweenCallback _callback)
        {
            Time.timeScale = 1;
            _sequence.Append(GetComponent<RectTransform>().DOAnchorPos(HidePosition, 0.15f).SetAs(_tParms));
            if (_callback != null)
            {
                _callback();
            }
        }

        void OnDestroy()
        {
            sequence.Kill();
        }
    }
}
