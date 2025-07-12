namespace Mibl_Test_Task.Scripts.StaticData
{
    [System.Flags]
    public enum FinishConditions
    {
        None = 0,
        KillCount = 1 << 1,
        Distance = 1 << 2
    }
}