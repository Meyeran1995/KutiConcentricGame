using Meyham.Events;
using UnityEngine;

namespace Meyham.GameMode
{
    public abstract class AGameLoopSystem : MonoBehaviour
    {
        [Header("Loop Events")]
        [SerializeField] private VoidEventChannelSO gameStartEvent;
        [SerializeField] private VoidEventChannelSO gameEndEvent, gameRestartEvent;
        
        protected virtual void Start()
        {
            gameStartEvent += OnGameStart;
            gameEndEvent += OnGameEnd;
            gameRestartEvent += OnGameRestart;
        }
        
        protected abstract void OnGameStart();
        protected abstract void OnGameEnd();
        protected abstract void OnGameRestart();
    }
}