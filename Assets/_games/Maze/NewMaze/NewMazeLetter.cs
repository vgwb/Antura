using System.Collections;
using System.Collections.Generic;
using Antura.Core;
using Antura.Database;
using Antura.LivingLetters;
using Antura.Minigames.Maze;
using TMPro;
using UnityEngine;

public class NewMazeLetter : MonoBehaviour
{
    public TextMeshPro[] Texts;
    public GameObject DottedLine;
    public GameObject ContourLine;

    public float ArrowPlacementDelta = 1f;
    public GameObject ArrowPrefabGO;

    public float TutorialPointPlacementDelta = 1f;
    public GameObject TutorialPointPrefabGO;

    public ShapeLetterData shapeData;
    public void SetupLetter(LL_LetterData ld)
    {
        shapeData = AppManager.I.AssetManager.GetShapeLetterData(ld.Data);
        if (shapeData == null)
            return;

        transform.position = new Vector3(-shapeData.Center.x, 0, -shapeData.Center.y + 1);

        //Debug.LogError("Found shape data: " + shapeData);
        ShapeManager.LoadSplinesOn(DottedLine, shapeData.Strokes);
        ShapeManager.LoadSplinesOn(ContourLine, shapeData.Contour);

        foreach (var textMeshPro in Texts)
        {
            textMeshPro.text = ld.TextForLivingLetter;
        }

        var arrowGos = ShapeManager.SpawnObjectsOnSplines(ArrowPrefabGO, transform, shapeData.Strokes, ArrowPlacementDelta, 0.1f, DottedLine.transform.localScale.x, out var tArray);
        for (var i = 0; i < arrowGos.Count; i++)
        {
            var arrowGo = arrowGos[i];
            var arrow = arrowGo.AddComponent<MazeArrow>();
            arrow.splineValue = tArray[i];
            //arrowGo.transform.rotation = Quaternion.LookRotation(arrowGo.transform.forward, Vector3.up);
            //arrowGo.transform.localEulerAngles.SetY(-90f); // Force pitch
            arrowGo.transform.position += Vector3.up * 0.2f;  // Offset
        }

        var tutorialPointGos = ShapeManager.SpawnObjectsOnSplines(TutorialPointPrefabGO, transform, shapeData.Strokes, TutorialPointPlacementDelta, 0.1f, DottedLine.transform.localScale.x, out var _);
        for (var i = 0; i < tutorialPointGos.Count; i++)
        {
            var tutorialPointGo = tutorialPointGos[i];
            tutorialPointGo.transform.position += Vector3.up * 0.2f;  // Offset
        }

    }
}
