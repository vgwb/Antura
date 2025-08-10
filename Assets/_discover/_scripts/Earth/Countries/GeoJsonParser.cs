using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Antura.Discover
{
    public class GeoJsonParser : MonoBehaviour
    {
        public TextAsset geoJsonFile;
        public Material countryMaterial;
        public GameObject countriesParent; // Public property for the parent GameObject (e.g., Earth)
        public float radius = 100f; // Radius of the sphere

        void Start()
        {
            if (geoJsonFile == null)
            {
                Debug.LogError("GeoJSON file is not assigned.");
                return;
            }

            if (countriesParent == null)
            {
                Debug.LogError("Countries parent GameObject is not assigned.");
                return;
            }

            // Parse the GeoJSON file
            JObject geoJson = JObject.Parse(geoJsonFile.text);

            // Process each feature in the GeoJSON
            foreach (var feature in geoJson["features"])
            {
                var properties = feature["properties"];
                var geometry = feature["geometry"];
                string geometryType = geometry["type"].ToString();

                if (geometryType == "Polygon")
                {
                    DrawPolygon(geometry["coordinates"], properties);
                }
                else if (geometryType == "MultiPolygon")
                {
                    foreach (var polygon in geometry["coordinates"])
                    {
                        DrawPolygon(polygon, properties);
                    }
                }
            }
        }

        void DrawPolygon(JToken coordinates, JToken properties)
        {
            foreach (var ring in coordinates)
            {
                List<Vector3> vertices = new List<Vector3>();
                foreach (var point in ring)
                {
                    if (point is JArray pointArray && pointArray.Count >= 2)
                    {
                        float lon = (float)pointArray[0];
                        float lat = (float)pointArray[1];
                        vertices.Add(ConvertToSphereCoordinates(lon, lat));
                    }
                }
                CreateMesh(vertices.ToArray(), properties);
            }
        }

        Vector3 ConvertToSphereCoordinates(float lon, float lat)
        {
            // Convert latitude and longitude to radians
            float latRad = Mathf.Deg2Rad * lat;
            float lonRad = Mathf.Deg2Rad * lon;

            // Calculate the 3D coordinates
            float x = radius * Mathf.Cos(latRad) * Mathf.Cos(lonRad);
            float y = radius * Mathf.Sin(latRad);
            float z = radius * Mathf.Cos(latRad) * Mathf.Sin(lonRad);

            return new Vector3(x, y, z);
        }

        void CreateMesh(Vector3[] vertices, JToken properties)
        {
            // Get the country name from properties
            string countryName = properties["name"].ToString();

            // Create a new GameObject for the country
            GameObject country = new GameObject(countryName);
            country.transform.parent = countriesParent.transform; // Set the parent to the assigned GameObject (e.g., Earth)

            // Add MeshFilter and MeshRenderer components
            MeshFilter meshFilter = country.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = country.AddComponent<MeshRenderer>();
            meshRenderer.material = countryMaterial;

            // Create and assign the mesh
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;

            // Create triangles (assumes the vertices are in correct winding order)
            int[] triangles = new int[(vertices.Length - 2) * 3];
            for (int i = 0; i < vertices.Length - 2; i++)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
            mesh.triangles = triangles;

            // Recalculate normals for proper lighting
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;

            // Attach the CountryData component and set properties
            CountryDataInfo countryData = country.AddComponent<CountryDataInfo>();
            countryData.countryName = countryName;
        }
    }
}
