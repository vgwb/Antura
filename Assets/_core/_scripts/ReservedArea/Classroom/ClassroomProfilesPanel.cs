using System.Collections;
using System.Collections.Generic;
using Antura.Profile;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    public class ClassroomProfilesPanel : MonoBehaviour
    {
        #region EVENTS

        public ActionEvent<UserProfile> OnProfileClicked = new("ClassroomProfilesPanel.OnProfileClicked");

        #endregion
        
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] ClassroomProfileView profileViewPrefab;
        [DeEmptyAlert]
        [SerializeField] ScrollRect scrollRect;
        [DeEmptyAlert]
        [SerializeField] GameObject content;

        #endregion

        RectTransform rt;
        RectTransform[] layoutGroupsRTs;
        readonly List<ClassroomProfileView> profileViews = new();
        Coroutine coRebuildLayout;

        #region Unity

        void Awake()
        {
            rt = this.GetComponent<RectTransform>();
            LayoutGroup[] layoutGroups = this.GetComponentsInChildren<LayoutGroup>(true);
            layoutGroupsRTs = new RectTransform[layoutGroups.Length];
            for (int i = 0; i < layoutGroups.Length; i++) layoutGroupsRTs[i] = layoutGroups[i].GetComponent<RectTransform>();
            
            profileViewPrefab.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
        }

        #endregion

        #region Public Methods
        
        public void Open(bool doOpen)
        {
            if (doOpen)
            {
                this.gameObject.SetActive(true);
                scrollRect.verticalNormalizedPosition = 1;
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        public void Fill(List<UserProfile> profiles)
        {
            Clear();
            
            int tot = profiles.Count;
            while (profileViews.Count < tot)
            {
                ClassroomProfileView view = Instantiate(profileViewPrefab, profileViewPrefab.transform.parent);
                profileViews.Add(view);
            }
            for (int i = 0; i < tot; i++)
            {
                ClassroomProfileView view = profileViews[i];
                view.Fill(profiles[i]);
                view.BtMain.onClick.AddListener(() => OnProfileClicked.Dispatch(view.Profile));
                view.gameObject.SetActive(true);
            }
            
            content.SetActive(false);
            this.RestartCoroutine(ref coRebuildLayout, CO_RebuildLayout());
        }

        #endregion

        #region Methods

        void Clear()
        {
            foreach (ClassroomProfileView profileView in profileViews)
            {
                profileView.gameObject.SetActive(false);
                profileView.BtMain.onClick.RemoveAllListeners();
            }
        }

        // Fix for longtime Unity bug: layout requires a frame to be forced to rebuild correctly
        IEnumerator CO_RebuildLayout()
        {
            yield return null;
            content.SetActive(true);
            foreach (RectTransform r in layoutGroupsRTs) LayoutRebuilder.ForceRebuildLayoutImmediate(r);
            coRebuildLayout = null;
        }

        #endregion
    }
}