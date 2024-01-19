using System;
using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.U2D;

namespace Meyham.Player
{
    public class PlayerBodyPart : MonoBehaviour
    {
        [SerializeField] private SpriteShapeController body;
        
        private Spline spline;
        
        //collisionen checken
            // vorwärts -> kann man skippen, wenn nicht kopf
            // unten -> kann man skippen wenn order == 0
        //hat eigene order
            // muss basierend auf der order lokale position der spline punkte animieren
        //kennt base settings für reset/pooling
        
        private void Start()
        {
            spline = body.spline;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            body = GetComponent<SpriteShapeController>();
        }

#endif
    }
}