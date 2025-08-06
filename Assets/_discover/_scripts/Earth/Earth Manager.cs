using Antura.Core;
using DG.DeExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{

    public class EarthManager : MonoBehaviour
    {
        public static EarthManager I;
        public List<CountrySpriteMapping> countrySprites;

        void Awake()
        {
            if (I == null)
            {
                I = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            // Debug.Log("Current contet: " + AppManager.I.ContentEdition.ContentID);
            if (AppManager.I.ContentEdition.ContentID == LearningContentID.Discover_Poland)
            {
                SelectCountry(Countries.Poland);
            }
            else
            {
                SelectCountry(Countries.France);
            }
        }

        public void SelectCountry(Countries selectedCountry)
        {
            foreach (var mapping in countrySprites)
            {

                if (mapping.country == selectedCountry)
                {
                    mapping.spriteRenderer.SetAlpha(1f);
                }
                else
                {
                    mapping.spriteRenderer.SetAlpha(0.3f);
                }
            }

            EarthUIManager.I.ShowCountry(selectedCountry);
        }

        public void SelectQuest(QuestData questData)
        {
            EarthUIManager.I.SelectQuest(questData);
        }

        public void OpenQuest(QuestData questData)
        {
            //Debug.Log("Load scene " + questData.scene);
            AppManager.I.NavigationManager.GoToDiscoverQuest(questData.scene);
        }
    }
}
