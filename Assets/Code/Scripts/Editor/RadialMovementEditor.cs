using Meyham.Player;
using UnityEditor;
using UnityEngine;

namespace Meyham.Editor
{
    [CustomEditor(typeof(RadialPlayerMovement)), CanEditMultipleObjects]
    public class RadialMovementEditor : UnityEditor.Editor
    {
        private bool deleteToggle;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (targets.Length > 1)
            {
                MultiInspector();
            }
            else
            {
                SingleInspector();
            }
        }

        private void SingleInspector()
        {
            var movement = (RadialPlayerMovement)target;
            
            if (GUILayout.Button("Snap to Starting Position"))
            {
                Undo.SetCurrentGroupName("Snapped to Starting position");
                int group = Undo.GetCurrentGroup();
                
                movement.EditorSnapToStartingPosition();
                
                Undo.CollapseUndoOperations(group);
            }
        }
        
        private void MultiInspector()
        {
            var movements = new RadialPlayerMovement[targets.Length];

            for (int i = 0; i < movements.Length; i++)
            {
                movements[i] = (RadialPlayerMovement)targets[i];
            }

            if (GUILayout.Button("Snap to Starting Position"))
            {
                Undo.SetCurrentGroupName("Snapped to Starting Position");
                int group = Undo.GetCurrentGroup();
                
                foreach (var corrector in movements)
                {
                    corrector.EditorSnapToStartingPosition();
                }
                
                Undo.CollapseUndoOperations(group);
            }
        }
    }
}