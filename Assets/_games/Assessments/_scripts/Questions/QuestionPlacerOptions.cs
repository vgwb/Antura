using UnityEngine;

namespace Antura.Assessment
{
    public class QuestionPlacerOptions : MonoBehaviour
    {
        [HideInInspector]
        public bool SpawnImageWithQuestion = false;
        public float QuestionWideness { set; private get; }
        public float AnswerWideness { set; private get; }

        public float QuestionSize { private set { } get { return QuestionWideness * 3; } }
        public float AnswerSize { private set { } get { return AnswerWideness * 3; } }
        public float SlotSize { private set { } get { return AnswerWideness * 3 + 0.03f; } }
        public readonly float ImageSize = 3;

        // Screen Limit extents
        public float TopY { private set { } get { return mainCamera.orthographicSize; } }
        public float BottomY { private set { } get { return -mainCamera.orthographicSize; } }
        public float RightX { private set { } get { return TopY * mainCamera.aspect; } }
        public float LeftX { private set { } get { return BottomY * mainCamera.aspect; } }
        public float QuestionY { private set { } get { return TopY - 5.0f; } }
        public float DefaultZ { private set { } get { return 5; } }

        //################################################
        //                  INSTANCE
        //################################################
        private Camera mainCamera;
        static QuestionPlacerOptions instance;
        public static QuestionPlacerOptions Instance
        {
            get
            {
                return instance;
            }
        }

        void Awake()
        {
            instance = this;
            mainCamera = Camera.main;
            if (mainCamera.orthographic == false)
            {
                Debug.LogWarning("main Camera is not the orthographic one");
            }
        }
    }
}
