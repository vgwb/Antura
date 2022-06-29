using Antura.Environment;
using UnityEditor;

public class WorldPrefabsEditorWindow : EditorWindow
{
    WorldID world = WorldID.Default;
    WorldID lastWorld = WorldID.Default;

    [MenuItem("Antura/Utility/ TestWorld Prefabs", false)]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        WorldPrefabsEditorWindow window = (WorldPrefabsEditorWindow)EditorWindow.GetWindow(typeof(WorldPrefabsEditorWindow));
        window.Show();
    }

    void OnGUI()
    {
        world = (WorldID)EditorGUILayout.EnumPopup(world);

        if (world != lastWorld)
        {
            var prefabs = FindObjectsOfType<AutoWorldPrefab>();
            var cameras = FindObjectsOfType<AutoWorldCameraColor>();

            foreach (var p in prefabs)
            {
                p.testWorld = world;
                p.SendMessage("Update");
            }

            foreach (var c in cameras)
            {
                c.testWorld = world;
                c.SendMessage("Update");
            }
        }
        lastWorld = world;
    }
}
