using Services.PersistenceProgress.Player;

namespace Services.PersistenceProgress
{
    public class PersistenceProgressService : IPersistenceProgressService
    {
        public PlayerData Player { get; set; }
    }
}