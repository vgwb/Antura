using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class SetupAudioSession : MonoBehaviour {

	void Start() {
#if UNITY_IOS
        // Check that it's actually an iOS device/simulator, not the Unity Player.
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            iOSAudio_setupAudioSession();
        }
#endif
	}

#if UNITY_IOS
    [DllImport ("__Internal")]
    private static extern void iOSAudio_setupAudioSession();
#endif

}