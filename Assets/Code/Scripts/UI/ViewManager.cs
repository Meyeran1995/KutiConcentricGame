using Meyham.EditorHelpers;
using Meyham.Events;
using Meyham.GameMode;
using Meyham.Set_Up;
using UnityEngine;

namespace Meyham.UI
{
    public class ViewManager : AGameLoopSystem
    {
        [Header("References")]
        [SerializeField] private AGameView[] gameViews;
        [SerializeField] private PlayerColors playerColors;
        [SerializeField] private GenericEventChannelSO<int> playerJoined;
        
        [Header("Debug")]
        [ReadOnly, SerializeField] private int currentView;

        private static readonly int ViewIsOpenId = Animator.StringToHash("IsOpen");

        private ViewColorProvider viewColorProvider;
        
        private void Awake()
        {
            currentView = 0;
        }

        protected override void Start()
        {
            base.Start();
            viewColorProvider = new ViewColorProvider(gameViews, playerColors);
            viewColorProvider.SubscribeToEvent(playerJoined);
            OpenCurrentView();
        }

        protected override void OnGameStart()
        {
            ChangeToView(1);
            
            if(viewColorProvider == null) return;
            
            viewColorProvider.UnSubscribeFromEvent(playerJoined);
            viewColorProvider = null;
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