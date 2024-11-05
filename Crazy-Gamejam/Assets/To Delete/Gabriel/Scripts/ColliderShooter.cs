using System.Collections;
using UnityEngine;
using SensorToolkit;

public class ColliderShooter : MonoBehaviour
{
    [Header("Shoot Parameters")]
    [Space(6)]
    [SerializeField] private int _damagePerTick;
    [SerializeField] private float _cooldownTime;

    [Header("References")]
    [Space(6)]
    [SerializeField] private TriggerSensor _sensor;
    [SerializeField] private GameObject _sensorDebug; // To remove
    [SerializeField] private GameObject _sensorDebugSilhouette; // To remove

    private void Awake()
    {
        _sensor.OnDetected.AddListener(OnDetectedHandler);
    }

    private void Start()
    {
        if (_sensor == null)
        {
            Debug.LogError("Sensor is NULL.");
            return;
        }
    }

    private void OnDetectedHandler(GameObject detectedObject, Sensor sensor)
    {
        ShootInEnemy();
    }

    private IEnumerator SensorActivationCycle()
    {
        yield return new WaitForSeconds(_cooldownTime);

        _sensor.enabled = true;
        _sensorDebugSilhouette.SetActive(false);
        _sensorDebug.SetActive(true);
    }

    private void ShootInEnemy()
    {
        foreach (GameObject obj in _sensor.DetectedObjects)
        {
            if (obj.TryGetComponent(out Health health))
            {
                health.TakeDamage(_damagePerTick);
            }
        }

        _sensor.enabled = false;
        _sensorDebug.SetActive(false);
        _sensorDebugSilhouette.SetActive(true);

        _sensor.DetectedObjects.Clear();

        StartCoroutine(SensorActivationCycle());
    }

    private void OnDestroy()
    {
        _sensor.OnDetected.RemoveListener(OnDetectedHandler);
        StopAllCoroutines();
    }
}
