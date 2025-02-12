using System.Collections.Generic;
using Demigiant.DemiTools;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
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

        #endregion

        readonly List<ClassroomProfileView> profileViews = new();

        #region Unity

        void Awake()
        {
            profileViewPrefab.gameObject.SetActive(false);
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

        #endregion
    }
}