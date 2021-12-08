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
    public CustomSceneManager sceneManager;
    public ShapeDataLogic DataPrefab;

    public void OnValidate()
    {
        if (!Application.isPlaying) return;
    }

    IEnumerator Start()
    {
        while (!AppManager.I.Loaded) yield return null;
        GlobalUI.I.gameObject.SetActive(false);
        var letters =  AppManager.I.DB.GetAllLetterData();
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
        var letters =  AppManager.I.DB.GetAllLetterData();
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


    public static List<GameObject> SpawnObjectsOnSplines(GameObject prefab, Transform parent, Stroke[] strokes, float delta, float scale = 1f)
    {
        var gos = new List<GameObject>();
        foreach (var stroke in strokes)
        {
            var n = stroke.Spline.GetPointCount() - 1;
            var t = 0.0f;
            while (t < 1f * n)
            {
                // t goes from 0 to n. Find the point that we are interested into.
                int iPoint = Mathf.FloorToInt(t);

                var inSplinePoint = t % 1;
                var p = BezierUtility.BezierPoint(stroke.Spline.GetPosition(iPoint),
                    stroke.Spline.GetLeftTangent(iPoint),
                    stroke.Spline.GetRightTangent(iPoint),
                    stroke.Spline.GetPosition(iPoint + 1),
                    inSplinePoint);

                var go = Instantiate(prefab, parent);
                go.transform.localPosition = p * scale;

                //go.transform.localEulerAngles =

                t += delta;
            }
        }
        return gos;
    }

    public static Vector3 TangentOnSpline(Spline spline, float t)
    {
        var n = spline.GetPointCount();
        int pointIndex = Mathf.FloorToInt(t);
        if (pointIndex == n - 1)
        {
            // TODO: last extremity
        }
        var a = spline.GetPosition(pointIndex);
        var b = spline.GetLeftTangent(pointIndex);
        var c = spline.GetRightTangent(pointIndex);
        var d = spline.GetPosition(pointIndex + 1);

        var C1 = ( d - (3.0f * c) + (3.0f * b) - a );
        var C2 = ( (3.0f * c) - (6.0f * b) + (3.0f * a) );
        var C3 = ( (3.0f * b) - (3.0f * a) );
        //var C4 = ( a );

        var slope = ( ( 3.0f * C1 * t* t ) + ( 2.0f * C2 * t ) + C3 );
        return slope;
    }

}


#endif