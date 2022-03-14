// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2019/06/26


using System;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Profile
{
    public class PlayerCreationAvatar : MonoBehaviour
    {
        #region Serialized
#pragma warning disable 0649

        [DeEmptyAlert]
        [SerializeField] Image _faceImg, _hairImg;

#pragma warning restore 0649
        #endregion

        public Image faceImg { get { return _faceImg; } }
        public Image hairImg { get { return _hairImg; } }

        #region Public Methods

        public void SetFace(Sprite sprite, Color skinColor)
        {
            _faceImg.sprite = sprite;
            SetFaceColor(skinColor);
        }

        public void SetHair(Sprite sprite)
        {
            _hairImg.sprite = sprite;
        }

        public void SetFaceColor(Color color)
        {
            _faceImg.color = color;
        }

        public void SetHairColor(Color color)
        {
            _hairImg.color = color;
        }

        #endregion
    }
}
