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
    }
}