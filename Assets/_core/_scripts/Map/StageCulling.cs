using UnityEngine;

public class StageCulling : MonoBehaviour
{
    public Camera cullingCamera;

    [System.Serializable]
    public class StageGroup
    {
        public Transform Center;
        public Vector3 Offset;
        public Vector3 Size;
    }

    public StageGroup[] Stages;

    bool initialized = false;
    Matrix4x4 oldMat;
    Plane[] planes = new Plane[6];

    private void Start()
    {
        Update();
    }

    void Update()
    {
        if (!initialized) {
            // Force visibility check
            cullingCamera.Render();
            initialized = true;
        }

        //planes = GeometryUtility.CalculateFrustumPlanes(cullingCamera);
        // Calculate planes manually in order to not create garbage per frame
        Matrix4x4 mat = cullingCamera.projectionMatrix * cullingCamera.worldToCameraMatrix;

        Vector3 n0 = new Vector3(mat.m30 + mat.m00, mat.m31 + mat.m01, mat.m32 + mat.m02); float d0 = mat.m33 + mat.m03; float m0 = n0.magnitude;
        Vector3 n1 = new Vector3(mat.m30 - mat.m00, mat.m31 - mat.m01, mat.m32 - mat.m02); float d1 = mat.m33 - mat.m03; float m1 = n1.magnitude;
        Vector3 n2 = new Vector3(mat.m30 + mat.m10, mat.m31 + mat.m11, mat.m32 + mat.m12); float d2 = mat.m33 + mat.m13; float m2 = n2.magnitude;
        Vector3 n3 = new Vector3(mat.m30 - mat.m10, mat.m31 - mat.m11, mat.m32 - mat.m12); float d3 = mat.m33 - mat.m13; float m3 = n3.magnitude;
        Vector3 n4 = new Vector3(mat.m30 + mat.m20, mat.m31 + mat.m21, mat.m32 + mat.m22); float d4 = mat.m33 + mat.m23; float m4 = n4.magnitude;
        Vector3 n5 = new Vector3(mat.m30 - mat.m20, mat.m31 - mat.m21, mat.m32 - mat.m22); float d5 = mat.m33 - mat.m23; float m5 = n5.magnitude;

        planes[0] = new Plane(n0 / m0, d0 / m0);
        planes[1] = new Plane(n1 / m1, d1 / m1);
        planes[2] = new Plane(n2 / m2, d2 / m2);
        planes[3] = new Plane(n3 / m3, d3 / m3);
        planes[4] = new Plane(n4 / m4, d4 / m4);
        planes[5] = new Plane(n5 / m5, d5 / m5);

        for (int i = 0; i < Stages.Length; ++i) {
            var s = Stages[i];

            bool visible = IsVisible(s.Center.position + s.Offset * transform.localScale.x, s.Size, planes);
            s.Center.gameObject.SetActive(visible);
        }
    }

    private bool IsVisible(Vector3 center, Vector3 size, Plane[] planes)
    {
        return GeometryUtility.TestPlanesAABB(planes, new Bounds(center, size));
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < Stages.Length; ++i) {
            var s = Stages[i];
            Gizmos.color = new Color(1, 1, 1, 0.5f);
            Gizmos.DrawCube(s.Center.position + s.Offset * transform.localScale.x, s.Size);
        }
    }
}
