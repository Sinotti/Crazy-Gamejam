using UnityEngine;
using UnityEngine.Events;
using Main.Gameplay.Player; // Certifique-se de que este namespace seja correto para acessar PlayerController

public class Health : MonoBehaviour, IDamageable
{
    [Header("Parameters")]
    [Space(6)]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    [Header("Events")]
    [Space(6)]
    [SerializeField] private UnityEvent OnTakeDamage;
    [SerializeField] private UnityEvent OnDie;

    // Refer�ncia ao PlayerController
    private PlayerController _playerController;

    private void Start()
    {
        _currentHealth = _maxHealth;

        // Busca automaticamente o PlayerController no objeto pai (assumindo que o objeto Player � o pai das unidades)
        _playerController = GetComponentInParent<PlayerController>();

        // Verifica se a refer�ncia foi encontrada
        if (_playerController == null)
        {
            Debug.LogError("PlayerController n�o encontrado no objeto pai. Certifique-se de que este script est� em uma unidade do player.");
        }
    }

    public void TakeDamage(int amount)
    {
        ChangeHealthValue(-amount);
        OnTakeDamage?.Invoke();
    }

    private void ChangeHealthValue(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_currentHealth <= 0) Die();
    }

    private void Die()
    {
        OnDie?.Invoke();

        // Remove a unidade do PlayerController antes de destru�-la
        if (_playerController != null)
        {
            _playerController.RemoveBodyUnit(transform);
        }

        // Destroi a unidade
        Destroy(gameObject);
    }
}