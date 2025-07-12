namespace Mibl_Test_Task.Scripts.Services.WinCondition
{
    public interface IWinConditionService
    {
        void WarmUp();
        bool AreWinConditionsMet();
        bool IsWon { get;}
        void SetWin(bool b);
    }
}