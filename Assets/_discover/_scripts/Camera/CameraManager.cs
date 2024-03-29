// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2024/03/27

using Cinemachine;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class CameraManager : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        public PlayerCameraController CamController;

        #endregion

        public static CameraManager I;

        #region Unity

        void Awake()
        {
            if (I != null)
            {
                Debug.LogError("CameraManager already exists in scene, destroying duplicate");
                Destroy(this.gameObject);
                return;
            }

            I = this;
        }

        void OnDestroy()
        {
            if (I == this)
                I = null;
        }

        #endregion
    }
}
