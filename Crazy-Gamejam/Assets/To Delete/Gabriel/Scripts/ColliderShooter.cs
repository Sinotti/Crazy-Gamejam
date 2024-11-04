using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;

public class ColliderShooter : MonoBehaviour
{
    [Header("Shoot Parameters")]
    [Space(6)]
    [SerializeField] private int _damagePerTick; // Tempo em segundos para o sensor ficar inativo
    [SerializeField] private float _cooldownTime; // Tempo em segundos para o sensor ficar inativo
    private float _activationTime = .5f;

    [Header("References")]
    [Space(6)]
    [SerializeField] private TriggerSensor _sensor;
    [SerializeField] private GameObject _sensorDebug; // To remove
    [SerializeField] private GameObject _sensorDebugSilhouette;// To remove
    
    private void Start()
    {
        if (_sensor == null)
        {
            Debug.LogError("Sensor is NULL.");
            return;
        }

        StartCoroutine(SensorActivationCycle());
    }

    private IEnumerator SensorActivationCycle()
    {
        while (true)
        {
            _sensor.enabled = true;
            _sensorDebugSilhouette.SetActive(false);
            _sensorDebug.SetActive(true);
            yield return new WaitForSeconds(_activationTime);

            ShootInEnemy();
            //LogDetectedObjects(); 
            _sensor.enabled = false;
            _sensorDebug.SetActive(false);
            _sensorDebugSilhouette.SetActive(true);

            _sensor.DetectedObjects.Clear();

            yield return new WaitForSeconds(_cooldownTime);
        }
    }

    private void LogDetectedObjects()
    {
        foreach (GameObject obj in _sensor.DetectedObjects)
        {
            Debug.Log($"Objeto em colisão: {obj.name}");
        }
    }

    private void ShootInEnemy()
    {
        for (int i = 0; i < _sensor.DetectedObjects.Count; i++)
        {
            if( _sensor.DetectedObjects[i].TryGetComponent(out Health health))
            {
                health.TakeDamage(_damagePerTick);
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
