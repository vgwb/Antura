using Antura.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Assessment
{
    internal class DefaultDragManager : IDragManager
    {
        private AssessmentAudioManager audioManager;
        private AnswerChecker checker;

        public DefaultDragManager(AssessmentAudioManager audioManager, AnswerChecker checker)
        {
            this.audioManager = audioManager;
            this.checker = checker;
            ResetRound();
        }

        private bool dragOnly = false;
        public void EnableDragOnly()
        {
            dragOnly = true;
            foreach (var a in answers)
            {
                a.Enable();
            }
        }

        List<PlaceholderBehaviour> placeholders = null;
        List<DroppableBehaviour> answers = null;
        List<IQuestion> questions = null;

        // This should be called onlye once
        public void AddElements(
                                    List<PlaceholderBehaviour> placeholders,
                                    List<Answer> answers,
                                    List<IQuestion> questions)
        {
            this.placeholders = placeholders;
            this.answers = BehaviourFromAnswers(answers);
            this.questions = questions;
        }

        private List<DroppableBehaviour> BehaviourFromAnswers(List<Answer> answers)
        {
            var list = new List<DroppableBehaviour>();

            foreach (var a in answers)
            {
                var droppable = a.gameObject.AddComponent<DroppableBehaviour>();
                droppable.SetDragManager(this);
                list.Add(droppable);
            }

            return list;
        }

        public bool AllAnswered()
        {
            if (!checker.IsAnimating() && checker.AreAllAnswered(placeholders))
            {
                checker.Check(placeholders, questions, this);
            }

            return checker.AllCorrect();
        }

        public void Enable()
        {
            dragOnly = false;
        }

        public void ResetRound()
        {
            placeholders = null;
            answers = null;
        }

        IDroppable droppable = null;

        // ALL NEEDED EVENTS ARE HERE
        public void StartDragging(IDroppable droppable)
        {
            if (this.droppable != null)
            {
                return;
            }

            audioManager.PlayUIPopup();

            this.droppable = droppable;
            PutDroppableOnTopOfList(droppable);
            droppable.StartDrag(x => RemoveFromUpdateAndPlaceholders(x));
        }

        private void PutDroppableOnTopOfList(IDroppable droppable)
        {
            DroppableBehaviour dropped = (DroppableBehaviour)droppable;
            answers.Remove(dropped);
            answers.Insert(0, dropped);

            float ZMin = 2;
            float ZMax = 4.9f;
            int count = answers.Count;
            int i = 0;

            foreach (var answer in answers)
            {
                answer.SetZ(ZMin + (i++) * (ZMax - ZMin) / count);
            }
        }

        void RemoveFromUpdateAndPlaceholders(IDroppable droppa)
        {
            RemoveFromUpdate();
            if (placeholders.Remove(droppa.GetLinkedPlaceholder()) == false)
            {
                throw new InvalidOperationException("Cannote remove the droppable");
            }
        }

        void RemoveFromUpdate()
        {
            droppable.StopDrag();
            droppable = null;
        }

        public void StopDragging(IDroppable droppable)
        {
            foreach (var p in placeholders)
            {
                p.gameObject.GetComponent<StillLetterBox>().FarSlot();
            }
            if (this.droppable == droppable && droppable != null)
            {
                audioManager.PlayUIPopup();

                if (dragOnly == false)
                {
                    CheckCollidedWithPlaceholder(droppable);
                }
                RemoveFromUpdate();
            }
        }

        private void CheckCollidedWithPlaceholder(IDroppable droppable)
        {
            foreach (var p in placeholders)
                if (NearEnoughToDrop(p.transform))
                {
                    droppable.Detach(false);
                    droppable.LinkToPlaceholder(p);
                    var set = p.Placeholder.GetQuestion().GetAnswerSet();
                    set.OnDroppedAnswer(droppable.GetAnswer());
                    return;
                }

            // In case we just moved out a LL
            droppable.Detach(false);
        }

        bool NearEnoughToDrop(Transform zone)
        {
            if (droppable == null)
            {
                return false;
            }

            var p1 = zone.transform.position;
            var p2 = droppable.GetTransform().localPosition;
            p1.z = p2.z = 0;
            return p1.DistanceIsLessThan(p2, 2f);
        }

        public void Update(float deltaTime)
        {
            if (droppable != null)
            {
                var currentDroppable = (DroppableBehaviour)droppable;

                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = currentDroppable.GetZ();
                droppable.GetTransform().localPosition = pos;

                foreach (var p in placeholders)
                    if (NearEnoughToDrop(p.transform))
                    {
                        p.gameObject.GetComponent<StillLetterBox>().NearbySlot();
                    }
                    else
                    {
                        p.gameObject.GetComponent<StillLetterBox>().FarSlot();
                    }
            }
        }

        public void DisableInput()
        {
            foreach (var a in answers)
            {
                a.Disable();
            }
        }

        public void EnableInput()
        {
            foreach (var a in answers)
            {
                a.Enable();
            }
        }

        public void RemoveDraggables()
        {
            dragOnly = true;
            if (droppable != null)
            {
                droppable.StopDrag();
                droppable = null;
            }
        }

        public void OnAnswerAdded()
        {

        }
    }
}
