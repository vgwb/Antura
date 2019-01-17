using Antura.Plugins.NativeGallery;
using System;
using UnityEngine;


namespace Antura.Core.Services.Gallery
{
    public class GalleryService
    {
        public GalleryService()
        {
        }

        public void SaveScreenshot(Texture2D texture)
        {
            // TODO need better way to generate incremental / unique images filenames
            NativeGallery.SaveToGallery(texture, "Antura", "AnturaSpace" + (Time.time / 1000) + ".png");
        }

    }
}