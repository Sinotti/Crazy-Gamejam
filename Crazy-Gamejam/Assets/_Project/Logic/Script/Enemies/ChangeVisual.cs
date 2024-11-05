using UnityEngine;

public class ChangeVisual : MonoBehaviour
{
    [SerializeField] private GameObject[] models;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        models[Random.Range(0, models.Length)].SetActive(false);
    }
}
