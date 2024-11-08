using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;  
    [SerializeField] private Image _healthFillImage;  

    private void Start()
    {
        if (_health != null) _health.OnDie.AddListener(HideHealthBar);
    }


    private void Update()
    {
        _healthFillImage.fillAmount = (float)_health.CurrentHealth / _health.MaxHealth;
    }

    private void HideHealthBar()
    {
        if (_healthFillImage != null)
        {
            _healthFillImage.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _health.OnDie.RemoveListener(HideHealthBar);
    }
}
