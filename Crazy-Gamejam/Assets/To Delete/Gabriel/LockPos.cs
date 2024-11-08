using Main.Gameplay.Player;
using UnityEngine;

public class LockPos : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _engate;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform _carroEngate;

    private void Start()
    {
        // Obtém a referência para o PlayerController na cena
        _playerController = FindFirstObjectByType<PlayerController>();

        if (_playerController == null)
        {
            Debug.LogError("PlayerController não encontrado!");
            return;
        }

        Debug.Log(_playerController);

        // Itera sobre os BodyUnits do PlayerController
        for (int i = 0; i < _playerController.BodyUnits.Count; i++)
        {
            if (_playerController.BodyUnits[i] != null)
            {
                if (gameObject == _playerController.BodyUnits[i].gameObject)
                {
                    // Se o índice for 0, o engate é o objeto com a tag "PlayerHead"
                    if (i == 0)
                    {
                        _engate = GameObject.FindWithTag("PlayerHead")?.transform;
                        if (_engate == null)
                        {
                            Debug.LogError("Objeto com a tag 'PlayerHead' não encontrado!");
                            return;
                        }
                    }
                    else
                    {
                        _engate = _playerController.BodyUnits[i - 1].Find("backHook").transform;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (_engate == null) return;

        transform.position = new Vector3(_engate.position.x, 0, _engate.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, _engate.parent.rotation, Time.deltaTime * _rotationSpeed);
    }
}
