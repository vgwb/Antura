#define GENERATE_IF_NOT_FOUND
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using Antura.Teacher;
using Antura.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.AddressableAssets;

public class ShapeManager : MonoBehaviour
{
    public LetterFilters LetterFilters;

    public ShapeDataLogic DataPrefab;

    private List<LetterData> letters;

    IEnumerator Start()
    {
        while (!AppManager.I.Loaded)
            yield return null;
        AppManager.I.AppSettingsManager.SetLearningContentID(AppManager.I.ContentEdition.ContentID);
        yield return AppManager.I.ReloadEdition();

        GlobalUI.I.gameObject.SetActive(false);
        letters = AppManager.I.VocabularyHelper.GetAllLetters(LetterFilters);
        for (var i = 0; i < letters.Count; i++)
        {
            var letter = letters[i];
            int x = i % 10 * 15;
            int y = i / 10 * 15;
            LoadLetter(letter, new Vector2(x, y));
        }
    }

    private void LoadLetter(ShapeLetterData d)
    {
        var unicode = d.name.Replace("shapedata_", "");
        LoadLetter(unicode);
    }

    private void LoadLetter(string _unicode)
    {
        var letterData = letters.FirstOrDefault(x => string.Equals(x.GetCompleteUnicodes(), _unicode, StringComparison.Ordinal));
        LoadLetter(letterData);
    }

