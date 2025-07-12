using Services.PersistenceProgress.Player;

namespace Services.PersistenceProgress
{
    public interface IPersistenceProgressService
    {
        PlayerData Player { get; set; }
    }
}