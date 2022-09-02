using Antura.Core;
using Antura.Helpers;
using System;
using SQLite;
using UnityEngine;

namespace Antura.Database
{
    /// <summary>
    /// Data defining a Minigame.
    /// One entry should exist for each minigame (or minigame variation).
    /// A Play Session contains one or more minigames that can be selected to play when reaching that play session.
    /// <seealso cref="PlaySessionData"/>
    /// </summary>
    [Serializable]
    public class MiniGameData : IData
    {  [PrimaryKey]
        public string CodeName
        {
            get { return _CodeName; }
            set { _CodeName = value; }
        }
        [SerializeField]
        private string _CodeName;


        [PrimaryKey]
        public MiniGameCode Code
        {
            get { return _Code; }
            set { _Code = value; }
        }
        [SerializeField]
        private MiniGameCode _Code;

        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        [SerializeField]
        private bool _Active;

        public bool CanBeSelected => AppManager.I.JourneyHelper.CanSelectMiniGame(Code);

        /// <summary>
        /// a Minigame can be a normal game or an assessment
        /// </summary>
        /// <value>The type.</value>
        public MiniGameDataType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        [SerializeField]
        private MiniGameDataType _Type;

        /// <summary>
        /// the main is the game name
        /// </summary>
        /// <value>The main.</value>
        public string Main
        {
            get { return _Main; }
            set { _Main = value; }
        }
        [SerializeField]
        private string _Main;

        public string Variation
        {
            get { return _Variation; }
            set { _Variation = value; }
        }
        [SerializeField]
        private string _Variation;

        public string Badge
        {
            get { return _Badge; }
            set { _Badge = value; }
        }
        [SerializeField]
        private string _Badge;

        public string BadgeLocId
        {
            get { return _BadgeLocId; }
            set { _BadgeLocId = value; }
        }
        [SerializeField]
        private string _BadgeLocId;

        public string Scene
        {
            get { return _Scene; }
            set { _Scene = value; }
        }
        [SerializeField]
        private string _Scene;

        public string TitleId
        {
            get { return _TitleId; }
            set { _TitleId = value; }
        }
        [SerializeField]
        private string _TitleId;

        public string VariationId
        {
            get { return _VariationId; }
            set { _VariationId = value; }
        }
        [SerializeField]
        private string _VariationId;

        public string IntroId
        {
            get { return _IntroId; }
            set { _IntroId = value; }
        }
        [SerializeField]
        private string _IntroId;

        public string TutorialId
        {
            get { return _TutorialId; }
            set { _TutorialId = value; }
        }
        [SerializeField]
        private string _TutorialId;

        [Ignore]
        public WeightedPlaySkill[] AffectedPlaySkills
        {
            get { return _AffectedPlaySkills; }
            set { _AffectedPlaySkills = value; }
        }

        [SerializeField]
        private WeightedPlaySkill[] _AffectedPlaySkills;
        public string AffectedPlaySkills_list
        {
            get { return _AffectedPlaySkills.ToJoinedString(); }
            set { }
        }

        public string GetId()
        {
            return Code.ToString();
        }

        public override string ToString()
        {
            return string.Format("[Minigame: id={0}, type={4}, available={1},  title_en={2}, title_ar={3}]", GetId(), Active, Title.NativeText, Title.LearningText, Type.ToString());
        }

        public string GetFullTitle(bool learning = true)
        {
            string fullTitle = "";
            if (learning)
            {
                fullTitle = Title.LearningText;
                if (VariationId != "")
                {
                    var varTitle = LocalizationManager.GetLocalizationData(VariationId).LearningText;
                    if (varTitle != "")
                    {
                        fullTitle += " - " + varTitle;
                    }
                }
            }
            else
            {
                fullTitle = Title.NativeText;
                if (VariationId != "")
                {
                    fullTitle += ": " + LocalizationManager.GetLocalizationData(VariationId).NativeText;
                }
            }
            return fullTitle;
        }

        public LocalizationData Title
        {
            get { return LocalizationManager.GetLocalizationData(TitleId); }
        }

        public LocalizationData VariationTitle
        {
            get { return LocalizationManager.GetLocalizationData(VariationId); }
        }

        public LocalizationData Intro
        {
            get { return LocalizationManager.GetLocalizationData(IntroId); }
        }

        public LocalizationData Tutorial
        {
            get { return LocalizationManager.GetLocalizationData(_TutorialId); }
        }

        public bool HasBadge => !string.IsNullOrEmpty(Badge) || !string.IsNullOrEmpty(BadgeLocId);


        public string GetTitleSoundFilename()
        {
            return Title.AudioKey;
        }

    }

    [Serializable]
    public struct WeightedPlaySkill
    {
        public PlaySkill Skill;
        public float Weight;

        public WeightedPlaySkill(PlaySkill skill, float weight)
        {
            Skill = skill;
            Weight = weight;
        }

        override public string ToString()
        {
            return Skill.ToString() + ":" + Weight;
        }
    }
}
