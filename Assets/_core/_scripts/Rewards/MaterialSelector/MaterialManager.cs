using Antura.Dog;
using UnityEngine;

namespace Antura.Rewards
{
    public class MaterialManager
    {
        public const string MATERIALS_RESOURCES_PATH = "Materials/Palettes/";
        public const string TEXTURES_MATERIALS = "Textures_and_Materials/";

        public static Material LoadMaterial(PaletteColors _color, PaletteTone _tone, PaletteType _type = PaletteType.diffuse_saturated)
        {
            return LoadMaterial(string.Format("{0}_{1}", _color.ToString(), _tone.ToString()), _type);
        }

        public static Material LoadMaterial(string _materialID, PaletteType _type = PaletteType.diffuse_saturated)
        {
            Material mat = Resources.Load<Material>(string.Format("{0}{1}",
                string.Format("{0}{1}/", MATERIALS_RESOURCES_PATH, _type.ToString()), _materialID));
            if (mat == null)
            {
                mat = Resources.Load<Material>(string.Format("{0}{1}_{2}", MATERIALS_RESOURCES_PATH, "white", "pure"));
                //Debug.LogFormat("Material not found {0}_{1} in path {2}", _color, _tone, MATERIALS_REOURCES_PATH);
            }
            return mat;
        }


        public static Material LoadTextureMaterial(AnturaPetType petType, string _materialID, string _variationId)
        {
            string materialName = $"{petType}/{TEXTURES_MATERIALS}{_materialID}_{_variationId}";
            Material mat = Resources.Load<Material>(materialName);
            return mat;
        }
    }
}
