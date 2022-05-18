#import <AVFoundation/AVFoundation.h>
 
void iOSAudio_setupAudioSession()
{
	// see http://stackoverflow.com/questions/21682502/audiosessionsetproperty-deprecated-in-ios-7-0-so-how-set-kaudiosessionproperty-o
	// and http://forum.unity3d.com/threads/setting-mediaplayback-audio-session-category.206958/

	/* OLD CODE
    // We want to make sure all audio stops from other apps, and we want to
    // ignore the ringer/mute switch.
    UInt32 sessionCategory = kAudioSessionCategory_MediaPlayback;
    OSStatus result = AudioSessionSetProperty(kAudioSessionProperty_AudioCategory, sizeof(sessionCategory), &sessionCategory);
   
    result = AudioSessionSetActive(YES);
    // Can check result if you want...
	*/
	
	/* NEW CODE */
	AVAudioSession *session = [AVAudioSession sharedInstance];

	NSError *setCategoryError = nil;
	if (![session setCategory:AVAudioSessionCategoryPlayback
			 withOptions:AVAudioSessionCategoryOptionMixWithOthers
			 error:&setCategoryError]) {
		// handle error
	}
}