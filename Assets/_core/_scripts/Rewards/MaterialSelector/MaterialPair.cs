using System;
using UnityEngine;

namespace Antura.Rewards
{
    /// <summary>
    /// Contains materials pair needed to set right color of Starndard Rewards.
    /// </summary>
    public struct MaterialPair
    {
        public Material Material1;
        public Material Material2;

        public MaterialPair(string _material1Name, string _material1PaletteType, string _material2Name, string _material2PaletteType)
        {
            Material1 = MaterialManager.LoadMaterial(_material1Name, (PaletteType)Enum.Parse(typeof(PaletteType), _material1PaletteType));
            Material2 = MaterialManager.LoadMaterial(_material2Name, (PaletteType)Enum.Parse(typeof(PaletteType), _material2PaletteType));
        }
    }
}
