using TowerSurvivors;
using UnityEditor;
using UnityEngine;

public class DeleteSaveWindow : EditorWindow
{
    [MenuItem("Tools/Delete Save Data")]
    public static void ShowWindow()
    {
        DeleteSaveWindow window = GetWindow<DeleteSaveWindow>("Delete Save Data");
        window.minSize = new Vector2(250f, 80f);
    }

    private void OnGUI()
    {
        GUILayout.Label("Are you sure you want to delete the current Save Data?", EditorStyles.wordWrappedLabel);

        if (GUILayout.Button("Delete Save Data", GUILayout.Height(30)))
        {
            DeleteAllPlayerPrefs();
            Close(); // Close the window after deletion
        }
    }

    private void DeleteAllPlayerPrefs()
    {
        SaveSystem.DeleteSave();
    }
}