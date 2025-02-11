using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class ClassroomProfilesPanel : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] ClassroomProfileView profileViewPrefab;

        #endregion

        readonly List<ClassroomProfileView> profileViews = new();

        #region Unity

        void Awake()
        {
            profileViewPrefab.gameObject.SetActive(false);
        }

        #endregion

        #region Public Methods

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
                profileViews[i].Fill(profiles[i]);
                profileViews[i].gameObject.SetActive(true);
            }
        }

        #endregion

        #region Methods

        void Clear()
        {
            foreach (ClassroomProfileView profileView in profileViews) profileView.gameObject.SetActive(false);
        }

        #endregion
    }
}