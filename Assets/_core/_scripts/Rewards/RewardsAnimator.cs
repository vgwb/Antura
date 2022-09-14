using Antura.Audio;
using Antura.Minigames;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Antura.Core;
using Antura.Helpers;
using Antura.UI;
using UnityEngine;

namespace Antura.Rewards
{
    [RequireComponent(typeof(RewardsScene))]
    /// <summary>
    /// Animates the appearance of the reward scene
    /// </summary>
    public class RewardsAnimator : MonoBehaviour
    {
        [Header("Settings")]
        public float Pedestal360Duration = 15f;
        public float Godrays360Duration = 15f;

        [Header("References")]
        public Transform Pedestal;

        public RectTransform Bottom;
        public RectTransform Godray0, Godray1;
        public RectTransform LockClosed, LockOpen;
        public ParticleSystem PoofParticle;
        public RectTransform AnturaButton;

        public bool IsComplete { get; private set; }

        private Sequence showTween, showTween2, godraysTween;
        private Tween pedestalTween;

        private RewardPack rewardPack = null;
        private GameObject newRewardInstantiatedGO;
        private RewardsScene rewardsSceneController;
        private float rotationAngleView = 0;

        private IAudioSource alarmClockSound;

        IEnumerator Start()
        {
            rewardsSceneController = GetComponent<RewardsScene>();
            rewardsSceneController.ClearLoadedRewardsOnAntura();

            LockClosed.gameObject.SetActive(false);
            LockOpen.gameObject.SetActive(false);
            Pedestal.gameObject.SetActive(true);
            Pedestal.transform.localScale = Vector3.one * 0.00001f;

            showTween = DOTween.Sequence().Pause()
                .Append(LockClosed.DOScale(0.0001f, 0.45f).From().SetEase(Ease.OutBack))
                .AppendInterval(0.3f)
                .AppendCallback(() => { alarmClockSound = AudioManager.I.PlaySound(Sfx.AlarmClock); })
                .Append(LockClosed.DOShakePosition(0.8f, 40f, 16, 90, false, false))
                .Join(LockClosed.DOShakeRotation(0.8f, new Vector3(0, 0, 70f), 16, 90, false))
                .Join(LockClosed.DOPunchScale(Vector3.one * 0.8f, 0.4f, 20))
                .AppendCallback(() =>
                {
                    LockClosed.gameObject.SetActive(false);
                    LockOpen.gameObject.SetActive(true);
                    if (alarmClockSound != null)
                    {
                        alarmClockSound.Stop();
                    }

                    AudioManager.I.PlaySound(Sfx.UIPopup);

                })
                .Join(LockClosed.DOScale(0.00001f, 0.4f))
                .Join(LockOpen.DOScale(0.00001f, 0.4f).From().SetEase(Ease.OutBack))
                .Join(Godray1.DOScale(0.00001f, 0.3f).From())
                .AppendInterval(0.7f)
                .Append(LockOpen.DOScale(0.00001f, 0.6f).SetEase(Ease.InBack))
                .AppendCallback(() => { AudioManager.I.PlaySound(Sfx.Win); })
                .Join(LockOpen.DORotate(new Vector3(0, 0, 360), 0.6f, RotateMode.FastBeyond360).SetRelative()
                    .SetEase(Ease.InCubic))
                .Join(Godray1.DOScale(0.00001f, 0.6f).SetEase(Ease.InCubic))
                .AppendCallback(() => SpawnRewardAndPoof());


            //showTween.Insert(showTween.Duration(false) - 1, AnturaButton.DOAnchorPosY(-200, 0.4f).From(true).SetEase(Ease.OutBack));

            godraysTween = DOTween.Sequence().SetLoops(-1, LoopType.Restart)
                .Append(Godray0.DORotate(new Vector3(0, 0, 360), Godrays360Duration, RotateMode.FastBeyond360).SetRelative()
                    .SetEase(Ease.Linear))
                .Join(Godray1.DORotate(new Vector3(0, 0, -360), Godrays360Duration, RotateMode.FastBeyond360).SetRelative()
                    .SetEase(Ease.Linear));

            pedestalTween = Pedestal.DORotate(new Vector3(0, 360, 0), Pedestal360Duration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear).SetLoops(-1).Pause();

            // Wait a couple frames to allow Unity to load correctly
            yield return new WaitForSeconds(0.3f);

            LockClosed.gameObject.SetActive(true);
            showTween.Play();
        }

