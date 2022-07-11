using UnityEngine;
using System.Collections.Generic;
using Antura.Audio;
using Antura.LivingLetters;
using DG.Tweening;
using Antura.Language;

namespace Antura.Minigames.FastCrowd
{

    public class DropContainer : MonoBehaviour
    {
        List<DropSingleArea> areas = new List<DropSingleArea>();

        int actualAreaIndex = 0;

        enum DropAreaPositions
        {
            ActivePos,
            NextPos,
            NextsPos,
            CompletedPos

        }

        public void AddArea(DropSingleArea area)
        {
            areas.Add(area);
            dropAreaSetPosition();
        }

        public void Clean()
        {
            actualAreaIndex = 0;

            foreach (var item in areas)
            {
                GameObject.Destroy(item.gameObject);
            }
            areas.Clear();
        }

        /// <summary>
        /// Return actual active DropSingleArea.
        /// </summary>
        /// <returns></returns>
        public DropSingleArea GetActualDropArea()
        {
            if (actualAreaIndex >= areas.Count)
                return null;
            return areas[actualAreaIndex];
        }

        public void NextArea()
        {
            if (actualAreaIndex < areas.Count - 1)
            {
                actualAreaIndex++;
                DOTween.Sequence().InsertCallback(1, delegate ()
                    {
                        if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Word
                            || FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Image
                            || FastCrowdConfiguration.Instance.IsOrderingVariation)
                            AudioManager.I.PlaySound(Sfx.Hit);
                        dropAreaSetPosition();
                    });
            }
            else
            {
                actualAreaIndex++;
                DOTween.Sequence().InsertCallback(1, delegate ()
                    {
                        dropAreaSetPosition(delegate ()
                            {
                                float waitAtEnd = 2;
                                if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Word
                                    || FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Image
                                    || FastCrowdConfiguration.Instance.IsOrderingVariation)
                                {
                                    AudioManager.I.PlaySound(Sfx.Hit);
                                    waitAtEnd = 1;
                                }

                                DOTween.Sequence().InsertCallback(waitAtEnd, delegate ()
                                    {
                                        if (OnObjectiveBlockCompleted != null)
                                            OnObjectiveBlockCompleted();
                                    });

                            });
                    });

            }
        }

        /// <summary>
        /// Set area positions with animation.
        /// </summary>
        void dropAreaSetPosition(TweenCallback _callback = null)
        {
            for (int i = 0; i < areas.Count; i++)
            {
                if (actualAreaIndex == i)
                {
                    positionigAreaDropElement(areas[i], DropAreaPositions.ActivePos);
                    areas[i].SetEnabled();
                }
                else if (actualAreaIndex > i && i == areas.Count - 1)
                { // for final one
                    positionigAreaDropElement(areas[i], DropAreaPositions.CompletedPos, delegate ()
                        {
                            if (_callback != null)
                                _callback();
                        });
                    areas[i].SetDisbled();
                }
                else if (actualAreaIndex > i)
                {
                    positionigAreaDropElement(areas[i], DropAreaPositions.CompletedPos);
                    areas[i].SetDisbled();
                }
                else if (actualAreaIndex + 1 == i)
                {
                    positionigAreaDropElement(areas[i], DropAreaPositions.NextPos);
                    areas[i].SetDisbled();
                }
                else
                {
                    positionigAreaDropElement(areas[i], DropAreaPositions.NextsPos);
                    areas[i].SetDisbled();
                }
            }

        }

        #region events

        public delegate void ObjectiveEvent();

        /// <summary>
        /// Happens when a peace of objective completed. Ex: Word match completed (word is a block objective).
        /// </summary>
        public event ObjectiveEvent OnObjectiveBlockCompleted;

        #endregion

        #region results events delegates

        private void Droppable_OnWrongMatch(LivingLetterController _letterView)
        {
            AudioManager.I.PlaySound(Sfx.KO);
        }

        /// <summary>
        /// Risen on letter or world match.
        /// </summary>
        private void Droppable_OnRightMatch(LivingLetterController _letterView)
        {
            AudioManager.I.PlaySound(Sfx.OK);
            NextArea();
        }

        #endregion

        #region Tween Animations

        /// <summary>
        /// Perform movement of "wheel" of letters to find, and change the position to a "next one".
        /// </summary>
        /// <param name="_dropArea"></param>
        /// <param name="_position"></param>
        /// <param name="_callback"></param>
        void positionigAreaDropElement(DropSingleArea _dropArea, DropAreaPositions _position, TweenCallback _callback = null)
        {
            float durantion = 0.4f;
            Sequence _sequence = DOTween.Sequence();
            bool needFade = false;

            if (_position == DropAreaPositions.CompletedPos)
                needFade = true;

            // - Actual elimination
            _dropArea.transform.DOLocalRotate(getRotation(_position), durantion);
            _sequence.Append(_dropArea.transform.DOLocalMove(getPosition(_position), durantion)).OnComplete(delegate ()
                {
                    if (needFade)
                    {
                        _sequence.Append(_dropArea.GetComponent<MeshRenderer>().materials[0].DOFade(0, durantion));
                        _sequence.Append(_dropArea.NumberLabel.transform.DOLocalMove(new Vector3(getPosition(_position).x, -2, getPosition(_position).z), durantion));
                        // pro only
                        // sequence.Append(Aree[actualAreaIndex].LetterLable.DOFade(0, 0.4f));
                        //if (_dropArea.DrawSprite)
                        //    _dropArea.DrawSprite.DOFade(0, 0.4f);
                        if (_dropArea.NumberLabel)
                        {
                            _dropArea.NumberLabel.DOFade(0, 0.4f);
                        }
                        if (_dropArea.LetterText)
                        {
                            _dropArea.LetterText.HideDrawingLabel();
                            _dropArea.LetterText.TMPText.DOFade(0, 0.4f);
                        }
                    }
                    if (_callback != null)
                        _callback();
                });
        }

        /// <summary>
        /// Return right position.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        Vector3 getPosition(DropAreaPositions _position)
        {
            int arrivalDir = 1; // Always arrive from the left, regardless of RTL

            switch (_position)
            {
                case DropAreaPositions.ActivePos:
                    return new Vector3(0, 0.1f, 0);
                case DropAreaPositions.NextPos:
                    return new Vector3(-6 * arrivalDir, 0.1f, -1.5f);
                case DropAreaPositions.NextsPos:
                    return new Vector3(-12 * arrivalDir, 0.1f, -6);
                case DropAreaPositions.CompletedPos:
                    return new Vector3(6 * arrivalDir, 0.1f, 0);
                default:
                    Debug.LogError("Position not found");
                    break;
            }
            return new Vector3(0, -10, 0); // underground...
        }

        /// <summary>
        /// Return right position.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        Vector3 getRotation(DropAreaPositions _position)
        {
            int arrivalDir = 1; // Always arrive from the left, regardless of RTL

            switch (_position)
            {
                case DropAreaPositions.ActivePos:
                    return new Vector3(90, 0, 0);
                case DropAreaPositions.NextPos:
                    return new Vector3(90, -30 * arrivalDir, 0);
                case DropAreaPositions.NextsPos:
                    return new Vector3(90, -30 * arrivalDir, 30);
                case DropAreaPositions.CompletedPos:
                    return new Vector3(90, 0, 0);
                default:
                    Debug.LogError("Position not found");
                    break;
            }
            return new Vector3(0, -10, 0); // underground...
        }

        #endregion

    }
}
