using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class MiniDrawingTool : MonoBehaviour
    {
        public void Update()
        {
            if ( MazeGame.instance.currentNewMazeLetter == null) return;
            var newLetter = MazeGame.instance.currentNewMazeLetter.GetComponent<NewMazeLetter>();
            var contourStrokes = newLetter.shapeData.Contour;

            var letter = newLetter.GetComponentInChildren<MazeLetter>();
            var letterPosOffset = newLetter.transform.localPosition + letter.transform.localPosition;
            if (!letter.isDrawing) return;

            if (!MazeGame.instance.drawingTool.activeSelf) return;

            var projectedPoint = transform.position;
            projectedPoint -= letterPosOffset;
            projectedPoint /= 1.5f;
            projectedPoint = Quaternion.AngleAxis(-90f, Vector3.right) * projectedPoint;

            ShapeManager.ClosestPointOnSpline(contourStrokes[0].Spline, projectedPoint, out var closestPoint, out var distance, letterPosOffset);

            closestPoint = Quaternion.AngleAxis(90, Vector3.right) * closestPoint;
            closestPoint *= 1.5f;
            closestPoint += letterPosOffset;
            //Debug.DrawLine(transform.position, closestPoint, Color.cyan);

            //Debug.LogWarning(distance);

            if (distance < 0.5f)
            {
                MazeGame.instance.OnDrawnLetterWrongly();
            }
        }

        // OLD Method: wait for exit.
        /*private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "MazeLetter")
            {
                Debug.LogError(other.name, other.gameObject);
                MazeGame.instance.OnDrawnLetterWrongly();
            }
        }*/
    }
}
