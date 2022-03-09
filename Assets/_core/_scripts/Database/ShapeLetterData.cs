using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Antura.Database
{
    [Serializable]
    public class DirectionalSpline : Spline
    {
        public Vector2[] Directions;
    }

    [Serializable]
    public class Stroke
    {
        public DirectionalSpline Spline = new DirectionalSpline();
    }

    [CreateAssetMenu(menuName = "Antura/Shape LetterData")]
    // Additional data partaining to a LetterData and not saved in the Static Database
    public class ShapeLetterData : ScriptableObject
    {
        public Vector2 Center;
        public Vector2[] EmptyZones;
        public Stroke[] Strokes;
        public Stroke[] Contour;
    }
}
