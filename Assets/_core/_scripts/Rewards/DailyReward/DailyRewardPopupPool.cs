using System.Collections.Generic;
using UnityEngine;

namespace Antura.Rewards
{
    /// <summary>
    /// Fake pool, since it works only once (no need to replicate)
    /// </summary>
    public class DailyRewardPopupPool : MonoBehaviour
    {
        #region Serialized
#pragma warning disable 649
        [SerializeField]
        private DailyRewardPopup popup;
#pragma warning restore 649
        #endregion

        bool initialized;

        #region Unity + INIT

        void Init()
        {
            if (initialized)
                return;

            initialized = true;
            popup.gameObject.SetActive(false);
        }

        void Awake()
        {
            Init();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of inactive popups
        /// </summary>
        public List<DailyRewardPopup> Spawn(int tot)
        {
            Init();
            List<DailyRewardPopup> res = new List<DailyRewardPopup>();
            if (tot == 0)
                return res;

            res.Add(popup);
            while (res.Count < tot)
            {
                DailyRewardPopup p = Instantiate(popup, popup.transform.parent, false);
                res.Add(p);
            }
            return res;
        }

        #endregion
    }
}
