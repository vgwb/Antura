using DG.DeExtensions;
using UnityEngine;

namespace Antura.Helpers
{
    public static class CameraHelper
    {
        public static void FitShopDecorationToUICamera(Transform _trans, Camera _cam, float scaleMultiplier, Vector3 eulerOffset)
        {
            _trans.localPosition = Vector3.zero;
            _trans.eulerAngles = eulerOffset;
            Bounds bounds = new Bounds(Vector3.zero, new Vector3(-1, -1, -1));
            Renderer[] rs = _trans.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer r in rs)
            {
                if (!r.gameObject.activeSelf)
                {
                    continue;
                }
                if (bounds.size.x < 0)
                {
                    bounds = r.bounds;
                }
                else
                {
                    bounds.Encapsulate(r.bounds);
                }
            }
            Vector3 diff = new Vector3(_trans.position.x - bounds.center.x, _trans.position.y - bounds.center.y,
                _trans.position.z - bounds.center.z);
            _trans.Translate(diff, Space.World);
            float frustumVal = bounds.size.y >= bounds.size.x ? bounds.size.y : bounds.size.x / _cam.aspect;
            float requiredDistance = frustumVal * 0.65f / Mathf.Tan(_cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

            requiredDistance /= scaleMultiplier;

            _trans.SetZ(_cam.transform.position.z + requiredDistance);
        }

        public static void FitRewardToUICamera(Transform _trans, Camera _cam, bool _flip = false)
        {
            _trans.localPosition = Vector3.zero;
            if (_flip)
            {
                _trans.eulerAngles = new Vector3(-45, 60, 150);
            }
            Bounds bounds = new Bounds(Vector3.zero, new Vector3(-1, -1, -1));
            Renderer[] rs = _trans.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer r in rs)
            {
                if (!r.gameObject.activeSelf)
                {
                    continue;
                }
                if (bounds.size.x < 0)
                {
                    bounds = r.bounds;
                }
                else
                {
                    bounds.Encapsulate(r.bounds);
                }
            }
            Vector3 diff = new Vector3(_trans.position.x - bounds.center.x, _trans.position.y - bounds.center.y,
                _trans.position.z - bounds.center.z);
            _trans.Translate(diff, Space.World);
            float frustumVal = bounds.size.y >= bounds.size.x ? bounds.size.y : bounds.size.x / _cam.aspect;
            float requiredDistance = frustumVal * 0.65f / Mathf.Tan(_cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            _trans.SetZ(_cam.transform.position.z + requiredDistance);
        }
    }
}
