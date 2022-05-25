using Antura.Database;
using Antura.LivingLetters;
using Antura.Teacher;
using System;
using UnityEngine;

namespace Antura.Minigames
{
    /// <summary>
    /// Abstract Game Configuration class with default behaviours.
    /// </summary>
    public abstract class AbstractGameConfiguration : IGameConfiguration
    {
        // Properties
        public MiniGameData GameData { get; set; }
        public IGameContext Context { get; set; }
        public IQuestionProvider Questions { get; set; }

        public float Difficulty { get; set; }
        public bool TutorialEnabled { get; set; }
        public bool InsideJourney { get; set; }
        public bool IgnoreJourney { get; set; }

        public LocalizationDataId TitleLocalizationId
        {
            get
            {
                if (GameData == null || string.IsNullOrEmpty(GameData.TitleId))
                {
                    Debug.LogWarning("No TitleId found for game " + GameData?.GetId());
                    return LocalizationDataId.None;
                }
                return (LocalizationDataId)Enum.Parse(typeof(LocalizationDataId), GameData.TitleId);
            }
        }
        public LocalizationDataId IntroLocalizationId
        {
            get
            {
                if (GameData == null || string.IsNullOrEmpty(GameData.IntroId))
                {
                    Debug.LogWarning("No IntroID found for game " + GameData?.GetId());
                    return LocalizationDataId.None;
                }
                return (LocalizationDataId)Enum.Parse(typeof(LocalizationDataId), GameData.IntroId);
            }
        }
        public LocalizationDataId TutorialLocalizationId
        {
            get
            {
                if (GameData == null || string.IsNullOrEmpty(GameData.TutorialId))
                {
                    Debug.LogWarning("No TutorialId found for game " + GameData?.GetId());
                    return LocalizationDataId.None;
                }
                return (LocalizationDataId)Enum.Parse(typeof(LocalizationDataId), GameData.TutorialId);
            }
        }
        public virtual bool AutoPlayIntro => true;

        public abstract void SetMiniGameCode(MiniGameCode code);

        public abstract IQuestionBuilder SetupBuilder();

        protected QuestionBuilderParameters InitQuestionBuilderParamaters()
        {
            var builderParams = new QuestionBuilderParameters { insideJourney = InsideJourney };
            builderParams.useJourneyForCorrect = !IgnoreJourney;
            builderParams.useJourneyForWrong = !IgnoreJourney;

            return builderParams;
        }

        public abstract MiniGameLearnRules SetupLearnRules();


        public virtual bool IsDataMatching(ILivingLetterData data1, ILivingLetterData data2)
        {
            return DataMatchingHelper.IsDataMatching(data1, data2, LetterEqualityStrictness.Letter);
        }

        public virtual LetterDataSoundType GetVocabularySoundType()
        {
            return LetterDataSoundType.Phoneme;
        }
    }
}
