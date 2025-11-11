using Antura.Discover.Audio;
using Antura.Language;
using Antura.UI;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;

namespace Antura.Discover
{
    public class TaskDisplay : MonoBehaviour
    {
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] TaskCounter counter;
        [DeEmptyAlert]
        [SerializeField] TextRender tfDescription;

        QuestNode currNode;
        bool usedLearningLanguage = true;

        #endregion

        public bool IsOpen { get; private set; }

        string taskFullDescription; // Not truncated after the first \r
        bool initialized;
        Tween showTween;

        #region Unity + INIT

        void Init()
        {
            if (initialized)
                return;

            initialized = true;

            bool wasActive = this.gameObject.activeInHierarchy;
            this.gameObject.SetActive(true);

            showTween = ((RectTransform)this.transform).DOAnchorPosY(180, 0.35f).From().SetAutoKill(false).Pause()
                .SetEase(Ease.OutQuart)
                .OnRewind(() => this.gameObject.SetActive(false));

            counter.gameObject.SetActive(true);
            counter.Hide();

            if (!wasActive)
                this.gameObject.SetActive(false);
        }

        void Awake()
        {
            Init();
            if (!IsOpen)
                this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Pass 0-or-less to targetItems to disable the counter
        /// </summary>
        public void Show(QuestNode infoNode, int targetItemsToCollect)
        {
            if (IsOpen)
                return;

            Init();
            IsOpen = true;
            usedLearningLanguage = true;
            currNode = infoNode;
            //            Debug.Log($"Show task (to collect: {targetItemsToCollect})");
            DisplayText(true);
            counter.Setup(0, targetItemsToCollect);
            showTween.PlayForward();
            gameObject.SetActive(true);
        }


        public void Hide()
        {
            if (!IsOpen)
                return;

            Init();
            IsOpen = false;

            showTween.PlayBackwards();
        }

        public void SetTargetItems(int value)
        {
            counter.SetTargetItemsTo(value);
        }

        public void SetTotItemsCollected(int value)
        {
            counter.SetTotItemsCollectedTo(value);
        }

        public void OnClick()
        {
            usedLearningLanguage = !usedLearningLanguage;
            DisplayText(usedLearningLanguage, true);
        }

        public void DisplayText(bool UseLearningLanguage, bool speak = false)
        {
            // Debug.Log("Displaying dialogue in " + UseLearningLanguage + " : " + currNode.Content + " / " + currNode.ContentNative);
            if (UseLearningLanguage)
            {
                tfDescription.text = TruncateDescription(currNode.Content);
                if (speak && currNode.AudioLearning != null)
                {
                    DiscoverAudioManager.I.PlayDialogue(currNode.AudioLearning);
                }
            }
            else
            {
                tfDescription.text = TruncateDescription(currNode.ContentNative);
                if (speak && currNode.AudioNative != null)
                {
                    DiscoverAudioManager.I.PlayDialogue(currNode.AudioNative);
                }
            }
        }

        #endregion

        #region Methods

        string TruncateDescription(string text)
        {
            string[] txts = text.Split('\n');
            return txts[0].Trim();
        }

        #endregion
    }
}
