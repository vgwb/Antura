using System.Collections.Generic;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Assessment
{
    /// <summary>
    /// This class is used to instantiate Assessment's specific items
    /// on the map. Those can be both instances already present on the
    /// map or injected prefabs.
    /// </summary>
    public class ItemFactory : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject StillLetterBox = null;
        public GameObject QuestionBoxPrefab = null;

        [Header("Folders")]
        public GameObject Questions = null;
        public GameObject Answers = null;
        public GameObject Placeholders = null;
        public GameObject QuestionBoxes = null;

        [Header("Positions")]
        public Transform AnturaStart = null;
        public Transform AnturaMiddle = null;

        private AnturaView antura = null;

        private int counter = 0;

        public AnturaView GetAntura()
        {
            return antura;
        }

        public StillLetterBox SpawnQuestion(ILivingLetterData data)
        {
            // Organize LLs in inspector's hierarchy view
            var letter = SpawnStillLetter(Questions);
            letter.Init(data, false);

            return letter;
        }

        public Answer SpawnAnswer(ILivingLetterData data, bool correct, AssessmentAudioManager dialogues)
        {
            if (AssessmentOptions.Instance.ShowAnswersAsImages)
            {
                data = new LL_ImageData(data.Id);
            }

            // Organize LLs in inspector's hierarchy view
            var letter = SpawnStillLetter(Answers);

            // Link LL to answer
            var answ = letter.gameObject.AddComponent<Answer>();
            letter.Init(data, true);
            answ.Init(correct, dialogues, data);
            return answ;
        }

        public StillLetterBox SpawnPlaceholder(LivingLetterDataType type)
        {
            // Organize LLs in inspector's hierarchy view
            var letter = SpawnStillLetter(Placeholders);
            letter.InitAsSlot(type);
            letter.gameObject.AddComponent<PlaceholderBehaviour>();
            return letter;
        }

        private StillLetterBox SpawnStillLetter(GameObject parent_Folder)
        {
            counter++;
            var letter = (Instantiate(StillLetterBox) as GameObject).GetComponent<StillLetterBox>();

            letter.gameObject.name = "instance_" + counter;
            letter.InstaShrink();
            letter.transform.SetParent(parent_Folder.transform);

            return letter;
        }

        public QuestionBox SpawnQuestionBox(IEnumerable<StillLetterBox> letterBoxes)
        {
            counter++;
            var qbox = (Instantiate(QuestionBoxPrefab) as GameObject).GetComponent<QuestionBox>();

            qbox.gameObject.name = "instance_" + counter;
            qbox.HideInstant();
            qbox.transform.SetParent(QuestionBoxes.transform);
            qbox.WrapBoxAroundWords(letterBoxes);
            return qbox;
        }

        void Awake()
        {
            instance = this;
            antura = FindObjectOfType<AnturaView>();
        }

        static ItemFactory instance;
        public static ItemFactory Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
