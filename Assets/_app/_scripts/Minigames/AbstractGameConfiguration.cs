using Antura.Database;
using Antura.LivingLetters;
using Antura.Teacher;

namespace Antura.Minigames
{
    /// <summary>
    /// Abstract Game Configuration class with default behaviours.
    /// </summary>
    public abstract class AbstractGameConfiguration : IGameConfiguration
    {
        // Properties
        public IGameContext Context { get; set; }
        public IQuestionProvider Questions { get; set; }

        public float Difficulty { get; set; }
        public bool TutorialEnabled { get; set; }

        public virtual LocalizationDataId TitleLocalizationId
        {
            get {
                return LocalizationDataId.None;
            }
        }

        public abstract void SetMiniGameCode(MiniGameCode code);

        public abstract IQuestionBuilder SetupBuilder();
        public abstract MiniGameLearnRules SetupLearnRules();

        public virtual bool IsDataMatching(ILivingLetterData data1, ILivingLetterData data2)
        {
            return DataMatchingHelper.IsDataMatching(data1, data2, LetterEqualityStrictness.LetterOnly);
        }

        public virtual LetterDataSoundType GetVocabularySoundType()
        {
            return LetterDataSoundType.Phoneme;
        }
    }
}