using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Parameters")]
    [Space(6)]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    [Header("Events")]
    [Space(6)]
    public UnityEvent OnTakeDamage;
    public UnityEvent OnDie;

    private IDestroyable _destroyableUnit;

    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    private void Start()
    {
        _currentHealth = _maxHealth;

        _destroyableUnit = GetComponentInParent<IDestroyable>();
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

        _destroyableUnit?.Destroyed(transform);

        Destroy(gameObject);
    }
}
