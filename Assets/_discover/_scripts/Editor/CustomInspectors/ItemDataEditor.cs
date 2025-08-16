using Antura.Discover;
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{

    public override bool HasPreviewGUI() => true;

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        var data = (ItemData)target;
        if (data.Icon != null)
            GUI.DrawTexture(r, data.Icon.texture, ScaleMode.ScaleToFit, true);
    }

    // public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    // {
    //     ItemData data = (ItemData)target;

    //     if (data == null || data.Icon == null)
    //         return null;

    //     Texture sourceTex = null;

    //     if (data.Icon != null)
    //         sourceTex = data.Icon.texture;

    //     if (sourceTex == null)
    //         return null; // fall back to default icon


    //     // Create the preview texture Unity will cache & show in Project + Object Picker
    //     var preview = new Texture2D(width, height, TextureFormat.RGBA32, false);

    //     // Draw the source into a temporary RT, respecting Sprite rect if needed
    //     var rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
    //     var prev = RenderTexture.active;
    //     RenderTexture.active = rt;
    //     GL.Clear(true, true, new Color(0, 0, 0, 0));

    //     // Compute UVs if using a sprite atlas
    //     Rect uv = new Rect(0, 0, 1, 1);
    //     if (data.Icon != null)
    //     {
    //         var sp = data.Icon;
    //         uv = new Rect(
    //             sp.textureRect.x / sp.texture.width,
    //             sp.textureRect.y / sp.texture.height,
    //             sp.textureRect.width / sp.texture.width,
    //             sp.textureRect.height / sp.texture.height);
    //     }

    //     // Draw the texture (full-bleed) into the RT
    //     Graphics.DrawTexture(
    //         new Rect(0, 0, width, height),
    //         sourceTex,
    //         uv, 0, 0, 0, 0);

    //     // Read back into Texture2D
    //     preview.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    //     preview.Apply();

    //     // Cleanup
    //     RenderTexture.active = prev;
    //     RenderTexture.ReleaseTemporary(rt);

    //     return preview;
    // }
}

