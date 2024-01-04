using UnityEditor;
using UnityEngine;

public class PlayerPrefsEditorWindow : EditorWindow
{
    [MenuItem("Tools/Delete All PlayerPrefs")]
    public static void ShowWindow()
    {
        PlayerPrefsEditorWindow window = GetWindow<PlayerPrefsEditorWindow>("Delete PlayerPrefs");
        window.minSize = new Vector2(250f, 80f);
    }

    private void OnGUI()
    {
        GUILayout.Label("Are you sure you want to delete all PlayerPrefs?", EditorStyles.wordWrappedLabel);

        if (GUILayout.Button("Delete PlayerPrefs", GUILayout.Height(30)))
        {
            DeleteAllPlayerPrefs();
            Close(); // Close the window after deletion
        }
    }

    private void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All PlayerPrefs deleted.");
    }
}