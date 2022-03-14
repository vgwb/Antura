using System;
using DG.DeExtensions;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class ShopSlotFeedback : MonoBehaviour
    {
        #region Feedback

        public GameObject focusGo;
        public GameObject spawnGo;
        public GameObject despawnGo;

        void Awake()
        {
            focusGo.SetActive(false);
            spawnGo.SetActive(false);
            despawnGo.SetActive(false);
        }

        public void FocusHighlight(bool choice)
        {
            focusGo.SetActive(choice);
            if (choice)
            {
                var ps = focusGo.GetComponentInChildren<ParticleSystem>();
                if (ps != null)
                    ps.Play();
            }
        }

        public void Spawn()
        {
            spawnGo.SetActive(true);
            spawnGo.GetComponentInChildren<ParticleSystem>().Play();
        }

        public void Despawn()
        {
            despawnGo.SetActive(true);
            despawnGo.GetComponentInChildren<ParticleSystem>().Play();
        }

        #endregion
    }
}
