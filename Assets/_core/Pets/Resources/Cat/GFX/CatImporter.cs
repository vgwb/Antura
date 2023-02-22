#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

public class CatImporter : AssetPostprocessor
{
    string[] loopKeywords = { "loop", "sheep", "run", "walk", "sleep", "idle", "breath" };
    string[] noloopKeywords = { "_start", "_end" };

    private void OnPreprocessAnimation()
    {
        if (!assetPath.Contains("cat_Anim_Takes")) return;

        ModelImporter modelImporter = assetImporter as ModelImporter;
        var clips = modelImporter.defaultClipAnimations;
        for (var index = 0; index < clips.Length; index++)
        {
            clips[index].loopTime = loopKeywords.Any(k => clips[index].name.Contains(k))
                                && noloopKeywords.All(k => !clips[index].name.Contains(k));
        }

        modelImporter.clipAnimations = clips;
        modelImporter.SaveAndReimport();
    }

}
#endif
