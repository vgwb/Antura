using Antura.Tutorial;
using UnityEngine;

namespace Antura.Minigames.MakeFriends
{
    public class RoundResultAnimator : MonoBehaviour
    {
        public ParticleSystem vfx;
        public Vector3 wrongMarkPosition;

        public void ShowWin()
        {
            vfx.gameObject.SetActive(true);
            vfx.Play();
        }

        public void ShowLose()
        {
            TutorialUI.MarkNo(wrongMarkPosition, TutorialUI.MarkSize.Huge);
            MakeFriendsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.KO);
        }

        public void Hide()
        {
            vfx.Stop();
        }
    }
}
