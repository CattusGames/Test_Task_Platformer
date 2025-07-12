namespace Infrastructure
{
    public interface ILoadingCurtain
    {
        void Show();
        void Hide();
        void ShowProgress(float progress);
        void ShowFirst();
        LoadingCurtain.Status Current { get; }
    }
}