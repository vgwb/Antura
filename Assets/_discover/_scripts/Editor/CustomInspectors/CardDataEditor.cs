using Antura.Discover;
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(CardData))]
public class CardDataEditor : Editor
{

    public override bool HasPreviewGUI() => true;

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        var data = (CardData)target;
        if (data.ImageAsset != null && data.ImageAsset.Image != null)
            GUI.DrawTexture(r, data.ImageAsset.Image.texture, ScaleMode.ScaleToFit, true);
    }
}
// using Antura.Discover;
// using UnityEditor;
// using UnityEngine;

// [CustomEditor(typeof(CardData))]
// [CanEditMultipleObjects]
// public class CardDataObjectEditor : Editor
// {
//     public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
//     {
//         var data = (CardData)target;
//         Texture sourceTex = null;

//         if (data.ImageAsset.Image != null)
//             sourceTex = data.Image.texture;

//         if (sourceTex == null)
//             return null; // fall back to default icon

//         // Create the preview texture Unity will cache & show in Project + Object Picker
//         var preview = new Texture2D(width, height, TextureFormat.RGBA32, false);

//         // Draw the source into a temporary RT, respecting Sprite rect if needed
//         var rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
//         var prev = RenderTexture.active;
//         RenderTexture.active = rt;
//         GL.Clear(true, true, new Color(0, 0, 0, 0));

//         // Compute UVs if using a sprite atlas
//         Rect uv = new Rect(0, 0, 1, 1);
//         if (data.Image != null)
//         {
//             var sp = data.Image;
//             uv = new Rect(
//                 sp.textureRect.x / sp.texture.width,
//                 sp.textureRect.y / sp.texture.height,
//                 sp.textureRect.width / sp.texture.width,
//                 sp.textureRect.height / sp.texture.height);
//         }

//         // Draw the texture (full-bleed) into the RT
//         Graphics.DrawTexture(
//             new Rect(0, 0, width, height),
//             (Texture)sourceTex,
//             uv, 0, 0, 0, 0);

//         // Read back into Texture2D
//         preview.ReadPixels(new Rect(0, 0, width, height), 0, 0);
//         preview.Apply();

//         // Cleanup
//         RenderTexture.active = prev;
//         RenderTexture.ReleaseTemporary(rt);

//         return preview;
//     }
// }

