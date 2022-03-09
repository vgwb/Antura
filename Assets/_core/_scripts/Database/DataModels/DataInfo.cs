using System.Collections.Generic;

// TODO: refactor this to separate JourneyInfo from VocabularyInfo (different rules)
// TODO: create separate LetterModel, WordModel and PhraseModel with custom methods.. integratic relative Data and Scores inside
namespace Antura.Database
{
    #region Info Wrappers

    /// <summary>
    /// Pairs the data related to a specific type T with its score and unlock state.
    /// </summary>
    public class DataInfo<T> where T : IData
    {
        public T data = default(T);
        public float score = 0f;
        public bool unlocked = false;
    }

    public class LearningBlockInfo : DataInfo<LearningBlockData>
    {
        public List<PlaySessionInfo> playSessions = new List<PlaySessionInfo>();
    }

    public class PlaySessionInfo : DataInfo<PlaySessionData> { }
    public class MiniGameInfo : DataInfo<MiniGameData> { }

    public class WordInfo : DataInfo<WordData> { }
    public class LetterInfo : DataInfo<LetterData> { }
    public class PhraseInfo : DataInfo<PhraseData> { }

    #endregion
}
