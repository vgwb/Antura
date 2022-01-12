using Antura.Plugins.NativeGallery;
using System;
using UnityEngine;

namespace Antura.Core.Services.Gallery
{
    public class GalleryService
    {
        public GalleryService()
        {
            DetectWriteAccess();
        }

        public bool HasWriteAccess;

        public void DetectWriteAccess()
        {
            HasWriteAccess = NativeGallery.CheckWriteAccess("Antura");
            if (!HasWriteAccess) Debug.LogWarning("Has no write access to the gallery!");
        }

        public bool SaveScreenshot(Texture2D texture)
        {
            if (!HasWriteAccess) return true;
            try
            {
                // TODO need better way to generate incremental / unique images filenames
                NativeGallery.SaveToGallery(texture, "Antura", "AnturaSpace" + (Time.time / 1000) + ".png");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception during gallery saving: " + ex.Message);
                return false;
            }
        }

    }
}