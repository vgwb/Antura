using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using Antura.UI;
using TMPro;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.AddressableAssets;
#endif
using UnityEngine;
using UnityEngine.U2D;


#if UNITY_EDITOR

public class ShapeManager : MonoBehaviour
{
    public ShapeDataLogic DataPrefab;

    IEnumerator Start()
    {
        while (!AppManager.I.Loaded) yield return null;
        GlobalUI.I.gameObject.SetActive(false);
        var letters = AppManager.I.DB.GetAllLetterData();
        for (var i = 0; i < letters.Count; i++)
        {
            var letter = letters[i];
            int x = i % 10 * 10;
            int y = i / 10 * 10;
            LoadLetter(letter, new Vector2(x,y));
        }
    }

    private void LoadLetter(ShapeLetterData d)
    {
        var letter = d.name.Replace("sideletter_", "");
        LoadLetter(letter);
    }

    private void LoadLetter(string _letter)
    {
        var letters = AppManager.I.DB.GetAllLetterData();
        var letterData = letters.FirstOrDefault(x => x.Id == _letter);
        LoadLetter(letterData);
    }

    private void LoadLetter(LetterData _letter, Vector2 pos = default)
    {
        var letterData = _letter;
        if (letterData == null) return;

        var el = Instantiate(DataPrefab, DataPrefab.transform.parent);
        el.transform.localPosition = pos;
        el.data = letterData;
        el.id = letterData.Id;

        el.name = $"Data_{letterData.Id}";
        Debug.LogError("Working on " + letterData.ToString());

        el.LetterTextMesh.font = AppManager.I.LanguageSwitcher.GetLangConfig(LanguageUse.Learning).LetterFont;
        el.LetterTextMesh.text = letterData.GetStringForDisplay();

        var lang = AppManager.I.LanguageSwitcher.GetLangConfig(LanguageUse.Learning).Code;
        var assetPath = $"Assets/_lang_bundles/{lang}/SideData/Letters/sideletter_{letterData.Id}.asset";

        var shapeData = AppManager.I.AssetManager.GetShapeLetterData(letterData.Id);
        if (shapeData == null)
        {
            // Generate it
            shapeData = ScriptableObject.CreateInstance<ShapeLetterData>();
            AssetDatabase.CreateAsset(shapeData, assetPath);
            AssetDatabase.SaveAssets();
            Debug.LogWarning("Generating shapeData for " + letterData.Id);

            // Add to addressables
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            var entry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, AddressableAssetSettingsDefaultObject.Settings.DefaultGroup);
            entry.address = $"{lang}/SideData/Letters/sideletter_{letterData.Id}";
        }
        el.shapeData = shapeData;

        // Load the current spline
        LoadSplinesOn(el.StrokesPivot, shapeData.Strokes);
        LoadSplinesOn(el.ContourPivot, shapeData.Contour);
    }

    public static void LoadSplinesOn(GameObject pivot, Stroke[] strokes)
    {
        var nStrokes = strokes?.Length ?? 0;
        if (strokes == null) return;
        var controllers = pivot.GetComponentsInChildren<SpriteShapeController>().ToList();
        while (controllers.Count < nStrokes)
        {
            var newController = Instantiate(controllers[0],controllers[0].transform.parent);
            controllers.Add(newController);
        }
        for (int i = 0; i < nStrokes; i++)
        {
            controllers[i].gameObject.SetActive(true);
            CopySpline(strokes[i].Spline, controllers[i].spline);
        }
        for (int i = nStrokes; i < controllers.Count; i++)
        {
            controllers[i].gameObject.SetActive(false);
        }
    }

    public static void CopySpline(Spline fromSp, Spline toSp)
    {
        toSp.Clear();
        toSp.isOpenEnded = fromSp.isOpenEnded;
        for (int j = 0; j < fromSp.GetPointCount(); j++)
        {
            toSp.InsertPointAt(j, fromSp.GetPosition(j));
            toSp.SetTangentMode(j, fromSp.GetTangentMode(j));
            toSp.SetLeftTangent(j, fromSp.GetLeftTangent(j));
            toSp.SetRightTangent(j, fromSp.GetRightTangent(j));
            toSp.SetCorner(j, fromSp.GetCorner(j));
            toSp.SetHeight(j, fromSp.GetHeight(j));
            toSp.SetSpriteIndex(j, fromSp.GetSpriteIndex(j));
        }
    }


    public static List<GameObject> SpawnObjectsOnSplines(GameObject prefab, Transform parent, Stroke[] strokes, float delta, float start, float scale = 1f)
    {
        var gos = new List<GameObject>();
        foreach (var stroke in strokes)
        {
            var n = stroke.Spline.GetPointCount() - 1;
            var t = start;
            while (t < 1f * n)
            {
                var pos = PositionOnSpline(stroke.Spline, t);
                var tangent = TangentOnSpline(stroke.Spline, t);
                var go = Instantiate(prefab, parent);
                go.transform.localPosition = pos * scale;
                go.transform.rotation = Quaternion.LookRotation(tangent);
                gos.Add(go);
                t += delta;
            }
        }
        return gos;
    }

    public static Vector3 PositionOnSpline(Spline spline, float t)
    {
        var n = spline.GetPointCount();
        int iPoint = Mathf.FloorToInt(t);
        if (iPoint < 0) return spline.GetPosition(0);
        if (iPoint >= n - 1) return spline.GetPosition(n-1);
        var t_in = t % 1;
        var p0 = spline.GetPosition(iPoint);
        var p1 = spline.GetPosition(iPoint + 1);
        var rt = p0 + spline.GetRightTangent(iPoint);
        var lt = p1 + spline.GetLeftTangent(iPoint + 1);
        var p = SplinePos(p0, rt, lt, p1, t_in);
        return p;
    }

    // @note: Unity's BezierPoint in BezierUtility is wrong! We are doing our own.
    public static Vector3 SplinePos(Vector3 p0, Vector3 rt, Vector3 lt, Vector3 p1, float t)
    {
        float s = 1.0f - t;
        return s * s * s * p0 + 3.0f * s * s * t * rt + 3.0f * s * t * t * lt + t * t * t * p1;
    }

    public static Vector3 TangentOnSpline(Spline spline, float t)
    {
        var n = spline.GetPointCount();
        int iPoint = Mathf.FloorToInt(t);
        if (iPoint < 0) return spline.GetRightTangent(0);
        if (iPoint >= n - 1) return spline.GetLeftTangent(iPoint);
        var t_in = t % 1;
        var p0 = spline.GetPosition(iPoint);
        var p1 = spline.GetPosition(iPoint + 1);
        var rt = p0 + spline.GetRightTangent(iPoint);
        var lt = p1 + spline.GetLeftTangent(iPoint + 1);
        var slope = SplineSlope(p0, rt, lt, p1, t_in);
        return slope;
    }
    public static Vector3 SplineSlope(Vector3 p0, Vector3 rt, Vector3 lt, Vector3 p1, float t)
    {
        float s = 1.0f - t;
        return 3.0f * s * s * (rt- p0) + 6.0f *  s * t * (lt - rt) + 3.0f  * t * t * (p1 - lt);
    }


}


#endif