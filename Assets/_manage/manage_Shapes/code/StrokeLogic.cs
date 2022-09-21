#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class StrokeLogic : MonoBehaviour
{
    public Color startColor = Color.yellow;

    private void OnValidate()
    {
        var index = transform.GetSiblingIndex();
        name = $"Stroke_{transform.GetSiblingIndex().ToString("00")}";
        GetComponent<SpriteShapeRenderer>().color = startColor;//Color.Lerp(startColor, Color.red, index / 5f);
    }

    private void OnDrawGizmosSelected()
    {
        var controller = GetComponent<SpriteShapeController>();
        var spline = controller.spline;
        for (int i = 0; i < spline.GetPointCount() - 1; i++)
        {
            Handles.color = Color.red;
            var tang = ShapeManager.TangentOnSpline(spline, i);

            Handles.ArrowHandleCap(
                0,
                transform.position + spline.GetPosition(i),
                transform.rotation * Quaternion.LookRotation(tang),
                1f,
                EventType.Repaint
            );
        }

        var pos = ShapeManager.PositionOnSpline(spline, testTangent);
        var tangent = ShapeManager.TangentOnSpline(spline, testTangent);

        Handles.color = Color.blue;
        Handles.ArrowHandleCap(
            0,
            transform.position + pos,
            transform.rotation * Quaternion.LookRotation(tangent),
            1f,
            EventType.Repaint
        );
    }

    public float testTangent;
}
#endif
