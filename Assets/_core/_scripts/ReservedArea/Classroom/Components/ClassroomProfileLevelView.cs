using System.Collections.Generic;
using Antura.Profile;
using DG.DeInspektor.Attributes;
using TMPro;
using UnityEngine;

namespace Antura.UI
{
    public class ClassroomProfileLevelView : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] TMP_Text tfTitle;
        [DeEmptyAlert]
        [SerializeField] ClassroomProfileLevelSectionView sectionViewPrefab;

        #endregion

        readonly List<ClassroomProfileLevelSectionView> sectionViews = new();

        #region Unity

        void Awake()
        {
            sectionViewPrefab.gameObject.SetActive(false);
        }

        #endregion

        #region Public Methods

        public void Fill(ClassroomProfileDetail.LanguageLevel level)
        {
            tfTitle.text = level.Name;
            
            int tot = level.Sections.Count;
            while (sectionViews.Count < tot)
            {
                ClassroomProfileLevelSectionView view = Instantiate(sectionViewPrefab, sectionViewPrefab.transform.parent);
                sectionViews.Add(view);
            }
            for (int i = 0; i < tot; i++)
            {
                ClassroomProfileLevelSectionView view = sectionViews[i];
                view.Fill(level.Sections[i]);
                view.gameObject.SetActive(true);
            }
        }

        #endregion

        #region Methods

        void Clear()
        {
            foreach (ClassroomProfileLevelSectionView view in sectionViews) view.gameObject.SetActive(false);
        }

        #endregion
    }
}