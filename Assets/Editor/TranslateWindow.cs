using TowerSurvivors.Localisation;
using UnityEditor;
using UnityEngine;

public class TranslateWindow : EditorWindow
{
    private string inputText = "";
    private string translatedText = "";

    [MenuItem("Tools/Translation Window")]
    public static void ShowWindow()
    {
        GetWindow<TranslateWindow>("Translation Window");
    }

    private void OnGUI()
    {
        GUILayout.Label("Enter Text to Translate:", EditorStyles.boldLabel);

        // Input text box
        inputText = EditorGUILayout.TextField("Input Text:", inputText);

        // Translate button
        if (GUILayout.Button("Translate"))
        {
            Translate();
        }

        // Display translated text
        GUILayout.Label("Translated Text:", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(translatedText, GUILayout.Height(50));
    }

    private void Translate()
    {
        // Replace this with your actual translation logic
        translatedText = Language.Get(inputText.ToUpper());
    }
}