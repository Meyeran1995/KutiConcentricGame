using Meyham.GameMode;
using UnityEditor;
using UnityEngine;

namespace Meyham.Editor
{
    [CustomEditor(typeof(WaveManager))]
    public class WaveManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if(!Application.isPlaying) return;
            
            var manager = (WaveManager)target;
            
            if (GUILayout.Button("Spawn Next Wave"))
            {
                manager.EditorSpawnWave();
            }
        }
    }
}