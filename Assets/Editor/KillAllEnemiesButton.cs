using TowerSurvivors.Game;
using UnityEditor;
using UnityEngine;

namespace TowerSurvivors
{
    public class KillAllEnemiesButton : EditorWindow
    {
        [MenuItem("Tools/Kill All Enemies Window")]
        public static void ShowWindow()
        {
            KillAllEnemiesButton window = GetWindow<KillAllEnemiesButton>("Kill All Enemies");
            window.minSize = new Vector2(250f, 80f);
        }

        private void OnGUI()
        {
            GUILayout.Label("Are you sure you want to kill all enemies and disable spawning?", EditorStyles.wordWrappedLabel);

            if (GUILayout.Button("Kill 'em all", GUILayout.Height(30)))
            {
                KillAllEnemies();
                Close(); // Close the window after deletion
            }
        }

        private void KillAllEnemies()
        {
            EnemySpawner.Instance.KillAllEnemies();
            Debug.Log("Enemies killed.");
        }
    }
}
