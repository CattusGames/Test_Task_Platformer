using Mibl_Test_Task.Scripts.Game;
using Mibl_Test_Task.Scripts.Services.GameStateService;
using Services.PersistenceProgress;
using UnityEngine;
using Zenject;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 1;

    private int _currentHealth;
    private bool _isPlayer;
    private ILevelStateService _levelState;
    private IPersistenceProgressService _persistenceProgressService;

    [Inject]
    public void Construct(ILevelStateService levelState,IPersistenceProgressService persistenceProgressService)
    {
        _persistenceProgressService = persistenceProgressService;
        _levelState = levelState;
        _isPlayer = GetComponent<Player>() != null;
    }

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            if (_isPlayer)
                _levelState.Lose();
            else
            {
                _persistenceProgressService.Player.IncreaseKillCount();
                Destroy(gameObject); 
            }
                
        }
    }
}