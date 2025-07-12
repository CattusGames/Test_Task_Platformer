namespace Mibl_Test_Task.Scripts.Services.GameStateService
{
    public interface ILevelStateService
    {
        void Lose();
        void Win();
        void Pause();
        void Resume();
        void Restart();
        void Quit();

        void CleanUp();
    }
}