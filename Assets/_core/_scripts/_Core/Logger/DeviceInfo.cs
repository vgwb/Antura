using UnityEngine;

namespace Antura.Core
{
    /// <summary>
    /// the DeviceInfo class is used to collect all technical details to be included in any debug report.
    /// this class can be easily encoded into JSON by JsonUtility.ToJson(new DeviceInfo())
    /// </summary>
    public class DeviceInfo
    {
        public string AppVersion;
        public string platform;
        public string systemLanguage;
        public string internetReachability;

        public string operatingSystem;
        public string operatingSystemFamily;
        public string deviceModel;
        public string deviceName;
        public string deviceType;
        public int systemMemorySize;

        public int screenWidth;
        public int screenHeight;
        public int screenDpi;

        public int graphicsDeviceID;
        public string graphicsDeviceName;
        public string graphicsDeviceType;
        public string graphicsDeviceVendor;
        public int graphicsDeviceVendorID;
        public string graphicsDeviceVersion;
        public int graphicsMemorySize;
        public bool graphicsMultiThreaded;
        public int graphicsShaderLevel;

        public bool supportsGyroscope;
        public bool supportsVibration;
        public bool supportsAccelerometer;
        public bool supportsLocationService;
        public bool supportsARGB32RenderTexture;
        public bool supportsAlpha8Texture;

        public DeviceInfo()
        {
            AppVersion = AppManager.I.AppEdition.AppVersion.ToString();
            platform = Application.platform.ToString();
            systemLanguage = Application.systemLanguage.ToString();
            internetReachability = Application.internetReachability.ToString();

            operatingSystem = SystemInfo.operatingSystem;
            operatingSystemFamily = SystemInfo.operatingSystemFamily.ToString();
            deviceModel = SystemInfo.deviceModel;
            deviceName = SystemInfo.deviceName;
            deviceType = SystemInfo.deviceType.ToString();
            systemMemorySize = SystemInfo.systemMemorySize;

            screenWidth = Screen.width;
            screenHeight = Screen.height;
            screenDpi = Mathf.RoundToInt(Screen.dpi);

            graphicsDeviceID = SystemInfo.graphicsDeviceID;
            graphicsDeviceName = SystemInfo.graphicsDeviceName;
            graphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();
            graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;
            graphicsDeviceVendorID = SystemInfo.graphicsDeviceVendorID;
            graphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;
            graphicsMemorySize = SystemInfo.graphicsMemorySize;
            graphicsMultiThreaded = SystemInfo.graphicsMultiThreaded;
            graphicsShaderLevel = SystemInfo.graphicsShaderLevel;

            supportsGyroscope = SystemInfo.supportsGyroscope;
            supportsVibration = SystemInfo.supportsVibration;
            supportsAccelerometer = SystemInfo.supportsAccelerometer;
            supportsLocationService = SystemInfo.supportsLocationService;
            supportsARGB32RenderTexture = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32);
            supportsAlpha8Texture = SystemInfo.SupportsTextureFormat(TextureFormat.Alpha8);
        }

        public string ToJsonData()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
