using UnityEngine;
using UnityEngine.U2D;
using Antura.Database;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShapeDataLogic : MonoBehaviour
{
    public LetterData data;
    public ShapeLetterData shapeData;

    public TextMeshPro LetterTextMesh;
    public GameObject StrokesPivot;
    public GameObject ContourPivot;
    public GameObject EmptyPointsPivot;
    public GameObject CenterPoint;

#if UNITY_EDITOR
    void Update()
    {
        if (data == null)
            return;
        if (shapeData == null)
            return;

        // Save current data
        var controllers = StrokesPivot.GetComponentsInChildren<SpriteShapeController>();
        shapeData.Strokes = new Stroke[controllers.Length];
        for (int i = 0; i < controllers.Length; i++)
        {
            ShapeManager.FlattenSpline(controllers[i].spline);

            shapeData.Strokes[i] = new Stroke();
            ShapeManager.CopySpline(controllers[i].spline, shapeData.Strokes[i].Spline);
        }

        controllers = ContourPivot.GetComponentsInChildren<SpriteShapeController>();
        shapeData.Contour = new Stroke[controllers.Length];
        for (int i = 0; i < controllers.Length; i++)
        {
            ShapeManager.FlattenSpline(controllers[i].spline);

            shapeData.Contour[i] = new Stroke();
            ShapeManager.CopySpline(controllers[i].spline, shapeData.Contour[i].Spline);
        }

        var emptyPoints = EmptyPointsPivot.GetComponentsInChildren<MeshFilter>();
        shapeData.EmptyZones = new Vector2[emptyPoints.Length];
        for (int i = 0; i < emptyPoints.Length; i++)
        {
            shapeData.EmptyZones[i] = emptyPoints[i].transform.localPosition;
        }

        shapeData.Center = CenterPoint.transform.localPosition;

        EditorUtility.SetDirty(shapeData);
    }
#endif
}
