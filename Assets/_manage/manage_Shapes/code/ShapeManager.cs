using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using Antura.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
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

        var shapeData = AppManager.I.AssetManager.GetSideLetterData(letterData.Id);
        el.shapeData = shapeData;
        if (shapeData == null) return;

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

}


#endif