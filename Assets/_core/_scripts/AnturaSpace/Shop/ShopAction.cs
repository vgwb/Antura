using System;
using Antura.Audio;
using Antura.Core;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class ShopAction : MonoBehaviour
    {
        [Header("Preview Parameters")]
        public float scaleMultiplier = 2f;
        public Vector3 eulOffset = Vector3.zero;
        public string LocID;

        public virtual GameObject ObjectToRender
        {
            get { return null; }
        }

        public Sprite iconSprite;
        public int bonesCost;

        public virtual bool IsOnTheSide
        {
            get { return false; }
        }

        public virtual bool CanPurchaseAnywhere
        {
            get { return false; }
        }

        public virtual bool IsLocked
        {
            get
            {
                return NotEnoughBones;
            }
        }

        public virtual bool IsClickButton
        {
            get { return false; }
        }

        public bool NotEnoughBones
        {
            get
            {
                return AppManager.I.Player.GetTotalNumberOfBones() < bonesCost;
            }
        }

        public Action OnActionCommitted;
        public Action OnActionRefreshed;

        #region Virtual

        public virtual void PerformAction()
        {
            // nothing to do here
        }

        public virtual void PerformDrag()
        {
            // nothing to do here
        }

        public virtual void CancelAction()
        {
            // nothing to do here
        }

        protected virtual string ActionKey => "";

        protected virtual void CommitAction()
        {
            CommitActionCheck(true);
        }

        protected void CommitActionCheck(bool success)
        {
            if (success)
            {
                AppManager.I.Services.Analytics.TrackItemBought(bonesCost, ActionKey);
                AppManager.I.Player.RemoveBones(bonesCost);
            }
            else
            {
                AudioManager.I.PlaySound(Sfx.KO);
            }

            OnActionCommitted?.Invoke();
        }

        protected void RefreshAction()
        {
            OnActionRefreshed?.Invoke();
        }

        #endregion


    }
}
