using Antura.Audio;
using Antura.Core;
using Antura.Core.Services.Gallery;
using Antura.Debugging;
using Antura.UI;
using Antura.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.AnturaSpace
{
    public class ShopPhotoManager : SingletonMonoBehaviour<ShopPhotoManager>
    {
        public GalleryManager GalleryManager;

        public Action OnPhotoConfirmationRequested;
        public Action OnPurchaseCompleted;
        public Action<bool> OnPurchaseSuccess;

        public List<GameObject> gameObjectsToHide = new List<GameObject>();

        [HideInInspector]
        public int CurrentPhotoCost;

        void Start()
        {
            var debugPanel = FindObjectOfType<DebugPanel>();
            if (debugPanel != null)
            {
                gameObjectsToHide.Add(debugPanel.gameObject);
            }

            gameObjectsToHide.Add(FindObjectOfType<GlobalUI>().gameObject);
        }

        public void TakePhoto()
        {
            StartCoroutine(TakeScreenshotCO());
        }

        private Texture2D currentPhotoTexture;
        private IEnumerator TakeScreenshotCO()
        {
            AudioManager.I.PlaySound(Sfx.Photo);

            // Hide objects first
            foreach (var go in gameObjectsToHide)
            {
                go.SetActive(false);
            }

            yield return new WaitForEndOfFrame();

            currentPhotoTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            currentPhotoTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            currentPhotoTexture.Apply();

            foreach (var go in gameObjectsToHide)
            {
                go.SetActive(true);
            }

            GalleryManager.ShowPreview(currentPhotoTexture);

            // Ask for confirmation
            ShopDecorationsManager.I.SetContextSpecialAction();
            if (OnPhotoConfirmationRequested != null)
                OnPhotoConfirmationRequested();
        }

        public void ConfirmPhoto()
        {
            bool success = AppManager.I.Services.Gallery.SaveScreenshot(currentPhotoTexture);

            currentPhotoTexture = null;
            if (OnPurchaseSuccess != null)
            { OnPurchaseSuccess(success); }
            if (OnPurchaseCompleted != null)
            { OnPurchaseCompleted(); }

            ShopDecorationsManager.I.SetContextClosed();

            AppManager.I.Player.Save();

            GalleryManager.HidePreview();
        }

        public void CancelPhoto()
        {
            if (AnturaSpaceScene.I.TutorialMode)
            { return; }

            ShopDecorationsManager.I.SetPreviousContext();

            currentPhotoTexture = null;

            GalleryManager.HidePreview();
        }
    }
}
