#if UNITY_EDITOR
using System.Linq;
using Antura.Database;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class ShapeDataLogic : MonoBehaviour
{
    public string id;
    public LetterData data;
    public ShapeLetterData shapeData;

    public TextMeshPro LetterTextMesh;
    public GameObject StrokesPivot;
    public GameObject ContourPivot;

    void Update()
    {
        if (data == null) return;
        if (shapeData == null) return;

        // Save current splines
        var controllers = StrokesPivot.GetComponentsInChildren<SpriteShapeController>().ToList();
        shapeData.Strokes = new Stroke[controllers.Count];
        for (int i = 0; i < controllers.Count; i++)
        {
            shapeData.Strokes[i] = new Stroke();
            ShapeManager.CopySpline(controllers[i].spline, shapeData.Strokes[i].Spline);
        }

        controllers = ContourPivot.GetComponentsInChildren<SpriteShapeController>().ToList();
        shapeData.Contour = new Stroke[controllers.Count];
        for (int i = 0; i < controllers.Count; i++)
        {
            shapeData.Contour[i] = new Stroke();
            ShapeManager.CopySpline(controllers[i].spline, shapeData.Contour[i].Spline);
        }
        EditorUtility.SetDirty(shapeData);
    }
}
#endif