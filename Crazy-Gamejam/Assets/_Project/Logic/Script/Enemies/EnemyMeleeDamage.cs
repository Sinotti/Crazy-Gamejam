using UnityEngine;

public class EnemyMeleeDamage : MonoBehaviour
{
    [Header("Damage Parameters")]
    [Space(6)]
    [SerializeField] private int _damage;

    [Header("Detection Parameters")]
    [Space(6)]
    [SerializeField] private Vector3 _detectionBoxSize;
    [SerializeField] private Vector3 _detectionBoxCenter; // Modifiquei o nome para deixar claro que é o centro do colisor
    [Space(6)]
    [SerializeField] LayerMask _detectionLayer;

    private Collider[] _bodyParts;

    private void Update()
    {
        // Usando o _colliderCenter para definir o centro da detecção
        _bodyParts = Physics.OverlapBox(transform.position + _detectionBoxCenter, _detectionBoxSize / 2, Quaternion.identity, _detectionLayer);

        if (_bodyParts.Length > 0)
        {
            for (int i = 0; i < _bodyParts.Length; i++)
            {
                if (_bodyParts[i].TryGetComponent(out Health health))
                {
                    health.TakeDamage(_damage);
                }
            }

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + _detectionBoxCenter, _detectionBoxSize);
    }
}