    private void LoadLetter(LetterData _letter, Vector2 pos = default)
    {
        var letterData = _letter;
        if (letterData == null)
            return;

        var el = Instantiate(DataPrefab, DataPrefab.transform.parent);
        el.transform.localPosition = pos;
        el.data = letterData;

        el.name = $"Data_{letterData.GetCompleteUnicodes()}_{letterData.Id}";
        Debug.Log($"Loading shape for {letterData}");

        el.LetterTextMesh.font = AppManager.I.LanguageSwitcher.GetLangConfig(LanguageUse.Learning).LanguageFont;
        el.LetterTextMesh.text = letterData.GetStringForDisplay(forceShowAccent:true);

        var shapeData = AppManager.I.AssetManager.GetShapeLetterData(letterData);
#if UNITY_EDITOR && GENERATE_IF_NOT_FOUND
        if (shapeData == null)
        {
            var fontName = AppManager.I.LanguageSwitcher.GetLangConfig(LanguageUse.Learning).LanguageFont.name.Split(' ').First().Split('_').Last();
            var assetPath = $"Assets/Resources/Fonts/Learning/Font {fontName}/ShapeData/shapedata_{letterData.GetCompleteUnicodes()}.asset";

            shapeData = AssetDatabase.LoadAssetAtPath<ShapeLetterData>(assetPath);
            if (shapeData != null)
            {
                Debug.LogWarning("We already have an asset with that path!");
            }
            else
            {
                // Generate it
                shapeData = ScriptableObject.CreateInstance<ShapeLetterData>();
                AssetDatabase.CreateAsset(shapeData, assetPath);
                AssetDatabase.SaveAssets();
                Debug.LogWarning($"Generating shapeData for {letterData.Id}");

                // Add to addressables
                var guid = AssetDatabase.AssetPathToGUID(assetPath);
                var entry = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, AddressableAssetSettingsDefaultObject.Settings.DefaultGroup);
                entry.address = $"{fontName}/shapedata_{letterData.GetCompleteUnicodes()}";
            }
        }
#endif
        if (shapeData != null)
        {
            el.shapeData = shapeData;

            // Load the current spline
            LoadSplinesOn(el.StrokesPivot, shapeData.Strokes);
            LoadSplinesOn(el.ContourPivot, shapeData.Contour);
            LoadPointsOn(el.EmptyPointsPivot, shapeData.EmptyZones);
            LoadPointOn(el.CenterPoint, shapeData.Center);
        }
    }

    public static void LoadPointOn(GameObject pointGo, Vector2 point)
    {
        pointGo.transform.localPosition = point;
    }

    public static void LoadPointsOn(GameObject pivot, Vector2[] points)
    {
        var nPoints = points?.Length ?? 0;
        if (points == null)
            return;
        var items = pivot.GetComponentsInChildren<MeshFilter>().ToList();
        while (items.Count < nPoints)
        {
            var newItem = Instantiate(items[0], items[0].transform.parent);
            items.Add(newItem);
        }
        for (int i = 0; i < nPoints; i++)
        {
            items[i].gameObject.SetActive(true);
            items[i].transform.localPosition = points[i];
        }
        for (int i = nPoints; i < items.Count; i++)
        {
            items[i].gameObject.SetActive(false);
        }
    }

    public static void LoadSplinesOn(GameObject pivot, Stroke[] strokes)
    {
        var nStrokes = strokes?.Length ?? 0;
        if (strokes == null)
            return;
        var controllers = pivot.GetComponentsInChildren<SpriteShapeController>().ToList();
        while (controllers.Count < nStrokes)
        {
            var newController = Instantiate(controllers[0], controllers[0].transform.parent);
            controllers.Add(newController);
        }
        for (int i = 0; i < nStrokes; i++)
        {
            controllers[i].name = $"Stroke_{i.ToString().PadLeft(2,'0')}";
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


    public static List<GameObject> SpawnObjectsOnSplines(GameObject prefab, Transform parent, Stroke[] strokes, float delta, float start, float scale, out List<float> tArray)
    {
        tArray = new List<float>();
        var gos = new List<GameObject>();
        for (var iStroke = 0; iStroke < strokes.Length; iStroke++)
        {
            var stroke = strokes[iStroke];
            var pivot = new GameObject($"{prefab.name}_stroke_{iStroke}");
            pivot.transform.SetParent(parent);
            pivot.transform.localEulerAngles = Vector3.zero;
            pivot.transform.localScale = Vector3.one;
            pivot.transform.localPosition = Vector3.zero;

            var n = stroke.Spline.GetPointCount() - 1;
            var t = start;
            int seq = 0;
            GameObject go;
            while (t < 1f * n)
            {
                go = SpawnAt(prefab, pivot, stroke.Spline, t, scale);
                go.name = $"{prefab.name}_{iStroke}_{(seq)}";
                gos.Add(go);
                tArray.Add(t);
                seq++;
                t += delta;
            }

            // Spawn at last one
            t = 1f * n;
            go = SpawnAt(prefab, pivot, stroke.Spline, t, scale);
            go.name = $"{prefab.name}_{iStroke}_{(seq)}";
            gos.Add(go);
            tArray.Add(t);
        }

        return gos;
    }

    public static GameObject SpawnAt(GameObject prefab, GameObject pivot, DirectionalSpline spline, float t, float scale = 1f)
    {
        var pos = PositionOnSpline(spline, t);
        var tangent = TangentOnSpline(spline, t).normalized;
        var go = Instantiate(prefab, pivot.transform);
        go.transform.localPosition = pos * scale;

        // @note: these are set to match Maze's orientations
        var angle = Vector2.SignedAngle(tangent, Vector2.left);
        go.transform.localEulerAngles = new Vector3(-angle, -90, 90);
        return go;
    }

    public static void PlaceObjectOnSpline(GameObject go, GameObject pivot, DirectionalSpline spline, float t, float scale = 1f)
    {
        var pos = PositionOnSpline(spline, t);
        var tangent = TangentOnSpline(spline, t).normalized;
        go.transform.parent = pivot.transform;
        go.transform.localPosition = pos * scale;

        // @note: these are set to match Maze's orientations
        var angle = Vector2.SignedAngle(tangent, Vector2.left);
        go.transform.localEulerAngles = new Vector3(-angle, -90, 90);
    }

    public static Vector3 PositionOnSpline(Spline spline, float t)
    {
        var n = spline.GetPointCount();
        int iPoint = Mathf.FloorToInt(t);

        var i0 = iPoint;
        var i1 = iPoint + 1;

        if (!spline.isOpenEnded && iPoint == n - 1)
        {
            i0 = iPoint;
            i1 = 0;
        }
        else
        {
            if (iPoint < 0)
                return spline.GetPosition(0);
            if (iPoint >= n - 1)
                return spline.GetPosition(n - 1);
        }

        var t_in = t % 1;
        var p0 = spline.GetPosition(i0);
        var p1 = spline.GetPosition(i1);
        var rt = p0 + spline.GetRightTangent(i0);
        var lt = p1 + spline.GetLeftTangent(i1);
        var p = SplinePos(p0, rt, lt, p1, t_in);
        return p;
    }

    // @note: Unity's BezierPoint in BezierUtility is wrong! We are doing our own.
    public static Vector3 SplinePos(Vector3 p0, Vector3 rt, Vector3 lt, Vector3 p1, float t)
    {
        float s = 1.0f - t;
        return s * s * s * p0 + 3.0f * s * s * t * rt + 3.0f * s * t * t * lt + t * t * t * p1;
    }

    public static float DistanceFromSpline(Spline spline, Vector3 pos)
    {
        ClosestPointOnSpline(spline, pos, out _, out var minDistance);
        return minDistance;
    }

    public static void ClosestPointOnSpline(Spline spline, Vector3 pos, out Vector3 closestPoint, out float minDistance, Vector3 offset = default)
    {
        // Approximate distance from the spline
        var delta = 0.01f;
        var nChecks = Mathf.FloorToInt(1f / delta) * (spline.GetPointCount() + 1);
        minDistance = Mathf.Infinity;
        closestPoint = Vector3.zero;
        for (int i = 0; i < nChecks; i++)
        {
            var t = i * delta;
            var splinePoint = PositionOnSpline(spline, t);

            //var projectedPoint = Quaternion.AngleAxis(90f, Vector3.right) * splinePoint * 1.5f + offset;
            //Debug.DrawLine(projectedPoint, projectedPoint + Vector3.forward*0.1f, Color.yellow);
            //var projectedPoint2 = Quaternion.AngleAxis(90f, Vector3.right) * pos * 1.5f + offset;
            //Debug.DrawLine(projectedPoint, projectedPoint2, Color.white);

            var dist = Vector3.Distance(pos, splinePoint);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestPoint = splinePoint;
            }
        }

        //var projectedPoint1b = Quaternion.AngleAxis(90f, Vector3.right) * closestPoint * 1.5f + offset;
        //var projectedPoint2b = Quaternion.AngleAxis(90f, Vector3.right) * pos * 1.5f + offset;
        //Debug.DrawLine(projectedPoint2b, projectedPoint1b, Color.red);
    }

    public static Vector3 TangentOnSpline(Spline spline, float t)
    {
        var n = spline.GetPointCount();
        int iPoint = Mathf.FloorToInt(t);
        if (iPoint < 0)
            return spline.GetRightTangent(0);
        if (iPoint >= n - 1)
            return -spline.GetLeftTangent(iPoint); // Inverted left tangent, as it goes out
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
        return 3.0f * s * s * (rt - p0) + 6.0f * s * t * (lt - rt) + 3.0f * t * t * (p1 - lt);
    }

    public static void FlattenSpline(Spline spline)
    {
        var n = spline.GetPointCount();
        for (int i = 0; i < n; i++)
        {
            var pos = spline.GetPosition(i);
            pos.z = 0;
            spline.SetPosition(i, pos);
            pos = spline.GetRightTangent(i);
            pos.z = 0;
            spline.SetRightTangent(i, pos);
            pos = spline.GetLeftTangent(i);
            pos.z = 0;
            spline.SetLeftTangent(i, pos);
        }
    }
}
