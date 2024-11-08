using Main.Gameplay.Player;
using UnityEngine;
using UnityEngine.UIElements;

public class LockPos : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _engate;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform _carroEngate;
    [SerializeField] private float _rotationLimit = 45f;

    private void Start()
    {
        _playerController = FindFirstObjectByType<PlayerController>();

        Debug.Log(_playerController);

        for (int i = 0; i < _playerController.BodyUnits.Count; i++)
        {
            if(_playerController.BodyUnits[i] != null)
            {
                if(gameObject == _playerController.BodyUnits[i].gameObject)
                {
                    _engate = _playerController.BodyUnits[i - 1].Find("backHook").transform;
                }
            }
        }
    }

    private void Update()
    {
        transform.position = new Vector3(_engate.position.x, 0, _engate.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, _engate.parent.rotation, Time.deltaTime * _rotationSpeed);
        Mathf.Clamp(transform.rotation.y, 0, _rotationLimit);
    }
}
