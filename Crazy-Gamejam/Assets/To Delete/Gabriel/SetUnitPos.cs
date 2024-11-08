using UnityEngine;

public class SetUnitPos : MonoBehaviour
{
    [SerializeField] private Transform _engateVeiculoFrent;
    [SerializeField] private float _distance;

    private void Start()
    {
        transform.position = new Vector3(_engateVeiculoFrent.position.x, transform.position.y, _engateVeiculoFrent.position.z + _distance);
    }


    private void Update()
    {
        transform.position = new Vector3(_engateVeiculoFrent.position.x, transform.position.y, _engateVeiculoFrent.position.z + _distance);
    }
}
