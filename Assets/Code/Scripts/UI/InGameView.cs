
namespace Meyham.UI
{
    public class InGameView : AGameView
    {
        public override void OpenView(int animatorId)
        {
            gameObject.SetActive(true);
        }

        public override void CloseView(int animatorId)
        {
            gameObject.SetActive(false);
        }
    }
}