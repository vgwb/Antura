using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class StrokeLogic : MonoBehaviour
{
    private void OnValidate()
    {
        var index = transform.GetSiblingIndex();
        name = $"Stroke_{transform.GetSiblingIndex().ToString("00")}";
        GetComponent<SpriteShapeRenderer>().color = Color.Lerp(Color.yellow, Color.red, index / 5f);
    }

    private void OnDrawGizmosSelected()
    {
        var controller = GetComponent<SpriteShapeController>();
        var spline = controller.spline;
        for (int i = 0; i < spline.GetPointCount()-1; i++)
        {
            var t = transform;
            Handles.color = Color.red;
            var pos0 = spline.GetPosition(i);
            var pos1 = spline.GetPosition(i + 1);

            Handles.ArrowHandleCap(
                0,
                transform.position + spline.GetPosition(i),
                transform.rotation * Quaternion.LookRotation(pos1-pos0),
                1f,
                EventType.Repaint
            );
        }
    }
}