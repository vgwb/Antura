using System.Collections;
using System.Collections.Generic;
using Antura.Core;
using Antura.LivingLetters;
using TMPro;
using UnityEngine;

public class NewMazeLetter : MonoBehaviour
{
    public TextMeshPro[] Texts;
    public GameObject DottedLine;
    public GameObject ContourLine;

    public void SetupLetter(LL_LetterData ld)
    {
        var shapeData = AppManager.I.AssetManager.GetSideLetterData(ld.Id);
        Debug.LogError("Found shape data: " + shapeData);
        ShapeManager.LoadSplinesOn(DottedLine, shapeData.Strokes);
        ShapeManager.LoadSplinesOn(ContourLine, shapeData.Contour);

        foreach (var textMeshPro in Texts)
        {
            textMeshPro.text = ld.TextForLivingLetter;
        }
    }
}
