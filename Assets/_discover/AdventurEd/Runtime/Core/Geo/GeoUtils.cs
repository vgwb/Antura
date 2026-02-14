using UnityEngine;

namespace Discover
{
    public static class GeoUtils
    {
        // WGS84 (World Geodetic System 1984) Earth radius in meters
        private const int EarthRadius = 6378137;

        public struct GeoCoordinate
        {
            // we use double because we need at least 15 decimal digits of precision
            // to represent coordinates with sub-meter accuracy
            public double Latitude;   // degrees
            public double Longitude;  // degrees

            public GeoCoordinate(double lat, double lon)
            {
                Latitude = lat;
                Longitude = lon;
            }
        }

        #region Mercator Projection
        public static Vector2 LatLonToMeters(GeoCoordinate coord)
        {
            double x = EarthRadius * Mathf.Deg2Rad * coord.Longitude;
            double y = EarthRadius *
                       System.Math.Log(
                           System.Math.Tan(
                               (System.Math.PI / 4.0) +
                               (Mathf.Deg2Rad * (float)coord.Latitude / 2.0)
                           )
                       );

            return new Vector2((float)x, (float)y);
        }

        public static GeoCoordinate MetersToLatLon(Vector2 meters)
        {
            double lon = (meters.x / EarthRadius) * Mathf.Rad2Deg;
            double lat = (2 * System.Math.Atan(System.Math.Exp(meters.y / EarthRadius)) - System.Math.PI / 2) * Mathf.Rad2Deg;

            return new GeoCoordinate(lat, lon);
        }

        public static Vector3 GeoToWorldPosition(
            GeoCoordinate target,
            GeoCoordinate origin,
            float unityScale = 1f
        )
        {
            Vector2 originMeters = LatLonToMeters(origin);
            Vector2 targetMeters = LatLonToMeters(target);

            Vector2 delta = targetMeters - originMeters;

            return new Vector3(
                delta.x * unityScale,
                0f,
                delta.y * unityScale
            );
        }
    }
    #endregion
}
