using System.Collections.Generic;

namespace Antura.Teacher
{
    /// <summary>
    /// Defines rules on how question packs can be generated for a specific mini game.
    /// </summary>
    public interface IQuestionBuilder
    {
        List<QuestionPackData> CreateAllQuestionPacks();
        QuestionBuilderParameters Parameters { get; }
    }
}
