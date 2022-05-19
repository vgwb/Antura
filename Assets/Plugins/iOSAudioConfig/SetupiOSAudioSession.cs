using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class SetupiOSAudioSession : MonoBehaviour
{
    void Start()
    {
#if UNITY_IOS
	    // we use this to configure this app audio to override the native "silent mode"
        // Check that it's actually an iOS device/simulator, not the Unity Player.
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            iOSAudio_setupAudioSessionCategory();
        }
#endif
    }

#if UNITY_IOS
    [DllImport ("__Internal")]
    private static extern void iOSAudio_setupAudioSessionCategory();
#endif

}
