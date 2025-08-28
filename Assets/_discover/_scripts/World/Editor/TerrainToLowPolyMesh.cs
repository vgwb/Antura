using UnityEngine;
using UnityEditor;

namespace Antura.Discover.Editor
{
    /// <summary>
    /// Exports a Unity Terrain as a low-poly mesh with flat shading + baked vertex colors.
    /// </summary>
    public class TerrainToLowPolyMesh : MonoBehaviour
    {
        [MenuItem("Antura/Discover/Export Terrain To Low-Poly Mesh (Vertex Colors)")]
        static void Export()
        {
            var terrain = Selection.activeGameObject ? Selection.activeGameObject.GetComponent<Terrain>() : null;
            if (!terrain)
            { Debug.LogError("Select a Terrain GameObject first."); return; }

            var td = terrain.terrainData;
            int res = 128; // grid resolution (lower = chunkier, lighter)
            float width = td.size.x;
            float height = td.size.y;
            float length = td.size.z;

            // Sample heights + splatmap colors
            var verts = new Vector3[(res + 1) * (res + 1)];
            var colors = new Color[(res + 1) * (res + 1)];

            for (int z = 0; z <= res; z++)
                for (int x = 0; x <= res; x++)
                {
                    float u = x / (float)res;
                    float v = z / (float)res;

                    float h = td.GetInterpolatedHeight(u, v);
                    verts[z * (res + 1) + x] = new Vector3(u * width, h, v * length);

                    // Get splatmap color
                    var splat = td.GetAlphamaps(Mathf.RoundToInt(u * td.alphamapWidth),
                                                Mathf.RoundToInt(v * td.alphamapHeight), 1, 1);

                    // Mix textures into a color
                    Color c = Color.black;
                    for (int i = 0; i < td.alphamapLayers; i++)
                    {
                        if (i < td.terrainLayers.Length)
                        {
                            var layer = td.terrainLayers[i];
                            if (layer != null)
                                c += layer.diffuseTexture ? layer.diffuseTexture.GetPixelBilinear(u, v) * splat[0, 0, i]
                                                          : layer.diffuseRemapMin * splat[0, 0, i];
                        }
                    }
                    if (c == Color.black)
                        c = Color.green; // fallback
                    colors[z * (res + 1) + x] = c;
                }

            // Build triangles
            var tris = new int[res * res * 6];
            int t = 0;
            for (int z = 0; z < res; z++)
                for (int x = 0; x < res; x++)
                {
                    int i0 = z * (res + 1) + x;
                    int i1 = i0 + 1;
                    int i2 = i0 + (res + 1);
                    int i3 = i2 + 1;

                    tris[t++] = i0;
                    tris[t++] = i2;
                    tris[t++] = i1;
                    tris[t++] = i1;
                    tris[t++] = i2;
                    tris[t++] = i3;
                }

            // Split vertices per triangle for flat shading
            var flatVerts = new Vector3[tris.Length];
            var flatColors = new Color[tris.Length];
            var flatTris = new int[tris.Length];
            for (int i = 0; i < tris.Length; i++)
            {
                flatVerts[i] = verts[tris[i]];
                flatColors[i] = colors[tris[i]];
                flatTris[i] = i;
            }

            var mesh = new Mesh();
            mesh.indexFormat = (flatVerts.Length > 65000) ?
                UnityEngine.Rendering.IndexFormat.UInt32 :
                UnityEngine.Rendering.IndexFormat.UInt16;
            mesh.vertices = flatVerts;
            mesh.colors = flatColors;
            mesh.triangles = flatTris;
            mesh.RecalculateNormals(); // normals are flat because of split vertices
            mesh.RecalculateBounds();

            // Save asset
            var path = EditorUtility.SaveFilePanelInProject("Save Low-Poly Terrain Mesh", "LowPolyTerrain", "asset", "");
            if (string.IsNullOrEmpty(path))
                return;
            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();

            // Create GameObject
            var go = new GameObject("LowPolyTerrain");
            var mf = go.AddComponent<MeshFilter>();
            var mr = go.AddComponent<MeshRenderer>();
            mf.sharedMesh = mesh;
            mr.sharedMaterial = new Material(Shader.Find("Unlit/Color"));

            Debug.Log("Low-poly mesh exported with vertex colors: " + path);
        }
    }
}