        void InstantiateReward()
        {
            newRewardInstantiatedGO = rewardsSceneController.InstantiateReward(rewardPack);
        }

        void SpawnRewardAndPoof()
        {
            // Reward
            rewardPack = rewardsSceneController.GetRewardPackToInstantiate();
            if (rewardPack == null)
            {
                StartCoroutine(BonesRewardCO());
                return;
            }

            rotationAngleView = AppManager.I.RewardSystemManager.GetAnturaRotationAngleViewForRewardCategory(rewardPack.Category);
            newRewardInstantiatedGO = rewardsSceneController.InstantiateReward(rewardPack);
            if (newRewardInstantiatedGO != null)
                newRewardInstantiatedGO.transform.localScale = Vector3.one * 0.001f;

            Pedestal.gameObject.SetActive(true);
            Pedestal.transform.localScale = Vector3.one;

            showTween2 = DOTween.Sequence().Pause()
                .Join(Pedestal.DOScale(0.00001f, 1f).From().SetDelay(0.5f).SetEase(Ease.OutBack))
                .AppendInterval(0.1f)
                .Append(Pedestal.DORotate(new Vector3(0, rotationAngleView, 0), 0.3f, RotateMode.LocalAxisAdd).SetEase(Ease.InExpo))
                .OnComplete(() => { IsComplete = true; })
                .AppendCallback(() =>
                {
                    if (PoofParticle == null || newRewardInstantiatedGO == null)
                    { return; }
                    PoofParticle.transform.position = newRewardInstantiatedGO.transform.position;
                    PoofParticle.Play();
                    AudioManager.I.PlaySound(Sfx.Poof);
                });
            if (newRewardInstantiatedGO != null)
                showTween2.Append(newRewardInstantiatedGO.transform.DOScale(1f, 0.8f).SetEase(Ease.OutElastic));
            showTween2
                .AppendInterval(0.3f)
                .AppendCallback(() => { pedestalTween.Play(); });
            showTween2.Play();
        }

        #region Bones Reward
        public BonesCounter bonesCounter;
        public GroupSpawner biscuitsSpawner;
        public DailyRewardPopupPool popupPool;
        public RectTransform toPopupPivot;
        public RectTransform fromPopupPivot;

        private IEnumerator BonesRewardCO()
        {
            int nNewBones = 30;

            bonesCounter.Show();
            biscuitsSpawner.Spawn(nNewBones);
            yield return new WaitForSeconds(3f);

            // Add the new reward (for now, just bones)
            List<DailyRewardPopup> popups = popupPool.Spawn(nNewBones);
            Vector2 toP = UIHelper.SwitchToRectTransform(toPopupPivot, popupPool.GetComponent<RectTransform>());
            for (int iBone = 0; iBone < nNewBones; iBone++)
            {
                bonesCounter.IncreaseByOne();
                popups[iBone].PopFromTo(fromPopupPivot.anchoredPosition, toP);
                yield return new WaitForSeconds(0.1f);
            }
            AppManager.I.Player.AddBones(nNewBones);

        }

        #endregion

        void OnDestroy()
        {
            showTween.Kill();
            showTween2.Kill();
            godraysTween.Kill();
            pedestalTween.Kill();
        }

        /*
        #region events

        private void OnEnable()
        {
            RewardSystemManager.OnNewRewardUnlocked += RewardSystemManager_OnRewardChanged;
        }

        private void RewardSystemManager_OnRewardChanged(RewardPack rewardPack)
        {
            this.rewardPack = rewardPack;
            if (rewardPack.BaseType == RewardBaseType.Prop) {
                rotationAngleView = AppManager.I.RewardSystemManager.GetAnturaRotationAngleViewForRewardCategory(rewardPack.Category);
            }
        }

        private void OnDisable()
        {
            RewardSystemManager.OnNewRewardUnlocked += RewardSystemManager_OnRewardChanged;
        }

        #endregion
        */
    }
}
