using UnityEngine;

namespace Antura.Utilities
{
    [ExecuteInEditMode]
    public class NineSlicedSprite : MonoBehaviour
    {
        static readonly int[] slicedTriangles = new int[]
        {
            0, 5, 4,
            0, 1, 5,
            1, 6, 5,
            1, 2, 6,
            2, 7, 6,
            2, 3, 7,
            4, 9, 8,
            4, 5, 9,
            5, 10, 9,
            5, 6, 10,
            6, 11, 10,
            6, 7, 11,
            8, 13, 12,
            8, 9, 13,
            9, 14, 13,
            9, 10, 14,
            10, 15, 14,
            10, 11, 15,
        };

        bool dirty = true;
        Mesh mesh;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        public float initialWidth;
        private float width = 1;

        public float Width
        {
            get { return width; }
            set
            {
                if (width == value)
                {
                    return;
                }

                width = value;
                dirty = true;
            }
        }

        public float initialHeight;
        private float height = 1.0f;

        public float Height
        {
            get { return height; }
            set
            {
                if (height == value)
                {
                    return;
                }

                height = value;
                dirty = true;
            }
        }

        public float initialBorderScale = 1;
        private float borderScale = 1.0f;

        public float BorderScale
        {
            get { return borderScale; }
            set
            {
                if (borderScale == value)
                {
                    return;
                }

                borderScale = value;
                dirty = true;
            }
        }

        public Sprite initialSprite;
        private Sprite sprite;

        public Sprite Sprite
        {
            get { return sprite; }
            set
            {
                if (sprite == value)
                {
                    return;
                }

                sprite = value;
                dirty = true;
            }
        }

        public Material initialMaterial;
        Material material;
        Material originalMaterial;

        public Material Material
        {
            get
            {
                if (material == null)
                {
                    meshRenderer.sharedMaterial = material = new Material(initialMaterial);
                }

                return material;
            }
            set
            {
                if (originalMaterial == value)
                {
                    return;
                }

                originalMaterial = value;
                meshRenderer.sharedMaterial = material = value == null ? null : new Material(value);
                if (material != null)
                {
                    material.SetTexture("_MainTex", sprite.texture);
                }
            }
        }

        void OnEnable()
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }
        }

        void OnDisable()
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }

        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();

            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
            meshFilter.hideFlags = HideFlags.HideAndDontSave;
            meshRenderer.hideFlags = HideFlags.HideAndDontSave;

            dirty = true;
            Sprite = initialSprite;
            Width = initialWidth;
            Height = initialHeight;
            BorderScale = initialBorderScale;
            Material = initialMaterial;
            CreateSlicedMesh();
        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                Sprite = initialSprite;
                Width = initialWidth;
                Height = initialHeight;
                BorderScale = initialBorderScale;
                Material = initialMaterial;
            }

            if (dirty)
            {
                CreateSlicedMesh();
            }
        }

        void CreateSlicedMesh()
        {
            dirty = false;
            if (mesh != null)
            {
                if (Application.isPlaying)
                {
                    GameObject.Destroy(mesh);
                }
                else
                {
                    GameObject.DestroyImmediate(mesh);
                }
                mesh = null;
            }

            if (sprite == null)
            {
                return;
            }

            mesh = new Mesh();

            float marginLeft = sprite.border.x / sprite.rect.size.x;
            float marginBottom = sprite.border.y / sprite.rect.size.y;
            float marginRight = sprite.border.z / sprite.rect.size.x;
            float marginTop = sprite.border.w / sprite.rect.size.y;

            float borderLeft = marginLeft * BorderScale;
            float borderBottom = marginBottom * BorderScale;
            float borderRight = marginRight * BorderScale;
            float borderTop = marginTop * BorderScale;

            Vector3 centerOffset = new Vector3(width * 0.5f, height * 0.5f, 0);

            mesh.vertices = new Vector3[]
            {
                -centerOffset + new Vector3(0, 0, 0),
                -centerOffset + new Vector3(borderLeft, 0, 0),
                -centerOffset + new Vector3(width - borderRight, 0, 0),
                -centerOffset + new Vector3(width, 0, 0),

                -centerOffset + new Vector3(0, borderBottom, 0),
                -centerOffset + new Vector3(borderLeft, borderBottom, 0),
                -centerOffset + new Vector3(width - borderRight, borderBottom, 0),
                -centerOffset + new Vector3(width, borderBottom, 0),

                -centerOffset + new Vector3(0, height - borderTop, 0),
                -centerOffset + new Vector3(borderLeft, height - borderTop, 0),
                -centerOffset + new Vector3(width - borderRight, height - borderTop, 0),
                -centerOffset + new Vector3(width, height - borderTop, 0),

                -centerOffset + new Vector3(0, height, 0),
                -centerOffset + new Vector3(borderLeft, height, 0),
                -centerOffset + new Vector3(width - borderRight, height, 0),
                -centerOffset + new Vector3(width, height, 0)
            };


            mesh.uv = new Vector2[]
            {
                new Vector2(0, 0), new Vector2(marginLeft, 0), new Vector2(1 - marginRight, 0), new Vector2(1, 0),
                new Vector2(0, marginBottom), new Vector2(marginLeft, marginBottom),
                new Vector2(1 - marginRight, marginBottom), new Vector2(1, marginBottom),
                new Vector2(0, 1 - marginTop), new Vector2(marginLeft, 1 - marginTop),
                new Vector2(1 - marginRight, 1 - marginTop), new Vector2(1, 1 - marginTop),
                new Vector2(0, 1), new Vector2(marginLeft, 1), new Vector2(1 - marginRight, 1), new Vector2(1, 1)
            };

            mesh.triangles = slicedTriangles;

            GetComponent<MeshFilter>().mesh = mesh;

            if (material != null)
            {
                material.SetTexture("_MainTex", sprite.texture);
            }
        }
    }
}
