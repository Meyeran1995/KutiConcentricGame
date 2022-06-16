using Meyham.Events;
using UnityEditor;
using UnityEngine;

namespace Meyham.Editor
{
    [CustomEditor(typeof(BoolEventChannelSO))]
    public class BoolEventChannelEditor : UnityEditor.Editor 
    {
        public override void OnInspectorGUI() 
        {
            DrawDefaultInspector();
        
            var channel = (BoolEventChannelSO)target;

            if (!channel.HasListeners) return;
        
            if(GUILayout.Button("Raise True")) 
                channel.InvokeTrue();
        
            if(GUILayout.Button("Raise False")) 
                channel.InvokeFalse();
        }
    }
}