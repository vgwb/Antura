namespace Antura.Assessment
{
    /// <summary>
    /// Manager used to place answers: answers position depends on type of assessment
    /// so multiple placers are needed.
    /// </summary>
    public interface IAnswerPlacer
    {
        /// <summary>
        /// Take a array of Answers and move theiry GameObjects to proper positions. This
        /// start a animation (poof FX) and we should wait the end of the animation.
        /// </summary>
        void Place(Answer[] answer);

        /// <summary>
        /// Remove all previously placed answers. This start a animation (poof FX)
        /// and we should wait the end of the animation.
        /// </summary>
        void RemoveAnswers();

        /// <summary>
        /// Once we call "Place/RemoveAnswers" an animation start, as long as the animation is
        /// playing (this method returns true) we have to wait before doing something else.
        /// </summary>
        bool IsAnimating();
    }
}
