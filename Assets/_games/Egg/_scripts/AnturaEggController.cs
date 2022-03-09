using Antura.Dog;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.Egg
{
    public class AnturaEggController : MonoBehaviour
    {
        GameObject anturaPrefab;
        AnturaAnimationController anturaAnimation;

        public bool anturaTest;
        public bool noEntry;

        public GameObject aspirationParticle;

        public Transform enterPosition;
        public Transform spitPosition;
        public Transform exitPosition;

        Tween moveTween;

        Action moveEndCallback;

        List<bool> anturaInGame = new List<bool>();
        int currentStage;
        int numberOfStage;
        int numberOfattemptsforStage;

        public void Initialize(GameObject anturaPrefab)
        {
            anturaAnimation = GameObject.Instantiate(anturaPrefab).GetComponent<AnturaAnimationController>();
            anturaAnimation.transform.SetParent(transform);
            anturaAnimation.transform.position = exitPosition.position;
            anturaAnimation.transform.localEulerAngles = new Vector3(0f, 90f);
            anturaAnimation.transform.localScale = Vector3.one;

            ChengeGameObjectLayer(anturaAnimation.gameObject);

            ChengeGameObjectLayer(aspirationParticle);

            aspirationParticle.SetActive(false);

            anturaTest = false;
            noEntry = false;
        }

        public void Enter(Action callback = null)
        {
            aspirationParticle.SetActive(false);
            anturaAnimation.State = AnturaAnimationStates.walking;

            Sequence aspirationSequence = DOTween.Sequence();

            aspirationSequence.AppendInterval(1.5f);
            aspirationSequence.AppendCallback(delegate ()
            { aspirationParticle.SetActive(true); });
            aspirationSequence.Play();

            Move(enterPosition.position, 1f, delegate ()
            {
                EggConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Dog_Inhale);
                anturaAnimation.State = AnturaAnimationStates.sucking;

                if (callback != null)
                {
                    callback();
                }
            });
        }

        public void DoSpit()
        {
            EggConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Dog_Exhale);
            anturaAnimation.DoSpit(false);
        }

        public void Exit(Action callback = null)
        {
            anturaAnimation.State = AnturaAnimationStates.walking;
            Move(exitPosition.position, 1f, callback);
        }

        public void SetOnSpitPosition(Action callback = null)
        {
            aspirationParticle.SetActive(false);
            anturaAnimation.State = AnturaAnimationStates.idle;
            Move(spitPosition.position, 0.5f, callback);
        }

        void Move(Vector3 position, float duration, Action callback)
        {
            if (moveTween != null)
            {
                moveTween.Kill();
            }

            moveEndCallback = callback;

            moveTween = anturaAnimation.transform.DOMove(position, duration).OnComplete(delegate ()
            { if (moveEndCallback != null) moveEndCallback(); });
        }

        void ChengeGameObjectLayer(GameObject go)
        {
            go.layer = LayerMask.NameToLayer("Ball");

            int childCount = go.transform.childCount;

            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    ChengeGameObjectLayer(go.transform.GetChild(i).gameObject);
                }
            }
        }

        public void ResetAnturaIn(int numberOfStage, int numberOfattemptsforStage)
        {
            currentStage = 0;
            this.numberOfStage = numberOfStage;
            this.numberOfattemptsforStage = numberOfattemptsforStage;

            CreateAnturaInGameList();
        }

        void CreateAnturaInGameList()
        {
            anturaInGame.Clear();

            var indexOfStages = new List<int>();

            for (int i = 0; i < numberOfStage; i++)
            {
                indexOfStages.Add(i);
            }

            var aInStages = new List<int>();

            for (int i = 0; i < 2; i++)
            {
                int stageIndex = UnityEngine.Random.Range(0, indexOfStages.Count);

                aInStages.Add(indexOfStages[stageIndex]);

                indexOfStages.RemoveAt(stageIndex);
            }

            for (int i = 0; i < numberOfStage; i++)
            {
                if (EggConfiguration.Instance.Variation != EggVariation.BuildWord && aInStages.Contains(i))
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        anturaInGame.Add(false);
                        anturaInGame.Add(true);
                    }
                    else
                    {
                        anturaInGame.Add(true);
                        anturaInGame.Add(false);
                    }
                }
                else
                {
                    anturaInGame.Add(false);
                    anturaInGame.Add(false);
                }
            }
        }

        void RemoveFromAnturaInGameList()
        {
            if (anturaInGame.Count > 0)
            {
                anturaInGame.RemoveAt(0);
            }
            else
            {
                CreateAnturaInGameList();
            }
        }

        public void NextStage()
        {
            currentStage++;

            int numberOfAttempts = numberOfStage - currentStage;

            if (!(anturaInGame.Count == numberOfAttempts * numberOfattemptsforStage))
            {
                if (anturaInGame.Count % 2 == 0)
                {
                    RemoveFromAnturaInGameList();
                }

                RemoveFromAnturaInGameList();
            }
        }

        public bool IsAnturaIn()
        {
            bool isAnturaIn = anturaInGame[0];

            RemoveFromAnturaInGameList();

            return (isAnturaIn || anturaTest) && !noEntry;
        }
    }
}
