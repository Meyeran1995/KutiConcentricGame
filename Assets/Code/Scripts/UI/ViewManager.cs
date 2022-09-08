using Meyham.EditorHelpers;
using Meyham.GameMode;
using UnityEngine;

namespace Meyham.UI
{
    public class ViewManager : AGameLoopSystem
    {
        [Header("References")]
        [SerializeField] private AGameView[] gameViews;
        [Header("Debug")]
        [ReadOnly, SerializeField] private int currentView;

        private static readonly int ViewIsOpenId = Animator.StringToHash("IsOpen");
        
        
        private void Awake()
        {
            currentView = 0;
        }

        protected override void Start()
        {
            base.Start();
            OpenCurrentView();
        }

        protected override void OnGameStart()
        {
            CloseCurrentView();
            currentView = 1;
        }

        protected override void OnGameEnd()
        {
            currentView = 2;
            OpenCurrentView();
        }

        protected override void OnGameRestart()
        {
            CloseCurrentView();
            currentView = 1;
            OpenCurrentView();
        }

        private void OpenCurrentView() => gameViews[currentView].OpenView(ViewIsOpenId);
        private void CloseCurrentView() => gameViews[currentView].CloseView(ViewIsOpenId);
    }
}