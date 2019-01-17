using Antura.Audio;
using Antura.Minigames;
using DG.Tweening;
using System.Collections;
using Antura.Core;
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

        private Sequence showTween, godraysTween;
        private Tween pedestalTween;

        private RewardPack rewardPack = null;
        private GameObject newRewardInstantiatedGO;
        private RewardsScene rewardsSceneController;
        private float rotationAngleView = 0;

        private IAudioSource alarmClockSound;

        IEnumerator Start()
        {
            LockClosed.gameObject.SetActive(false);
            LockOpen.gameObject.SetActive(false);
            Pedestal.gameObject.SetActive(true);

            // Reward 
            rewardsSceneController = GetComponent<RewardsScene>();
            rewardsSceneController.ClearLoadedRewardsOnAntura();
            rewardPack = rewardsSceneController.GetRewardPackToInstantiate();
            rotationAngleView = AppManager.I.RewardSystemManager.GetAnturaRotationAngleViewForRewardCategory(rewardPack.Category);
            newRewardInstantiatedGO = rewardsSceneController.InstantiateReward(rewardPack);

            if (newRewardInstantiatedGO) {
                newRewardInstantiatedGO.transform.DOScale(0.00001f, 0f).SetEase(Ease.OutBack);
                showTween = DOTween.Sequence().Pause()
                    .Append(LockClosed.DOScale(0.0001f, 0.45f).From().SetEase(Ease.OutBack))
                    .AppendInterval(0.3f)
                    .AppendCallback(() => { alarmClockSound = AudioManager.I.PlaySound(Sfx.AlarmClock); })
                    .Append(LockClosed.DOShakePosition(0.8f, 40f, 16, 90, false, false))
                    .Join(LockClosed.DOShakeRotation(0.8f, new Vector3(0, 0, 70f), 16, 90, false))
                    .Join(LockClosed.DOPunchScale(Vector3.one * 0.8f, 0.4f, 20))
                    .AppendCallback(() => {
                        LockClosed.gameObject.SetActive(false);
                        LockOpen.gameObject.SetActive(true);
                        if (alarmClockSound != null) {
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
                    .Join(LockOpen.DORotate(new Vector3(0, 0, 360), 0.6f, RotateMode.FastBeyond360).SetRelative().SetEase(Ease.InCubic))
                    .Join(Godray1.DOScale(0.00001f, 0.6f).SetEase(Ease.InCubic))
                    .Join(Pedestal.DOScale(0.00001f, 1f).From().SetDelay(0.5f).SetEase(Ease.OutBack))
                    .Append(Pedestal.DORotate(new Vector3(0, rotationAngleView, 0), 0.3f, RotateMode.LocalAxisAdd).SetEase(Ease.InExpo))
                    .OnComplete(() => { IsComplete = true; })
                    .AppendInterval(0.1f)
                    .AppendCallback(() => playParticle())
                    .Append(newRewardInstantiatedGO.transform.DOScale(1f, 0.8f).SetEase(Ease.OutElastic))
                    .AppendInterval(0.3f)
                    .AppendCallback(() => { pedestalTween.Play(); });
            } else {
                showTween = DOTween.Sequence().Pause()
                    .Append(LockClosed.DOScale(0.0001f, 0.45f).From().SetEase(Ease.OutBack))
                    .AppendInterval(0.3f)
                    .AppendCallback(() => { alarmClockSound = AudioManager.I.PlaySound(Sfx.AlarmClock); })
                    .Append(LockClosed.DOShakePosition(0.8f, 40f, 16, 90, false, false))
                    .Join(LockClosed.DOShakeRotation(0.8f, new Vector3(0, 0, 70f), 16, 90, false))
                    .Join(LockClosed.DOPunchScale(Vector3.one * 0.8f, 0.4f, 20))
                    .AppendCallback(() => {
                        LockClosed.gameObject.SetActive(false);
                        LockOpen.gameObject.SetActive(true);
                        if (alarmClockSound != null) {
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
                    .Join(LockOpen.DORotate(new Vector3(0, 0, 360), 0.6f, RotateMode.FastBeyond360).SetRelative().SetEase(Ease.InCubic))
                    .Join(Godray1.DOScale(0.00001f, 0.6f).SetEase(Ease.InCubic))
                    .Join(Pedestal.DOScale(0.00001f, 1f).From().SetDelay(0.5f).SetEase(Ease.OutBack))
                    .Append(Pedestal.DORotate(new Vector3(0, rotationAngleView, 0), 0.3f, RotateMode.LocalAxisAdd).SetEase(Ease.InExpo))
                    .OnComplete(() => { IsComplete = true; })
                    .AppendInterval(0.1f)
                    .AppendCallback(() => playParticle())
                    .AppendInterval(0.3f)
                    .AppendCallback(() => { pedestalTween.Play(); });
            }
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

        void playParticle()
        {
            if (PoofParticle == null || newRewardInstantiatedGO == null) { return; }
            PoofParticle.transform.position = newRewardInstantiatedGO.transform.position;
            PoofParticle.Play();
            AudioManager.I.PlaySound(Sfx.Poof);
        }

        void OnDestroy()
        {
            showTween.Kill();
            godraysTween.Kill();
            pedestalTween.Kill();
        }

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
    }
}