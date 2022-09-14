using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class MiniDrawingTool : MonoBehaviour
    {
        public static bool DISTANCE_FROM_BORDERS = false; // True: error if hitting borders. False: error if getting too far from the inner line.

        public void Update()
        {
            if ( MazeGame.instance.currentNewMazeLetter == null) return;
            var newLetter = MazeGame.instance.currentNewMazeLetter.GetComponent<NewMazeLetter>();
            var contourStrokes = newLetter.shapeData.Contour;
            var letterStrokes = newLetter.shapeData.Strokes;

            var letter = newLetter.GetComponentInChildren<MazeLetter>();
            var letterPosOffset = newLetter.transform.localPosition + letter.transform.localPosition;
            if (!letter.isDrawing) return;

            if (!MazeGame.instance.drawingTool.activeSelf) return;

            var projectedPoint = transform.position;
            projectedPoint -= letterPosOffset;
            projectedPoint /= 1.5f;
            projectedPoint = Quaternion.AngleAxis(-90f, Vector3.right) * projectedPoint;

            var closestPoint = Vector3.zero;
            var distance = 0f;
            if (DISTANCE_FROM_BORDERS)
            {
                ShapeManager.ClosestPointOnSpline(contourStrokes[0].Spline, projectedPoint, out closestPoint, out distance, letterPosOffset);
            }
            else
            {
                ShapeManager.ClosestPointOnSpline(letterStrokes[MazeGame.instance.currentCharacter.currentFruitList].Spline, projectedPoint, out closestPoint, out distance, letterPosOffset);
            }

            closestPoint = Quaternion.AngleAxis(90, Vector3.right) * closestPoint;
            closestPoint *= 1.5f;
            closestPoint += letterPosOffset;
            Debug.DrawLine(transform.position, closestPoint, Color.cyan);

            //Debug.LogWarning(distance);

            bool error = false;
            if (DISTANCE_FROM_BORDERS) error = distance < 0.5f;
            else error = distance > 1f;

            if (error)
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
