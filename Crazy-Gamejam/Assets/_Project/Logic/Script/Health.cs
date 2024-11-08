using Main.Gameplay.Enemies;
using MoreMountains.Feedbacks;
using SensorToolkit;
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
    [SerializeField] private MMF_Player mmfPlayer;
    [SerializeField] private BoxCollider boxCollider;
    private EnemyMovement movement;
    [SerializeField] private ChangeVisual changeVisual;
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    private void Awake()
    {
        changeVisual = GetComponent<ChangeVisual>();
        mmfPlayer = changeVisual.selectVisual();

    }

    private void Start()
    {

        //if (mmfPlayer == null && )
        //{
        //    mmfPlayer = GetComponentInChildren<MMF_Player>();
        //}

        _currentHealth = _maxHealth;
        _destroyableUnit = GetComponentInParent<IDestroyable>();
        movement = GetComponent<EnemyMovement>();
        boxCollider = GetComponent<BoxCollider>();
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
        boxCollider.enabled = false;

        //_destroyableUnit?.Destroyed(transform);
        movement?.Stop();
        mmfPlayer?.PlayFeedbacks();
        Destroy(gameObject, 2f);
    }
}
