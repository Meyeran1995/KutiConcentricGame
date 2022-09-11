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
            gameViews[1].gameObject.SetActive(false);
            gameViews[2].gameObject.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            OpenCurrentView();
        }

        protected override void OnGameStart()
        {
            ChangeToView(1);
        }

        protected override void OnGameEnd()
        {
            ChangeToView(2);
        }

        protected override void OnGameRestart()
        {
            ChangeToView(1);
        }

        private void ChangeToView(int view)
        {
            CloseCurrentView();
            currentView = view;
            OpenCurrentView();
        }
        
        private void OpenCurrentView() => gameViews[currentView].OpenView(ViewIsOpenId);
        private void CloseCurrentView() => gameViews[currentView].CloseView(ViewIsOpenId);
    }
}