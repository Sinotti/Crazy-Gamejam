using UnityEngine;

public class EnemyMeleeDamage : MonoBehaviour
{
    [Header("Detection Parameters")]
    [Space(6)]
    [SerializeField] private Vector3 _detectionBoxSize;
    [Space(6)]
    [SerializeField] LayerMask _detectionLayer;

    private Collider[] _bodyParts;
    private Vector3 _colliderSize = Vector3.zero;

    private void Update()
    {
        _bodyParts = Physics.OverlapBox(transform.position + _colliderSize, _detectionBoxSize / 2, Quaternion.identity, _detectionLayer);

        if(_bodyParts.Length > 0)
        {
            for (int i = 0; i < _bodyParts.Length; i++)
            {
                _bodyParts[i].gameObject.SetActive(false);
            }

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + _colliderSize, _detectionBoxSize);
    }
}
