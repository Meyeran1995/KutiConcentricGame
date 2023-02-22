using System;
using Meyham.GameMode;
using UnityEngine;

namespace Meyham.Set_Up
{
    public abstract class AGameStep : MonoBehaviour
    {
        public event Action StepFinished;
            
        public abstract void SeTup();

        public abstract void Link(GameLoop loop);

        public virtual void Activate()
        {
            enabled = true;
        }

        public virtual void Deactivate()
        {
            enabled = false;
            StepFinished?.Invoke();
        }
    }
}