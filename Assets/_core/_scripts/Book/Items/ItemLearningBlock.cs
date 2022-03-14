using Antura.Database;
using Antura.UI;
using Antura.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Antura.Book
{
    /// <summary>
    /// Displays an item in the LearningBlock page of the Player Book.
    /// </summary>
    public class ItemLearningBlock : MonoBehaviour, IPointerClickHandler
    {
        LearningBlockInfo learningBlockInfo;
        public TextRender Title;
        public TextRender Info;
        public TextRender SubTitle;
        public Image LockIcon;

        JourneyPanel manager;

        UIButton uIButton;

        public void Init(JourneyPanel _manager, LearningBlockInfo _info)
        {
            uIButton = GetComponent<UIButton>();

            learningBlockInfo = _info;
            manager = _manager;

            Info.text = learningBlockInfo.data.Id;
            Title.text = learningBlockInfo.data.Title_LearningLang;
            SubTitle.text = learningBlockInfo.data.Title_NativeLang;

            if (learningBlockInfo.unlocked || AppManager.I.Player.IsDemoUser)
            {
                LockIcon.enabled = false;
            }
            else
            {
                LockIcon.enabled = true;
            }

            //var score = learningBlockInfo.score;
            // @note: we should already save the score when a block is finished, and not compute it when showing it
            //var score = TeacherAI.I.GetLearningBlockScore(info.data);

            //Info.text = "Score: " + score;
            Highlight(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            manager.DetailLearningBlock(learningBlockInfo);
        }

        public void Select(string code)
        {
            Highlight(code == learningBlockInfo.data.Id);
        }

        void Highlight(bool _status)
        {
            uIButton.Toggle(_status);
        }
    }
}
