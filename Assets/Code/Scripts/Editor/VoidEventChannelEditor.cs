using Meyham.Events;
using UnityEditor;
using UnityEngine;

namespace Meyham.Editor
{
    [CustomEditor(typeof(VoidEventChannelSO))]
    public class VoidEventChannelEditor : UnityEditor.Editor 
    {
        public override void OnInspectorGUI() 
        {
            DrawDefaultInspector();
        
            var channel = (VoidEventChannelSO)target;

            if (!channel.HasListeners) return;
        
            if(GUILayout.Button("Raise Event")) 
                channel.RaiseEvent();
        }
    }
}