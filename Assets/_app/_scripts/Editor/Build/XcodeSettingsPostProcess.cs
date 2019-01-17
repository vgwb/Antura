#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.IO;

public class XcodeSettingsPostProcess
{
    [PostProcessBuildAttribute(0)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        // Stop processing if targe is NOT iOS
        if (buildTarget != BuildTarget.iOS)
            return;

        // Initialize PbxProject
        var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);
        string targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");

        // Sample of adding build property
        //pbxProject.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-all_load");

        // Sample of setting build property
        pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");

        // Sample of update build property
        //pbxProject.UpdateBuildProperty(targetGuid, "OTHER_LDFLAGS", new string[] { "-ObjC" }, new string[] { "-weak_framework" });

        // Sample of adding REQUIRED framwrok
        //pbxProject.AddFrameworkToProject(targetGuid, "Security.framework", false);

        // Sample of adding OPTIONAL framework
        //pbxProject.AddFrameworkToProject(targetGuid, "SafariServices.framework", true);

        // Sample of setting compile flags
        //var guid = pbxProject.FindFileGuidByProjectPath("Classes/UI/Keyboard.mm");
        //var flags = pbxProject.GetCompileFlagsForFile(targetGuid, guid);
        //flags.Add("-fno-objc-arc");
        //pbxProject.SetCompileFlagsForFile(targetGuid, guid, flags);

        // Apply settings
        File.WriteAllText(projectPath, pbxProject.WriteToString());

        // Samlpe of editing Info.plist
        var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // Add string setting
        //plist.root.SetString("hogehogeId", "dummyid");

        // enable file sharing through iTunes
        plist.root.SetBoolean("UIFileSharingEnabled", true);

        // authorize the saving screenshots into Camera Roll
        plist.root.SetString("NSPhotoLibraryUsageDescription", "Save Antura photos");
        // this is new for iOS 11
        plist.root.SetString("NSPhotoLibraryAddUsageDescription", "Save Antura photos");

        // Add URL Scheme
        //var array = plist.root.CreateArray("CFBundleURLTypes");
        //var urlDict = array.AddDict();
        //urlDict.SetString("CFBundleURLName", "hogehogeName");
        //var urlInnerArray = urlDict.CreateArray("CFBundleURLSchemes");
        //urlInnerArray.AddString("hogehogeValue");

        // Apply editing settings to Info.plist
        plist.WriteToFile(plistPath);
    }
}
#endif